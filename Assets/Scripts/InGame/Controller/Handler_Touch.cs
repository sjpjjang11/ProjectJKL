using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_Touch : Handler_Input
{
	protected BoxCollider m_BoxCollider = null;

	public Camera m_UICamera = null;

	protected UserTouch m_CurrentTouch;

	protected int m_iLastFingerID = 0;          // 마지막으로 터치한 손가락 식별

	protected override void Awake()
	{
		base.Awake();

		Register();

		m_BoxCollider = GetComponent<BoxCollider>();

		m_iLastFingerID = -1;
	}

	protected override void Start()
	{
		base.Start();

		m_BoxCollider.size = GetComponent<RectTransform>().rect.size;
	}

	protected virtual void TouchBegan(UserTouch _touch)
	{
		if (m_iLastFingerID != -1)
		{
			return;
		}

		m_iLastFingerID = _touch.m_iFingerID;
	}

	protected virtual void TouchMoved(UserTouch _touch)
	{

	}

	protected virtual void TouchStationary(UserTouch _touch)
	{

	}

	protected virtual void TouchEnded(UserTouch _touch)
	{
		if (m_iLastFingerID != _touch.m_iFingerID)
		{
			return;
		}

		m_iLastFingerID = -1;
	}
}
