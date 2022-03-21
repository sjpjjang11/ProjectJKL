using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class csInfo_Hero : csInfo_Bugs
{
	public StateHandlerSettings_ByState[] m_StateHandlerSettings_ByState;
	public StateHandlerSettings_Multi m_StateHandlerSettings_Multi;

	protected float m_fStateCancelTime = 2.0f;

	public const int ActionCount = 4; //7

	public csInfo_Hero()
	{
		m_Health.Max = m_Health.Cur = 100;
		m_Power.Min = 5; //5
		m_Power.Max = 30; //30
		m_Power.Limit_Min = m_Power.Max;
		m_Power.Limit_Max = m_Health.Max;
		m_WalkSpeed = 2.0f;
		m_RunSpeed = 5.0f; //5
		m_BugsAction = new BugsAction[ActionCount];
		m_StateHandlerSettings_ByState = new StateHandlerSettings_ByState[ActionCount];

		Settings_Escape();
	}

	private void Settings_Escape()
	{
		// BugsAction_Escape
		BugsAction BugsAction_Escape = m_BugsAction[csOwner_Hero.Index_Action_Escape];

		BugsAction_Escape.CrowdControl = null;

		BugsAction_Escape.AnimationName = "Escape";

		BugsAction_Escape.Range = 15.0f;
		BugsAction_Escape.ActivationTime = 100.0f;
		BugsAction_Escape.CoolTime = 0.0f;

		BugsAction_Escape.ActionIndex = csOwner_Hero.Index_Action_Escape;

		BugsAction_Escape.IsActivation = false;
		BugsAction_Escape.IsImmediatelyCoolDown = false;

		m_BugsAction[csOwner_Hero.Index_Action_Escape] = BugsAction_Escape;

		// StateHandlerSettings_ByState_Escape
		JoystickHandlerSettings JoystickHandlerSettings = new JoystickHandlerSettings(JoystickType.Static, false);

		Dictionary<TouchEvent, StateControlType> DicTouchStateHandlerBindings = new Dictionary<TouchEvent, StateControlType>(new TouchEventCompare())
		{
			{ TouchEvent.TouchBegan, StateControlType.StateReady },
			{ TouchEvent.TouchEnded, StateControlType.StateAction }
		};

		eBugsStateType StateReadyType = eBugsStateType.Escape_Ready;
		eBugsStateType StateActionType = eBugsStateType.Escape_Action;
		eBugsStateType StateUpdateType = eBugsStateType.None;
		eBugsStateType ReservationCancelType = eBugsStateType.None;

		m_StateHandlerSettings_ByState[csOwner_Hero.Index_Action_Escape] = new StateHandlerSettings_ByState(
			JoystickHandlerSettings,
			DicTouchStateHandlerBindings,
			null,
			StateReadyType,
			StateActionType,
			StateUpdateType,
			ReservationCancelType);
	}
}
