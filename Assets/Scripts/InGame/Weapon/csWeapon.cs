using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class csWeapon : MonoBehaviour {

	protected csOwner m_Owner;

	[HideInInspector]
	public Transform m_Transform = null;

	public Vector3 RenderDirection
	{
		get
		{
			return EventHandler_Hero.RenderDirection.Get()[m_iActionIndex];
		}
	}

	public Vector3 ActionDirection
	{
		get
		{
			return EventHandler_Hero.ActionDirection.Get();
		}
	}

	protected csBattleManager m_BattleManager = null;
	public csBattleManager BattleManager
	{
		get
		{
			if (m_BattleManager == null)
			{
				m_BattleManager = csBattleManager.Instance;
			}

			return m_BattleManager;
		}
	}

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

	protected int m_iDefaultPoolCount = 10;
	protected int m_iActionIndex = 0;

	protected virtual void Awake()
	{
		m_Transform = transform;
	}

	protected virtual void Start()
	{

	}

	public virtual void Settings(csOwner _owner, int _actionIndex)
	{
		m_Owner = _owner;
		
		m_iActionIndex = _actionIndex;
	}	

	public virtual void ChangeActionIndex(int _actionIndex)
	{
		m_iActionIndex = _actionIndex;
	}

	public virtual void Release()
	{

	}

	public virtual csOwner GetOwner()
	{
		return m_Owner;
	}
}
