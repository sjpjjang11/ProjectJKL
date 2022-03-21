using System;
using UnityEngine;

public class csJoystickHandler_Multi : csJoystickHandler_Action
{
	protected override void Awake()
	{
		base.Awake();

		m_StateHandler = GetComponent<csStateHandler_Multi>();
	}

	public override void Settings(int _heroIndex, int _actionIndex)
	{
		base.Settings(_heroIndex, _actionIndex);

		m_StateHandler.Settings(m_Info_Hero);

		m_StateHandlerSettings = m_Info_Hero.m_StateHandlerSettings_Multi;

		JoystickHandlerSettings JoystickHandlerSettings = new JoystickHandlerSettings(JoystickType.Dynamic, true);

		m_Joystick.Settings(JoystickHandlerSettings);

		TouchEventRegister();
	}
}
