using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csInfo_Gunner : csInfo_Hero
{
	public csInfo_Gunner()
	{
		BugsCollider GunnerCollider;

		GunnerCollider.Center = new Vector3(0.0f, 1.0f, 0.0f);
		GunnerCollider.Radius = 0.5f;
		GunnerCollider.Height = 2.0f;

		m_BugsCollider = GunnerCollider;

		//Settings_Action();

		Settings_Attack();
		Settings_Skill_1();
		Settings_Skill_Default();
	}

	private void Settings_Action()
	{
		// 0 : Attack
		// 1 : Escape
		// 2 : Default
		// 3 : none

		// BugsAction_Attack
		BugsAction BugsAction_Attack = m_BugsAction[csOwner_Hero.Index_Action_Attack];

		BugsAction_Attack.CrowdControl = new CrowdControl[]
		{
			new CrowdControl_KnockBack(eBugsStateType.KnockBack, 5.0f, 0.5f)
		};

		BugsAction_Attack.AnimationName = "Attack";

		BugsAction_Attack.Range = 30.0f;
		BugsAction_Attack.ActivationTime = 0.0f;
		BugsAction_Attack.CoolTime = 0.0f;

		BugsAction_Attack.ActionIndex = csOwner_Hero.Index_Action_Attack;

		BugsAction_Attack.IsActivation = false;
		BugsAction_Attack.IsImmediatelyCoolDown = false;

		m_BugsAction[csOwner_Hero.Index_Action_Attack] = BugsAction_Attack;

		// BugsAction_Skill_1
		BugsAction BugsAction_Skill_1 = m_BugsAction[csOwner_Hero.Index_Action_Skill_1];

		BugsAction_Skill_1.CrowdControl = new CrowdControl[]
		{
			new CrowdControl_KnockBack(eBugsStateType.KnockBack, 1.5f, 0.1f)
		};

		BugsAction_Skill_1.AnimationName = "Attack";

		BugsAction_Skill_1.Range = 10.0f;
		BugsAction_Skill_1.ActivationTime = 0.0f;
		BugsAction_Skill_1.CoolTime = 0.0f;

		BugsAction_Skill_1.ActionIndex = csOwner_Hero.Index_Action_Skill_1;

		BugsAction_Skill_1.IsActivation = false;
		BugsAction_Skill_1.IsImmediatelyCoolDown = false;

		m_BugsAction[csOwner_Hero.Index_Action_Skill_1] = BugsAction_Skill_1;

		// BugsAction_Skill_Default
		BugsAction BugsAction_Skill_Default = m_BugsAction[csOwner_Hero.Index_Action_Skill_Default];

		BugsAction_Skill_Default.CrowdControl = null;

		BugsAction_Skill_Default.AnimationName = "Skill_Default";

		BugsAction_Skill_Default.Range = 10.0f;
		BugsAction_Skill_Default.ActivationTime = 0.0f;
		BugsAction_Skill_Default.CoolTime = 2.0f;

		BugsAction_Skill_Default.ActionIndex = csOwner_Hero.Index_Action_Skill_Default;

		BugsAction_Skill_Default.IsActivation = false;
		BugsAction_Skill_Default.IsImmediatelyCoolDown = true;

		m_BugsAction[csOwner_Hero.Index_Action_Skill_Default] = BugsAction_Skill_Default;

		Dictionary<TouchEvent, StateControlType> DicTouchStateHandlerBindings = new Dictionary<TouchEvent, StateControlType>(new TouchEventCompare())
		{
			{ TouchEvent.TouchBegan, StateControlType.TouchBegan },
			{ TouchEvent.TouchMoved, StateControlType.TouchMoved },
			{ TouchEvent.TouchStationary, StateControlType.TouchStationary },
			{ TouchEvent.TouchEnded, StateControlType.TouchEnded }
		};

		eBugsStateType[] LockState_Attack =
			new eBugsStateType[]
			{
				eBugsStateType.Walk,
				eBugsStateType.Run,
				eBugsStateType.Rotate,
			};

		eBugsStateType[] LockState_Skill_1 =
			new eBugsStateType[]
			{
				eBugsStateType.Walk,
				eBugsStateType.Run,
				eBugsStateType.Rotate,
			};

		eBugsStateType[] LockState_Skill_Default =
			new eBugsStateType[]
			{
				eBugsStateType.Walk,
				eBugsStateType.Run,
				eBugsStateType.Rotate,
				eBugsStateType.Attack
			};

		Dictionary<eBugsStateType, eBugsStateType[]> DicStateLockStateBindings = new Dictionary<eBugsStateType, eBugsStateType[]>(new BugsStateTypeCompare())
		{
			{ eBugsStateType.Attack, LockState_Attack },
			{ eBugsStateType.Skill_1, LockState_Skill_1 },
			{ eBugsStateType.Skill_Default, LockState_Skill_Default }
		};

		Dictionary<eBugsStateType, eBugsStateType> DicStateReservationCancelBindings = new Dictionary<eBugsStateType, eBugsStateType>(new BugsStateTypeCompare())
		{
			{ eBugsStateType.Attack, eBugsStateType.Run },
			{ eBugsStateType.Skill_1, eBugsStateType.Run },
			{ eBugsStateType.Skill_Default, eBugsStateType.Run }
		};

		m_StateHandlerSettings_Multi = new StateHandlerSettings_Multi(DicTouchStateHandlerBindings, DicStateLockStateBindings, DicStateReservationCancelBindings);
	}

	private void Settings_Attack()
	{
		// BugsAction_Attack
		BugsAction BugsAction_Attack = m_BugsAction[csOwner_Hero.Index_Action_Attack];

		BugsAction_Attack.StateType = eBugsStateType.Attack;

		BugsAction_Attack.CrowdControl = new CrowdControl[]
		{
			new CrowdControl_KnockBack(eBugsStateType.KnockBack, 2.0f, 0.5f)
		};

		BugsAction_Attack.AnimationName = "Attack";

		BugsAction_Attack.Range = 30.0f;
		BugsAction_Attack.ActivationTime = 0.0f;
		BugsAction_Attack.CoolTime = 0.0f;

		BugsAction_Attack.ActionIndex = csOwner_Hero.Index_Action_Attack;

		BugsAction_Attack.IsActivation = false;
		BugsAction_Attack.IsImmediatelyCoolDown = false;

		m_BugsAction[csOwner_Hero.Index_Action_Attack] = BugsAction_Attack;

		// StateHandlerSettings_ByState_Attack
		JoystickHandlerSettings JoystickHandlerSettings = new JoystickHandlerSettings(JoystickType.Static, false);

		Dictionary<TouchEvent, StateControlType> DicTouchStateHandlerBindings = new Dictionary<TouchEvent, StateControlType>(new TouchEventCompare())
		{
			{ TouchEvent.TouchBegan, StateControlType.StateAction },
			{ TouchEvent.TouchMoved, StateControlType.StateUpdate },
			{ TouchEvent.TouchStationary, StateControlType.StateUpdate },
			{ TouchEvent.TouchEnded, StateControlType.StateEnded },
			{ TouchEvent.TouchEnded_Additional, StateControlType.StateStop}
		};

		eBugsStateType[] LockState =
			new eBugsStateType[]
			{
				eBugsStateType.Walk,
				eBugsStateType.Run,
				eBugsStateType.Rotate,
				eBugsStateType.Skill_1_Action,
				eBugsStateType.Skill_2_Action,
				eBugsStateType.Skill_3_Action,
			};

		eBugsStateType StateReady = eBugsStateType.None;
		eBugsStateType StateAction = eBugsStateType.Attack;
		eBugsStateType StateUpdate = eBugsStateType.None;
		eBugsStateType ReservationCancel = eBugsStateType.None;

		m_StateHandlerSettings_ByState[csOwner_Hero.Index_Action_Attack] = new StateHandlerSettings_ByState(
			JoystickHandlerSettings,
			DicTouchStateHandlerBindings,
			LockState,
			StateReady,
			StateAction,
			StateUpdate,
			ReservationCancel);
	}

	private void Settings_Skill_1()
	{
		// BugsAction_Skill_1
		BugsAction BugsAction_Skill_1 = m_BugsAction[csOwner_Hero.Index_Action_Skill_1];

		BugsAction_Skill_1.StateType = eBugsStateType.Skill_1_Action;

		BugsAction_Skill_1.CrowdControl = new CrowdControl[]
		{
			new CrowdControl_KnockBack(eBugsStateType.KnockBack, 1.5f, 0.1f)
		};

		BugsAction_Skill_1.AnimationName = "Attack";

		BugsAction_Skill_1.Range = 10.0f;
		BugsAction_Skill_1.ActivationTime = 0.0f;
		BugsAction_Skill_1.CoolTime = 0.0f;

		BugsAction_Skill_1.ActionIndex = csOwner_Hero.Index_Action_Skill_1;

		BugsAction_Skill_1.IsActivation = false;
		BugsAction_Skill_1.IsImmediatelyCoolDown = false;

		m_BugsAction[csOwner_Hero.Index_Action_Skill_1] = BugsAction_Skill_1;

		// StateHandlerSettings_ByState_Skill_1
		JoystickHandlerSettings JoystickHandlerSettings = new JoystickHandlerSettings(JoystickType.Static, false);

		eBugsStateType StateReady = eBugsStateType.Skill_1_Ready;
		eBugsStateType StateAction = eBugsStateType.Skill_1_Action;
		eBugsStateType StateUpdate = eBugsStateType.Skill_1_Ready;
		eBugsStateType ReservationCancel = eBugsStateType.None;

		Dictionary<TouchEvent, StateControlType> DicTouchStateHandlerBindings = new Dictionary<TouchEvent, StateControlType>(new TouchEventCompare())
		{
			{ TouchEvent.TouchBegan, StateControlType.StateReady },
			{ TouchEvent.TouchEnded, StateControlType.StateAction },
			{ TouchEvent.TouchEnded_Additional, StateControlType.StateEnded }
		};

		eBugsStateType[] LockState =
			new eBugsStateType[]
			{
				eBugsStateType.Walk,
				eBugsStateType.Run,
				eBugsStateType.Attack_Action,
				eBugsStateType.Skill_2_Action,
				eBugsStateType.Skill_3_Action,
				eBugsStateType.Skill_Default_Action
			};

		m_StateHandlerSettings_ByState[csOwner_Hero.Index_Action_Skill_1] = new StateHandlerSettings_ByState(
			JoystickHandlerSettings,
			DicTouchStateHandlerBindings,
			LockState,
			StateReady,
			StateAction,
			StateUpdate,
			ReservationCancel);
	}

	private void Settings_Skill_Default()
	{
		// BugsAction_Skill_Default
		BugsAction BugsAction_Skill_Default = m_BugsAction[csOwner_Hero.Index_Action_Skill_Default];

		BugsAction_Skill_Default.StateType = eBugsStateType.Skill_Default_Action;

		BugsAction_Skill_Default.CrowdControl = null;

		BugsAction_Skill_Default.AnimationName = "Skill_Default";

		BugsAction_Skill_Default.Range = 10.0f;
		BugsAction_Skill_Default.ActivationTime = 0.0f;
		BugsAction_Skill_Default.CoolTime = 0.0f;

		BugsAction_Skill_Default.ActionIndex = csOwner_Hero.Index_Action_Skill_Default;

		BugsAction_Skill_Default.IsActivation = false;
		BugsAction_Skill_Default.IsImmediatelyCoolDown = true;

		m_BugsAction[csOwner_Hero.Index_Action_Skill_Default] = BugsAction_Skill_Default;

		// StateHandlerSettings_ByState_Skill_Default
		JoystickHandlerSettings JoystickHandlerSettings = new JoystickHandlerSettings(JoystickType.Static, false);

		Dictionary<TouchEvent, StateControlType> DicTouchStateHandlerBindings = new Dictionary<TouchEvent, StateControlType>(new TouchEventCompare())
		{
			{ TouchEvent.TouchBegan, StateControlType.StateReady },
			{ TouchEvent.TouchEnded, StateControlType.StateAction},
			{ TouchEvent.TouchEnded_Additional, StateControlType.StateEnded }
		};

		eBugsStateType[] LockState =
			new eBugsStateType[]
			{
				eBugsStateType.Walk,
				eBugsStateType.Run,
				eBugsStateType.Rotate,
				//eBugsStateType.Attack,
				eBugsStateType.Skill_1_Action,
				eBugsStateType.Skill_2_Action
			};

		eBugsStateType StateReady = eBugsStateType.Skill_Default_Ready;
		eBugsStateType StateAction = eBugsStateType.Skill_Default_Action;
		eBugsStateType StateUpdate = eBugsStateType.None;
		eBugsStateType ReservationCancel = eBugsStateType.Run;

		m_StateHandlerSettings_ByState[csOwner_Hero.Index_Action_Skill_Default] = new StateHandlerSettings_ByState(
			JoystickHandlerSettings,
			DicTouchStateHandlerBindings,
			LockState,
			StateReady,
			StateAction,
			StateUpdate,
			ReservationCancel);
	}

	private void Settings_None()
	{
		// BugsAction_None
		BugsAction BugsAction_None = m_BugsAction[csOwner_Hero.Index_Action_None];

		BugsAction_None.CrowdControl = null;

		BugsAction_None.AnimationName = "";

		BugsAction_None.Range = 0.0f;
		BugsAction_None.ActivationTime = 0.0f;
		BugsAction_None.CoolTime = 0.0f;

		BugsAction_None.ActionIndex = csOwner_Hero.Index_Action_None;

		BugsAction_None.IsActivation = false;
		BugsAction_None.IsImmediatelyCoolDown = true;

		m_BugsAction[csOwner_Hero.Index_Action_None] = BugsAction_None;

		// StateHandlerSettings_ByState_None
		StateHandlerSettings_ByState StateHandlerSettings_ByState_None = m_StateHandlerSettings_ByState[csOwner_Hero.Index_Action_None];

		StateHandlerSettings_ByState_None.JoystickHandlerSettings = new JoystickHandlerSettings(JoystickType.Static, false);

		StateHandlerSettings_ByState_None.StateReadyType = eBugsStateType.None;
		StateHandlerSettings_ByState_None.StateActionType = eBugsStateType.None;
		StateHandlerSettings_ByState_None.StateUpdateType = eBugsStateType.None;

		StateHandlerSettings_ByState_None.DicTouchStateHandlerBindings = null;

		StateHandlerSettings_ByState_None.LockStates = null;

		m_StateHandlerSettings_ByState[csOwner_Hero.Index_Action_None] = StateHandlerSettings_ByState_None;
	}
}
