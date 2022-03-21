using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ProjectK;

/// <summary>
/// 제거 예정
/// </summary>
public class csBattleManager : Singleton<csBattleManager>
{
	public enum GameState
	{
		Waiting,
		Pause,
		Battle,
		GameOver
	}

	public GameState m_GameState;

	public delegate void Delegate();
	public event Delegate StartBattle;
	public event Delegate EndBattle;

	[HideInInspector]
	public csOwner_Hero m_Hero = null;

	public csEventHandler_Game EventHandler_Game { get; private set; }
	[HideInInspector]
	public csEventHandler_Hero m_EventHandler_Hero = null;
	public Dictionary<int, csOwner> m_DicObjectColliderIndex = new Dictionary<int, csOwner>();
	public Dictionary<int, csOwner> m_DicOwner = new Dictionary<int, csOwner>();

	public csBattleUIManager m_BattleUIManager = null;
	[HideInInspector]
	public csBugsInfoManager m_BugsInfoManager = null;
	[HideInInspector]
	public csSpawnManager m_SpawnManager = null;
	[HideInInspector]
	public csItemManager m_ItemManager = null;
	[HideInInspector]
	public csPlayerCamera m_PlayerCamera = null;
	public csGrid m_Grid = null;

	private Timer m_Timer = null;

	//[HideInInspector]
	public static Transform m_WaitingPool = null;
	//[HideInInspector]
	public static Transform m_WaitingPool_Monster = null;
	//[HideInInspector]
	public static Transform m_WaitingPool_Bullet = null;
	//[HideInInspector]
	public static Transform m_WaitingPool_Effect = null;
	//[HideInInspector]
	public static Transform m_WaitingPool_CommonEffect = null;
	//[HideInInspector]
	public static Transform m_WaitingPool_Item;

	private Scene m_BattleScene;

	public float m_fTimeScale = 1.0f;
	[ExposeProperty]
	public float TimeScale
	{
		get
		{
			return m_fTimeScale;
		}
		set
		{
			m_fTimeScale = value;
			Time.timeScale = m_fTimeScale;
		}
	}
	
	public const int MaxUserCount = 32;
	public const int HeroType = 0;
	public const int MonsterType = 1;
	public const int MaxPopulation = 100;
	public int m_iCollisionLayerMask_Hero = 0;
	public int m_iCollisionLayerMask_Monster = 0;
	public int m_iLayerMask_Hero = 0;
	public int m_iLayerMask_Monster = 0;
	public int m_iLayerMask_Map = 0;
	public int m_iLayerMask_Floor = 0;
	private int m_iTotalUser = 0;
	private int m_iCurrentKillCount = 0;
	[HideInInspector]
	public int m_iCurrentIndex = 0;
	[HideInInspector]
	public int m_iPrevIndex = 0;
	[HideInInspector]
	public int m_iRemoveIndex = 0;

	public bool m_bIsBackground = false;

	protected override void Awake()
    {
        base.Awake();

		SceneManager.sceneLoaded += OnSceneLoaded;

		EventHandler_Game = GetComponent<csEventHandler_Game>();
		m_BugsInfoManager = GetComponent<csBugsInfoManager>();
		m_SpawnManager = GetComponent<csSpawnManager>();
		m_ItemManager = GetComponent<csItemManager>();

		m_iCollisionLayerMask_Hero = Layer.CollisionLayerMask(eCollisionLayerType.Monster, eCollisionLayerType.Map, eCollisionLayerType.Floor);
		m_iCollisionLayerMask_Monster = Layer.CollisionLayerMask(eCollisionLayerType.Hero, eCollisionLayerType.Map, eCollisionLayerType.Floor);
		m_iLayerMask_Hero = Layer.CollisionLayerMask(eCollisionLayerType.Hero);
		m_iLayerMask_Monster = Layer.CollisionLayerMask(eCollisionLayerType.Monster);
		m_iLayerMask_Map = Layer.CollisionLayerMask(eCollisionLayerType.Map);
		m_iLayerMask_Floor = Layer.CollisionLayerMask(eCollisionLayerType.Floor);
	}

    private void Start()
    {
        csSoundManager.Instance.Initialize(10);
        csSoundManager.PLAY_AMB = true;
        csSoundManager.PLAY_BGM = true;
        csSoundManager.PLAY_SFX = true;

		m_Timer = new Timer();

		m_BattleUIManager.Settings();
	}

	private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
	{
		m_BattleScene = _scene;

		SceneManager.sceneLoaded -= OnSceneLoaded;

		if (m_PlayerCamera == null)
		{
			m_PlayerCamera = Instantiate(Resources.Load("Prefabs/HeroRelated/PlayerCamera") as GameObject).GetComponent<csPlayerCamera>();
			ObjectMoveToBattleScene(m_PlayerCamera.gameObject);
		}

		if (m_BattleUIManager == null)
		{
			m_BattleUIManager = Instantiate(Resources.Load("Prefabs/UI/BattleUIManager") as GameObject).GetComponent<csBattleUIManager>();
			ObjectMoveToBattleScene(m_BattleUIManager.gameObject);		
		}

		if(m_Grid == null)
		{
			m_Grid = Instantiate(Resources.Load("Prefabs/NodeCreator") as GameObject).GetComponent<csGrid>();
			ObjectMoveToBattleScene(m_Grid.gameObject);
		}

		CreateWaitingPool();		
		BattleSettings();
		CountDown();
	}

	private void CreateWaitingPool()
	{
		m_WaitingPool = new GameObject("WaitingPool").transform;
		m_WaitingPool.transform.position = Vector3.zero;
		ObjectMoveToBattleScene(m_WaitingPool.gameObject);
		m_WaitingPool.SetAsFirstSibling();

		m_WaitingPool_Monster = new GameObject("Monster").transform;
		m_WaitingPool_Monster.transform.position = Vector3.zero;
		m_WaitingPool_Monster.SetParent(m_WaitingPool);
		m_WaitingPool_Bullet = new GameObject("Bullet").transform;
		m_WaitingPool_Bullet.transform.position = Vector3.zero;
		m_WaitingPool_Bullet.SetParent(m_WaitingPool);
		m_WaitingPool_Effect = new GameObject("Effect").transform;
		m_WaitingPool_Effect.transform.position = Vector3.zero;
		m_WaitingPool_Effect.SetParent(m_WaitingPool);
		m_WaitingPool_CommonEffect = new GameObject("CommonEffect").transform;
		m_WaitingPool_CommonEffect.transform.position = Vector3.zero;
		m_WaitingPool_CommonEffect.SetParent(m_WaitingPool);
		m_WaitingPool_Item = new GameObject("Item").transform;
		m_WaitingPool_Item.transform.position = Vector3.zero;
		m_WaitingPool_Item.SetParent(m_WaitingPool);
	}

	private void BattleSettings()
	{
		//m_BattleUIManager.BattleETC.m_SelectHero.OnClickOpenTheSelect();
		m_GameState = GameState.Waiting;

		m_SpawnManager.CreateUser((user) =>
		{
			OwnerSettings(HeroType, 0, user);
		});		

		m_ItemManager.Settings();

		m_SpawnManager.CreateMonster();

		Utility.DestroyScene(csProjectManager.Instance.SceneType_Loding_InUse.ToString());		
	}

	public bool IsHero(csBugs _bugs)
	{
		bool IsHero = false;

		if (m_Hero.Bugs == _bugs)
		{
			IsHero = true;
		}

		return IsHero;
	}

	public void ObjectMoveToBattleScene(GameObject _obj)
	{
		SceneManager.MoveGameObjectToScene(_obj, m_BattleScene);
	}

	public void ForceEscape()
	{
		if (!m_Hero.IsActiveState(eBugsStateType.Pet))
		{
			m_Hero.StateStart(eBugsStateType.Groggy);
		}		
	}

	public void ForceCreateGrid()
	{
		m_Grid.CreateGrid();
	}

	public void OwnerSettings(int _ownerType, int _ownerIndex, csOwner _owner)
	{
		//Debug.Log("ownerIndex : " + _ownerIndex);
		if (_ownerType == HeroType)
		{
			//Debug.Log("IsLocal(UserIndex)");
			m_Hero = (csOwner_Hero)_owner;

			m_EventHandler_Hero = _owner.GetComponent<csEventHandler_Hero>();
			
			if (m_EventHandler_Hero != null)
			{
				m_PlayerCamera.Settings(m_Hero);

				m_Hero.StateStart(eBugsStateType.Waiting);
			}
		}

		if (!m_DicOwner.ContainsKey(_ownerIndex))
		{
			m_DicOwner.Add(_ownerIndex, _owner);
		}

		// 충돌체 구분을 위한(혹은 레이어 마스크) 콜라이더의 인스턴스 값 저장
		if (!m_DicObjectColliderIndex.ContainsKey(_owner.Collider.GetInstanceID()))
		{
			m_DicObjectColliderIndex.Add(_owner.Collider.GetInstanceID(), _owner);
		}

		_owner.Settings(_ownerType, _ownerIndex);
	}

	public void ForceStartBattle()
	{
		CountDown();
	}

	private void CountDown()
	{
		/*if (Utility.IsActive(m_BattleUIManager.BattleETC.m_StartBattle))
		{
			m_BattleUIManager.BattleETC.DeactivateStartBattle();
		}

		m_BattleUIManager.BattleETC.m_SelectHero.CloseSelectHero();
		m_BattleUIManager.BattleETC.m_SelectHero.Lock();*/	

		// 타이머 시작 5초
		m_SpawnManager.BattleStartTimer(0, () =>
		{
			//m_BattleUIManager.BattleETC.m_WaitingRoomUI.CloseWaitingRoomUI();

			m_Hero.StateStop(eBugsStateType.Waiting);

			m_GameState = GameState.Battle;

			BattleStart();
		});
	}

	private void BattleStart()
	{
		m_BattleUIManager.BattleController.Settings(m_Hero.Hero.ObjectIndex);

		StartBattle();

		m_iTotalUser = m_DicOwner.Count;

		m_Timer.StartTimer();
	}

	public void IncreaseKillCount()
	{
		m_iCurrentKillCount++;

		m_BattleUIManager.BattleTop.m_Population.IncreaseKillCount(m_iCurrentKillCount);

		if(m_iCurrentKillCount == MaxPopulation)
		{
			m_BattleUIManager.BattleETC.m_ResultScore.OpenTheResult(1, 1527, 1, MaxPopulation, m_iCurrentKillCount);
			EndBattle();
		}
	}

	public void GameOver()
    {
		EventHandler_Game.GameOver.Start();

		foreach (csOwner Owner in m_DicOwner.Values)
		{
			Owner.GameOver();
		}

		m_BattleUIManager.BattleETC.GameOver();
	}

	public void Restart()
    {
		csFadeController.FadeIn(() =>
		{
			m_BattleUIManager.BattleETC.Restart();
			m_SpawnManager.Initialize();

			m_iCurrentKillCount = 0;

			m_BattleUIManager.BattleTop.m_Population.IncreaseKillCount(m_iCurrentKillCount);

			foreach (csOwner Owner in m_DicOwner.Values)
            {
				Owner.Restart();
            }

			csFadeController.FadeOut(() =>
			{
				EventHandler_Game.GameOver.Stop();
			});
		});
    }
}
