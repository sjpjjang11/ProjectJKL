using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csREAD_PJProtocol : MonoBehaviour
{
	public static bool csREAD_PJ_LOGIN(byte[] _buffer, ref _S_PJ_LOGIN _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(char) * 16);
		ms.Write(_buffer, index, sizeof(char) * 16);
		_data.ID = Encoding.Unicode.GetString(ms.GetBuffer());
		index += sizeof(char) * 16;
		ms.Close();
		ms = new MemoryStream(sizeof(char) * 16);
		ms.Write(_buffer, index, sizeof(char) * 16);
		_data.PASSWORD = Encoding.Unicode.GetString(ms.GetBuffer());
		index += sizeof(char) * 16;
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.VERSION = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_LOGIN_SUCC_U(byte[] _buffer, ref _S_PJ_LOGIN_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(char) * 16);
		ms.Write(_buffer, index, sizeof(char) * 16);
		_data.ID = Encoding.Unicode.GetString(ms.GetBuffer());
		index += sizeof(char) * 16;
		ms.Close();
		ms = new MemoryStream(sizeof(char) * 16);
		ms.Write(_buffer, index, sizeof(char) * 16);
		_data.NAME = Encoding.Unicode.GetString(ms.GetBuffer());
		index += sizeof(char) * 16;
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_LOGIN_FAIL_U(byte[] _buffer, ref _S_PJ_LOGIN_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_ENTER_GAMEROOM(byte[] _buffer, ref _S_PJ_ENTER_GAMEROOM _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ISTEAMGAME = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_ENTER_GAMEROOM_START_U(byte[] _buffer, ref _S_PJ_ENTER_GAMEROOM_START_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_ENTER_GAMEROOM_U(byte[] _buffer, ref _S_PJ_ENTER_GAMEROOM_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.COUNT = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		if(_data.COUNT > 50)
			return false;
		_data.DATA = new csCHARACTERINFO[_data.COUNT];
		for(int i = 0; i < _data.COUNT; i++)
		{
			_data.DATA[i] = new csCHARACTERINFO();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwUserindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(char) * 16);
			ms.Write(_buffer, index, sizeof(char) * 16);
			_data.DATA[i].m_strUsername = Encoding.Unicode.GetString(ms.GetBuffer());
			index += sizeof(char) * 16;
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharacterindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharactertype = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			_data.DATA[i].m_Curpos = new csVECTOR2[1];
			for(int j = 0; j < 1; j++)
			{
				_data.DATA[i].m_Curpos[j] = new csVECTOR2();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Curpos[j].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Curpos[j].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
			}
			_data.DATA[i].m_Direction = new csVECTOR2[1];
			for(int j = 0; j < 1; j++)
			{
				_data.DATA[i].m_Direction[j] = new csVECTOR2();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Direction[j].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Direction[j].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
			}
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharstate = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwLasttimestamp = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_iEnergy = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dSpeed = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dPower = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dAttackRange = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dAttackcooltime = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwTeamindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
		}
		if(_data.COUNT > 50)
			return false;
		_data.PETINFO = new csPETINFO[_data.COUNT];
		for(int i = 0; i < _data.COUNT; i++)
		{
			_data.PETINFO[i] = new csPETINFO();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.PETINFO[i].m_dwPetindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(char) * 16);
			ms.Write(_buffer, index, sizeof(char) * 16);
			_data.PETINFO[i].m_strPetrname = Encoding.Unicode.GetString(ms.GetBuffer());
			index += sizeof(char) * 16;
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.PETINFO[i].m_iEnergy = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.PETINFO[i].m_dSpeed = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_ENTER_GAMEROOM_END_U(byte[] _buffer, ref _S_PJ_ENTER_GAMEROOM_END_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_ENTER_GAMEROOM_FAIL_U(byte[] _buffer, ref _S_PJ_ENTER_GAMEROOM_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_ENTER_GAMEROOM_M(byte[] _buffer, ref _S_PJ_ENTER_GAMEROOM_M _data)
	{
		MemoryStream ms;
		int index = 0;

		_data.DATA = new csCHARACTERINFO[1];
		for(int i = 0; i < 1; i++)
		{
			_data.DATA[i] = new csCHARACTERINFO();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwUserindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(char) * 16);
			ms.Write(_buffer, index, sizeof(char) * 16);
			_data.DATA[i].m_strUsername = Encoding.Unicode.GetString(ms.GetBuffer());
			index += sizeof(char) * 16;
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharacterindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharactertype = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			_data.DATA[i].m_Curpos = new csVECTOR2[1];
			for(int j = 0; j < 1; j++)
			{
				_data.DATA[i].m_Curpos[j] = new csVECTOR2();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Curpos[j].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Curpos[j].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
			}
			_data.DATA[i].m_Direction = new csVECTOR2[1];
			for(int j = 0; j < 1; j++)
			{
				_data.DATA[i].m_Direction[j] = new csVECTOR2();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Direction[j].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Direction[j].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
			}
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharstate = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwLasttimestamp = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_iEnergy = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dSpeed = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dPower = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dAttackRange = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dAttackcooltime = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwTeamindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
		}
		_data.PETINFO = new csPETINFO[1];
		for(int i = 0; i < 1; i++)
		{
			_data.PETINFO[i] = new csPETINFO();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.PETINFO[i].m_dwPetindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(char) * 16);
			ms.Write(_buffer, index, sizeof(char) * 16);
			_data.PETINFO[i].m_strPetrname = Encoding.Unicode.GetString(ms.GetBuffer());
			index += sizeof(char) * 16;
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.PETINFO[i].m_iEnergy = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.PETINFO[i].m_dSpeed = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_EXIT_GAMEROOM_M(byte[] _buffer, ref _S_PJ_EXIT_GAMEROOM_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_ENTER_LOADING_COMPLETE(byte[] _buffer, ref _S_PJ_ENTER_LOADING_COMPLETE _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_ENTER_LOADING_COMPLETE_SUCC_U(byte[] _buffer, ref _S_PJ_ENTER_LOADING_COMPLETE_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_ENTER_LOADING_COMPLETE_FAIL_U(byte[] _buffer, ref _S_PJ_ENTER_LOADING_COMPLETE_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_BATTLEROOM_COUNTDOWN_M(byte[] _buffer, ref _S_PJ_BATTLEROOM_COUNTDOWN_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.SECOND = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_MOVETO_BATTLEROMM(byte[] _buffer, ref _S_PJ_MOVETO_BATTLEROMM _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.COUNT = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		if(_data.COUNT > 100)
			return false;
		_data.DATA = new csCHARACTERINFO[_data.COUNT];
		for(int i = 0; i < _data.COUNT; i++)
		{
			_data.DATA[i] = new csCHARACTERINFO();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwUserindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(char) * 16);
			ms.Write(_buffer, index, sizeof(char) * 16);
			_data.DATA[i].m_strUsername = Encoding.Unicode.GetString(ms.GetBuffer());
			index += sizeof(char) * 16;
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharacterindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharactertype = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			_data.DATA[i].m_Curpos = new csVECTOR2[1];
			for(int j = 0; j < 1; j++)
			{
				_data.DATA[i].m_Curpos[j] = new csVECTOR2();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Curpos[j].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Curpos[j].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
			}
			_data.DATA[i].m_Direction = new csVECTOR2[1];
			for(int j = 0; j < 1; j++)
			{
				_data.DATA[i].m_Direction[j] = new csVECTOR2();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Direction[j].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Direction[j].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
			}
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharstate = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwLasttimestamp = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_iEnergy = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dSpeed = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dPower = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dAttackRange = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dAttackcooltime = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwTeamindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_BATTLEROOM_MONTERINFO_M(byte[] _buffer, ref _S_PJ_BATTLEROOM_MONTERINFO_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.COUNT = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		if(_data.COUNT > 100)
			return false;
		_data.MONSTERS = new csCHARACTERINFO[_data.COUNT];
		for(int i = 0; i < _data.COUNT; i++)
		{
			_data.MONSTERS[i] = new csCHARACTERINFO();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.MONSTERS[i].m_dwUserindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(char) * 16);
			ms.Write(_buffer, index, sizeof(char) * 16);
			_data.MONSTERS[i].m_strUsername = Encoding.Unicode.GetString(ms.GetBuffer());
			index += sizeof(char) * 16;
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.MONSTERS[i].m_dwCharacterindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.MONSTERS[i].m_dwCharactertype = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			_data.MONSTERS[i].m_Curpos = new csVECTOR2[1];
			for(int j = 0; j < 1; j++)
			{
				_data.MONSTERS[i].m_Curpos[j] = new csVECTOR2();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.MONSTERS[i].m_Curpos[j].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.MONSTERS[i].m_Curpos[j].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
			}
			_data.MONSTERS[i].m_Direction = new csVECTOR2[1];
			for(int j = 0; j < 1; j++)
			{
				_data.MONSTERS[i].m_Direction[j] = new csVECTOR2();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.MONSTERS[i].m_Direction[j].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.MONSTERS[i].m_Direction[j].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
			}
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.MONSTERS[i].m_dwCharstate = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.MONSTERS[i].m_dwLasttimestamp = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.MONSTERS[i].m_iEnergy = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.MONSTERS[i].m_dSpeed = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.MONSTERS[i].m_dPower = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.MONSTERS[i].m_dAttackRange = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.MONSTERS[i].m_dAttackcooltime = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.MONSTERS[i].m_dwTeamindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_BATTLEROOM_BATTLESTART_M(byte[] _buffer, ref _S_PJ_BATTLEROOM_BATTLESTART_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_MOVE_START(byte[] _buffer, ref _S_PJ_MOVE_START _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.DIRECTION = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.DIRECTION[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_MOVE_START_SUCC_U(byte[] _buffer, ref _S_PJ_MOVE_START_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_MOVE_START_FAIL_U(byte[] _buffer, ref _S_PJ_MOVE_START_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_MOVE_START_M(byte[] _buffer, ref _S_PJ_MOVE_START_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.DIRECTION = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.DIRECTION[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.MONSTERACT = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_MOVE_STOP(byte[] _buffer, ref _S_PJ_MOVE_STOP _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_MOVE_STOP_SUCC_U(byte[] _buffer, ref _S_PJ_MOVE_STOP_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_MOVE_STOP_FAIL_U(byte[] _buffer, ref _S_PJ_MOVE_STOP_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_MOVE_STOP_M(byte[] _buffer, ref _S_PJ_MOVE_STOP_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.MONSTERACT = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_CHANGE_DIRECTION_START(byte[] _buffer, ref _S_PJ_CHANGE_DIRECTION_START _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.DIRECTION = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.DIRECTION[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.ROTATE = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.ROTATE[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ROTATE[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ROTATE[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_CHANGE_DIRECTION_START_SUCC_U(byte[] _buffer, ref _S_PJ_CHANGE_DIRECTION_START_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_CHANGE_DIRECTION_START_FAIL_U(byte[] _buffer, ref _S_PJ_CHANGE_DIRECTION_START_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_CHANGE_DIRECTION_START_M(byte[] _buffer, ref _S_PJ_CHANGE_DIRECTION_START_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.DIRECTION = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.DIRECTION[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.ROTATE = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.ROTATE[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ROTATE[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ROTATE[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_CHANGE_DIRECTION_STOP(byte[] _buffer, ref _S_PJ_CHANGE_DIRECTION_STOP _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.DIRECTION = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.DIRECTION[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_CHANGE_DIRECTION_STOP_SUCC_U(byte[] _buffer, ref _S_PJ_CHANGE_DIRECTION_STOP_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_CHANGE_DIRECTION_STOP_FAIL_U(byte[] _buffer, ref _S_PJ_CHANGE_DIRECTION_STOP_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_CHANGE_DIRECTION_STOP_M(byte[] _buffer, ref _S_PJ_CHANGE_DIRECTION_STOP_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.DIRECTION = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.DIRECTION[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_CHANGE_DIRECTION(byte[] _buffer, ref _S_PJ_CHANGE_DIRECTION _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.DIRECTION = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.DIRECTION[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_CHANGE_DIRECTION_SUCC_U(byte[] _buffer, ref _S_PJ_CHANGE_DIRECTION_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_CHANGE_DIRECTION_FAIL_U(byte[] _buffer, ref _S_PJ_CHANGE_DIRECTION_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_CHANGE_DIRECTION_M(byte[] _buffer, ref _S_PJ_CHANGE_DIRECTION_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.DIRECTION = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.DIRECTION[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_CHANGE_AIM(byte[] _buffer, ref _S_PJ_CHANGE_AIM _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.AIM = new csVECTOR3[1];
		for(int i = 0; i < 1; i++)
		{
			_data.AIM[i] = new csVECTOR3();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.AIM[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.AIM[i].m_dy = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.AIM[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.DIRECTION = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.DIRECTION[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_CHANGE_AIM_SUCC_U(byte[] _buffer, ref _S_PJ_CHANGE_AIM_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_CHANGE_AIM_FAIL_U(byte[] _buffer, ref _S_PJ_CHANGE_AIM_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_CHANGE_AIM_M(byte[] _buffer, ref _S_PJ_CHANGE_AIM_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.AIM = new csVECTOR3[1];
		for(int i = 0; i < 1; i++)
		{
			_data.AIM[i] = new csVECTOR3();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.AIM[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.AIM[i].m_dy = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.AIM[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.DIRECTION = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.DIRECTION[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DIRECTION[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_SHOOT_ENEMY(byte[] _buffer, ref _S_PJ_SHOOT_ENEMY _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.AIM = new csVECTOR3[1];
		for(int i = 0; i < 1; i++)
		{
			_data.AIM[i] = new csVECTOR3();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.AIM[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.AIM[i].m_dy = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.AIM[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ISHIT = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		_data.ENEMYPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.ENEMYPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ENEMYPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ENEMYPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.SKILLTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_SHOOT_ENEMY_SUCC_U(byte[] _buffer, ref _S_PJ_SHOOT_ENEMY_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_SHOOT_ENEMY_FAIL_U(byte[] _buffer, ref _S_PJ_SHOOT_ENEMY_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_SHOOT_ENEMY_M(byte[] _buffer, ref _S_PJ_SHOOT_ENEMY_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.AIM = new csVECTOR3[1];
		for(int i = 0; i < 1; i++)
		{
			_data.AIM[i] = new csVECTOR3();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.AIM[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.AIM[i].m_dy = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.AIM[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYENERGY = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ISHIT = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		_data.ENEMYPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.ENEMYPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ENEMYPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ENEMYPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.SKILLTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_HIT_ME(byte[] _buffer, ref _S_PJ_HIT_ME _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_HIT_ME_SUCC_U(byte[] _buffer, ref _S_PJ_HIT_ME_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_HIT_ME_FAIL_U(byte[] _buffer, ref _S_PJ_HIT_ME_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_HIT_ME_M(byte[] _buffer, ref _S_PJ_HIT_ME_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERENERGY = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_HIT_ENEMY(byte[] _buffer, ref _S_PJ_HIT_ENEMY _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		_data.ENEMYPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.ENEMYPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ENEMYPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ENEMYPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		_data.MYCURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.MYCURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.MYCURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.MYCURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_HIT_ENEMY_SUCC_U(byte[] _buffer, ref _S_PJ_HIT_ENEMY_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_HIT_ENEMY_FAIL_U(byte[] _buffer, ref _S_PJ_HIT_ENEMY_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_HIT_ENEMY_M(byte[] _buffer, ref _S_PJ_HIT_ENEMY_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ENEMYENERGY = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		_data.ENEMYPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.ENEMYPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ENEMYPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ENEMYPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_BATTLEROOM_RESULTSCORE_U(byte[] _buffer, ref _S_PJ_BATTLEROOM_RESULTSCORE_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ISWIN = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.KILLCOUNT = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.RANKING = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TOTALDAMAGE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.SURVIVALTIME = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_DEAD_CHARACTER_M(byte[] _buffer, ref _S_PJ_DEAD_CHARACTER_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.DEADTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.DEADINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.KILLERTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.KILLERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_TEMP_STARTBATTLE(byte[] _buffer, ref _S_PJ_TEMP_STARTBATTLE _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_TOUCH_MYTEAM_ZOMBI(byte[] _buffer, ref _S_PJ_TOUCH_MYTEAM_ZOMBI _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.MYTEAMTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.MYTEAMINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_TOUCH_MYTEAM_ZOMBI_SUCC_U(byte[] _buffer, ref _S_PJ_TOUCH_MYTEAM_ZOMBI_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_TOUCH_MYTEAM_ZOMBI_FAIL_U(byte[] _buffer, ref _S_PJ_TOUCH_MYTEAM_ZOMBI_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_TOUCH_MYTEAM_ZOMBI_M(byte[] _buffer, ref _S_PJ_TOUCH_MYTEAM_ZOMBI_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.MYTEAMTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.MYTEAMINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.MYTEAMENERGY = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_MAPFLOW_M(byte[] _buffer, ref _S_PJ_MAPFLOW_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.FLOWINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.RADIUS = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_MONSTERDEAD_ITEM_M(byte[] _buffer, ref _S_PJ_MONSTERDEAD_ITEM_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ITEMINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ITEMTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		_data.ITEMPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.ITEMPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ITEMPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.ITEMPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_EAT_ITEM(byte[] _buffer, ref _S_PJ_EAT_ITEM _data)
	{
		MemoryStream ms;
		int index = 0;
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ITEMINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_EAT_ITEM_SUCC_U(byte[] _buffer, ref _S_PJ_EAT_ITEM_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;
		return true;
	}

	public static bool csREAD_PJ_EAT_ITEM_FAIL_U(byte[] _buffer, ref _S_PJ_EAT_ITEM_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_EAT_ITEM_M(byte[] _buffer, ref _S_PJ_EAT_ITEM_M _data)
	{
		MemoryStream ms;
		int index = 0;
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ITEMINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		_data.CURPOS = new csVECTOR2[1];
		for(int i = 0; i < 1; i++)
		{
			_data.CURPOS[i] = new csVECTOR2();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.CURPOS[i].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ITEMTYPE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.ITEMDATA = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_SELECT_CHARACTER(byte[] _buffer, ref _S_PJ_SELECT_CHARACTER _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.CHARACTERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_SELECT_CHARACTER_SUCC_U(byte[] _buffer, ref _S_PJ_SELECT_CHARACTER_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		_data.DATA = new csCHARACTERINFO[1];
		for(int i = 0; i < 1; i++)
		{
			_data.DATA[i] = new csCHARACTERINFO();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwUserindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(char) * 16);
			ms.Write(_buffer, index, sizeof(char) * 16);
			_data.DATA[i].m_strUsername = Encoding.Unicode.GetString(ms.GetBuffer());
			index += sizeof(char) * 16;
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharacterindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharactertype = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			_data.DATA[i].m_Curpos = new csVECTOR2[1];
			for(int j = 0; j < 1; j++)
			{
				_data.DATA[i].m_Curpos[j] = new csVECTOR2();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Curpos[j].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Curpos[j].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
			}
			_data.DATA[i].m_Direction = new csVECTOR2[1];
			for(int j = 0; j < 1; j++)
			{
				_data.DATA[i].m_Direction[j] = new csVECTOR2();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Direction[j].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Direction[j].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
			}
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharstate = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwLasttimestamp = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_iEnergy = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dSpeed = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dPower = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dAttackRange = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dAttackcooltime = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwTeamindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_SELECT_CHARACTER_FAIL_U(byte[] _buffer, ref _S_PJ_SELECT_CHARACTER_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_SELECT_CHARACTER_M(byte[] _buffer, ref _S_PJ_SELECT_CHARACTER_M _data)
	{
		MemoryStream ms;
		int index = 0;

		_data.DATA = new csCHARACTERINFO[1];
		for(int i = 0; i < 1; i++)
		{
			_data.DATA[i] = new csCHARACTERINFO();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwUserindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(char) * 16);
			ms.Write(_buffer, index, sizeof(char) * 16);
			_data.DATA[i].m_strUsername = Encoding.Unicode.GetString(ms.GetBuffer());
			index += sizeof(char) * 16;
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharacterindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharactertype = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			_data.DATA[i].m_Curpos = new csVECTOR2[1];
			for(int j = 0; j < 1; j++)
			{
				_data.DATA[i].m_Curpos[j] = new csVECTOR2();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Curpos[j].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Curpos[j].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
			}
			_data.DATA[i].m_Direction = new csVECTOR2[1];
			for(int j = 0; j < 1; j++)
			{
				_data.DATA[i].m_Direction[j] = new csVECTOR2();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Direction[j].m_dx = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
				ms = new MemoryStream(sizeof(double));
				ms.Write(_buffer, index, sizeof(double));
				_data.DATA[i].m_Direction[j].m_dz = BitConverter.ToDouble(ms.GetBuffer(), 0);
				index += sizeof(double);
				ms.Close();
			}
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwCharstate = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwLasttimestamp = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_iEnergy = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dSpeed = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dPower = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dAttackRange = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.DATA[i].m_dAttackcooltime = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.DATA[i].m_dwTeamindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_SELECT_PET(byte[] _buffer, ref _S_PJ_SELECT_PET _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.PETINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_SELECT_PET_SUCC_U(byte[] _buffer, ref _S_PJ_SELECT_PET_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		_data.PETINFO = new csPETINFO[1];
		for(int i = 0; i < 1; i++)
		{
			_data.PETINFO[i] = new csPETINFO();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.PETINFO[i].m_dwPetindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(char) * 16);
			ms.Write(_buffer, index, sizeof(char) * 16);
			_data.PETINFO[i].m_strPetrname = Encoding.Unicode.GetString(ms.GetBuffer());
			index += sizeof(char) * 16;
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.PETINFO[i].m_iEnergy = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.PETINFO[i].m_dSpeed = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_SELECT_PET_FAIL_U(byte[] _buffer, ref _S_PJ_SELECT_PET_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_SELECT_PET_M(byte[] _buffer, ref _S_PJ_SELECT_PET_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		_data.PETINFO = new csPETINFO[1];
		for(int i = 0; i < 1; i++)
		{
			_data.PETINFO[i] = new csPETINFO();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.PETINFO[i].m_dwPetindex = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(char) * 16);
			ms.Write(_buffer, index, sizeof(char) * 16);
			_data.PETINFO[i].m_strPetrname = Encoding.Unicode.GetString(ms.GetBuffer());
			index += sizeof(char) * 16;
			ms.Close();
			ms = new MemoryStream(sizeof(int));
			ms.Write(_buffer, index, sizeof(int));
			_data.PETINFO[i].m_iEnergy = BitConverter.ToInt32(ms.GetBuffer(), 0);
			index += sizeof(int);
			ms.Close();
			ms = new MemoryStream(sizeof(double));
			ms.Write(_buffer, index, sizeof(double));
			_data.PETINFO[i].m_dSpeed = BitConverter.ToDouble(ms.GetBuffer(), 0);
			index += sizeof(double);
			ms.Close();
		}
		return true;
	}

	public static bool csREAD_PJ_TRANSFORM_PET(byte[] _buffer, ref _S_PJ_TRANSFORM_PET _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ISPET = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_TRANSFORM_PET_SUCC_U(byte[] _buffer, ref _S_PJ_TRANSFORM_PET_SUCC_U _data)
	{
		MemoryStream ms;
		int index = 0;

		return true;
	}

	public static bool csREAD_PJ_TRANSFORM_PET_FAIL_U(byte[] _buffer, ref _S_PJ_TRANSFORM_PET_FAIL_U _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ERROR_CODE = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_TRANSFORM_PET_M(byte[] _buffer, ref _S_PJ_TRANSFORM_PET_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.USERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.ISPET = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.CHARACTERINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.PETINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		return true;
	}

	public static bool csREAD_PJ_KEEPALIVE_M(byte[] _buffer, ref _S_PJ_KEEPALIVE_M _data)
	{
		MemoryStream ms;
		int index = 0;

		ms = new MemoryStream(sizeof(int));
		ms.Write(_buffer, index, sizeof(int));
		_data.TIMEINDEX = BitConverter.ToInt32(ms.GetBuffer(), 0);
		index += sizeof(int);
		ms.Close();
		ms = new MemoryStream(sizeof(double));
		ms.Write(_buffer, index, sizeof(double));
		_data.TIMESTAMP = BitConverter.ToDouble(ms.GetBuffer(), 0);
		index += sizeof(double);
		ms.Close();
		return true;
	}

}
