using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csHUD : MonoBehaviour
{
	protected IEnumerator m_CoHUDFollowTarget = null;

	[HideInInspector]
	public Transform m_Transform = null;

	public GameObject m_Front = null;

	protected csBattleManager m_BattleManager = null;

	protected csOwner m_Owner;

	protected csPlayerCamera m_PlayerCamera = null;
	protected Camera m_UICamera = null;

	protected csInfo_Bugs m_Info_Bugs
	{
		get
		{
			//Debug.LogError("@@@@@@@@@@@@@ : " + m_Owner + "               " + name);
			return m_Owner.Info_Bugs;
		}
	}

	protected Transform m_FollowTarget = null;

	protected virtual void Awake()
	{
		m_Transform = GetComponent<Transform>();
	}

	public virtual void Settings(csOwner _owner)
	{
		m_BattleManager = csBattleManager.Instance;

		m_Owner = _owner;

		m_PlayerCamera = m_BattleManager.m_PlayerCamera;
		m_UICamera = m_BattleManager.m_BattleUIManager.m_UICamera;		
	}

	public virtual void Release()
	{
		UnregisterEventHandler();

		HUDOff();

		Destroy(gameObject);
	}

	public virtual void ChangeTarget(Transform _target)
	{
		m_FollowTarget = _target;
	}

	public virtual void RegisterEventHandler()
	{
		m_Owner.m_EventHandler_Bugs.Register(this);

		//m_HealthBar.RegisterEventHandler();
	}

	public virtual void UnregisterEventHandler()
	{
		m_Owner.m_EventHandler_Bugs.Unregister(this);

		//m_HealthBar.UnregisterEventHandler();
	}

	public void HUDOn()
	{
		m_CoHUDFollowTarget = CoHUDFollowTarget();
		StartCoroutine(m_CoHUDFollowTarget);
	}

	public void HUDOff()
	{
		if (m_CoHUDFollowTarget != null)
		{
			StopCoroutine(m_CoHUDFollowTarget);

			m_CoHUDFollowTarget = null;
		}

		Utility.Activate(m_Front, false);
	}

	protected virtual Vector3 GetHUDFollowTargetPosition()
	{
		Vector3 ScreenPoint;
		Vector3 HpBarPosition;

		ScreenPoint = m_PlayerCamera.Camera.WorldToScreenPoint(m_FollowTarget.position);
		//HpBarPosition = m_UICamera.ScreenToWorldPoint(ScreenPoint);
		HpBarPosition = ScreenPoint;
		//HpBarPosition.z = 0.0f;

		return HpBarPosition;
	}

	#region Coroutine

	protected virtual IEnumerator CoHUDFollowTarget()
	{
		while (true)
		{
			yield return YieldCache.WaitForEndOfFrame;
			//Debug.Log("$$$$$ ");
			//Debug.Log("!!!!!!!!! : " + m_MyOwner.name);

			if(m_FollowTarget != null)
			{
				m_Transform.position = GetHUDFollowTargetPosition();
			}			
		}
	}
	
	#endregion
}
