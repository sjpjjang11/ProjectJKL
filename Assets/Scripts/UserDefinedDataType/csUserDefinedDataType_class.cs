using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHandlerSettings
{
	public Dictionary<TouchEvent, StateControlType> DicTouchStateHandlerBindings;
}

// 상태별 조작
[Serializable]
public class StateHandlerSettings_ByState : StateHandlerSettings
{
	public StateHandlerSettings_ByState(
		JoystickHandlerSettings _joyStickSettings,
		Dictionary<TouchEvent, StateControlType> _dicTouchStateHandlerBindings,
		eBugsStateType[] _lockStates,
		eBugsStateType _readyType,
		eBugsStateType _actionType,
		eBugsStateType _updateType,
		eBugsStateType _reservationCancelType)
	{
		JoystickHandlerSettings = _joyStickSettings;

		DicTouchStateHandlerBindings = _dicTouchStateHandlerBindings;

		LockStates = _lockStates;

		StateReadyType = _readyType;
		StateActionType = _actionType;
		StateUpdateType = _updateType;
		ReservationCancelType = _reservationCancelType;
	}

	public JoystickHandlerSettings JoystickHandlerSettings;

	public eBugsStateType[] LockStates;

	public eBugsStateType StateReadyType;
	public eBugsStateType StateActionType;
	public eBugsStateType StateUpdateType;
	public eBugsStateType ReservationCancelType;
}

// 한꺼번에 조작
[Serializable]
public class StateHandlerSettings_Multi : StateHandlerSettings
{
	public StateHandlerSettings_Multi(
		Dictionary<TouchEvent, StateControlType> _dicTouchStateHandlerBindings,
		Dictionary<eBugsStateType, eBugsStateType[]> _dicStateLockStateBindings,
		Dictionary<eBugsStateType, eBugsStateType> _dicStateReservationCancelBindings)
	{
		DicTouchStateHandlerBindings = _dicTouchStateHandlerBindings;
		DicStateLockStateBindings = _dicStateLockStateBindings;
		DicStateReservationCancelBindings = _dicStateReservationCancelBindings;

		Attack = eBugsStateType.Attack;
		Skill_1 = eBugsStateType.Skill_1;
		Skill_Default = eBugsStateType.Skill_Default;
	}

	public Dictionary<eBugsStateType, eBugsStateType[]> DicStateLockStateBindings;
	public Dictionary<eBugsStateType, eBugsStateType> DicStateReservationCancelBindings;

	public eBugsStateType Attack;
	public eBugsStateType Skill_1;
	public eBugsStateType Skill_Default;
}

public class CrowdControl
{
	public eBugsStateType CrowdControlType;

	public CrowdControl(eBugsStateType _crowdControlType)
	{
		CrowdControlType = _crowdControlType;
	}
}

public class CrowdControl_Slow : CrowdControl
{
	public float Degree;
	public float Duration;

	public CrowdControl_Slow(eBugsStateType _crowdControlType, float _degree, float _duration) : base(_crowdControlType)
	{
		Degree = _degree;
		Duration = _duration;
	}
}

public class CrowdControl_Stun : CrowdControl
{
	public float Duration;

	public CrowdControl_Stun(eBugsStateType _crowdControlType, float _duration) : base(_crowdControlType)
	{
		Duration = _duration;
	}
}

public class CrowdControl_KnockBack : CrowdControl
{
	public Vector3 KnockBacDirection;

	public float KnockBackDistance;
	public float Time;

	public CrowdControl_KnockBack(eBugsStateType _crowdControlType, float _knockBackDistance, float _time) : base(_crowdControlType)
	{
		KnockBackDistance = _knockBackDistance;

		Time = _time;
	}
}
