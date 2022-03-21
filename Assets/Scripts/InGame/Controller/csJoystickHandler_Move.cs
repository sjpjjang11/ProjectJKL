using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csJoystickHandler_Move : csJoystickHandler {

	[SerializeField]
	private Vector3 m_MoveDirection = Vector3.zero;
	public Vector3 MoveDirection
	{
		get
		{
			return m_MoveDirection;
		}
		set
		{
			m_MoveDirection = value;
			EventHandler_Hero.MoveDirection.Set(m_MoveDirection);
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

		m_Joystick.RegisterTouchesMovedCallback(TouchMoved);
		m_Joystick.RegisterTouchesStationaryCallback(TouchStationary);
		m_Joystick.RegisterTouchesEndedCallback(TouchEnded);
		m_Joystick.RegisterTest(Test);
	}

	public override void Settings(int _heroIndex, int _actionIndex)
	{
		base.Settings(_heroIndex, _actionIndex);

		JoystickHandlerSettings JoystickHandlerSettings = new JoystickHandlerSettings(JoystickType.Dynamic, true);
		m_Joystick.Settings(JoystickHandlerSettings);
	}

	private void Test(UserTouch _test)
	{
		Debug.Log(_test.m_Position);
		Debug.Log(_test.m_DeltaPosition);
	}

	protected override void TouchBegan()
	{
		base.TouchBegan();
	}
	public Vector3 TestPrevDirection;
	public float TestPrevDistance;
	protected override void TouchMoved()
	{
		if (EventHandler_Hero.IsTouchMove.Get() != true)
		{
			EventHandler_Hero.IsTouchMove.Set(true);
		}
		TestPrevDistance = Vector3.Distance(EventHandler_Hero.MoveDirection.Get(), TestPrevDirection);
		MoveDirection = m_Joystick.GetDirection();
		TestPrevDirection = MoveDirection;
		ActionDirection_Original = m_Joystick.GetDirection_Original();
	}

	protected override void TouchStationary()
	{
		MoveDirection = m_Joystick.GetDirection();		
		ActionDirection_Original = m_Joystick.GetDirection_Original();
	}

	protected override void TouchEnded()
	{
		//Debug.LogError("TouchEnded");
		MoveDirection = Vector3.zero;

		EventHandler_Hero.IsTouchMove.Set(false);
	}
}
