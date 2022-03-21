using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIndicator : MonoBehaviour
{
	private Transform m_Transform = null;

	public Transform m_Target = null;

	public Transform m_HeroTransform = null;

	public GameObject m_Front = null;

	public Camera m_Camera;

	public float m_fOffScreenEdge = 0.95f;
	public Vector2 m_OffSceenEdge = new Vector2(0.9f, 0.95f);
	public float m_fOnScreenEdge = 1.1f;
	public float m_fDisableDistance = 50.0f;

	[Header("Test")]
	public Vector2 m_Size_Screen;

	private void Awake()
	{
		m_Transform = GetComponent<Transform>();

		StartCoroutine(CoHUDFollowTarget());
	}

    private void Update()
    {
		m_Size_Screen = new Vector2(Screen.width, Screen.height);
	}

    private bool IsOnScreen(Vector3 _screenPosition)
	{
		if (_screenPosition.x < (Screen.width * m_fOnScreenEdge) && _screenPosition.x > (Screen.width - Screen.width * m_fOnScreenEdge) &&
			_screenPosition.y < (Screen.height * m_fOnScreenEdge) && _screenPosition.y > (Screen.height - Screen.height * m_fOnScreenEdge) &&
			_screenPosition.z > m_Camera.nearClipPlane && _screenPosition.z < m_Camera.farClipPlane)
		{
			return true;
		}

		return false;
	}

	public Vector3 m_ScreenCenter;
	public Vector3 m_ScreenBounds;
	public Vector3 m_NewIndicatorPos;
	public Vector3 m_Test;

	public float m_Cos;
	public float m_Sin;
	public float m_Men;

	public float m_NewScaleSize;

	public void UpdateOffScreen(Vector3 _screenPoint)
	{
		Vector3 ScreenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
		//Vector3 ScreenCenter = m_ScreenCenter;
		m_ScreenCenter = ScreenCenter;
		Vector3 NewIndicatorPos = _screenPoint - ScreenCenter;

		if (NewIndicatorPos.z < 0)
		{
			NewIndicatorPos *= -1;
		}

		float Angle = Mathf.Atan2(NewIndicatorPos.y, NewIndicatorPos.x);
		Angle -= 90 * Mathf.Deg2Rad;

		float Cos = Mathf.Cos(Angle);
		m_Cos = Cos;
		float Sin = Mathf.Sin(Angle);
		m_Sin = Sin;
		float m = Cos / Sin;
		m_Men = m;

		Vector3 ScreenBounds = new Vector3(ScreenCenter.x * m_OffSceenEdge.x, ScreenCenter.y * m_OffSceenEdge.y);
		m_ScreenBounds = ScreenBounds;
		if (Cos > 0)
		{
			// 유저 화면시점에서 적이 위에 있을 때
			Debug.LogError("1 if");
			NewIndicatorPos = new Vector2(-ScreenBounds.y / m, ScreenBounds.y);
		}
		else
		{
			// 유저 화면시점에서 적이 아래에 있을 때
			Debug.LogError("1 else");
			NewIndicatorPos = new Vector2(ScreenBounds.y / m, -ScreenBounds.y);
		}

		if (NewIndicatorPos.x > ScreenBounds.x)
		{
			// 유저 화면시점에서 적이 오른쪽에 있을 때
			Debug.LogError("2 if");
			NewIndicatorPos = new Vector2(ScreenBounds.x, -ScreenBounds.x * m);
		}
		else if (NewIndicatorPos.x < -ScreenBounds.x)
		{
			// 유저 화면시점에서 적이 왼쪽에 있을 때
			Debug.LogError("2 else");
			NewIndicatorPos = new Vector2(-ScreenBounds.x, ScreenBounds.x * m);
		}
		m_NewIndicatorPos = NewIndicatorPos;
		NewIndicatorPos += ScreenCenter;
		m_Transform.position = NewIndicatorPos;
		m_Transform.position += m_Test;
		//m_Transform.position += csBattleUIManager.Instance.transform.position;
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
		m_NewScaleSize = m_fScalingFactor / _distance;

		m_NewScaleSize = Mathf.Clamp(m_NewScaleSize, m_fMinScaleSize, m_fMaxScaleSize);

		m_Transform.localScale = new Vector2(m_NewScaleSize, m_NewScaleSize);
	}
	public Vector3 m_ScreenPoint;
	#region Coroutine

	private IEnumerator CoHUDFollowTarget()
	{
		Vector3 ScreenPoint = Vector3.zero;
		Vector3 HpBarPosition = Vector3.zero;

		float Distance = 0.0f;

		while (true)
		{
			yield return YieldCache.WaitForEndOfFrame;
			//Debug.Log("$$$$$ ");
			//Debug.Log("!!!!!!!!! : " + m_MyOwner.name);

			ScreenPoint = m_Camera.WorldToScreenPoint(m_Target.position);
			m_ScreenPoint = ScreenPoint;
			if (IsOnScreen(ScreenPoint))
			{
				Utility.Activate(m_Front, false);
			}
			else
			{
				//Distance = GetDistance(m_Owner.Transform.position, LocalUserTransform.position);
				Distance = Vector3.Distance(m_Target.position, m_HeroTransform.position);

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
