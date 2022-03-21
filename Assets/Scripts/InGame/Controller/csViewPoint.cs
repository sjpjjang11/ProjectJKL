using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csViewPoint : csTouch {

	public Vector2 m_Sensitivity;

	[SerializeField]
	private Vector2 m_ViewPoint;
	public Vector3 ViewPoint
	{
		get
		{
			return m_ViewPoint;
		}
		set
		{
			m_ViewPoint = value;

			EventHandler_Hero.ViewPoint.Set(m_ViewPoint);
		}
	}

	protected override void TouchBegan(UserTouch _touch)
	{
		base.TouchBegan(_touch);
	}

	protected override void TouchMoved(UserTouch _touch)
	{
		if (m_iLastFingerID != _touch.m_iFingerID)
		{
			return;
		}

		ViewPoint = new Vector3(((_touch.m_DeltaPosition) * m_Sensitivity.x * Time.deltaTime).x, ((_touch.m_DeltaPosition) * m_Sensitivity.y * Time.deltaTime).y);
	}

	protected override void TouchStationary(UserTouch _touch)
	{
		if (m_iLastFingerID != _touch.m_iFingerID)
		{
			return;
		}

		ViewPoint = Vector2.zero;
	}

	protected override void TouchEnded(UserTouch _touch)
	{
		base.TouchEnded(_touch);

		ViewPoint = Vector2.zero;
	}
}
