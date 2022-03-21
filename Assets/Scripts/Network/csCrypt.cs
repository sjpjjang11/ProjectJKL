using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
2018.06.08
keiwalk
Network packet chunk encrypt, decrypt..
*/
public class csCrypt : MonoBehaviour 
{
	// 암호화를 위한 상수들..
	const int C1 = 52845;
	const int C2 = 22719;
	const int KEY = 72957;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	/*
	2018.06.08
	keiwalk
	encrypt code
	*/
	static public bool Encrypt(byte[] _source, byte[] _dest, int _len)
	{
		if(_len <= 0)
			return false;

		int Key = KEY;
		for(int i = 0; i < _len; i++)
		{
			_dest[i] = (byte)(_source[i] ^ Key >> 8);
			Key = (_dest[i] + Key) * C1 + C2;
		}
		return true;
	}

	/*
	2018.06.08
	keiwalk
	decrypt code
	*/
	static public bool Decrypt(byte[] _source, byte[] _dest, int _len)
	{
		if(_len <= 0)
			return false;

		int Key = KEY;
		byte Prevblock;
		for(int i = 0; i < _len; i++)
		{
			Prevblock = _source[i];
			_dest[i] = (byte)((int)_source[i] ^ Key >> 8);
			Key = (Prevblock + Key) * C1 + C2;
		}
		return true;
	}
}
