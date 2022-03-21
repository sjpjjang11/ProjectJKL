using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;

public abstract class csAITransitionCondition : csAI {

	public abstract bool Check();

	//public BugsState m_ProhibitTransition;
	//protected csState m_ProhibitTransitionState;

	public Vector3 AICenter
	{
		get
		{
			return m_Owner.BugsCenter;
		}
	}

	protected int m_iLayerMask_Hero;
	protected int m_iLayerMask_Map;

	protected override void Awake()
	{
		base.Awake();

		//m_Collider = m_Owner.GetComponent<Collider>();

		m_iLayerMask_Hero = csBattleManager.Instance.m_iLayerMask_Hero;
		m_iLayerMask_Map = csBattleManager.Instance.m_iLayerMask_Map;
		//m_iTargetLayerMask = BattleLayer.CollisionLayerMask(gameObject);		
	}

	public virtual void OnEnterState()
	{
		//Debug.LogError("OnEnterState : " + m_Owner.Monster.CurrentState.m_BotState);
	}

	public virtual void OnExitState()
	{
		//Debug.LogError("OnExitState : " + m_Owner.Monster.CurrentState.m_BotState);
	}

	protected virtual bool IsHeroLookAtMe(Vector3 _from)
	{
		float DistanceFromToHero = GetDistance(_from, Hero.BugsCenter);
		Vector3 DirectionFromToHero = GetDirection(_from, Hero.BugsCenter);

		Ray Ray = new Ray(_from, DirectionFromToHero);
		Debug.DrawRay(_from, DirectionFromToHero * DistanceFromToHero);
		if (!Physics.Raycast(Ray, DistanceFromToHero, m_iLayerMask_Map))
		{			
			return true;
		}

		return false;
	}	
}
