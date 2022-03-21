using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class csOwner_Monster : csOwner
{
	protected List<MonsterName> m_ListMonsterName =
		new List<MonsterName>()
		{
			//{ MonsterName.WOLF },
			//{ MonsterName.WRAITH },
			{ MonsterName.GOBLIN },
			//{ MonsterName.ONE_EYED },
		};

	public csEventHandler_Monster m_EventHandler_Monster
	{
		get;
		private set;
	}

	public csMonster Monster
	{
		get;
		protected set;
	}

	private csInfo_Monster m_Info_Monster = null;
	public csInfo_Monster Info_Monster
	{
		get
		{
			if (m_Info_Monster == null)
			{
				m_Info_Monster = (csInfo_Monster)BugsInfoManager.GetBugsInfo(OwnerType, Bugs.ObjectIndex);
			}

			return m_Info_Monster;
		}
	}

	[HideInInspector]
	public csActingPower m_ActingPower = null;

	protected csHUD_Monster m_HUD_Monster = null;
	protected csHUD_Indicator m_HUD_Indicator = null;
	protected csHUD_ActingPower m_HUD_ActingPower = null;

	public const int Index_Action_Attack = 0;

	protected string m_MonsterLoadPath = "";

	private void OnEnable()
	{
		if (BattleManager.m_GameState == csBattleManager.GameState.Battle)
		{
			m_Timer.OnDelay(1.0f, () =>
			{
				StartBattle();
			});			
		}
	}

	protected override void Awake()
	{
		base.Awake();
		m_EventHandler_Monster = (csEventHandler_Monster)m_EventHandler_Bugs;
		m_EventHandler_Monster.Register(this);

		m_ActingPower = GetComponent<csActingPower>();
	}

	public override void Settings(int _userType, int _userIndex)
	{
		base.Settings(_userType, _userIndex);

		CreateMonster();

		m_HUD_Monster = Instantiate(Resources.Load("Prefabs/UI/HUD_Monster") as GameObject, BattleManager.m_BattleUIManager.BattleHUD.m_HUD_MonsterPanel).GetComponent<csHUD_Monster>();
		m_HUD_Monster.Settings(this);

		m_HUD_Indicator = Instantiate(Resources.Load("Prefabs/UI/HUD_Indicator") as GameObject, BattleManager.m_BattleUIManager.BattleHUD.m_IndicatorPanel).GetComponent<csHUD_Indicator>();
		m_HUD_Indicator.Settings(this);

		m_HUD_ActingPower = m_HUD_Monster.m_ActingPower_HUD;
		m_HUD_ActingPower.Settings(this);

		m_Move.Settings(this);

		m_EventHandler_Monster.Grid.Set(m_BattleManager.m_Grid.m_EventHandler);
		m_EventHandler_Monster.HUD_ActivateFront.Send(false);
	}

	protected virtual void CreateMonster()
	{
		int RandNum = UnityEngine.Random.Range(0, m_ListMonsterName.Count);

		m_MonsterLoadPath = "Prefabs/Monster/";

		Monster = Instantiate(Resources.Load(m_MonsterLoadPath + m_ListMonsterName[RandNum].ToString()) as GameObject, Transform).GetComponent<csMonster>();

		if (Monster != null)
		{
			Bugs = Monster;

			Monster.Settings(this);			
		}
	}

	public override void Release()
	{
		base.Release();

		StopAI();

		if (m_HUD_Monster != null)
		{
			m_HUD_Monster.Release();
		}

		if (m_HUD_Indicator != null)
		{
			m_HUD_Indicator.Release();
		}

		m_EventHandler_Monster.Unregister(this);
		Utility.Activate(m_Move, false);
		m_Move.Release();
	}

	protected override void StartBattle()
	{
		if(!Utility.IsActive(gameObject))
		{
			return;
		}

		if(IsActiveState(eBugsStateType.Die))
		{
			RespawnInitialize();
		}

		m_EventHandler_Monster.HUD_ActivateFront.Send(true);

		m_ActingPower.Settings(Info_Monster.ActingPower);

		m_HUD_Monster.HUDOn();

		m_HUD_Indicator.HUDOn();

		base.StartBattle();
	
		m_Timer.OnDelay(1.0f, () =>
		{
			Utility.Activate(CharacterController, true);

			StartAI();

			//TransitionToState(BotState.Patrol);

			/*m_CoState = CoState();
			StartCoroutine(m_CoState);*/
		});		
	}

    public override void GameOver()
    {
        base.GameOver();
		Debug.LogError("GameOVer");
		StopAI();
		AllStateStop();
	}

    public override void Restart()
    {
        base.Restart();		

		Utility.Activate(gameObject, false);
	}

    protected virtual void RespawnInitialize()
	{
		Monster.Initialize();

		StateStop(eBugsStateType.Die);
	}

	protected virtual void StartAI()
	{
		Monster.StartAI();
	}

	protected virtual void StopAI()
	{
		Monster.StopAI();

		m_HUD_Indicator.HUDOff();

		m_EventHandler_Monster.HUD_ActivateFront.Send(false);
	}

	#region State Method

	protected override void OnStart_Attack(Action _callback = null)
	{
		if(_callback != null)
		{
			Monster.Attack_Start(_callback);
		}
		else
		{
			Monster.Attack_Start();
		}

		/*if (_callback != null)
		{
			_callback();
		}*/
	}

	protected override void OnStop_Attack(Action _callback = null)
	{
		Monster.Attack_Stop();

		_callback?.Invoke();
	}

	protected override void OnStart_Die(Action _callback = null)
	{
		Utility.Activate(CharacterController, false);

		BattleManager.IncreaseKillCount();

		StopAI();
		AllStateStop();
		Bugs.Die();

		m_Timer.OnDelay(2.0f, () =>
		{	
			Utility.Activate(gameObject, false);
			BattleManager.m_SpawnManager.RespawnMonster();
		});

		_callback?.Invoke();
	}

	#endregion

	#region Command Method

	#endregion

	#region Coroutine

	#endregion
}
