using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csStateHandler : MonoBehaviour
{
	private csOwner_Hero m_Owner = null;
	protected csOwner_Hero Owner
	{
		get
		{
			if(m_Owner == null)
			{
				m_Owner = csBattleManager.Instance.m_Hero;
			}

			return m_Owner;
		}
	}

	private csEventHandler_Hero m_EventHandler_Hero = null;
	protected csEventHandler_Hero EventHandler_Hero
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

	}

	public virtual void Settings(csInfo_Hero _info, int _actionIndex)
	{

	}

	public virtual void Settings(csInfo_Hero _info)
	{

	}
}
