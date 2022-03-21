using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csWRITE_PJProtocol : MonoBehaviour
{
	static public long WRITE_PJ_LOGIN(ref MemoryStream _ms, string ID, string PASSWORD, double VERSION)
	{
		int index = 0;

		_ms.Write(System.Text.Encoding.Unicode.GetBytes(ID), 0, sizeof(char) * ID.Length);
		index += sizeof(char) * 16;
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(System.Text.Encoding.Unicode.GetBytes(PASSWORD), 0, sizeof(char) * PASSWORD.Length);
		index += sizeof(char) * 16;
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(VERSION), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_LOGIN_SUCC_U(ref MemoryStream _ms, string ID, string NAME, int USERINDEX)
	{
		int index = 0;

		_ms.Write(System.Text.Encoding.Unicode.GetBytes(ID), 0, sizeof(char) * ID.Length);
		index += sizeof(char) * 16;
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(System.Text.Encoding.Unicode.GetBytes(NAME), 0, sizeof(char) * NAME.Length);
		index += sizeof(char) * 16;
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(USERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_LOGIN_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_ENTER_GAMEROOM(ref MemoryStream _ms, int ISTEAMGAME)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ISTEAMGAME), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_ENTER_GAMEROOM_START_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_ENTER_GAMEROOM_U(ref MemoryStream _ms, int COUNT, csCHARACTERINFO[] DATA, csPETINFO[] PETINFO)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(COUNT), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < COUNT; i++)
		{
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwUserindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(System.Text.Encoding.Unicode.GetBytes(DATA[i].m_strUsername), 0, sizeof(char) * DATA[i].m_strUsername.Length);
			index += sizeof(char) * 16;
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharacterindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharactertype), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			for(int j = 0; j < 1; j++)
			{
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Curpos[j].m_dx), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Curpos[j].m_dz), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
			}
			for(int j = 0; j < 1; j++)
			{
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Direction[j].m_dx), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Direction[j].m_dz), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
			}
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharstate), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwLasttimestamp), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_iEnergy), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dSpeed), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dPower), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dAttackRange), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dAttackcooltime), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwTeamindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < COUNT; i++)
		{
			_ms.Write(BitConverter.GetBytes(PETINFO[i].m_dwPetindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(System.Text.Encoding.Unicode.GetBytes(PETINFO[i].m_strPetrname), 0, sizeof(char) * PETINFO[i].m_strPetrname.Length);
			index += sizeof(char) * 16;
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(PETINFO[i].m_iEnergy), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(PETINFO[i].m_dSpeed), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_ENTER_GAMEROOM_END_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_ENTER_GAMEROOM_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_ENTER_GAMEROOM_M(ref MemoryStream _ms, csCHARACTERINFO[] DATA, csPETINFO[] PETINFO)
	{
		int index = 0;

		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwUserindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(System.Text.Encoding.Unicode.GetBytes(DATA[i].m_strUsername), 0, sizeof(char) * DATA[i].m_strUsername.Length);
			index += sizeof(char) * 16;
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharacterindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharactertype), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			for(int j = 0; j < 1; j++)
			{
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Curpos[j].m_dx), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Curpos[j].m_dz), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
			}
			for(int j = 0; j < 1; j++)
			{
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Direction[j].m_dx), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Direction[j].m_dz), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
			}
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharstate), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwLasttimestamp), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_iEnergy), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dSpeed), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dPower), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dAttackRange), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dAttackcooltime), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwTeamindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(PETINFO[i].m_dwPetindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(System.Text.Encoding.Unicode.GetBytes(PETINFO[i].m_strPetrname), 0, sizeof(char) * PETINFO[i].m_strPetrname.Length);
			index += sizeof(char) * 16;
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(PETINFO[i].m_iEnergy), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(PETINFO[i].m_dSpeed), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_EXIT_GAMEROOM_M(ref MemoryStream _ms, int USERINDEX)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(USERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_ENTER_LOADING_COMPLETE(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_ENTER_LOADING_COMPLETE_SUCC_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_ENTER_LOADING_COMPLETE_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_BATTLEROOM_COUNTDOWN_M(ref MemoryStream _ms, int SECOND)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(SECOND), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_MOVETO_BATTLEROMM(ref MemoryStream _ms, int COUNT, csCHARACTERINFO[] DATA)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(COUNT), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < COUNT; i++)
		{
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwUserindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(System.Text.Encoding.Unicode.GetBytes(DATA[i].m_strUsername), 0, sizeof(char) * DATA[i].m_strUsername.Length);
			index += sizeof(char) * 16;
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharacterindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharactertype), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			for(int j = 0; j < 1; j++)
			{
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Curpos[j].m_dx), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Curpos[j].m_dz), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
			}
			for(int j = 0; j < 1; j++)
			{
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Direction[j].m_dx), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Direction[j].m_dz), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
			}
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharstate), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwLasttimestamp), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_iEnergy), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dSpeed), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dPower), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dAttackRange), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dAttackcooltime), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwTeamindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_BATTLEROOM_MONTERINFO_M(ref MemoryStream _ms, int COUNT, csCHARACTERINFO[] MONSTERS)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(COUNT), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < COUNT; i++)
		{
			_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_dwUserindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(System.Text.Encoding.Unicode.GetBytes(MONSTERS[i].m_strUsername), 0, sizeof(char) * MONSTERS[i].m_strUsername.Length);
			index += sizeof(char) * 16;
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_dwCharacterindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_dwCharactertype), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			for(int j = 0; j < 1; j++)
			{
				_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_Curpos[j].m_dx), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
				_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_Curpos[j].m_dz), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
			}
			for(int j = 0; j < 1; j++)
			{
				_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_Direction[j].m_dx), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
				_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_Direction[j].m_dz), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
			}
			_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_dwCharstate), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_dwLasttimestamp), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_iEnergy), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_dSpeed), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_dPower), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_dAttackRange), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_dAttackcooltime), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(MONSTERS[i].m_dwTeamindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_BATTLEROOM_BATTLESTART_M(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_MOVE_START(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP, csVECTOR2[] CURPOS, csVECTOR2[] DIRECTION)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_MOVE_START_SUCC_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_MOVE_START_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_MOVE_START_M(ref MemoryStream _ms, int USERTYPE, int USERINDEX, int TIMEINDEX, double TIMESTAMP, csVECTOR2[] CURPOS, csVECTOR2[] DIRECTION, int MONSTERACT)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(USERTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(USERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		_ms.Write(BitConverter.GetBytes(MONSTERACT), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_MOVE_STOP(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP, csVECTOR2[] CURPOS)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_MOVE_STOP_SUCC_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_MOVE_STOP_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_MOVE_STOP_M(ref MemoryStream _ms, int USERTYPE, int USERINDEX, int TIMEINDEX, double TIMESTAMP, csVECTOR2[] CURPOS, int MONSTERACT)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(USERTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(USERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		_ms.Write(BitConverter.GetBytes(MONSTERACT), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_DIRECTION_START(ref MemoryStream _ms, double TIMESTAMP, csVECTOR2[] CURPOS, csVECTOR2[] DIRECTION, csVECTOR2[] ROTATE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(ROTATE[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(ROTATE[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_DIRECTION_START_SUCC_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_DIRECTION_START_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_DIRECTION_START_M(ref MemoryStream _ms, int USERINDEX, double TIMESTAMP, csVECTOR2[] CURPOS, csVECTOR2[] DIRECTION, csVECTOR2[] ROTATE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(USERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(ROTATE[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(ROTATE[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_DIRECTION_STOP(ref MemoryStream _ms, double TIMESTAMP, csVECTOR2[] CURPOS, csVECTOR2[] DIRECTION)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_DIRECTION_STOP_SUCC_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_DIRECTION_STOP_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_DIRECTION_STOP_M(ref MemoryStream _ms, int USERINDEX, double TIMESTAMP, csVECTOR2[] CURPOS, csVECTOR2[] DIRECTION)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(USERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_DIRECTION(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP, csVECTOR2[] CURPOS, csVECTOR2[] DIRECTION)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_DIRECTION_SUCC_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_DIRECTION_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_DIRECTION_M(ref MemoryStream _ms, int USERINDEX, int TIMEINDEX, double TIMESTAMP, csVECTOR2[] CURPOS, csVECTOR2[] DIRECTION)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(USERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_AIM(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP, csVECTOR2[] CURPOS, csVECTOR3[] AIM, csVECTOR2[] DIRECTION)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(AIM[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(AIM[i].m_dy), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(AIM[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_AIM_SUCC_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_AIM_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_CHANGE_AIM_M(ref MemoryStream _ms, int USERINDEX, int TIMEINDEX, double TIMESTAMP, csVECTOR2[] CURPOS, csVECTOR3[] AIM, csVECTOR2[] DIRECTION)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(USERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(AIM[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(AIM[i].m_dy), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(AIM[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DIRECTION[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_SHOOT_ENEMY(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP, csVECTOR2[] CURPOS, csVECTOR3[] AIM, int ENEMYTYPE, int ENEMYINDEX, int ISHIT, csVECTOR2[] ENEMYPOS, int SKILLTYPE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(AIM[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(AIM[i].m_dy), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(AIM[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		_ms.Write(BitConverter.GetBytes(ENEMYTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ENEMYINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ISHIT), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(ENEMYPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(ENEMYPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		_ms.Write(BitConverter.GetBytes(SKILLTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_SHOOT_ENEMY_SUCC_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_SHOOT_ENEMY_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_SHOOT_ENEMY_M(ref MemoryStream _ms, int USERTYPE, int USERINDEX, int TIMEINDEX, double TIMESTAMP, csVECTOR2[] CURPOS, csVECTOR3[] AIM, int ENEMYTYPE, int ENEMYINDEX, int ENEMYENERGY, int ISHIT, csVECTOR2[] ENEMYPOS, int SKILLTYPE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(USERTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(USERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(AIM[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(AIM[i].m_dy), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(AIM[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		_ms.Write(BitConverter.GetBytes(ENEMYTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ENEMYINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ENEMYENERGY), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ISHIT), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(ENEMYPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(ENEMYPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		_ms.Write(BitConverter.GetBytes(SKILLTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_HIT_ME(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP, csVECTOR2[] CURPOS, int ENEMYTYPE, int ENEMYINDEX)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		_ms.Write(BitConverter.GetBytes(ENEMYTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ENEMYINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_HIT_ME_SUCC_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_HIT_ME_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_HIT_ME_M(ref MemoryStream _ms, int USERINDEX, int TIMEINDEX, double TIMESTAMP, csVECTOR2[] CURPOS, int USERENERGY, int ENEMYTYPE, int ENEMYINDEX)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(USERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		_ms.Write(BitConverter.GetBytes(USERENERGY), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ENEMYTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ENEMYINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_HIT_ENEMY(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP, int ENEMYTYPE, int ENEMYINDEX, csVECTOR2[] ENEMYPOS, csVECTOR2[] MYCURPOS)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ENEMYTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ENEMYINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(ENEMYPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(ENEMYPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(MYCURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(MYCURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_HIT_ENEMY_SUCC_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_HIT_ENEMY_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_HIT_ENEMY_M(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP, int ENEMYTYPE, int ENEMYINDEX, int ENEMYENERGY, csVECTOR2[] ENEMYPOS)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ENEMYTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ENEMYINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ENEMYENERGY), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(ENEMYPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(ENEMYPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_BATTLEROOM_RESULTSCORE_U(ref MemoryStream _ms, int ISWIN, int KILLCOUNT, int RANKING, int TOTALDAMAGE, int SURVIVALTIME)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ISWIN), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(KILLCOUNT), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(RANKING), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TOTALDAMAGE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(SURVIVALTIME), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_DEAD_CHARACTER_M(ref MemoryStream _ms, int DEADTYPE, int DEADINDEX, int KILLERTYPE, int KILLERINDEX)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(DEADTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(DEADINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(KILLERTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(KILLERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_TEMP_STARTBATTLE(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_TOUCH_MYTEAM_ZOMBI(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP, int MYTEAMTYPE, int MYTEAMINDEX)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(MYTEAMTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(MYTEAMINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_TOUCH_MYTEAM_ZOMBI_SUCC_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_TOUCH_MYTEAM_ZOMBI_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_TOUCH_MYTEAM_ZOMBI_M(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP, int MYTEAMTYPE, int MYTEAMINDEX, int MYTEAMENERGY)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(MYTEAMTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(MYTEAMINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(MYTEAMENERGY), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_MAPFLOW_M(ref MemoryStream _ms, int FLOWINDEX, double RADIUS)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(FLOWINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(RADIUS), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_MONSTERDEAD_ITEM_M(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP, int ITEMINDEX, int ITEMTYPE, csVECTOR2[] ITEMPOS)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ITEMINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ITEMTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(ITEMPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(ITEMPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_EAT_ITEM(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP, int ITEMINDEX, csVECTOR2[] CURPOS)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ITEMINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_EAT_ITEM_SUCC_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_EAT_ITEM_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_EAT_ITEM_M(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP, int ITEMINDEX, int USERINDEX, csVECTOR2[] CURPOS, int ITEMTYPE, double ITEMDATA)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ITEMINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(USERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dx), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(CURPOS[i].m_dz), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}
		_ms.Write(BitConverter.GetBytes(ITEMTYPE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ITEMDATA), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_SELECT_CHARACTER(ref MemoryStream _ms, int CHARACTERINDEX)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(CHARACTERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_SELECT_CHARACTER_SUCC_U(ref MemoryStream _ms, csCHARACTERINFO[] DATA)
	{
		int index = 0;

		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwUserindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(System.Text.Encoding.Unicode.GetBytes(DATA[i].m_strUsername), 0, sizeof(char) * DATA[i].m_strUsername.Length);
			index += sizeof(char) * 16;
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharacterindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharactertype), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			for(int j = 0; j < 1; j++)
			{
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Curpos[j].m_dx), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Curpos[j].m_dz), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
			}
			for(int j = 0; j < 1; j++)
			{
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Direction[j].m_dx), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Direction[j].m_dz), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
			}
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharstate), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwLasttimestamp), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_iEnergy), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dSpeed), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dPower), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dAttackRange), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dAttackcooltime), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwTeamindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_SELECT_CHARACTER_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_SELECT_CHARACTER_M(ref MemoryStream _ms, csCHARACTERINFO[] DATA)
	{
		int index = 0;

		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwUserindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(System.Text.Encoding.Unicode.GetBytes(DATA[i].m_strUsername), 0, sizeof(char) * DATA[i].m_strUsername.Length);
			index += sizeof(char) * 16;
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharacterindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharactertype), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			for(int j = 0; j < 1; j++)
			{
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Curpos[j].m_dx), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Curpos[j].m_dz), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
			}
			for(int j = 0; j < 1; j++)
			{
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Direction[j].m_dx), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
				_ms.Write(BitConverter.GetBytes(DATA[i].m_Direction[j].m_dz), 0, sizeof(double));
				index += sizeof(double);
				_ms.Seek(index, SeekOrigin.Begin);
			}
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwCharstate), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwLasttimestamp), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_iEnergy), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dSpeed), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dPower), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dAttackRange), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dAttackcooltime), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(DATA[i].m_dwTeamindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_SELECT_PET(ref MemoryStream _ms, int PETINDEX)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(PETINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_SELECT_PET_SUCC_U(ref MemoryStream _ms, csPETINFO[] PETINFO)
	{
		int index = 0;

		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(PETINFO[i].m_dwPetindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(System.Text.Encoding.Unicode.GetBytes(PETINFO[i].m_strPetrname), 0, sizeof(char) * PETINFO[i].m_strPetrname.Length);
			index += sizeof(char) * 16;
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(PETINFO[i].m_iEnergy), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(PETINFO[i].m_dSpeed), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_SELECT_PET_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_SELECT_PET_M(ref MemoryStream _ms, int USERINDEX, csPETINFO[] PETINFO)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(USERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		for(int i = 0; i < 1; i++)
		{
			_ms.Write(BitConverter.GetBytes(PETINFO[i].m_dwPetindex), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(System.Text.Encoding.Unicode.GetBytes(PETINFO[i].m_strPetrname), 0, sizeof(char) * PETINFO[i].m_strPetrname.Length);
			index += sizeof(char) * 16;
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(PETINFO[i].m_iEnergy), 0, sizeof(int));
			index += sizeof(int);
			_ms.Seek(index, SeekOrigin.Begin);
			_ms.Write(BitConverter.GetBytes(PETINFO[i].m_dSpeed), 0, sizeof(double));
			index += sizeof(double);
			_ms.Seek(index, SeekOrigin.Begin);
		}

		return _ms.Position;
	}

	static public long WRITE_PJ_TRANSFORM_PET(ref MemoryStream _ms, int ISPET)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ISPET), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_TRANSFORM_PET_SUCC_U(ref MemoryStream _ms)
	{
		int index = 0;


		return _ms.Position;
	}

	static public long WRITE_PJ_TRANSFORM_PET_FAIL_U(ref MemoryStream _ms, int ERROR_CODE)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(ERROR_CODE), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_TRANSFORM_PET_M(ref MemoryStream _ms, int USERINDEX, int ISPET, int CHARACTERINDEX, int PETINDEX)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(USERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(ISPET), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(CHARACTERINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(PETINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

	static public long WRITE_PJ_KEEPALIVE_M(ref MemoryStream _ms, int TIMEINDEX, double TIMESTAMP)
	{
		int index = 0;

		_ms.Write(BitConverter.GetBytes(TIMEINDEX), 0, sizeof(int));
		index += sizeof(int);
		_ms.Seek(index, SeekOrigin.Begin);
		_ms.Write(BitConverter.GetBytes(TIMESTAMP), 0, sizeof(double));
		index += sizeof(double);
		_ms.Seek(index, SeekOrigin.Begin);

		return _ms.Position;
	}

}
