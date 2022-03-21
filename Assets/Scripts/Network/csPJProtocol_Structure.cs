using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct csVECTOR2
{
	public double m_dx;
	public double m_dz;
}
public struct csVECTOR3
{
	public double m_dx;
	public double m_dy;
	public double m_dz;
}
public struct csCHARACTERINFO
{
	public int m_dwUserindex;
	public string m_strUsername;
	public int m_dwCharacterindex;
	public int m_dwCharactertype;
	public csVECTOR2[] m_Curpos;
	public csVECTOR2[] m_Direction;
	public int m_dwCharstate;
	public int m_dwLasttimestamp;
	public int m_iEnergy;
	public double m_dSpeed;
	public double m_dPower;
	public double m_dAttackRange;
	public double m_dAttackcooltime;
	public int m_dwTeamindex;
}
public struct csPETINFO
{
	public int m_dwPetindex;
	public string m_strPetrname;
	public int m_iEnergy;
	public double m_dSpeed;
}
public struct _S_PJ_LOGIN
{
	public string ID;
	public string PASSWORD;
	public double VERSION;
}
public struct _S_PJ_LOGIN_SUCC_U
{
	public string ID;
	public string NAME;
	public int USERINDEX;
}
public struct _S_PJ_LOGIN_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_ENTER_GAMEROOM
{
	public int ISTEAMGAME;
}
public struct _S_PJ_ENTER_GAMEROOM_START_U
{
}
public struct _S_PJ_ENTER_GAMEROOM_U
{
	public int COUNT;
	public csCHARACTERINFO[] DATA;
	public csPETINFO[] PETINFO;
}
public struct _S_PJ_ENTER_GAMEROOM_END_U
{
}
public struct _S_PJ_ENTER_GAMEROOM_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_ENTER_GAMEROOM_M
{
	public csCHARACTERINFO[] DATA;
	public csPETINFO[] PETINFO;
}
public struct _S_PJ_EXIT_GAMEROOM_M
{
	public int USERINDEX;
}
public struct _S_PJ_ENTER_LOADING_COMPLETE
{
}
public struct _S_PJ_ENTER_LOADING_COMPLETE_SUCC_U
{
}
public struct _S_PJ_ENTER_LOADING_COMPLETE_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_BATTLEROOM_COUNTDOWN_M
{
	public int SECOND;
}
public struct _S_PJ_MOVETO_BATTLEROMM
{
	public int COUNT;
	public csCHARACTERINFO[] DATA;
}
public struct _S_PJ_BATTLEROOM_MONTERINFO_M
{
	public int COUNT;
	public csCHARACTERINFO[] MONSTERS;
}
public struct _S_PJ_BATTLEROOM_BATTLESTART_M
{
	public int TIMEINDEX;
	public double TIMESTAMP;
}
public struct _S_PJ_MOVE_START
{
	public int TIMEINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public csVECTOR2[] DIRECTION;
}
public struct _S_PJ_MOVE_START_SUCC_U
{
}
public struct _S_PJ_MOVE_START_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_MOVE_START_M
{
	public int USERTYPE;
	public int USERINDEX;
	public int TIMEINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public csVECTOR2[] DIRECTION;
	public int MONSTERACT;
}
public struct _S_PJ_MOVE_STOP
{
	public int TIMEINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
}
public struct _S_PJ_MOVE_STOP_SUCC_U
{
}
public struct _S_PJ_MOVE_STOP_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_MOVE_STOP_M
{
	public int USERTYPE;
	public int USERINDEX;
	public int TIMEINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public int MONSTERACT;
}
public struct _S_PJ_CHANGE_DIRECTION_START
{
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public csVECTOR2[] DIRECTION;
	public csVECTOR2[] ROTATE;
}
public struct _S_PJ_CHANGE_DIRECTION_START_SUCC_U
{
}
public struct _S_PJ_CHANGE_DIRECTION_START_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_CHANGE_DIRECTION_START_M
{
	public int USERINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public csVECTOR2[] DIRECTION;
	public csVECTOR2[] ROTATE;
}
public struct _S_PJ_CHANGE_DIRECTION_STOP
{
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public csVECTOR2[] DIRECTION;
}
public struct _S_PJ_CHANGE_DIRECTION_STOP_SUCC_U
{
}
public struct _S_PJ_CHANGE_DIRECTION_STOP_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_CHANGE_DIRECTION_STOP_M
{
	public int USERINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public csVECTOR2[] DIRECTION;
}
public struct _S_PJ_CHANGE_DIRECTION
{
	public int TIMEINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public csVECTOR2[] DIRECTION;
}
public struct _S_PJ_CHANGE_DIRECTION_SUCC_U
{
}
public struct _S_PJ_CHANGE_DIRECTION_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_CHANGE_DIRECTION_M
{
	public int USERINDEX;
	public int TIMEINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public csVECTOR2[] DIRECTION;
}
public struct _S_PJ_CHANGE_AIM
{
	public int TIMEINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public csVECTOR3[] AIM;
	public csVECTOR2[] DIRECTION;
}
public struct _S_PJ_CHANGE_AIM_SUCC_U
{
}
public struct _S_PJ_CHANGE_AIM_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_CHANGE_AIM_M
{
	public int USERINDEX;
	public int TIMEINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public csVECTOR3[] AIM;
	public csVECTOR2[] DIRECTION;
}
public struct _S_PJ_SHOOT_ENEMY
{
	public int TIMEINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public csVECTOR3[] AIM;
	public int ENEMYTYPE;
	public int ENEMYINDEX;
	public int ISHIT;
	public csVECTOR2[] ENEMYPOS;
	public int SKILLTYPE;
}
public struct _S_PJ_SHOOT_ENEMY_SUCC_U
{
}
public struct _S_PJ_SHOOT_ENEMY_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_SHOOT_ENEMY_M
{
	public int USERTYPE;
	public int USERINDEX;
	public int TIMEINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public csVECTOR3[] AIM;
	public int ENEMYTYPE;
	public int ENEMYINDEX;
	public int ENEMYENERGY;
	public int ISHIT;
	public csVECTOR2[] ENEMYPOS;
	public int SKILLTYPE;
}
public struct _S_PJ_HIT_ME
{
	public int TIMEINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public int ENEMYTYPE;
	public int ENEMYINDEX;
}
public struct _S_PJ_HIT_ME_SUCC_U
{
}
public struct _S_PJ_HIT_ME_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_HIT_ME_M
{
	public int USERINDEX;
	public int TIMEINDEX;
	public double TIMESTAMP;
	public csVECTOR2[] CURPOS;
	public int USERENERGY;
	public int ENEMYTYPE;
	public int ENEMYINDEX;
}
public struct _S_PJ_HIT_ENEMY
{
	public int TIMEINDEX;
	public double TIMESTAMP;
	public int ENEMYTYPE;
	public int ENEMYINDEX;
	public csVECTOR2[] ENEMYPOS;
	public csVECTOR2[] MYCURPOS;
}
public struct _S_PJ_HIT_ENEMY_SUCC_U
{
}
public struct _S_PJ_HIT_ENEMY_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_HIT_ENEMY_M
{
	public int TIMEINDEX;
	public double TIMESTAMP;
	public int ENEMYTYPE;
	public int ENEMYINDEX;
	public int ENEMYENERGY;
	public csVECTOR2[] ENEMYPOS;
}
public struct _S_PJ_BATTLEROOM_RESULTSCORE_U
{
	public int ISWIN;
	public int KILLCOUNT;
	public int RANKING;
	public int TOTALDAMAGE;
	public int SURVIVALTIME;
}
public struct _S_PJ_DEAD_CHARACTER_M
{
	public int DEADTYPE;
	public int DEADINDEX;
	public int KILLERTYPE;
	public int KILLERINDEX;
}
public struct _S_PJ_TEMP_STARTBATTLE
{
}
public struct _S_PJ_TOUCH_MYTEAM_ZOMBI
{
	public int TIMEINDEX;
	public double TIMESTAMP;
	public int MYTEAMTYPE;
	public int MYTEAMINDEX;
}
public struct _S_PJ_TOUCH_MYTEAM_ZOMBI_SUCC_U
{
}
public struct _S_PJ_TOUCH_MYTEAM_ZOMBI_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_TOUCH_MYTEAM_ZOMBI_M
{
	public int TIMEINDEX;
	public double TIMESTAMP;
	public int MYTEAMTYPE;
	public int MYTEAMINDEX;
	public int MYTEAMENERGY;
}
public struct _S_PJ_MAPFLOW_M
{
	public int FLOWINDEX;
	public double RADIUS;
}
public struct _S_PJ_MONSTERDEAD_ITEM_M
{
	public int TIMEINDEX;
	public double TIMESTAMP;
	public int ITEMINDEX;
	public int ITEMTYPE;
	public csVECTOR2[] ITEMPOS;
}
public struct _S_PJ_EAT_ITEM
{
	public int TIMEINDEX;
	public double TIMESTAMP;
	public int ITEMINDEX;
	public csVECTOR2[] CURPOS;
}
public struct _S_PJ_EAT_ITEM_SUCC_U
{
}
public struct _S_PJ_EAT_ITEM_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_EAT_ITEM_M
{
	public int TIMEINDEX;
	public double TIMESTAMP;
	public int ITEMINDEX;
	public int USERINDEX;
	public csVECTOR2[] CURPOS;
	public int ITEMTYPE;
	public double ITEMDATA;
}
public struct _S_PJ_SELECT_CHARACTER
{
	public int CHARACTERINDEX;
}
public struct _S_PJ_SELECT_CHARACTER_SUCC_U
{
	public csCHARACTERINFO[] DATA;
}
public struct _S_PJ_SELECT_CHARACTER_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_SELECT_CHARACTER_M
{
	public csCHARACTERINFO[] DATA;
}
public struct _S_PJ_SELECT_PET
{
	public int PETINDEX;
}
public struct _S_PJ_SELECT_PET_SUCC_U
{
	public csPETINFO[] PETINFO;
}
public struct _S_PJ_SELECT_PET_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_SELECT_PET_M
{
	public int USERINDEX;
	public csPETINFO[] PETINFO;
}
public struct _S_PJ_TRANSFORM_PET
{
	public int ISPET;
}
public struct _S_PJ_TRANSFORM_PET_SUCC_U
{
}
public struct _S_PJ_TRANSFORM_PET_FAIL_U
{
	public int ERROR_CODE;
}
public struct _S_PJ_TRANSFORM_PET_M
{
	public int USERINDEX;
	public int ISPET;
	public int CHARACTERINDEX;
	public int PETINDEX;
}
public struct _S_PJ_KEEPALIVE_M
{
	public int TIMEINDEX;
	public double TIMESTAMP;
}
