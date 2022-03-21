using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAIActionTakeCover : csAIAction
{
	public override void OnExitState()
	{
		base.OnExitState();

		m_bIsPerform = false;

		m_Agent.StopAgent();
	}

	public override void PerformAction()
	{
		if (!m_bIsPerform)
		{
			m_bIsPerform = true;

			//Vector3 Target = m_Agent.GetLastWayPoint();
			Vector3 Target = m_Owner.m_EventHandler_Monster.TargetWayPoint.Get();
			Debug.Log("PerformAction : " + Target);
			//m_TargetWayPoint = Target;

			m_Agent.StartAgent(Target, eBugsStateType.Run);
		}
	}
}
