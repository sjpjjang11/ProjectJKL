using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public enum StateControlType
{
	StateReady,
	StateAction,
	StateUpdate,
	StateEnded,
	StateStop,
	TouchBegan,
	TouchMoved,
	TouchStationary,
	TouchEnded
}

public class csJoystickHandler : MonoBehaviour {

	protected delegate void TouchEventHandler();
	protected Dictionary<TouchEvent, TouchEventHandler> m_DicTouchEventBindings =
		new Dictionary<TouchEvent, TouchEventHandler>(new TouchEventCompare())
		{
			{ TouchEvent.TouchBegan, null },
			{ TouchEvent.TouchBegan_Additional, null },
			{ TouchEvent.TouchMoved, null },
			{ TouchEvent.TouchMoved_Additional, null },
			{ TouchEvent.TouchStationary, null },
			{ TouchEvent.TouchStationary_Additional, null },
			{ TouchEvent.TouchEnded, null },
			{ TouchEvent.TouchEnded_Additional, null }
		};

	protected List<string> m_ListMethodName = new List<string>();

	protected StateHandlerSettings m_StateHandlerSettings = null;

	protected csJoystick m_Joystick = null;
	protected csStateHandler m_StateHandler = null;

	protected csInfo_Hero m_Info_Hero= null;	

	protected csEventHandler_Hero m_EventHandler_Hero = null;
	public csEventHandler_Hero EventHandler_Hero
	{
		get
		{
			if (m_EventHandler_Hero == null)
			{
				m_EventHandler_Hero = csBattleManager.Instance.m_EventHandler_Hero;
			}

			return m_EventHandler_Hero;
		}
	}

	protected virtual void Awake()
	{		
		m_Joystick = GetComponent<csJoystick>();
	}

	public virtual void Settings(int _heroIndex, int _actionIndex)
	{
		m_Info_Hero = (csInfo_Hero)csBattleManager.Instance.m_BugsInfoManager.GetBugsInfo(csBattleManager.HeroType, _heroIndex);				
	}

	public csJoystick GetJoystick()
	{
		return m_Joystick;
	}

	public virtual void Activate()
	{
		Utility.Activate(gameObject, true);
	}

	public virtual void Deactivate()
	{
		m_Joystick.Initialized();
		Utility.Activate(gameObject, false);	
	}

	public virtual void TouchEventRegister()
	{
		MethodInfo[] MethodInfos = m_StateHandler.GetType().GetMethods
				(
					BindingFlags.NonPublic |
					BindingFlags.Instance
				);

		m_ListMethodName = new List<string>();

		for (int i = 0; i < MethodInfos.Length; i++)
		{
			m_ListMethodName.Add(MethodInfos[i].Name);
		}

		foreach (TouchEvent a in m_StateHandlerSettings.DicTouchStateHandlerBindings.Keys)
		{
			StateControlType StateControlType = m_StateHandlerSettings.DicTouchStateHandlerBindings[a];

			string MethodName = m_ListMethodName.Find(x => StateControlType.ToString() == x);

			if (MethodName == null)
			{
				continue;
			}

			// TouchEventHandler 타입과 메소드 이름으로 델리게이트 생성 
			TouchEventHandler GetDelegate = (TouchEventHandler)Delegate.CreateDelegate(typeof(TouchEventHandler), m_StateHandler, MethodName);

			// Key에 해당하는 Velue에 델리게이트 연결
			m_DicTouchEventBindings[a] += GetDelegate;

			if (name == "EscapeController")
			{
				Debug.Log(a);
				Debug.Log(MethodName);
				Debug.Log(GetDelegate);
				Debug.Log(GetDelegate.GetType());
				Debug.Log(GetDelegate.Method);
				Debug.Log(GetDelegate.ToString());
			}
		}
	}

	protected virtual void TouchBegan()
	{

	}

	protected virtual void TouchMoved()
	{

	}

	protected virtual void TouchStationary()
	{

	}

	protected virtual void TouchEnded()
	{

	}
}
