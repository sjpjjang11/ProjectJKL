using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csHUD_Indicator : csHUD {

	private Transform m_HeroTransform = null;
	protected Transform HeroTransform
	{
		get
		{
			if (m_HeroTransform == null)
			{
				m_HeroTransform = csBattleManager.Instance.m_Hero.Transform;
			}

			return m_HeroTransform;
		}
	}

	public Vector2 m_OffSceenEdge = new Vector2(0.9f, 0.95f);

	public float m_fOffScreenEdge = 0.95f;
	public float m_fOnScreenEdge = 1.1f;
	public float m_fDisableDistance = 50.0f;

	public override void Settings(csOwner _owner)
	{
		base.Settings(_owner);

		Utility.Activate(m_Front, false);

		m_FollowTarget = m_Owner.Bugs.m_HpBarTr;
	}

	private bool IsOnScreen(Vector3 _screenPosition)
	{
		if (_screenPosition.x < (Screen.width * m_fOnScreenEdge) && _screenPosition.x > (Screen.width - Screen.width * m_fOnScreenEdge) &&
			_screenPosition.y < (Screen.height * m_fOnScreenEdge) && _screenPosition.y > (Screen.height - Screen.height * m_fOnScreenEdge) &&
			_screenPosition.z > m_PlayerCamera.Camera.nearClipPlane && _screenPosition.z < m_PlayerCamera.Camera.farClipPlane)
		{
			return true;
		}
			
		return false;
	}

	public void UpdateOffScreen(Vector3 _screenPoint)
	{
		Vector3 ScreenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;

		Vector3 NewIndicatorPos = _screenPoint - ScreenCenter;

		if (NewIndicatorPos.z < 0)
		{
			NewIndicatorPos *= -1;
		}
			
		float Angle = Mathf.Atan2(NewIndicatorPos.y, NewIndicatorPos.x);
		Angle -= 90 * Mathf.Deg2Rad;

		float Cos = Mathf.Cos(Angle);
		float Sin = Mathf.Sin(Angle);
		float m = Cos / Sin;

		Vector3 ScreenBounds = new Vector3(ScreenCenter.x * m_OffSceenEdge.x, ScreenCenter.y * m_OffSceenEdge.y);

		if (Cos > 0)
		{
			// 유저 화면시점에서 적이 위에 있을 때
			NewIndicatorPos = new Vector2(-ScreenBounds.y / m, ScreenBounds.y);
		}
		else
		{
			// 유저 화면시점에서 적이 아래에 있을 때
			NewIndicatorPos = new Vector2(ScreenBounds.y / m, -ScreenBounds.y);
		}

		if (NewIndicatorPos.x > ScreenBounds.x)
		{
			// 유저 화면시점에서 적이 오른쪽에 있을 때
			NewIndicatorPos = new Vector2(ScreenBounds.x, -ScreenBounds.x * m);
		}
		else if (NewIndicatorPos.x < -ScreenBounds.x)
		{
			// 유저 화면시점에서 적이 왼쪽에 있을 때
			NewIndicatorPos = new Vector2(-ScreenBounds.x, ScreenBounds.x * m);
		}

		NewIndicatorPos += ScreenCenter;
		m_Transform.position = NewIndicatorPos;

		m_Transform.rotation = Quaternion.Euler(0, 0, Angle * Mathf.Rad2Deg);
	}

	private float GetDistance(Vector3 PosA, Vector3 PosB)
	{
		Vector3 Heading;

		Heading.x = PosA.x - PosB.x;
		Heading.y = PosA.y - PosB.y;
		Heading.z = PosA.z - PosB.z;

		return Mathf.Sqrt((Heading.x * Heading.x) + (Heading.y * Heading.y) + (Heading.z * Heading.z));
	}

	public float m_fScalingFactor = 10.0f;
	public float m_fMinScaleSize = 0.2f;
	public float m_fMaxScaleSize = 100.0f;

	private void UpdateScale(float _distance)
	{
		float NewScaleSize = m_fScalingFactor / _distance;

		NewScaleSize = Mathf.Clamp(NewScaleSize, m_fMinScaleSize, m_fMaxScaleSize);

		m_Transform.localScale = new Vector2(NewScaleSize, NewScaleSize);
	}

	#region Coroutine

	protected override IEnumerator CoHUDFollowTarget()
	{
		Vector3 ScreenPoint = Vector3.zero;
		Vector3 HpBarPosition = Vector3.zero;

		float Distance = 0.0f;

		while (true)
		{
			yield return YieldCache.WaitForEndOfFrame;
			//Debug.Log("$$$$$ ");
			//Debug.Log("!!!!!!!!! : " + m_MyOwner.name);

			ScreenPoint = m_PlayerCamera.Camera.WorldToScreenPoint(m_FollowTarget.position);

			if (IsOnScreen(ScreenPoint))
			{
				Utility.Activate(m_Front, false);
			}
			else
			{
				//Distance = GetDistance(m_Owner.Transform.position, LocalUserTransform.position);
				Distance = Vector3.Distance(m_Owner.Transform.position, HeroTransform.position);

				if (Distance >= m_fDisableDistance)
				{
					Utility.Activate(m_Front, false);
				}
				else
				{
					UpdateOffScreen(ScreenPoint);

					UpdateScale(Distance);
					Utility.Activate(m_Front, true);
				}				
			}
		}
	}

#endregion
}
