using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LobbySceneType
{
	Lobby_Moba
}

public enum LoadingSceneType
{
	Loading
}

public enum InGameSceneType
{
	InGame
}

public enum MapSceneType
{
	Map_BattleRoyale,
	Map_Temp
}


public enum SceneType
{
	Lobby,
	BattleScene
}

public enum PoolType
{

}

public enum eBotStateType
{
	None,
	Waiting,
	Chase,
	Battle,
	RunAway,
	TakeCover,
	OnGuard,
}

public enum eDetectViewAngleType
{
	Move,
	Battle
}

public enum eNodeType
{
	None,
	Start,
	End,
	Walkable,
	Obstacle,
	Current,
	Path
}

public enum eDirectionType
{
	None,
	Forward,
	Back,
	Right,
	Left
}

public enum eBugsStateType
{
	None,
	Waiting,
	Walk,
	Run,
	Rotate,
	Attack_Ready,
	Attack_Action,
	Attack,
	Skill_1_Ready,
	Skill_1_Action,
	Skill_1,
	Skill_2_Ready,
	Skill_2_Action,
	Skill_2,
	Skill_3_Ready,
	Skill_3_Action,
	Skill_3,
	Skill_Default_Ready,
	Skill_Default_Action,
	Skill_Default,
	Slow,
	Stun,
	KnockBack,
	Hero,
	Pet,
	Groggy,
	Escape_Ready,
	Escape_Action,
	Escape,
	Revival,
	Die,
	Pause,
	GameOver
}

public enum eCollisionLayerType
{
	Hero = 8,
	Monster = 9,
	Map = 10,
	Floor = 11
}

public enum ItemName
{
	Speed,
	Power,
	Heal,
	Skill
}

public enum MonsterName
{
	WOLF,
	WRAITH,
	GOBLIN,
	ONE_EYED,
	PIRATE
}

public enum eAIActionableCriterion
{
	Consumption,
	Full
}
