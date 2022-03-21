using System;
using UnityEngine;

public class csJoystickHandler_ByState : csJoystickHandler_Action
{
	protected override void Awake()
	{
		base.Awake();

		m_StateHandler = GetComponent<csStateHandler_ByState_UI>();
	}

	public override void Settings(int _heroIndex, int _actionIndex)
	{
		base.Settings(_heroIndex, _actionIndex);
		Debug.LogError("Settings : " + _heroIndex + "   " + _actionIndex + "   " + m_StateHandler + "   " + name);
		m_StateHandler.Settings(m_Info_Hero, _actionIndex);

		m_StateHandlerSettings = m_Info_Hero.m_StateHandlerSettings_ByState[_actionIndex];
		StateHandlerSettings_ByState StateHandlerSettings_ByState = (StateHandlerSettings_ByState)m_StateHandlerSettings;
		m_Joystick.Settings(StateHandlerSettings_ByState.JoystickHandlerSettings);

		if (m_Info_Hero.m_StateHandlerSettings_ByState[_actionIndex].JoystickHandlerSettings.Type == JoystickType.None)
		{
			Utility.Activate(m_Joystick, false);
			return;
		}
		else
		{
			Utility.Activate(m_Joystick, true);

			TouchEventRegister();
		}
	}	
}
