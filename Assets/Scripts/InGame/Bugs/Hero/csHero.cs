using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

public abstract class csHero : csBugs
{
	public Transform m_PetTr = null;

	protected csEventHandler_Hero m_EventHandler_Hero = null;

	public csOwner_Hero m_Owner_Hero;

	protected ObjectDetection m_ObjectDetection = null;

	protected csActionRangeRenderer m_ActionRangeRenderer = null;

	protected CountNumber m_CountNumber = null;

	[SerializeField]
	private csInfo_Hero m_Info_Hero;
	public csInfo_Hero Info_Hero
	{
		get
		{
			return m_Owner_Hero.Info_Hero;
		}
	}

	protected csActivity_Dash m_Activity_Dash = null;

	private csCharge m_Charge = null;
	protected csCharge Charge
	{
		get
		{
			if(m_Charge == null)
			{
				m_Charge = GetComponent<csCharge>();

				if(m_Charge == null)
				{
					m_Charge = gameObject.AddComponent<csCharge>();
				}
			}

			return m_Charge;
		}
	}

	protected BugsAction m_BugsAction_Attack;
	protected BugsAction m_BugsAction_Skill_1;
	protected BugsAction m_BugsAction_Skill_2;
	protected BugsAction m_BugsAction_Skill_Default;

	public float m_fFOVDistance = 50.0f;
	public bool m_bIsAttack = false;

	protected int m_iLayerMask_Monster = 0;
	protected int m_iLayerMask_Map = 0;

	protected override void Awake()
	{
		base.Awake();

		m_CountNumber = new CountNumber();	
	}

	public override void Settings(csOwner _owner)
	{
		base.Settings(_owner);

		m_Owner_Hero = (csOwner_Hero)m_Owner;

		m_ObjectDetection = new ObjectDetection();

		GetBugsAction();

		m_RotationToTarget.Settings(m_Owner_Hero);
		m_Activity.Settings(m_Owner_Hero);

		m_Activity_Dash = GetComponent<csActivity_Dash>();
		m_Activity_Dash.Settings(m_Owner_Hero);
		Charge.Settings(m_Owner_Hero);

		m_EventHandler_Hero = m_Owner_Hero.m_EventHandler_Hero;

		m_iLayerMask_Monster = m_BattleManager.m_iLayerMask_Monster;
		m_iLayerMask_Map = m_BattleManager.m_iLayerMask_Map;

		for(int i = 0; i < Animator.runtimeAnimatorController.animationClips.Length; i++)
		{
			//Debug.LogError(Animator.runtimeAnimatorController.animationClips[i].name);		
		}

		BugsCollider BugsCollider = Info_Hero.m_BugsCollider;

		m_Owner.CharacterController.center = BugsCollider.Center;
		m_Owner.CharacterController.radius = BugsCollider.Radius;
		m_Owner.CharacterController.height = BugsCollider.Height;

		//StateHandlerSettings();

		//BBBBB
		StateHandlerSettings(csOwner_Hero.Index_Action_Attack);
		StateHandlerSettings(csOwner_Hero.Index_Action_Skill_Default);
	}

	public override void Release()
	{
		base.Release();

		StopAllCoroutines();
	}

	protected override void GetBugsAction()
	{
		m_BugsAction_Attack = Info_Hero.m_BugsAction[csOwner_Hero.Index_Action_Attack];
		m_BugsAction_Skill_1 = Info_Hero.m_BugsAction[csOwner_Hero.Index_Action_Skill_1];
		m_BugsAction_Skill_2 = Info_Hero.m_BugsAction[csOwner_Hero.Index_Action_Skill_2];
		m_BugsAction_Skill_Default = Info_Hero.m_BugsAction[csOwner_Hero.Index_Action_Skill_Default];
	}

	// 공격, 스킬 개별 설정(현재 사용)
	protected virtual void StateHandlerSettings(int _index)
	{
		StateHandlerSettings_ByState StateHandlerSettings_ByState = Info_Hero.m_StateHandlerSettings_ByState[_index];

		csState State = m_EventHandler_Hero.GetState(StateHandlerSettings_ByState.StateActionType);
		State.ClearLockState();

		for (int i = 0; i < StateHandlerSettings_ByState.LockStates.Length; i++)
		{
			State.AddLockState(StateHandlerSettings_ByState.LockStates[i]);
		}

		csState ReservationCancel = m_EventHandler_Hero.GetState(StateHandlerSettings_ByState.ReservationCancelType);

		if(ReservationCancel != null)
		{
			State.m_ReservationCancel = ReservationCancel;
		}
	}

	// 공격, 구르기 설정(지금 사용 안 함)
	protected virtual void StateHandlerSettings()
	{
		StateHandlerSettings_Multi StateHandlerSettings_Multi = Info_Hero.m_StateHandlerSettings_Multi;

		csState Attack = m_EventHandler_Hero.GetState(StateHandlerSettings_Multi.Attack);

		Attack.ClearLockState();

		eBugsStateType[] AttackLockStateTypes = StateHandlerSettings_Multi.DicStateLockStateBindings[StateHandlerSettings_Multi.Attack];

		for(int i = 0; i < AttackLockStateTypes.Length; i++)
		{
			Attack.AddLockState(AttackLockStateTypes[i]);
		}

		csState AttackReservationCancel = m_EventHandler_Hero.GetState(StateHandlerSettings_Multi.DicStateReservationCancelBindings[StateHandlerSettings_Multi.Attack]);

		if (AttackReservationCancel != null)
		{
			Attack.m_ReservationCancel = AttackReservationCancel;
		}

		csState Skill_Default = m_EventHandler_Hero.GetState(StateHandlerSettings_Multi.Skill_Default);
		Skill_Default.ClearLockState();

		eBugsStateType[] Skill_DefaultLockStateTypes = StateHandlerSettings_Multi.DicStateLockStateBindings[StateHandlerSettings_Multi.Skill_Default];

		for (int i = 0; i < Skill_DefaultLockStateTypes.Length; i++)
		{
			Skill_Default.AddLockState(Skill_DefaultLockStateTypes[i]);
		}

		csState Skill_DefaultReservationCancel = m_EventHandler_Hero.GetState(StateHandlerSettings_Multi.DicStateReservationCancelBindings[StateHandlerSettings_Multi.Skill_Default]);

		if (Skill_DefaultReservationCancel != null)
		{
			Skill_Default.m_ReservationCancel = Skill_DefaultReservationCancel;
		}
	}

	public void StartBattle()
	{
		StartCoroutine(CoDetectionClosestObject());
	}

	protected virtual void UpdateHeroInfo(BugsAction _bugsAction)
	{
		Info_Hero.m_BugsAction[_bugsAction.ActionIndex] = _bugsAction;
	}

	#region Animation Event

	#endregion

	#region Local Hero Action

	public virtual void Attack_ReadyStart()
	{
		Debug.Log("Attack_ReadyStart");
	}

	public virtual void Attack_ReadyStop()
	{

	}

	public override void Attack_ActionStart()
	{
		base.Attack_ActionStart();
	}

	public override void Attack_ActionStop()
	{
		base.Attack_ActionStop();
	}

	public override void Attack_Start()
	{
		base.Attack_Start();
	}

	public override void Attack_Stop()
	{
		base.Attack_Stop();
	}

	public override void Attack_ForceStop()
	{
		base.Attack_ForceStop();
	}

	public virtual void Skill_1_ReadyStart()
	{

	}

	public virtual void Skill_1_ReadyStop()
	{

	}

	public virtual void Skill_1_ActionStart()
	{

	}

	public virtual void Skill_1_ActionStop()
	{

	}

	public virtual void Skill_1_Start()
	{

	}

	public virtual void Skill_1_Stop()
	{

	}

	public virtual void Skill_2_ReadyStart()
	{

	}

	public virtual void Skill_2_ReadyStop()
	{

	}

	public virtual void Skill_2_ActionStart()
	{

	}

	public virtual void Skill_2_ActionStop()
	{

	}

	public virtual void Skill_2_Start()
	{

	}

	public virtual void Skill_2_Stop()
	{

	}

	public virtual void Skill_3_ReadyStart()
	{

	}

	public virtual void Skill_3_ReadyStop()
	{

	}

	public virtual void Skill_3_ActionStart()
	{
		
	}

	public virtual void Skill_3_ActionStop()
	{

	}

	public virtual void Skill_3_Start()
	{

	}

	public virtual void Skill_3_Stop()
	{

	}

	public virtual void Skill_Default_ReadyStart()
	{

	}

	public virtual void Skill_Default_ReadyStop()
	{

	}

	public virtual void Skill_Default_ActionStart()
	{

	}

	public virtual void Skill_Default_ActionStop()
	{

	}

	public virtual void Skill_Default_Start()
	{

	}

	public virtual void Skill_Default_Stop()
	{

	}

	public virtual void Groggy()
	{
		Animator.SetTrigger("Groggy");

		m_AnimationEventCallback = null;
	}

	public virtual void Escape_ReadyStart_Local()
	{
		m_ActionRangeRenderer.RenderStart_Escape(csOwner_Hero.Index_Action_Escape);
	}

	public virtual void Escape_ReadyStop_Local()
	{
		m_ActionRangeRenderer.RenderStop_Escape();
	}

	public virtual void Escape_ActionStart_Local()
	{
		Animator.SetTrigger("Escape");
	}

	public virtual void Escape_ActionStop_Local()
	{

	}

	public virtual void Escape_Start_Local()
	{

	}

	public virtual void Escape_Stop_Local()
	{

	}

	public virtual void Escape()
	{
		Animator.SetTrigger("Escape");
	}

	public virtual void Revival_Start()
	{
		Animator.SetTrigger("Revival");

		Utility.Activate(m_Effect[csOwner_Hero.Index_Effect_Revival], true);
	}

	public virtual void Revival_Stop()
	{
		Utility.Activate(m_Effect[csOwner_Hero.Index_Effect_Revival], false);
	}

	#endregion

	protected void HeroActionEnter(BugsAction _bugsAction)
	{
		Vector3 Target = m_EventHandler_Hero.Hero_CurrentAimTarget.Get().Transform.position;
		Vector3 ActionDirection = Utility.RelativePosition(m_Owner.Transform.position, Target).normalized;

		m_Owner_Hero.m_EventHandler_Hero.ActionDirection_Enter.Set(ActionDirection);

		LookAtDirection(ActionDirection, () =>
		{
			if(!m_Owner.IsActiveState(_bugsAction.StateType))
			{
				return;
			}

			Animator.SetTrigger(_bugsAction.AnimationName);
		});
	}

	protected Vector3 DetectionTargetWithinRadius()
	{
		Vector3 ActionDirection = Vector3.zero;

		bool IsDetectionClosestObject = m_ObjectDetection.DetectionClosestObject
				(m_Owner.BugsCenter, m_BugsAction_Skill_1.Range, out csOwner _owner, m_iLayerMask_Monster);

		if (IsDetectionClosestObject)
		{
			Vector3 RelativePosition = Utility.RelativePosition(m_Owner_Hero.Transform.position, _owner.Transform.position);
			RelativePosition.y = 0.0f;
			ActionDirection = RelativePosition;
		}
		else
		{
			ActionDirection = m_Owner.Transform.forward;
		}

		return ActionDirection;
	}

	protected void LookAt_Target()
	{
		Vector3 ActionDirection = DetectionTargetWithinRadius();

		m_EventHandler_Hero.ActionDirection.Set(ActionDirection);

		LookAtDirection(ActionDirection);	
	}

	protected void LookAt_ActionDirection()
	{
		Vector3 ActionDirection = m_EventHandler_Hero.ActionDirection.Get();

		LookAtDirection(ActionDirection);
	}
	
	protected void LookAt_MoveDirection()
	{
		Vector3 MoveDirection = m_EventHandler_Hero.MoveDirection.Get();

		LookAtDirection(MoveDirection);
	}

	protected void Dash_Target_Start()
	{
		//Vector3 ActionDirection = DetectionTargetWithinRadius();

		Vector3 Target = m_EventHandler_Hero.Hero_CurrentAimTarget.Get().Transform.position;
		Vector3 ActionDirection = Utility.RelativePosition(m_Owner.Transform.position, Target).normalized;

		m_EventHandler_Hero.ActionDirection.Set(ActionDirection);

		LookAtDirection(ActionDirection, () =>
		{
			m_Activity_Dash.MoveStart(ActionDirection.normalized, m_BugsAction_Skill_1.Range, 0.1f, false);
		});
	}

	protected void Dash_Target_Stop()
	{
		m_Activity_Dash.MoveStop();
	}

	protected void LookAtDirection(Vector3 _direction, Action _callback = null)
	{
		if (_direction != Vector3.zero)
		{
			RotateToTarget(m_Owner_Hero.Transform, _direction, 0.0f, () =>
			{
				_callback?.Invoke();
			});
		}
		else
		{
			_callback?.Invoke();
		}
	}

	protected void Tumble_Start()
	{
		Vector3 TumbleDirection = m_EventHandler_Hero.ActionDirection.Get();

		if(TumbleDirection == Vector3.zero)
		{
			TumbleDirection = m_Owner.Transform.forward;
		}
		Debug.LogError("TumbleStart");
		LookAtDirection(TumbleDirection, () =>
		{
			Debug.LogError("PlayAnimation");
			Animator.SetTrigger(m_BugsAction_Skill_Default.AnimationName);
		});

		m_AnimationEventCallback = () =>
		{
			//Debug.LogError(TumbleDirection + "  " + m_BugsAction_Skill_Default.Range);
			m_Activity.MoveStart(TumbleDirection, m_BugsAction_Skill_Default.Range, 0.5f, true);
		};
	}

	#region Coroutine

	protected virtual IEnumerator CoDetectionClosestObject()
	{
		Vector3 MoveDirection = Vector3.zero;
		Vector3 ActionDirection = Vector3.zero;
		while(true)
		{
			yield return YieldCache.WaitForFixedUpdate;
		
			MoveDirection = m_Owner.m_EventHandler_Bugs.MoveDirection_Usable.Get();
			//Debug.LogError(Animator.GetCurrentAnimatorStateInfo(0).IsName("RUN"));
			// 이동 중일 때
			if(m_Owner.IsActiveState(eBugsStateType.Run))
			{
				bool IsDetectionClosestObject = m_ObjectDetection.DetectionClosestObject_MoveDirection
				(m_Owner.BugsCenter, MoveDirection, m_BugsAction_Skill_1.Range - 2.0f, Info_Hero.m_BugsCollider.Radius, out csOwner _owner, m_iLayerMask_Monster);

				m_EventHandler_Hero.Hero_CurrentAimTarget.Set(_owner);

				m_EventHandler_Hero.HUD_AimTarget.Send(IsDetectionClosestObject);

				if(IsDetectionClosestObject)
				{
					m_bIsAttack = true;
					
					Debug.LogError("m_bIsAttack = true");

					if(!m_Owner.IsActiveState(eBugsStateType.Attack) && !Animator.GetCurrentAnimatorStateInfo(1).IsName("COMBO_3"))
					{
						m_Owner.StateStart(eBugsStateType.Attack);
					}					
				}
			}
			else
			{
				if(m_Owner.IsActiveState(eBugsStateType.Attack))
				{
					if(MoveDirection == Vector3.zero && m_bIsAttack)
					{
						//Debug.LogError("MoveDirection == Vector3.zero && m_bIsCheckCombo");
						//m_Owner.StateForceStop(eBugsStateType.Attack);

						m_bIsAttack = false;

						//m_Owner.StateStop(eBugsStateType.Attack);

						continue;
					}

					ActionDirection = m_Owner_Hero.m_EventHandler_Hero.ActionDirection_Enter.Get();

					if(Vector3.Distance(MoveDirection, ActionDirection) > 1.0f && m_bIsAttack)
					{
						//Debug.LogError("Prev > 1.0f && m_bIsCheckCombo");
						//m_Owner.StateForceStop(eBugsStateType.Attack);

						m_bIsAttack = false;

						//m_Owner.StateStop(eBugsStateType.Attack);

						continue;
					}

					if(m_Owner.IsActiveState(eBugsStateType.Skill_Default_Action))
					{
						//Debug.LogError("m_Owner.IsActiveState(eBugsStateType.Skill_Default_Action) && m_bIsCheckCombo");
						m_Owner.StateForceStop(eBugsStateType.Attack);

						continue;
					}
				}
			}
		}
	}

	#endregion
}
