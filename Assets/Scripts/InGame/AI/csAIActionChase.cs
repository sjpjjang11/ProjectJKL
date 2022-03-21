using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAIActionChase : csAIAction {

	protected Vector3 m_PrevHeroPosition = Vector3.zero;

	public float m_fAllowPathUpdate = 3.0f;
	public float m_fChaseCycle = 2.0f;
	public float m_fCurrentChaseCycle = 0.0f;
	public float m_fRange = 0.0f;

	/*private void Update()
	{
		Debug.Log("@@@@@");
		//csPathRequestManager.RequestPath(transform.position, m_Target.position, OnPathFound);
		if (Input.GetButtonDown("Jump"))
		{
			Debug.Log("Jump");
			csPathRequestManager.RequestPath(transform.position, m_Owner.m_Target.Transform.position, OnPathFound);
		}		
	}*/

	public override void Settings(csOwner_Monster _owner)
	{
		base.Settings(_owner);

		m_fRange = m_Owner.Info_Monster.m_BugsAction[csOwner_Monster.Index_Action_Attack].Range;
	}

	public override void OnEnterState()
	{
		base.OnEnterState();

		m_Agent.StartAgent(Hero.Transform.position, eBugsStateType.Run, ReAgent);

		m_PrevHeroPosition = Hero.Transform.position;
	}

	public override void OnExitState()
	{
		base.OnExitState();

		m_Agent.StopAgent();
	}

	public override void PerformAction()
	{
		float DistanceFromPrevToCurrent = Vector3.Distance(m_PrevHeroPosition, Hero.Transform.position);
		
		if (DistanceFromPrevToCurrent >= m_fAllowPathUpdate/* || m_fCurrentChaseCycle <= 0.0f*/)
		{
			m_PrevHeroPosition = Hero.Transform.position;
			m_fCurrentChaseCycle = m_fChaseCycle;

			m_Agent.StartAgent(Hero.Transform.position, eBugsStateType.Run, ReAgent);
		}

		m_fCurrentChaseCycle -= Time.deltaTime;
	}

	private void ReAgent()
	{
		Debug.LogError("ReAgent : " + IsWithinRangeHero(m_fRange) + "  " + m_Owner.name);
		if (!IsWithinRangeHero(m_fRange))
		{
			m_Agent.StartAgent(Hero.Transform.position, eBugsStateType.Run, ReAgent);
		}
	}
}
