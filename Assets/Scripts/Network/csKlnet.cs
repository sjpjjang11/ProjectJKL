using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public struct GamePacket
{
    public int Protocol;
    public byte[] Buffer;
}

/*
2018.06.07
keiwalk
TCP/IP streaming network class
*/
public class csKlnet : MonoBehaviour {

    public const int DEFAULT_BUFFER_SIZE = 4096 * 2;

	public string m_strIp;							// ip address, 유니티 툴에서 입력
	public int m_iPort;								// port

	private Socket m_Socket = null;					// socket, stream TCP로 생성할거임
	private Thread m_ClientThread = null;			// recv thread, 렌더링과 별개의 쓰레드로 게임 매니지 클래스에 recv된 패킷을 전달한다
	private Queue m_Packetqueues = new Queue();		// 패킷 메시지큐
	private object m_Lockobj = new object();		// recv 쓰레드와 렌더링 프로세스간의 동기화 처리

    virtual protected void OnConnected() {}
    virtual protected void OnPacketRecv(GamePacket _gp) {}
    virtual protected void OnDisconnected() {}

	// Use this for initialization
	void Start () 
	{
		// 씬이 시작됌과 함께 접속한다		
		//ConnectToServer(m_strIp, m_iPort);
	}
	
	// Update is called once per frame
    protected void Update () 
	{		
 		// 게임 매니저 객체 쪽으로 패킷을 순차적으로 보낸다
		lock(m_Lockobj)
		{
			foreach(GamePacket gp in m_Packetqueues)
			{
                if(gp.Protocol == 0)
                {
                    //                  m_GameManager.SendMessage("OnDisconnected");
                    OnDisconnected();
                }
                else
                {
//                    m_GameManager.SendMessage("OnPacketRecv", gp);
                    OnPacketRecv(gp);
                }
			}
            m_Packetqueues.Clear();
		}
	}

	/*
	2018.06.07
	keiwalk
	서버 접속 메써드. 연결을 하고 recv thread를 실행한다.
	*/
	public bool ConnectToServer(string _ip, int _port)
	{
		try
		{
			m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			m_Socket.Connect(new IPEndPoint(IPAddress.Parse(_ip), _port));			
		}
		catch(SocketException se)
		{
			Debug.Log("ConnectToServer error - " + se.ToString());
			return false;
		}

		Debug.Log("ConnectToServer connected - " + _ip);
		m_ClientThread = new System.Threading.Thread(ClientThread);
        m_ClientThread.Start();

        OnConnected();
//        m_GameManager.SendMessage("OnConnected", this);

        return true;
	}

    /*
    2018.06.11
    keiwalk
    Packet send
    */
    public bool SendPacket(int _protocol, byte[] _buffer, int _len)
    {
        // encrypt packet
        csCrypt.Encrypt(_buffer, _buffer, _len);

        // len + protocol + packet content
        MemoryStream ms = new MemoryStream(_len + sizeof(int) * 2);
        ms.Write(BitConverter.GetBytes(_len + sizeof(int) * 2), 0, sizeof(int));
        ms.Write(BitConverter.GetBytes(_protocol), 0, sizeof(int));
        ms.Write(_buffer, 0, _len);

        m_Socket.Send(ms.GetBuffer(), _len + sizeof(int) * 2, 0);

        ms.Close();
        return true;
    }

	/*
	2018.06.07
	keiwalk
	렌더링쪽과 별개의 쓰레드로 동작하여 서버에서 전달하는 패킷을 recv 하고 메시지큐에 넣는다
	*/
	private void ClientThread()
	{
        Debug.Log("Network Thread Start...");

        byte[] Recvbuffer = new byte[DEFAULT_BUFFER_SIZE];
		int Recvlen = 0;
		// recv한 버퍼를 MemoryStream 객체에 보관
        MemoryStream Memstream = new MemoryStream(DEFAULT_BUFFER_SIZE * 2);
        while(true)
		{
            try
            {
                Recvlen = m_Socket.Receive(Recvbuffer);
            }
            catch (SocketException se)
            {
                Debug.Log("socket exception: " + se.Message);
                break;
            }
//            Debug.Log("recvlen = " + Recvlen);
            if (Recvlen == -1)
                break;

 			Memstream.Write(Recvbuffer, 0, Recvlen);
         	
			// [LENGTH][PROTOCOL] 길이보다 크고 전달한 Packetlen만큼 받았으면
            while(Memstream.Position >= 8)
			{
                byte[] Lenbuffer = new byte[sizeof(int)];
                Buffer.BlockCopy(Memstream.GetBuffer(), 0, Lenbuffer, 0, sizeof(int));
                int Packetlen = BitConverter.ToInt32(Lenbuffer, 0);

                if (Packetlen > Memstream.Position)
                    break;
                
                byte[] Protocolbuffer = new byte[sizeof(int)];
                Buffer.BlockCopy(Memstream.GetBuffer(), sizeof(int), Protocolbuffer, 0, sizeof(int));

                GamePacket gp = new GamePacket();
                gp.Protocol = BitConverter.ToInt32(Protocolbuffer, 0);
				gp.Buffer = new byte[Packetlen - sizeof(int) * 2];
                Buffer.BlockCopy(Memstream.GetBuffer(), sizeof(int) * 2, gp.Buffer, 0, Packetlen - sizeof(int) * 2);

                csCrypt.Decrypt(gp.Buffer, gp.Buffer, Packetlen - sizeof(int) * 2);
				lock(m_Lockobj)
				{
					m_Packetqueues.Enqueue(gp);
				}

				// MemoryStream 객체에 남은 패킷버퍼를 계속 사용할 수있게 이동
                Buffer.BlockCopy(Memstream.GetBuffer(), Packetlen, Memstream.GetBuffer(), 0, (int)Memstream.Position - Packetlen);
                Memstream.Seek((int)Memstream.Position - Packetlen, SeekOrigin.Begin);
			}
		}

		m_Socket.Close();

        lock (m_Lockobj)
        {
            GamePacket gp = new GamePacket();
            gp.Protocol = 0;
            m_Packetqueues.Enqueue(gp);
        }
	}

    void OnApplicationQuit()
    {
        //m_Socket.Disconnect(false);
    }
}
