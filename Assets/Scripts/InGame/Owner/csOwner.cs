using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProjectK;

public abstract partial class csOwner : MonoBehaviour {

	public csEventHandler_Bugs m_EventHandler_Bugs
	{
		get;
		private set;
	}

	protected csStateManager m_StateManager = null;

	[HideInInspector]
	public csMove m_Move = null;
	protected csHitEffect m_HitEffect = null;

	protected csCrowdControl m_CrowdControl = null;

	public Transform m_WaitingPool_Bullet = null;
	public Transform m_WaitingPool_Effect = null;

	[HideInInspector]
	public csHealth m_Health;
	[HideInInspector]
	public csPower m_Power;
	
	public int OwnerType
	{
		get;
		protected set;
	}

	public int OwnerIndex
	{
		get;
		private set;
	}

	protected Transform m_Transform = null;
	public Transform Transform
	{
		get
		{
			if (m_Transform == null)
			{
				m_Transform = transform;
			}

			return m_Transform;
		}
	}

	protected Collider m_Collider = null;
	public Collider Collider
	{
		get
		{
			if (m_Collider == null)
			{
				m_Collider = GetComponent<Collider>();
			}

			return m_Collider;
		}
	}

	protected CharacterController m_CharacterController = null;
	public CharacterController CharacterController
	{
		get
		{
			if (m_CharacterController == null)
			{
				m_CharacterController = gameObject.GetComponent<CharacterController>();
			}

			return m_CharacterController;
		}
	}

	protected csBattleManager m_BattleManager = null;
	public csBattleManager BattleManager
	{
		get
		{
			if (m_BattleManager == null)
			{
				m_BattleManager = csBattleManager.Instance;
			}

			return m_BattleManager;
		}
	}

	protected csBugsInfoManager m_BugsInfoManager = null;
	public csBugsInfoManager BugsInfoManager
	{
		get
		{
			if (m_BugsInfoManager == null)
			{
				m_BugsInfoManager = BattleManager.m_BugsInfoManager;
			}

			return m_BugsInfoManager;
		}
	}

	//[SerializeField]
	private csInfo_Bugs m_Info_Bugs = null;
	public csInfo_Bugs Info_Bugs
	{
		get
		{			
			if (m_Info_Bugs == null)
			{
				m_Info_Bugs = BugsInfoManager.GetBugsInfo(OwnerType, Bugs.ObjectIndex);
			}

			return m_Info_Bugs;
		}
	}

	protected csBugs m_Bugs = null;
	public csBugs Bugs
	{
		get
		{
			return m_Bugs;
		}
		protected set
		{
			m_Bugs = value;
		}
	}

	protected Vector3 m_BugsCenter = Vector3.zero;
	public Vector3 BugsCenter
	{
		get
		{
			return Collider.bounds.center;
		}
	}

	protected Timer m_Timer = null;

	protected virtual void Awake()
	{
		m_EventHandler_Bugs = GetComponent<csEventHandler_Bugs>();
		m_StateManager = GetComponent<csStateManager>();
		m_Health = GetComponent<csHealth>();
		m_Power = GetComponent<csPower>();

		m_Timer = new Timer();

		m_EventHandler_Bugs.ActivatedWeapon.Set(new List<csWeapon>());
	}

	public virtual void Settings(int _userType, int _userIndex)
	{
		OwnerType = _userType;
		OwnerIndex = _userIndex;

		CreateWaitingPool();

		m_HitEffect = GetComponent<csHitEffect>();
		m_HitEffect.Settings(this);

		m_Move = GetComponent<csMove>();

		BattleManager.StartBattle += StartBattle;
		BattleManager.EndBattle += EndBattle;
	}

	public virtual void Release()
	{
		if (m_WaitingPool_Bullet != null)
		{
			Destroy(m_WaitingPool_Bullet);
		}

		if (m_WaitingPool_Effect != null)
		{
			Destroy(m_WaitingPool_Effect);
		}

		Utility.Activate(CharacterController, false);
	}

	private void CreateWaitingPool()
	{
		/*csCreator_PoolSpace.CreatePool("WaitingPool/Bullet/" + Utility.DetermineObjectPoolName("Bullet", name), gameObject.scene, out csPoolSpace _poolSpace);
		m_WaitingPool_Bullet = _poolSpace.Transform;
		csCreator_PoolSpace.CreatePool("WaitingPool/Effect/" + Utility.DetermineObjectPoolName("Effect", name), gameObject.scene, out _poolSpace);
		m_WaitingPool_Effect = _poolSpace.Transform;*/

		m_WaitingPool_Bullet = new GameObject(Utility.DetermineObjectPoolName("Bullet", name)).transform;
		m_WaitingPool_Effect = new GameObject(Utility.DetermineObjectPoolName("Effect", name)).transform;
		m_WaitingPool_Bullet.SetParent(csBattleManager.m_WaitingPool_Bullet);
		m_WaitingPool_Bullet.SetAsFirstSibling();
		m_WaitingPool_Effect.SetParent(csBattleManager.m_WaitingPool_Effect);
		m_WaitingPool_Effect.SetAsFirstSibling();
	}

	protected virtual void StartBattle()
	{
		BattleManager.StartBattle -= StartBattle;

		m_Health.Settings(Info_Bugs.m_Health);
		m_Power.Settings(Info_Bugs.m_Power);
	}

	public virtual void StateStart(eBugsStateType _stateType, Action _callback = null)
	{
		csState State = m_EventHandler_Bugs.GetState(_stateType);
		StateStart(State, _callback);
	}

	public virtual void StateStart(string _state, Action _callback = null)
	{
		csState State = (csState)m_EventHandler_Bugs.GetEvent(_state);
		StateStart(State, _callback);
	}

	public virtual void StateStart(csState _state, Action _callback = null)
	{
		m_StateManager.StateStart(_state, _callback);
	}
	
	public virtual void StateStop(eBugsStateType _stateType, Action _callback = null)
	{
		csState State = m_EventHandler_Bugs.GetState(_stateType);
		StateStop(State, _callback);
	}

	public virtual void StateStop(string _state, Action _callback = null)
	{	
		csState State = (csState)m_EventHandler_Bugs.GetEvent(_state);
		StateStop(State, _callback);
	}

	public virtual void StateStop(csState _state, Action _callback = null)
	{
		/*if(_state.EventName == "Attack_Action")
		{
			Debug.LogError("StateStop : " + name + "   " + _state.EventName + "  " + _state.Active);
		}*/
		
		m_StateManager.StateStop(_state, _callback);
	}

	public virtual void StateForceStop(eBugsStateType _stateType, Action _callback = null)
	{
		csState State = m_EventHandler_Bugs.GetState(_stateType);
		StateForceStop(State, _callback);
	}

	public virtual void StateForceStop(string _state, Action _callback = null)
	{
		csState State = (csState)m_EventHandler_Bugs.GetEvent(_state);
		StateForceStop(State, _callback);
	}

	public virtual void StateForceStop(csState _state, Action _callback = null)
	{
		m_StateManager.StateForceStop(_state, _callback);
		//Bugs.Initialize();
	}

	public virtual void AllStateStop()
	{
		m_StateManager.AllStateStop();
	}

	public virtual bool IsActiveState(eBugsStateType _stateType)
	{
		csState State = m_EventHandler_Bugs.GetState(_stateType);
		return State.Active;
	}

	public virtual bool IsActiveState(csState _state)
	{
		return _state.Active;
	}
	
	public virtual void HitMe(int _attackerType, int _attackerIndex, int _damage, BugsAction _bugsAction)
	{
		if (_bugsAction.CrowdControl != null)
		{
			if(m_Health.Health_Cur > _damage)
			{
				Debug.Log("HitMe : " + _bugsAction.CrowdControl.ToString());

				m_EventHandler_Bugs.CrowdControl.Set(_bugsAction.CrowdControl[0]);

				StateStart(_bugsAction.CrowdControl[0].CrowdControlType);
			}			
		}

		// 피격 이펙트
		m_HitEffect.PlayEffect(_damage);

		m_Health.HealthDecrease(_damage, () =>
		{
			StateStart(eBugsStateType.Die);
			return;
		});
	}

	public virtual void GameOver()
    {

    }

	public virtual void Restart()
    {
		m_Move.MoveSpeed = Info_Bugs.m_RunSpeed;
		m_Health.Initialize();
		m_Power.Initialize();
	}

	protected virtual void EndBattle()
	{
		BattleManager.EndBattle -= EndBattle;

		Release();
	}

	#region State Method

	protected virtual void OnStart_Walk(Action _callback = null)
	{
		m_Move.MoveSpeed = Info_Bugs.m_WalkSpeed;

		Bugs.WalkStart();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Walk(Action _callback = null)
	{
		Bugs.WalkStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Run(Action _callback = null)
	{
		m_Move.MoveSpeed = Info_Bugs.m_RunSpeed;

		Bugs.RunStart();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Run(Action _callback = null)
	{
		Bugs.RunStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Attack(Action _callback = null)
	{
		_callback?.Invoke();
	}

	protected virtual void OnStop_Attack(Action _callback = null)
	{
		_callback?.Invoke();
	}

	protected virtual void OnForceStop_Attack(Action _callback = null)
	{
		_callback?.Invoke();
	}

	protected virtual void OnStart_Slow(Action _callback = null)
	{
		_callback?.Invoke();
	}

	protected virtual void OnStop_Slow(Action _callback = null)
	{
		_callback?.Invoke();
	}

	protected virtual void OnStart_Stun(Action _callback = null)
	{
		_callback?.Invoke();
	}

	protected virtual void OnStop_Stun(Action _callback = null)
	{
		_callback?.Invoke();
	}

	protected virtual void OnStart_KnockBack(Action _callback = null)
	{		
		Bugs.KnockBack_Start();

		_callback?.Invoke();
	}

	protected virtual void OnStop_KnockBack(Action _callback = null)
	{
		Bugs.KnockBack_Stop();
		_callback?.Invoke();
	}

	protected virtual void OnStart_Die(Action _callback = null)
	{
		_callback?.Invoke();
	}

	protected virtual void OnStop_Die(Action _callback = null)
	{
		_callback?.Invoke();
	}

	#endregion

	#region Command Method

	#endregion

	public float m_TestRaycastRange = 0.0f;
	public Vector3 m_TestRaycastDirection;
	void OnDrawGizmosSelected()
	{
		/*if (MyHandler.AimDirection != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(Collider.bounds.center, MyHandler.AimDirection.Get());
			Gizmos.DrawWireSphere(MyHandler.AimDirection.Get(), 0.1f);
		}	*/

		if(Collider != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(Collider.bounds.center, Collider.bounds.center + m_TestRaycastDirection * m_TestRaycastRange);
		}	
	}
}
