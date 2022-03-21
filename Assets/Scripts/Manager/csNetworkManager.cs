using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
2018.07.14
keiwalk
게임 클라이언트와 서버간의 데이터 교환을 하는 인터페이스
*/
public class csNetworkManager : csSimpleNetwork
{
    public GameObject m_BattleManager;

    public csNetworkManager()
    {
    }

    protected override void LoginSuccU(_S_PJ_LOGIN_SUCC_U _data)
    {
        Debug.Log("S_PJ_LOGIN_SUCC_U]");
        m_BattleManager.SendMessage("LoginSuccU", _data);
    }


    /// <summary>
    /// 여기서 부터 서버에 Send...
    /// </summary>

    // 테스트방에 들어간다고 서버에 전송
    // isteamgame: 팀전이면 1, 개인전이면 0
    public void EnterRoom(bool isteamgame)
    {
        base.EnterRoom(isteamgame);
    }

    // 캐릭터들 로딩이 전부 완료되면 호출
    public void LoadingComplete()
    {
        base.LoadingComplete();
    }

    // 캐릭터 움직이기 시작할 때 호출
    // csVECTOR2[] curpos = new csVECTOR[1];
    // curpos[0] = new cvVECTOR();
    // MoveStart(curpos, ...
    public void MoveStart(int timeindex, float timestamp, csVECTOR2[] curpos, csVECTOR2[] direction)
    {
        base.MoveStart(timeindex, timestamp, curpos, direction);
    }

    // 캐릭터 멈출 때 호출
    public void MoveStop(int timeindex, float timestamp, csVECTOR2[] curpos)
    {
        base.MoveStop(timeindex, timestamp, curpos);
    }

    // 캐릭터 회전할때 호출
    // direction은 현재 보고있는 방향이고
    // rotate는 회전하는 방향이다
    // 멈춰있을때나 뛰고있을때나 방향을 회전하기 시작하면 호출
    /*   public void ChangedirecetionStart(csVECTOR2[] curpos, csVECTOR2[] direction, csVECTOR2[] rotate)
       {
           base.ChangedirecetionStart(curpos, direction, rotate);
       }

       // 회전을 멈출 때 호출
       public void ChangedirecetionStop(csVECTOR2[] curpos, csVECTOR2[] direction)
       {
           base.ChangedirecetionStop(curpos, direction);
       }*/

    // 회전 정보 전송
    public void Changedirecetion(int timeindex, float timestamp, csVECTOR2[] curpos, csVECTOR2[] direction)
    {
        base.Changedirecetion(timeindex, timestamp, curpos, direction);
    }

    // 화면 터치 드래그로 시점을 회전시켰을때 전송
    public void Changeaim(int timeindex, float timestamp, csVECTOR2[] curpos, csVECTOR3[] aim, csVECTOR2[] direction)
    {
        base.Changeaim(timeindex, timestamp, curpos, aim, direction);
    }

    // 총쏘기 !!!!
    // enemytype - 0:사용자, 1:몬스터, 2:봇
    // ishit은 누군가 바로 맞췄을때 적정보와 함께 쏴야한다.. false 면 적정보를 무시함(쏘는 연출만 하면 돼니까)
    public void ShootEnemy(int timeindex, float timestamp, csVECTOR2[] curpos, csVECTOR3[] aim, int enemytype, int enemyindex, int ishit, csVECTOR2[] enemypos, int skilltype)
    {
        base.ShootEnemy(timeindex, timestamp, curpos, aim, enemytype, enemyindex, ishit, enemypos, skilltype);
    }

    // 어떤 다른넘이 원거리 공격을 했을때 해당 포탄에 맞는 여부는 맞는놈이 체크해서 보낸다...
    // enemytype, enemyindex 는 포탄을 쏜놈 데이터
    // enemytype - 0:사용자, 1:몬스터, 2:봇
    public void HitMe(int timeindex, float timestamp, csVECTOR2[] curpos, int enemytype, int enemyindex)
    {
        base.HitMe(timeindex, timestamp, curpos, enemytype, enemyindex);
    }

    // 원거리 공격시 해당 탄막에 봇 혹은 몬스터가 맞았는지 체크는 쏜놈이 해야한다!
    // enemytype - 1:몬스터. 2:봇
    public void HitEnemy(int timeindex, float timestamp, int enemytype, int enemyindex, csVECTOR2[] enemypos, csVECTOR2[] mypos)
    {
        base.HitEnemy(timeindex, timestamp, enemytype, enemyindex, enemypos, mypos);
    }

    // 임시로 게임배틀 시작.. 이미 시작이 되있거나 들어와있는 유저가 LoadingComplete을 하지 않았으면 실패
    public void TempStartBattle()
    {
        base.TempStartBattle();
    }

    // 몬스터가 떨군 아이템을 먹었다!! 
    public void EatItem(int timeindex, float timestamp, int itemindex, csVECTOR2[] mypos)
    { 
        base.EatItem(timeindex, timestamp, itemindex, mypos);
    }

    // 에너지가 10이하인 놈(myteam)을 터치(?)하면 50으로 에너지가 늘어난다
    public void TouchMyteam(int timeindex, float timestamp, int myteamtype, int myteamindex)
    {
        base.TouchMyteam(timeindex, timestamp, myteamtype, myteamindex);
    }

    // 캐릭터 고르기.. 배틀 시작하기 전에만 가능
    public void SelectCharacter(int characterindex)
    {
        base.SelectCharacter(characterindex);
    }

    // 펫 고르기.. 배틀 시작하기 전에만 가능
    public void SelectPet(int petindex)
    {
        base.SelectPet(petindex);
    }

    // 펫으로 변신 or 캐릭터로 다시 변신
    // ispet:1 이면 펫으, 0이면 캐릭터로
    public void TransformPet(int ispet)
    {
        base.TransformPet(ispet);
    }

    protected override void EnterRoomStartU(_S_PJ_ENTER_GAMEROOM_START_U _data)
    {
        Debug.Log("[PJ_ENTER_GAMEROOM_START_U]");
        m_BattleManager.SendMessage("EnterRoomStartU", _data);
    }

    protected override void EnterRoomU(_S_PJ_ENTER_GAMEROOM_U _data)
    {
        Debug.Log("[PJ_ENTER_GAMEROOM_U] COUNT = " + _data.COUNT + ", DATA[0] = " + _data.DATA[0].m_strUsername);
        m_BattleManager.SendMessage("EnterRoomU", _data);
    }

    protected override void EnterRoomEndU(_S_PJ_ENTER_GAMEROOM_END_U _data)
    {
        Debug.Log("[PJ_ENTER_GAMEROOM_END_U]");
        m_BattleManager.SendMessage("EnterRoomEndU", _data);
    }

    protected override void EnterRoomM(_S_PJ_ENTER_GAMEROOM_M _data)
    {
        Debug.Log("[PJ_ENTER_GAMEROOM_M]");
        m_BattleManager.SendMessage("EnterRoomM", _data);
    }

    protected override void ExitRoomM(_S_PJ_EXIT_GAMEROOM_M _data)
    {
        Debug.Log("[PJ_EXIT_GAMEROOM_M]");
        m_BattleManager.SendMessage("ExitRoomM", _data);
    }

    protected override void LoadingCompleteSuccU(_S_PJ_ENTER_LOADING_COMPLETE_SUCC_U _data)
    {
        Debug.Log("[PJ_ENTER_LOADING_COMPLETE_SUCC_U]");
        m_BattleManager.SendMessage("LoadingCompleteSuccU", _data);
    }

    protected override void BattleroomMonsterinfoM(_S_PJ_BATTLEROOM_MONTERINFO_M _data)
    {
        Debug.Log("[PJ_BATTLEROOM_MONTERINFO_M]");
        m_BattleManager.SendMessage("BattleroomMonsterinfoM", _data);
    }

    protected override void BattleroomCountdownM(_S_PJ_BATTLEROOM_COUNTDOWN_M _data)
    {
        Debug.Log("[PJ_BATTLEROOM_BATTLESTART_M]");
        m_BattleManager.SendMessage("BattleroomCountdownM", _data);
    }

    protected override void BattleroomBattlestartM(_S_PJ_BATTLEROOM_BATTLESTART_M _data)
    {
        Debug.Log("[PJ_BATTLEROOM_COUNTDOWN_M]");
        m_BattleManager.SendMessage("BattleroomBattlestartM", _data);
    }

    protected override void MoveStartM(_S_PJ_MOVE_START_M _data)
    {
        Debug.Log("[PJ_MOVE_START_M]");
        m_BattleManager.SendMessage("MoveStartM", _data);
    }

    protected override void MoveStopM(_S_PJ_MOVE_STOP_M _data)
    {
        Debug.Log("[PJ_MOVE_STOP_M]");
        m_BattleManager.SendMessage("MoveStopM", _data);
    }

    protected override void ChangedirecetionStartM(_S_PJ_CHANGE_DIRECTION_START_M _data)
    {
        Debug.Log("[PJ_CHANGE_DIRECTION_START_M]");
        m_BattleManager.SendMessage("ChangedirecetionStartM", _data);
    }

    protected override void ChangedirecetionStopM(_S_PJ_CHANGE_DIRECTION_STOP_M _data)
    {
        Debug.Log("[PJ_CHANGE_DIRECTION_STOP_M]");
        m_BattleManager.SendMessage("ChangedirecetionStopM", _data);
    }

    protected override void ChangedirecetionM(_S_PJ_CHANGE_DIRECTION_M _data)
    {
        Debug.Log("[PJ_CHANGE_DIRECTION_M]");
        m_BattleManager.SendMessage("ChangedirecetionM", _data);
    }

    protected override void ChangeaimM(_S_PJ_CHANGE_AIM_M _data)
    {
        Debug.Log("[PJ_CHANGE_AIM_M]");
        m_BattleManager.SendMessage("ChangeaimM", _data);
    }

    protected override void ShootEnemyM(_S_PJ_SHOOT_ENEMY_M _data)
    {
        Debug.Log("[PJ_SHOOT_ENEMY_M]");
        m_BattleManager.SendMessage("ShootEnemyM", _data);
    }

    protected override void HitMeM(_S_PJ_HIT_ME_M _data)
    {
        Debug.Log("[PJ_HIT_ME_M]");
        m_BattleManager.SendMessage("HitMeM", _data);
    }

    protected override void HitEnemyM(_S_PJ_HIT_ENEMY_M _data)
    {
        Debug.Log("[_S_PJ_HIT_ENEMY_M]");
        m_BattleManager.SendMessage("HitEnemyM", _data);
    }

    protected override void BattleroomResultscoreU(_S_PJ_BATTLEROOM_RESULTSCORE_U _data)
    {
        Debug.Log("[PJ_BATTLEROOM_RESULTSCORE_U]");
        m_BattleManager.SendMessage("BattleroomResultscoreU", _data);
    }

    protected override void DeadCharacterM(_S_PJ_DEAD_CHARACTER_M _data)
    {
        Debug.Log("[PJ_DEAD_CHARACTER_M]");
        m_BattleManager.SendMessage("DeadCharacterM", _data);
    }

    protected override void TouchMyteamM(_S_PJ_TOUCH_MYTEAM_ZOMBI_M _data)
    {
        Debug.Log("[PJ_TOUCH_MYTEAM_ZOMBI_M]");
        m_BattleManager.SendMessage("TouchMyteamM", _data);
    }

    protected override void MapFlowM(_S_PJ_MAPFLOW_M _data)
    {
        Debug.Log("[PJ_MAPFLOW_M]");
        m_BattleManager.SendMessage("MapFlowM", _data);
    }

    protected override void MonsterdeadItemM(_S_PJ_MONSTERDEAD_ITEM_M _data)
    {
        Debug.Log("[PJ_MONSTERDEAD_ITEM_M]");
        m_BattleManager.SendMessage("MonsterdeadItemM", _data);
    }

    protected override void EatItemM(_S_PJ_EAT_ITEM_M _data)
    {
        Debug.Log("[PJ_EAT_ITEM_M]");
        m_BattleManager.SendMessage("EatItemM", _data);
    }

    protected override void SelectCharacterM(_S_PJ_SELECT_CHARACTER_M _data) 
    { 
        Debug.Log("[PJ_SELECT_CHARACTER_M]");
        m_BattleManager.SendMessage("SelectCharacterM", _data);
    }

    protected override void SelectPetM(_S_PJ_SELECT_PET_M _data) 
    { 
        Debug.Log("[PJ_SELECT_PET_M]");
        m_BattleManager.SendMessage("SelectPetM", _data);
    }

    protected override void TransformPetM(_S_PJ_TRANSFORM_PET_M _data)
    {
        Debug.Log("[PJ_TRANSFORM_PET_M]");
        m_BattleManager.SendMessage("TransformPetM", _data);
    }

    protected override void KeepAliveM(_S_PJ_KEEPALIVE_M _data)
    {
        //Debug.Log("[PJ_KEEPALIVE_M]");
        m_BattleManager.SendMessage("KeepAliveM", _data);
    }
}