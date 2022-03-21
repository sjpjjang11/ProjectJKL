using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csJoystickHandler_Action : csJoystickHandler
{
	[SerializeField]
	private Vector3 m_ActionDirection = Vector3.zero;
	public Vector3 ActionDirection
	{
		get
		{
			return m_ActionDirection;
		}
		set
		{
			m_ActionDirection = value;
			//Debug.Log(m_ActionDirection);
			EventHandler_Hero.ActionDirection.Set(m_ActionDirection);
		}
	}

	[SerializeField]
	private Vector3 m_ActionDirection_Original = Vector3.zero;
	public Vector3 ActionDirection_Original
	{
		get
		{
			return m_ActionDirection_Original;
		}
		set
		{
			m_ActionDirection_Original = value;
			EventHandler_Hero.ActionDirection_Original.Set(m_ActionDirection_Original);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		 
		m_ListMethodName = new List<string>();

		m_Joystick.RegisterTouchesBeganCallback(TouchBegan);
		m_Joystick.RegisterTouchesMovedCallback(TouchMoved);
		m_Joystick.RegisterTouchesStationaryCallback(TouchStationary);
		m_Joystick.RegisterTouchesEndedCallback(TouchEnded);
	}

	protected override void TouchBegan()
	{
		//Debug.Log("TouchBegan : " + m_DicTouchEventBindings[TouchEvent.TouchBegan]);

		ActionDirection = m_Joystick.GetDirection();
		ActionDirection_Original = m_Joystick.GetDirection_Original();

		m_DicTouchEventBindings[TouchEvent.TouchBegan]?.Invoke();

		m_DicTouchEventBindings[TouchEvent.TouchBegan_Additional]?.Invoke();
	}

	protected override void TouchMoved()
	{
		//Debug.Log("TouchMoved : " + m_DicTouchEventBindings[TouchEvent.TouchMoved]);
		ActionDirection = m_Joystick.GetDirection();
		ActionDirection_Original = m_Joystick.GetDirection_Original();

		m_DicTouchEventBindings[TouchEvent.TouchMoved]?.Invoke();

		m_DicTouchEventBindings[TouchEvent.TouchMoved_Additional]?.Invoke();

		if (m_Joystick.GetDistance() > m_Joystick.m_fMinRangeKnob && !EventHandler_Hero.IsUsedStick.Get())
		{
			EventHandler_Hero.IsUsedStick.Set(true);
		}
	}

	protected override void TouchStationary()
	{
		//Debug.Log("TouchStationary : " + m_DicTouchEventBindings[TouchEvent.TouchStationary]);
		ActionDirection = m_Joystick.GetDirection();
		ActionDirection_Original = m_Joystick.GetDirection_Original();

		m_DicTouchEventBindings[TouchEvent.TouchStationary]?.Invoke();

		m_DicTouchEventBindings[TouchEvent.TouchStationary_Additional]?.Invoke();
	}

	protected override void TouchEnded()
	{
		//Debug.Log("TouchEnded : " + m_DicTouchEventBindings[TouchEvent.TouchEnded]);
		//m_StateHandler_ByState_UI.StateAction();

		m_DicTouchEventBindings[TouchEvent.TouchEnded]?.Invoke();

		m_DicTouchEventBindings[TouchEvent.TouchEnded_Additional]?.Invoke();	

		EventHandler_Hero.IsUsedStick.Set(false);
	}
}
