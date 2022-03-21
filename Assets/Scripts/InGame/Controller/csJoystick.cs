using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum JoystickType
{
	None,
	Dynamic,
	Static,
	EventTrigger
}

public class csJoystick : csTouch
{
	public JoystickType m_JoystickType;

	private Action m_TouchesBeganCallback = null;
	private Action m_TouchesMovedCallback = null;
	private Action m_TouchesStationaryCallback = null;
	private Action m_TouchesEndedCallback = null;
	private Action<UserTouch> m_Test;

	public Transform m_Knob = null;                                                 // 조이스틱 Transform                                                      
	public Transform m_KnobBackground = null;                                       // 조이스틱 배경 Transform       
	public Transform m_JoystickParent = null;                                       // 조이스틱 부모 Transform

	private Image m_KnobImage;
	private Image m_KnobBackgroundImage;

	[SerializeField]
	private Vector3 m_DefaultKnobPosition = Vector3.zero;
	[SerializeField]
	private Vector3 m_Direction = Vector3.zero;
	[SerializeField]
	private Vector3 m_Direction_Original = Vector3.zero;
	//public Vector3 m_TestPos;
	[SerializeField]
	private float m_fDistance = 0.0f;

	public Color m_KnobOnColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public Color m_KnobOffColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	public Color m_KnobBackgroundOnColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public Color m_KnobBackgroundOffColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

	public float m_JoystickAniSpeed = 10.0f;
	public float m_fKnobMoveSpeed = 1.0f;
	public float m_fMinRangeKnob = 100.0f;
	public float m_fMaxRangeKnob = 150.0f;
	public float m_fMaxRangeTarget = 1.0f;
	public float m_fDecimalTruncateUnit = 10.0f;

	public bool m_bIsFade = false;

	protected override void Awake()
	{
		base.Awake();

		// 기본 위치 설정		
		m_Knob.localPosition = Vector3.zero;
		m_DefaultKnobPosition = m_Knob.position;
	}

	protected override void Update()
	{
		//base.Update();
		//Debug.Log("~~~~~~~~~~~~~~~ : " + LocalHandler.IsStick.Get());
		if (m_JoystickType != JoystickType.None && m_JoystickType != JoystickType.EventTrigger)
		{
			JoystickUpdate();
		}
	}

	public void Settings(JoystickHandlerSettings _settings)
	{
		m_JoystickType = _settings.Type;
		m_bIsFade = _settings.IsFade;

		if (m_bIsFade)
		{
			if (m_Knob != null)
			{
				m_KnobImage = m_Knob.GetComponent<Image>();
			}

			if (m_KnobBackground != null)
			{
				m_KnobBackgroundImage = m_KnobBackground.GetComponent<Image>();
			}
		}
	}

	public void RegisterTouchesBeganCallback(Action _callback)
	{
		m_TouchesBeganCallback += _callback;
	}

	public void RegisterTouchesMovedCallback(Action _callback)
	{
		m_TouchesMovedCallback += _callback;
	}

	public void RegisterTouchesStationaryCallback(Action _callback)
	{
		m_TouchesStationaryCallback += _callback;
	}

	public void RegisterTouchesEndedCallback(Action _callback)
	{
		m_TouchesEndedCallback += _callback;
	}

	public void RegisterTest(Action<UserTouch> _callback)
	{
		m_Test += _callback;
	}

	public void Initialized()
	{
		m_JoystickParent.localPosition = Vector3.zero;
		m_Knob.localPosition = Vector3.zero;
	}

	public Vector3 GetDirection()
	{
		return m_Direction;
	}

	public Vector3 GetDirection_Original()
	{
		return m_Direction_Original;
	}

	public float GetDistance()
	{
		return m_fDistance;
	}

	protected override void TouchBegan(UserTouch _touch)
	{
		if (m_JoystickType == JoystickType.None)
		{
			return;
		}

		if (m_iLastFingerID != -1)
		{
			return;
		}

		if (!RaycastControl(_touch))
		{
			return;
		}

		// 임시. 주말
		if (GetComponent<csJoystickHandler_Move>() == null && EventHandler_Hero.IsUsingOtherStick.Get())
		{
			return;
		}

		// 임시. 주말
		if (GetComponent<csJoystickHandler_Move>() == null)
		{
			EventHandler_Hero.IsUsingOtherStick.Set(true);
		}

		m_iLastFingerID = _touch.m_iFingerID;

		if (m_JoystickType == JoystickType.Dynamic)
		{
			Vector3 TouchesBeganPosition = UICamera.ScreenToWorldPoint(_touch.m_Position);

			TouchesBeganPosition.z = m_JoystickParent.position.z;

			m_JoystickParent.position = TouchesBeganPosition;
		}

		m_Knob.localPosition = Vector3.zero;
		m_DefaultKnobPosition = m_Knob.position;

		m_TouchesBeganCallback?.Invoke();
	}

	protected override void TouchMoved(UserTouch _touch)
	{
		if (m_iLastFingerID != _touch.m_iFingerID)
		{
			return;
		}

		m_CurrentTouch = _touch;

		//Debug.LogError("TouchesMoved : " + name);

		Vector2 ScreenPos = UICamera.ScreenToWorldPoint(_touch.m_Position);
		Vector2 DistanceVector = ScreenPos - (Vector2)m_DefaultKnobPosition;

		Vector2 DistanceVector_Clamp = Vector2.ClampMagnitude((DistanceVector * 0.1f), 1.0f);
		//m_Direction_Original = new Vector3(DistanceVector_Clamp.x, 0.0f, DistanceVector_Clamp.y);
		//TestDistance = Distance;
		m_fDistance = Vector2.Distance(ScreenPos, m_DefaultKnobPosition);

		//Distance = ConstrainToKnob(Distance);
		//TestConstrain = Distance;
		Vector2 KnobPosition = Vector2.ClampMagnitude(DistanceVector * m_fKnobMoveSpeed, m_fMaxRangeKnob);
		Vector2 TargetMoveDirection = Vector2.ClampMagnitude(DistanceVector, m_fMaxRangeTarget);
		//TargetMoveDirection = new Vector2((float)Math.Round(TargetMoveDirection.x * 1000.0f) / 1000.0f, (float)Math.Round(TargetMoveDirection.y * 1000.0f) / 1000.0f);
		m_Direction_Original = new Vector3(KnobPosition.x * 0.01f, 0.0f, KnobPosition.y * 0.01f);
		m_Knob.localPosition = KnobPosition;

		if (m_fDistance < m_fMinRangeKnob)
		{
			m_Direction_Original = Vector3.zero;
			m_Direction = Vector3.zero;
			return;
		}

		Vector3 TestPos = new Vector3(EvaluateInputValue(TargetMoveDirection.x), 0.0f, EvaluateInputValue(TargetMoveDirection.y));
		//m_TestPos = TargetMoveDirection.normalized;
		m_Direction = new Vector3((float)(Math.Truncate(TestPos.x * m_fDecimalTruncateUnit) / m_fDecimalTruncateUnit), 0.0f, (float)(Math.Truncate(TestPos.z * m_fDecimalTruncateUnit) / m_fDecimalTruncateUnit));
		//Debug.Log("TouchesMoved : " + m_Direction);
		m_TouchesMovedCallback?.Invoke();
	}

	protected virtual float EvaluateInputValue(float _position)
	{
		return Mathf.InverseLerp(0, m_fMaxRangeTarget, Mathf.Abs(_position)) * Mathf.Sign(_position);
	}

	protected override void TouchStationary(UserTouch _touch)
	{
		if (m_iLastFingerID != _touch.m_iFingerID)
		{
			return;
		}

		//Debug.LogError("TouchesStationary : " + name);

		m_TouchesStationaryCallback?.Invoke();
	}

	protected override void TouchEnded(UserTouch _touch)
	{
		if (m_iLastFingerID != _touch.m_iFingerID)
		{
			return;
		}

		// 임시. 주말
		if (GetComponent<csJoystickHandler_Move>() == null)
		{
			EventHandler_Hero.IsUsingOtherStick.Set(false);
		}

		m_iLastFingerID = -1;

		//_touch.m_DeltaPosition = Vector2.zero;
		//EventHandler_Hero.Touch.Set(_touch);

		m_Direction = Vector3.zero;
		m_fDistance = 0.0f;

		m_TouchesEndedCallback?.Invoke();
	}

	private void JoystickUpdate()
	{
		if (m_iLastFingerID != -1)
		{
			if (m_bIsFade)
			{
				if (m_KnobImage != null)
				{
					m_KnobImage.color = Color.Lerp(m_KnobImage.color, m_KnobOnColor, Time.deltaTime * m_JoystickAniSpeed);
				}

				if (m_KnobBackgroundImage != null)
				{
					m_KnobBackgroundImage.color = Color.Lerp(m_KnobBackgroundImage.color, m_KnobBackgroundOnColor, Time.deltaTime * m_JoystickAniSpeed);
				}
			}
		}
		else
		{
			if (m_Knob != null)
			{
				//m_Knob.position = Vector3.Slerp(m_Knob.position, m_DefaultKnobPosition, Time.deltaTime * m_JoystickAniSpeed);
				m_Knob.localPosition = Vector3.Slerp(m_Knob.localPosition, Vector3.zero, Time.deltaTime * m_JoystickAniSpeed);
			}

			if (m_bIsFade)
			{
				if (m_KnobImage != null)
				{
					m_KnobImage.color = Color.Lerp(m_KnobImage.color, m_KnobOffColor, Time.deltaTime * m_JoystickAniSpeed);
				}

				if (m_KnobBackground != null && m_KnobBackgroundImage != null)
				{
					m_KnobBackgroundImage.color = Color.Lerp(m_KnobBackgroundImage.color, m_KnobBackgroundOffColor, Time.deltaTime * m_JoystickAniSpeed);
				}
			}
		}
	}

	private Vector2 ConstrainToKnob(Vector3 _pos)
	{
		if (Mathf.Abs(_pos.x) < m_fMinRangeKnob)
		{
			_pos.x = m_fMinRangeKnob * Mathf.Sign(_pos.x);
		}

		if (Mathf.Abs(_pos.y) < m_fMinRangeKnob)
		{
			_pos.y = m_fMinRangeKnob * Mathf.Sign(_pos.x);
		}

		return _pos;
	}
}
