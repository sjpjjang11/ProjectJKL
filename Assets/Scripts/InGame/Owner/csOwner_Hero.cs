using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;

public class csOwner_Hero : csOwner
{
	public Transform m_CameraTarget = null;
	public Transform m_LookAtTarget = null;

	public csEventHandler_Hero m_EventHandler_Hero
	{
		get;
		private set;
	}

	private csInfo_Hero m_Info_Hero = null;
	public csInfo_Hero Info_Hero
	{
		get
		{
			if (m_Info_Hero == null)
			{
				csInfo_Bugs Info_Bugs = BugsInfoManager.GetBugsInfo(OwnerType, Bugs.ObjectIndex);
				m_Info_Hero = (csInfo_Hero)Info_Bugs;
			}

			return m_Info_Hero;
		}
	}

	private csHUD_Hero m_HUD_Hero = null;
	private csHUD_Target m_HUD_Target = null;

	public csHero Hero
	{
		get;
		protected set;
	}

	public csPet Pet
	{
		get;
		protected set;
	}

	protected string m_HeroLoadPath = "";
	protected string m_PetLoadPath = "";

	public float m_fAttackRotationSpeed = 40.0f;

	public const int Index_Button_Attack = 0;
	public const int Index_Button_Skill_Item = 1;
	public const int Index_Button_Skill_Default = 1;
	public const int Index_Button_Skill_1 = 1;
	public const int Index_Button_Skill_2 = 2;
	public const int Index_Button_Escape = 2;
	public const int Index_Button_Multi = 5;

	public const int Index_Action_Attack = 0;
	public const int Index_Action_Skill_1 = 1;
	public const int Index_Action_Skill_2 = 2;
	public const int Index_Action_None = csInfo_Hero.ActionCount - 1;
	public const int Index_Action_Skill_Default = csInfo_Hero.ActionCount - 2;
	public const int Index_Action_Escape = csInfo_Hero.ActionCount - 1;

	public const int Index_Effect_Revival = 0;
	public const int Index_Effect_Dash = 1;

	protected override void Awake()
	{
		base.Awake();

		m_EventHandler_Hero = (csEventHandler_Hero)m_EventHandler_Bugs;
		m_EventHandler_Hero.Register(this);
	}

	public override void Settings(int _userType, int _userIndex)
	{
		base.Settings(_userType, _userIndex);

		CreateHero();
		CreatePet();

		m_HUD_Hero = Instantiate(Resources.Load("Prefabs/UI/HUD_Hero") as GameObject, BattleManager.m_BattleUIManager.BattleHUD.m_HUD_HeroPanel).GetComponent<csHUD_Hero>();
		m_HUD_Hero.Settings(this);

		m_HUD_Target = Instantiate(Resources.Load("Prefabs/UI/HUD_Target") as GameObject, BattleManager.m_BattleUIManager.BattleHUD.m_TargetPanel).GetComponent<csHUD_Target>();
		m_HUD_Target.Settings(this);
	
		m_Move.RegisterWalkDirectionCallback(WalkDirection);

		//BBBBB
		/*JoystickHandlerSettings(0, Index_Action_None);
		JoystickHandlerSettings(1, Index_Action_None);
		JoystickHandlerSettings(2, Index_Action_None);
		JoystickHandlerSettings(3, Index_Action_Skill_Default);*/

		m_Move.Settings(this);
	}

	public override void Release()
	{
		base.Release();

		if (m_HUD_Hero != null)
		{
			m_HUD_Hero.Release();
		}

		if (m_HUD_Target != null)
		{
			m_HUD_Target.Release();
		}

		m_StateManager.AllStateStop();

		m_EventHandler_Hero.Unregister(this);

		m_Move.Release();
	}

	protected virtual void CreateHero()
	{
		//csHero Hero;

		string HeroName = "";

		if (csProjectManager.Instance.m_pDataManager.m_DicHeroData.ContainsKey(csProjectManager.Instance.m_iHeroSelect))
		{
			HeroName = csProjectManager.Instance.m_pDataManager.m_DicHeroData[csProjectManager.Instance.m_iHeroSelect];
		}

		m_HeroLoadPath = "Prefabs/Hero/";

		Hero = Instantiate(Resources.Load(m_HeroLoadPath + HeroName) as GameObject, Transform).GetComponent<csHero>();

		if (Hero != null)
		{
			if (Utility.IsActive(Hero.gameObject))
			{
				//Utility.Activate(Hero.gameObject, false);
			}

			Bugs = Hero;

			Hero.gameObject.layer = gameObject.layer;

			Hero.Settings(this);

			BugsCollider BugsCollider = Info_Bugs.m_BugsCollider;

			CharacterController.center = BugsCollider.Center;
			CharacterController.radius = BugsCollider.Radius;
			CharacterController.height = BugsCollider.Height;
		}
	}

	protected virtual void CreatePet()
	{
		string PetName = "";

		if (csProjectManager.Instance.m_pDataManager.m_DicPetData.ContainsKey(csProjectManager.Instance.m_iPetSelect))
		{
			PetName = csProjectManager.Instance.m_pDataManager.m_DicPetData[csProjectManager.Instance.m_iPetSelect];
		}

		m_PetLoadPath = "Prefabs/Pet/";

		Pet = Instantiate(Resources.Load(m_PetLoadPath + PetName) as GameObject, Transform).GetComponent<csPet>();

		Pet.gameObject.layer = gameObject.layer;

		Pet.Settings(this);

		Utility.Activate(Pet.gameObject, false);

		Pet.Transform.position = Hero.m_PetTr.position;
	}

	protected override void StartBattle()
	{
		base.StartBattle();
		
		UpdateAbilities();
		
		m_HUD_Hero.HUDOn();
		m_HUD_Target.HUDOn();

		Hero.StartBattle();
	}

	protected virtual void UpdateAbilities()
	{
		m_Move.MoveSpeed = Info_Bugs.m_RunSpeed;

		//Health.Max = Health.Cur = ObjectInfo.Health.Max;

		//m_EventHandler.SetHealth.Send();
	}

	protected virtual void WalkDirection(Vector3 _direction)
	{
		Vector3 Direction;
		//Direction = (Transform.forward * _direction.z) + (Transform.right * -_direction.x);
		_direction.x *= -1.0f;
		Direction = Transform.rotation * _direction;

		Hero.Animator.SetFloat("Horizontal", Direction.x * -1.0f);
		Hero.Animator.SetFloat("Vertical", Direction.z);
	}

	private int m_iTempRevivalCount = 1;
	public override void HitMe(int _attackerType, int _attackerIndex, int _damage, BugsAction _bugsAction)
	{
		if (_bugsAction.CrowdControl != null)
		{
			if (m_Health.Health_Cur > _damage)
			{
				Debug.Log("HitMe : " + _bugsAction.CrowdControl.ToString());

				StateStart(_bugsAction.CrowdControl[0].CrowdControlType);
			}
		}

		// 피격 이펙트
		m_HitEffect.PlayEffect(_damage);

		m_Health.HealthDecrease(_damage, () =>
		{
			if (m_iTempRevivalCount <= 0)
			{
				StateStart(eBugsStateType.Die);
				m_iTempRevivalCount = 1;

				return;
			}
			else
			{
				m_iTempRevivalCount = 0;
				m_Health.SetHealth(10);
				StateStart(eBugsStateType.Groggy);
			}
		});
	}

	public virtual void EatItem(int _itemType, int _itemValue)
	{
		m_EventHandler_Hero.UIGetBuff.Send(_itemType);

		switch (_itemType)
		{
			case csItemManager.SpeedType:

				m_Move.MoveSpeed += _itemValue;

				break;

			case csItemManager.PowerType:

				m_Power.PowerIncrease(_itemValue);

				break;

			case csItemManager.HealType:

				m_Health.HealthIncrease(_itemValue);

				break;

			case csItemManager.SkillType:

				//AAAAA
				/*m_EventHandler_Hero.Index_Action_Skill_Item.Set(_itemValue);

				if(!IsActiveState(eBugsStateType.Pet))
				{
					JoystickHandlerSettings(Index_Button_Skill_Item, _itemValue);
				}*/

				break;
		}
	}

	//BBBBB
	/*public void JoystickHandlerSettings(int _joyStickIndex, int _actionIndex)
	{
		BattleManager.m_BattleUIManager.BattleController.JoystickHandlerSettings(Hero.ObjectIndex, _joyStickIndex, _actionIndex);
	}*/

	protected virtual void Groggy_Start()
	{
		Debug.Log("Groggy");

		m_EventHandler_Hero.HUD_ActivateFront.Send(false);
		Utility.Activate(CharacterController, false);

		float EscapeableTime = Info_Bugs.m_BugsAction[Index_Action_Escape].ActivationTime;

		m_Timer.OnTime(EscapeableTime, () =>
		{
			StateStart(eBugsStateType.Die);
		});

		//AAAAA
		/*JoystickHandlerSettings(Index_Button_Attack, Index_Action_None);
		JoystickHandlerSettings(Index_Button_Skill_Item, Index_Action_None);
		JoystickHandlerSettings(Index_Button_Skill_Default, Index_Action_None);*/

		//BBBBB
		/*JoystickHandlerSettings(0, Index_Action_None);
		JoystickHandlerSettings(1, Index_Action_None);
		JoystickHandlerSettings(2, Index_Action_None);
		JoystickHandlerSettings(3, Index_Action_None);*/

		//BBBBB
		//BattleManager.m_BattleUIManager.BattleController.m_MoveController.Deactivate();

		//BattleManager.m_BattleUIManager.BattleController.m_JoystickHandler[Index_Button_Multi].Deactivate();
		BattleManager.m_BattleUIManager.BattleController.m_MoveController.Deactivate();
		BattleManager.m_BattleUIManager.BattleController.m_JoystickHandler[Index_Button_Skill_Default].Deactivate();
		BattleManager.m_BattleUIManager.BattleController.m_JoystickHandler[3].Deactivate();
		BattleManager.m_BattleUIManager.BattleController.m_JoystickHandler[Index_Button_Escape].Activate();
	}

	protected virtual void Groggy_Stop()
	{
		m_Timer.OffTime();
	}

	protected virtual void Escape_Start()
	{		
		Pet.Transform.parent = null;

		//Vector3 EscapeDirection = m_EventHandler.ActionDirection_Original.Get() + (Transform.up * 0.5f);
		Vector3 EscapeDirection = m_EventHandler_Hero.EscapeVelocity.Get();

		Pet.Escape(() =>
		{
			Debug.Log("Escape");
		
			StateStart(eBugsStateType.Pet);

		}, EscapeDirection);

		m_EventHandler_Hero.Camera_ChangeTarget.Send(Pet.Transform);

		m_Timer.OffTime();
		
		//BattleManager.m_BattleUIManager.BattleController.m_JoystickHandler[Index_Button_Multi].Activate();
		BattleManager.m_BattleUIManager.BattleController.m_MoveController.Activate();
		BattleManager.m_BattleUIManager.BattleController.m_JoystickHandler[Index_Button_Skill_Default].Activate();
		BattleManager.m_BattleUIManager.BattleController.m_JoystickHandler[3].Activate();
		BattleManager.m_BattleUIManager.BattleController.m_JoystickHandler[Index_Button_Escape].Deactivate();
	}

	protected virtual void Escape_Stop()
	{
		StateStop(eBugsStateType.Groggy);
	}

	public virtual void Revival()
	{
		StateStop(eBugsStateType.Pet);	
		//AllStateStop();
		StateStart(eBugsStateType.Hero);
		//StateStop(eBugsStateType.Revival);
	}

	protected virtual void ChangeToPet()
	{
		m_EventHandler_Hero.HUD_ActivateFront.Send(true);
		Utility.Activate(CharacterController, true);
		Hero.Transform.parent = null;
		Transform.position = Pet.Transform.position;
		Pet.Transform.parent = Transform;
		Pet.Transform.localPosition = Vector3.zero;
		Pet.Transform.localEulerAngles = Vector3.zero;
		Bugs = Pet;

		StateStop(eBugsStateType.Escape_Action);

		StartCoroutine(CoRevivalCheck());

		m_EventHandler_Hero.Camera_ChangeTarget.Send(Transform);
	}

	protected virtual void ChangeToHero()
	{
		Utility.Activate(Pet.gameObject, false);		
		Hero.Transform.parent = Transform;
		Hero.Transform.localPosition = Vector3.zero;
		Hero.Transform.localEulerAngles = Vector3.zero;
		Pet.Transform.position = Hero.m_PetTr.position;
		Bugs = Hero;
		Utility.Activate(Hero.gameObject, true);
		//StateStop(eBugsStateType.Hero);
	}

    public override void Restart()
    {
        base.Restart();

		if(IsActiveState(eBugsStateType.Pet))
        {
			StateStart(eBugsStateType.Revival);
        }

		if (IsActiveState(eBugsStateType.Groggy))
		{
			StateStop(eBugsStateType.Groggy);

			m_EventHandler_Hero.HUD_ActivateFront.Send(true);
			Utility.Activate(CharacterController, true);

			BattleManager.m_BattleUIManager.BattleController.m_MoveController.Activate();
			BattleManager.m_BattleUIManager.BattleController.m_JoystickHandler[Index_Button_Skill_Default].Activate();
			BattleManager.m_BattleUIManager.BattleController.m_JoystickHandler[3].Activate();
			BattleManager.m_BattleUIManager.BattleController.m_JoystickHandler[Index_Button_Escape].Deactivate();

			Hero.Animator.SetTrigger("Initialize");
		}

		m_HUD_Hero.Restart();
	}

    protected override void EndBattle()
	{
		BattleManager.EndBattle -= EndBattle;

		Release();
	}

	#region State Method

	protected virtual void OnStart_Waiting(Action _callback = null)
	{
		_callback?.Invoke();
	}

	protected virtual void OnStop_Waiting(Action _callback = null)
	{
		_callback?.Invoke();
	}

	protected override void OnStart_Run(Action _callback = null)
	{
		Bugs.RunStart();

		m_Move.LocalMoveStart();

		_callback?.Invoke();
	}

	protected override void OnStop_Run(Action _callback = null)
	{
		base.OnStop_Run(_callback);
		m_Move.LocalMoveStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Rotate(Action _callback = null)
	{
		m_Move.LocalRotateStart();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Rotate(Action _callback = null)
	{
		m_Move.LocalRotateStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Attack_Ready(Action _callback = null)
	{
		Hero.Attack_ReadyStart();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Attack_Ready(Action _callback = null)
	{
		Hero.Attack_ReadyStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Attack_Action(Action _callback = null)
	{
		Hero.Attack_ActionStart();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Attack_Action(Action _callback = null)
	{
		Hero.Attack_ActionStop();

		_callback?.Invoke();
	}

	protected override void OnStart_Attack(Action _callback = null)
	{
		Hero.Attack_Start();

		_callback?.Invoke();
	}

	protected override void OnStop_Attack(Action _callback = null)
	{
		Hero.Attack_Stop();

		_callback?.Invoke();
	}

	protected override void OnForceStop_Attack(Action _callback = null)
	{
		Hero.Attack_ForceStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Skill_1_Ready(Action _callback = null)
	{
		Hero.Skill_1_ReadyStart();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Skill_1_Ready(Action _callback = null)
	{
		Hero.Skill_1_ReadyStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Skill_1_Action(Action _callback = null)
	{
		Hero.Skill_1_ActionStart();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Skill_1_Action(Action _callback = null)
	{
		Hero.Skill_1_ActionStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Skill_1(Action _callback = null)
	{
		Hero.Skill_1_Start();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Skill_1(Action _callback = null)
	{
		Hero.Skill_1_Stop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Skill_2_Ready(Action _callback = null)
	{
		Hero.Skill_2_ReadyStart();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Skill_2_Ready(Action _callback = null)
	{
		Hero.Skill_2_ReadyStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Skill_2_Action(Action _callback = null)
	{
		Hero.Skill_2_ActionStart();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Skill_2_Action(Action _callback = null)
	{
		Hero.Skill_2_ActionStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Skill_2(Action _callback = null)
	{
		Hero.Skill_2_Start();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Skill_2(Action _callback = null)
	{
		Hero.Skill_2_Stop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Skill_3_Ready(Action _callback = null)
	{
		Hero.Skill_3_ReadyStart();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Skill_3_Ready(Action _callback = null)
	{
		Hero.Skill_3_ReadyStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Skill_3_Action(Action _callback = null)
	{
		Hero.Skill_3_ActionStart();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Skill_3_Action(Action _callback = null)
	{
		Hero.Skill_3_ActionStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Skill_3(Action _callback = null)
	{
		Hero.Skill_3_Start();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Skill_3(Action _callback = null)
	{
		Hero.Skill_3_Stop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Skill_Default_Ready(Action _callback = null)
	{
		Hero.Skill_Default_ReadyStart();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Skill_Default_Ready(Action _callback = null)
	{
		Hero.Skill_Default_ReadyStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Skill_Default_Action(Action _callback = null)
	{
		Hero.Skill_Default_ActionStart();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Skill_Default_Action(Action _callback = null)
	{
		Hero.Skill_Default_ActionStop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Skill_Default(Action _callback = null)
	{
		Hero.Skill_Default_Start();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Skill_Default(Action _callback = null)
	{
		Hero.Skill_Default_Stop();

		_callback?.Invoke();
	}

	protected override void OnStart_Slow(Action _callback = null)
	{
		Bugs.Slow_Start();

		_callback?.Invoke();
	}

	protected override void OnStop_Slow(Action _callback = null)
	{
		Bugs.Slow_Stop();

		_callback?.Invoke();
	}

	protected override void OnStart_Stun(Action _callback = null)
	{
		Debug.Log("OnStart_Stun : " + Transform.position);
		Bugs.Stun_Start();

		m_CrowdControl.StartCrowdControl(this, () =>
		{

		}, () =>
		{
			StateStop(eBugsStateType.Stun);
		});

		_callback?.Invoke();
	}

	protected override void OnStop_Stun(Action _callback = null)
	{
		Debug.Log("OnStop_Stun : " + Transform.position);
		Bugs.Stun_Stop();

		if (m_CrowdControl.m_CrowdControlType == eBugsStateType.Stun)
		{
			//m_CrowdControl = null;
		}

		_callback?.Invoke();
	}

	protected override void OnStart_KnockBack(Action _callback = null)
	{
		Debug.Log("OnStart_KnockBack");
		Bugs.KnockBack_Start();

		Vector3 Position = Transform.position;

		//m_Collision.RegisterTriggerCallback(HitCollider);

		Action KnockBackCollisionMap = () =>
		{
			m_CrowdControl.StopCrowdControl(this);

			Transform.position = Position;
			Debug.Log("Result Position : " + Transform.position);

			m_CrowdControl.m_Callback(this);
		};

		int MapLayerMask = csBattleManager.Instance.m_iLayerMask_Map;
		csKnockBack KnockBack = (csKnockBack)m_CrowdControl;
		m_TestRaycastDirection = KnockBack.m_KnockBackDirection;

		if (Physics.Raycast(Collider.bounds.center, m_TestRaycastDirection, m_TestRaycastRange, MapLayerMask))
		{
			Position = Transform.position;
			Debug.Log("m_bIsCollisionMap : " + Position);
			KnockBackCollisionMap();

			Debug.Log("Stop State : " + m_EventHandler_Hero.KnockBack);
			StateStop(eBugsStateType.KnockBack);

			return;
		}

		m_CrowdControl.StartCrowdControl(this, () =>
		{
			if (Physics.Raycast(Collider.bounds.center, m_TestRaycastDirection, m_TestRaycastRange, MapLayerMask))
			{
				Position = Transform.position;
				Debug.Log("m_bIsCollisionMap : " + Position);
				KnockBackCollisionMap();
			}
		}, () =>
		{
			Debug.Log("Stop State : " + m_EventHandler_Hero.KnockBack);
			StateStop(eBugsStateType.KnockBack);
		});

		_callback?.Invoke();
	}

	protected override void OnStop_KnockBack(Action _callback = null)
	{
		Debug.Log("OnStop_KnockBack : " + Transform.position);
		Bugs.KnockBack_Stop();

		//m_Collision.UnregisterTriggerCallback();

		//m_bIsCollisionMap = false;

		if (m_CrowdControl.m_CrowdControlType == eBugsStateType.KnockBack)
		{
			Debug.Log("m_CrowdControl.m_CrowdControlType == eBugsStateType.KnockBack");
			//m_CrowdControl = null;
		}

		_callback?.Invoke();
	}

	protected virtual void OnStart_Hero(Action _callback = null)
	{
		ChangeToHero();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Hero(Action _callback = null)
	{
		_callback?.Invoke();
	}

	protected virtual void OnStart_Pet(Action _callback = null)
	{
		ChangeToPet();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Pet(Action _callback = null)
	{
		_callback?.Invoke();
	}

	protected virtual void OnStart_Groggy(Action _callback = null)
	{
		Hero.Groggy();

		Groggy_Start();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Groggy(Action _callback = null)
	{
		Groggy_Stop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Escape_Ready(Action _callback = null)
	{
		Debug.Log("OnStart_Escape_Ready");
		Hero.Escape_ReadyStart_Local();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Escape_Ready(Action _callback = null)
	{
		Debug.Log("OnStop_Escape_Ready");
		Hero.Escape_ReadyStop_Local();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Escape_Action(Action _callback = null)
	{
		Debug.Log("OnStart_Escape_Action");
		Hero.Escape_ActionStart_Local();

		Escape_Start();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Escape_Action(Action _callback = null)
	{
		Debug.Log("OnStop_Escape_Action");
		Hero.Escape_ActionStop_Local();

		Escape_Stop();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Escape(Action _callback = null)
	{
		Hero.Escape_Start_Local();

		Escape_Start();

		_callback?.Invoke();
	}

	protected virtual void OnStop_Escape(Action _callback = null)
	{
		Hero.Escape_Stop_Local();

		_callback?.Invoke();
	}

	protected virtual void OnStart_Revival(Action _callback = null)
	{
		Revival();
		Hero.Revival_Start();

		m_EventHandler_Hero.Camera_ChangeTarget.Send(m_CameraTarget);
		m_EventHandler_Hero.ChangeMoveSpeed.Send(0.1f);
		m_EventHandler_Hero.ChangeRotationSpeed.Send(0.1f);

		_callback?.Invoke();
	}

	protected virtual void OnStop_Revival(Action _callback = null)
	{
		StateStop(eBugsStateType.Hero);

		Hero.Revival_Stop();

		m_EventHandler_Hero.ChangeMoveSpeed.Send(10.0f);
		m_EventHandler_Hero.ChangeRotationSpeed.Send(10.0f);
		//AAAAA
		/*JoystickHandlerSettings(Index_Button_Attack, Index_Action_Attack);
		JoystickHandlerSettings(Index_Button_Skill_Item, LocalHandler.Index_Skill_Item.Get());
		JoystickHandlerSettings(Index_Button_Skill_Default, Index_Action_Skill_Default);*/

		//BBBBB
		/*JoystickHandlerSettings(0, Index_Action_Attack);
		JoystickHandlerSettings(1, 1);
		JoystickHandlerSettings(2, 2);
		JoystickHandlerSettings(3, 5);*/

		_callback?.Invoke();
	}

	/*2018.08.07
    * hkh
    * 사망 상태 시작할 때 호출되는 메소드(이벤트 csActivity)
    */
	protected override void OnStart_Die(Action _callback = null)
	{
		Release();
		Bugs.Die();

		if(IsActiveState(eBugsStateType.Pet))
		{
			Hero.Transform.parent = Transform;

			float Delay = Hero.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

			m_Timer.OnDelay(Delay, () =>
			{
				Utility.Activate(Hero.gameObject, false);
			});					
		}

		BattleManager.GameOver();
		BattleManager.m_BattleUIManager.BattleController.m_JoystickHandler[Index_Button_Escape].Deactivate();

		_callback?.Invoke();
	}

	/*2018.08.07
    * hkh
    * 사망 상태 종료될 때 호출되는 메소드(이벤트 csActivity)
    */
	protected override void OnStop_Die(Action _callback = null)
	{
		_callback?.Invoke();
	}

	#endregion

	#region Command Method

	#endregion

	#region Coroutine

	protected IEnumerator CoCheckGround(Action _callback)
	{
		while(true)
		{
			yield return null;

			if(CharacterController.isGrounded)
			{
				_callback?.Invoke();

				break;
			}
		}
	}
	
	protected IEnumerator CoRevivalCheck()
	{
		float Distance = 0.0f;

		while(true)
		{
			yield return null;

			Distance = Vector3.Distance(Transform.position, Hero.Transform.position);

			if(Distance <= 2.0f)
			{
				StateStart(eBugsStateType.Revival);

				break;
			}
		}
	}

	#endregion
}
