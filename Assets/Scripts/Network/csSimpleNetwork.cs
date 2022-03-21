using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
2018.07.15
keiwalk
기본 데이터 통신을 위한 네트웍 매니저
*/
public class csSimpleNetwork : csKlnet
{
    public csSimpleNetwork()
    {
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected void EnterRoom(bool isteamgame)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_ENTER_GAMEROOM, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_ENTER_GAMEROOM(ref ms, Convert.ToInt32(isteamgame)));
    }

    protected void LoadingComplete()
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_ENTER_LOADING_COMPLETE, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_ENTER_LOADING_COMPLETE(ref ms));
    }

    protected void MoveStart(int timeindex, float timestamp, csVECTOR2[] curpos, csVECTOR2[] direction)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_MOVE_START, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_MOVE_START(ref ms, timeindex, timestamp, curpos, direction));
    }

    protected void MoveStop(int timeindex, float timestamp, csVECTOR2[] curpos)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_MOVE_STOP, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_MOVE_STOP(ref ms, timeindex, timestamp, curpos));
    }

    protected void ChangedirecetionStart(csVECTOR2[] curpos, csVECTOR2[] direction, csVECTOR2[] rotate)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_CHANGE_DIRECTION_START, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_CHANGE_DIRECTION_START(ref ms, 0.0f, curpos, direction, rotate));
    }

    // 회전을 멈출 때 호출
    protected void ChangedirecetionStop(csVECTOR2[] curpos, csVECTOR2[] direction)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_CHANGE_DIRECTION_STOP, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_CHANGE_DIRECTION_STOP(ref ms, 0.0f, curpos, direction));
    }

    protected void Changedirecetion(int timeindex, float timestamp, csVECTOR2[] curpos, csVECTOR2[] direction)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_CHANGE_DIRECTION, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_CHANGE_DIRECTION(ref ms, timeindex, timestamp, curpos, direction));
    }

    protected void Changeaim(int timeindex, float timestamp, csVECTOR2[] curpos, csVECTOR3[] aim, csVECTOR2[] direction)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_CHANGE_AIM, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_CHANGE_AIM(ref ms, timeindex, timestamp, curpos, aim, direction));
    }

    protected void ShootEnemy(int timeindex, float timestamp, csVECTOR2[] curpos, csVECTOR3[] aim, int enemytype, int enemyindex, int ishit, csVECTOR2[] enemypos, int skilltype)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_SHOOT_ENEMY, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_SHOOT_ENEMY(ref ms, timeindex, timestamp, curpos, aim, enemytype, enemyindex, ishit, enemypos, skilltype));
    }

    protected void HitMe(int timeindex, float timestamp, csVECTOR2[] curpos, int enemytype, int enemyindex)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_HIT_ME, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_HIT_ME(ref ms, timeindex, timestamp, curpos, enemytype, enemyindex));
    }

    protected void HitEnemy(int timeindex, float timestamp, int enemytype, int enemyindex, csVECTOR2[] enemypos, csVECTOR2[] mypos)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_HIT_ENEMY, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_HIT_ENEMY(ref ms, timeindex, timestamp, enemytype, enemyindex, enemypos, mypos));
    }

    protected void TempStartBattle()
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_TEMP_STARTBATTLE, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_TEMP_STARTBATTLE(ref ms));
    }

    protected void TouchMyteam(int timeindex, float timestamp, int myteamtype, int myteamindex)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_TOUCH_MYTEAM_ZOMBI, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_TOUCH_MYTEAM_ZOMBI(ref ms, timeindex, timestamp, myteamtype, myteamindex));
    }

    protected void EatItem(int timeindex, float timestamp, int itemindex, csVECTOR2[] mypos)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_EAT_ITEM, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_EAT_ITEM(ref ms, timeindex, timestamp, itemindex, mypos));
    }

    protected void SelectCharacter(int characterindex)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_SELECT_CHARACTER, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_SELECT_CHARACTER(ref ms, characterindex));
    }

    protected void SelectPet(int petindex)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_SELECT_PET, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_SELECT_PET(ref ms, petindex));
    }

    protected void TransformPet(int ispet)
    {
        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_TRANSFORM_PET, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_TRANSFORM_PET(ref ms, ispet));
    }

    virtual protected void LoginSuccU(_S_PJ_LOGIN_SUCC_U _data) { }
    virtual protected void EnterRoomStartU(_S_PJ_ENTER_GAMEROOM_START_U _data) { }
    virtual protected void EnterRoomU(_S_PJ_ENTER_GAMEROOM_U _data) { }
    virtual protected void EnterRoomEndU(_S_PJ_ENTER_GAMEROOM_END_U _data) { }
    virtual protected void EnterRoomM(_S_PJ_ENTER_GAMEROOM_M _data) { }
    virtual protected void ExitRoomM(_S_PJ_EXIT_GAMEROOM_M _data) { }
    virtual protected void LoadingCompleteSuccU(_S_PJ_ENTER_LOADING_COMPLETE_SUCC_U _data) { }
    virtual protected void BattleroomMonsterinfoM(_S_PJ_BATTLEROOM_MONTERINFO_M _data) { }
    virtual protected void BattleroomCountdownM(_S_PJ_BATTLEROOM_COUNTDOWN_M _data) { }
    virtual protected void BattleroomBattlestartM(_S_PJ_BATTLEROOM_BATTLESTART_M _data) { }
    virtual protected void MoveStartM(_S_PJ_MOVE_START_M _data) { }
    virtual protected void MoveStopM(_S_PJ_MOVE_STOP_M _data) { }
    virtual protected void ChangedirecetionStartM(_S_PJ_CHANGE_DIRECTION_START_M _data) { }
    virtual protected void ChangedirecetionStopM(_S_PJ_CHANGE_DIRECTION_STOP_M _data) { }
    virtual protected void ChangedirecetionM(_S_PJ_CHANGE_DIRECTION_M _data) { }
    virtual protected void ChangeaimM(_S_PJ_CHANGE_AIM_M _data) { }
    virtual protected void ShootEnemyM(_S_PJ_SHOOT_ENEMY_M _data) { }
    virtual protected void BattleroomResultscoreU(_S_PJ_BATTLEROOM_RESULTSCORE_U _data) { }
    virtual protected void HitMeM(_S_PJ_HIT_ME_M _data) { }
    virtual protected void HitEnemyM(_S_PJ_HIT_ENEMY_M _data) { }
    virtual protected void DeadCharacterM(_S_PJ_DEAD_CHARACTER_M _data) { }
    virtual protected void TouchMyteamM(_S_PJ_TOUCH_MYTEAM_ZOMBI_M _data) { }
    virtual protected void MapFlowM(_S_PJ_MAPFLOW_M _data) { }
    virtual protected void MonsterdeadItemM(_S_PJ_MONSTERDEAD_ITEM_M _data) { }
    virtual protected void EatItemM(_S_PJ_EAT_ITEM_M _data) { }
    virtual protected void SelectCharacterM(_S_PJ_SELECT_CHARACTER_M _data) { }
    virtual protected void SelectPetM(_S_PJ_SELECT_PET_M _data) { }
    virtual protected void TransformPetM(_S_PJ_TRANSFORM_PET_M _data) { }
    virtual protected void KeepAliveM(_S_PJ_KEEPALIVE_M _data) { }

    protected override void OnConnected()
    {
        Debug.Log("[OnConnected....]");

        MemoryStream ms = new MemoryStream(csKlnet.DEFAULT_BUFFER_SIZE);
        base.SendPacket((int)ePJProtocolenum.PJ_LOGIN, ms.GetBuffer(), (int)csWRITE_PJProtocol.WRITE_PJ_LOGIN(ref ms, "TEST", "1111", 1.0));
    }

    protected override void OnDisconnected()
    {
        Debug.Log("OnDisconnected..");
    }

    protected override void OnPacketRecv(GamePacket _gp)
    {
//        Debug.Log("[OnPacketRecv....]");
        switch (_gp.Protocol)
        {
            case (int)ePJProtocolenum.PJ_LOGIN_SUCC_U:
                onPJ_LOGIN_SUCC_U(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_ENTER_GAMEROOM_START_U:
                onPJ_ENTER_GAMEROOM_START_U(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_ENTER_GAMEROOM_U:
                onPJ_ENTER_GAMEROOM_U(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_ENTER_GAMEROOM_END_U:
                onPJ_ENTER_GAMEROOM_END_U(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_ENTER_GAMEROOM_M:
                onPJ_ENTER_GAMEROOM_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_EXIT_GAMEROOM_M:
                onPJ_EXIT_GAMEROOM_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_ENTER_LOADING_COMPLETE_SUCC_U:
                onPJ_ENTER_LOADING_COMPLETE_SUCC_U(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_BATTLEROOM_COUNTDOWN_M:
                onPJ_BATTLEROOM_COUNTDOWN_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_BATTLEROOM_MONTERINFO_M:
                onPJ_BATTLEROOM_MONTERINFO_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_BATTLEROOM_BATTLESTART_M:
                onPJ_BATTLEROOM_BATTLESTART_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_MOVE_START_M:
                onPJ_MOVE_START_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_MOVE_STOP_M:
                onPJ_MOVE_STOP_M(_gp.Buffer);
                break;
   /*         case (int)ePJProtocolenum.PJ_CHANGE_DIRECTION_START_M:
                onPJ_CHANGE_DIRECTION_START_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_CHANGE_DIRECTION_STOP_M:
                onPJ_CHANGE_DIRECTION_STOP_M(_gp.Buffer);
                break;*/
            case (int)ePJProtocolenum.PJ_CHANGE_DIRECTION_M:
                onPJ_CHANGE_DIRECTION_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_CHANGE_AIM_M:
                onPJ_CHANGE_AIM_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_SHOOT_ENEMY_M:
                onPJ_SHOOT_ENEMY_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_HIT_ME_M:
                onPJ_HIT_ME_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_HIT_ENEMY_M:
                onPJ_HIT_ENEMY_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_BATTLEROOM_RESULTSCORE_U:
                onPJ_BATTLEROOM_RESULTSCORE_U(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_DEAD_CHARACTER_M:
                onPJ_DEAD_CHARACTER_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_TOUCH_MYTEAM_ZOMBI_M:
                onPJ_TOUCH_MYTEAM_ZOMBI_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_MAPFLOW_M:
                onPJ_MAPFLOW_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_MONSTERDEAD_ITEM_M:
                onPJ_MONSTERDEAD_ITEM_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_EAT_ITEM_M:
                onPJ_EAT_ITEM_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_SELECT_CHARACTER_M:
                onPJ_SELECT_CHARACTER_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_SELECT_PET_M:
                onPJ_SELECT_PET_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_TRANSFORM_PET_M:
                onPJ_TRANSFORM_PET_M(_gp.Buffer);
                break;
            case (int)ePJProtocolenum.PJ_KEEPALIVE_M:
                onPJ_KEEPALIVE_M(_gp.Buffer);
                break;
         }
    }

    void onPJ_LOGIN_SUCC_U(byte[] _buffer)
    {
        _S_PJ_LOGIN_SUCC_U Data = new _S_PJ_LOGIN_SUCC_U();
        csREAD_PJProtocol.csREAD_PJ_LOGIN_SUCC_U(_buffer, ref Data);

        LoginSuccU(Data);
    }

    void onPJ_ENTER_GAMEROOM_START_U(byte[] _buffer)
    {
        _S_PJ_ENTER_GAMEROOM_START_U Data = new _S_PJ_ENTER_GAMEROOM_START_U();
        csREAD_PJProtocol.csREAD_PJ_ENTER_GAMEROOM_START_U(_buffer, ref Data);

        EnterRoomStartU(Data);
    }

    void onPJ_ENTER_GAMEROOM_U(byte[] _buffer)
    {
        _S_PJ_ENTER_GAMEROOM_U Data = new _S_PJ_ENTER_GAMEROOM_U();
        csREAD_PJProtocol.csREAD_PJ_ENTER_GAMEROOM_U(_buffer, ref Data);

        EnterRoomU(Data);
    }

    void onPJ_ENTER_GAMEROOM_END_U(byte[] _buffer)
    {
        _S_PJ_ENTER_GAMEROOM_END_U Data = new _S_PJ_ENTER_GAMEROOM_END_U();
        csREAD_PJProtocol.csREAD_PJ_ENTER_GAMEROOM_END_U(_buffer, ref Data);

        EnterRoomEndU(Data);
    }

    void onPJ_ENTER_GAMEROOM_M(byte[] _buffer)
    {
        _S_PJ_ENTER_GAMEROOM_M Data = new _S_PJ_ENTER_GAMEROOM_M();
        csREAD_PJProtocol.csREAD_PJ_ENTER_GAMEROOM_M(_buffer, ref Data);

        EnterRoomM(Data);
    }

    void onPJ_EXIT_GAMEROOM_M(byte[] _buffer)
    {
        _S_PJ_EXIT_GAMEROOM_M Data = new _S_PJ_EXIT_GAMEROOM_M();
        csREAD_PJProtocol.csREAD_PJ_EXIT_GAMEROOM_M(_buffer, ref Data);

        ExitRoomM(Data);
    }

    void onPJ_ENTER_LOADING_COMPLETE_SUCC_U(byte[] _buffer)
    {
        _S_PJ_ENTER_LOADING_COMPLETE_SUCC_U Data = new _S_PJ_ENTER_LOADING_COMPLETE_SUCC_U();
        csREAD_PJProtocol.csREAD_PJ_ENTER_LOADING_COMPLETE_SUCC_U(_buffer, ref Data);

        LoadingCompleteSuccU(Data);
    }

    void onPJ_BATTLEROOM_MONTERINFO_M(byte[] _buffer)
    {
        _S_PJ_BATTLEROOM_MONTERINFO_M Data = new _S_PJ_BATTLEROOM_MONTERINFO_M();
        csREAD_PJProtocol.csREAD_PJ_BATTLEROOM_MONTERINFO_M(_buffer, ref Data);

        BattleroomMonsterinfoM(Data);
    }

    void onPJ_BATTLEROOM_COUNTDOWN_M(byte[] _buffer)
    {
        _S_PJ_BATTLEROOM_COUNTDOWN_M Data = new _S_PJ_BATTLEROOM_COUNTDOWN_M();
        csREAD_PJProtocol.csREAD_PJ_BATTLEROOM_COUNTDOWN_M(_buffer, ref Data);

        BattleroomCountdownM(Data);
    }

    void onPJ_BATTLEROOM_BATTLESTART_M(byte[] _buffer)
    {
        _S_PJ_BATTLEROOM_BATTLESTART_M Data = new _S_PJ_BATTLEROOM_BATTLESTART_M();
        csREAD_PJProtocol.csREAD_PJ_BATTLEROOM_BATTLESTART_M(_buffer, ref Data);

//        m_uiTimestamp = (uint)Data.TIMESTAMP;

        BattleroomBattlestartM(Data);
    }

    void onPJ_MOVE_START_M(byte[] _buffer)
    {
        _S_PJ_MOVE_START_M Data = new _S_PJ_MOVE_START_M();
        csREAD_PJProtocol.csREAD_PJ_MOVE_START_M(_buffer, ref Data);

        MoveStartM(Data);
    }

    void onPJ_MOVE_STOP_M(byte[] _buffer)
    {
        _S_PJ_MOVE_STOP_M Data = new _S_PJ_MOVE_STOP_M();
        csREAD_PJProtocol.csREAD_PJ_MOVE_STOP_M(_buffer, ref Data);

        MoveStopM(Data);
    }

    void onPJ_CHANGE_DIRECTION_START_M(byte[] _buffer)
    {
        _S_PJ_CHANGE_DIRECTION_START_M Data = new _S_PJ_CHANGE_DIRECTION_START_M();
        csREAD_PJProtocol.csREAD_PJ_CHANGE_DIRECTION_START_M(_buffer, ref Data);

        ChangedirecetionStartM(Data);
    }

    void onPJ_CHANGE_DIRECTION_STOP_M(byte[] _buffer)
    {
        _S_PJ_CHANGE_DIRECTION_STOP_M Data = new _S_PJ_CHANGE_DIRECTION_STOP_M();
        csREAD_PJProtocol.csREAD_PJ_CHANGE_DIRECTION_STOP_M(_buffer, ref Data);

        ChangedirecetionStopM(Data);
    }

    void onPJ_CHANGE_DIRECTION_M(byte[] _buffer)
    {
        _S_PJ_CHANGE_DIRECTION_M Data = new _S_PJ_CHANGE_DIRECTION_M();
        csREAD_PJProtocol.csREAD_PJ_CHANGE_DIRECTION_M(_buffer, ref Data);

        ChangedirecetionM(Data);
    }

    void onPJ_CHANGE_AIM_M(byte[] _buffer)
    {
        _S_PJ_CHANGE_AIM_M Data = new _S_PJ_CHANGE_AIM_M();
        csREAD_PJProtocol.csREAD_PJ_CHANGE_AIM_M(_buffer, ref Data);

        ChangeaimM(Data);
    }

    void onPJ_SHOOT_ENEMY_M(byte[] _buffer)
    {
        _S_PJ_SHOOT_ENEMY_M Data = new _S_PJ_SHOOT_ENEMY_M();
        csREAD_PJProtocol.csREAD_PJ_SHOOT_ENEMY_M(_buffer, ref Data);

        ShootEnemyM(Data);
    }

    void onPJ_HIT_ME_M(byte[] _buffer)
    {
        _S_PJ_HIT_ME_M Data = new _S_PJ_HIT_ME_M();
        csREAD_PJProtocol.csREAD_PJ_HIT_ME_M(_buffer, ref Data);

        HitMeM(Data);
    }

    void onPJ_HIT_ENEMY_M(byte[] _buffer)
    {
        _S_PJ_HIT_ENEMY_M Data = new _S_PJ_HIT_ENEMY_M();
        csREAD_PJProtocol.csREAD_PJ_HIT_ENEMY_M(_buffer, ref Data);

        HitEnemyM(Data);
    }

    void onPJ_BATTLEROOM_RESULTSCORE_U(byte[] _buffer)
    {
        _S_PJ_BATTLEROOM_RESULTSCORE_U Data = new _S_PJ_BATTLEROOM_RESULTSCORE_U();
        csREAD_PJProtocol.csREAD_PJ_BATTLEROOM_RESULTSCORE_U(_buffer, ref Data);

        BattleroomResultscoreU(Data);
    }

    void onPJ_DEAD_CHARACTER_M(byte[] _buffer)
    {
        _S_PJ_DEAD_CHARACTER_M Data = new _S_PJ_DEAD_CHARACTER_M();
        csREAD_PJProtocol.csREAD_PJ_DEAD_CHARACTER_M(_buffer, ref Data);

        DeadCharacterM(Data);
    }

    void onPJ_TOUCH_MYTEAM_ZOMBI_M(byte[] _buffer)
    {
        _S_PJ_TOUCH_MYTEAM_ZOMBI_M Data = new _S_PJ_TOUCH_MYTEAM_ZOMBI_M();
        csREAD_PJProtocol.csREAD_PJ_TOUCH_MYTEAM_ZOMBI_M(_buffer, ref Data);

        TouchMyteamM(Data);
    }

    void onPJ_MAPFLOW_M(byte[] _buffer)
    {
        _S_PJ_MAPFLOW_M Data = new _S_PJ_MAPFLOW_M();
        csREAD_PJProtocol.csREAD_PJ_MAPFLOW_M(_buffer, ref Data);

        MapFlowM(Data);
    }

    void onPJ_MONSTERDEAD_ITEM_M(byte[] _buffer)
    {
        _S_PJ_MONSTERDEAD_ITEM_M Data = new _S_PJ_MONSTERDEAD_ITEM_M();
        csREAD_PJProtocol.csREAD_PJ_MONSTERDEAD_ITEM_M(_buffer, ref Data);

        MonsterdeadItemM(Data);
    }

    void onPJ_EAT_ITEM_M(byte[] _buffer)
    {
        _S_PJ_EAT_ITEM_M Data = new _S_PJ_EAT_ITEM_M();
        csREAD_PJProtocol.csREAD_PJ_EAT_ITEM_M(_buffer, ref Data);

        EatItemM(Data);
    }

    void onPJ_SELECT_CHARACTER_M(byte[] _buffer)
    {
        _S_PJ_SELECT_CHARACTER_M Data = new _S_PJ_SELECT_CHARACTER_M();
        csREAD_PJProtocol.csREAD_PJ_SELECT_CHARACTER_M(_buffer, ref Data);

        SelectCharacterM(Data);
    }

    void onPJ_SELECT_PET_M(byte[] _buffer)
    {
        _S_PJ_SELECT_PET_M Data = new _S_PJ_SELECT_PET_M();
        csREAD_PJProtocol.csREAD_PJ_SELECT_PET_M(_buffer, ref Data);

        SelectPetM(Data);
    }

    void onPJ_TRANSFORM_PET_M(byte[] _buffer)
    {
        _S_PJ_TRANSFORM_PET_M Data = new _S_PJ_TRANSFORM_PET_M();
        csREAD_PJProtocol.csREAD_PJ_TRANSFORM_PET_M(_buffer, ref Data);

        TransformPetM(Data);
    }

    void onPJ_KEEPALIVE_M(byte[] _buffer)
    {
        _S_PJ_KEEPALIVE_M Data = new _S_PJ_KEEPALIVE_M();
        csREAD_PJProtocol.csREAD_PJ_KEEPALIVE_M(_buffer, ref Data);

        KeepAliveM(Data);
    }
}
