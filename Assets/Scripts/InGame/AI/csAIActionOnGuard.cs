using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAIActionOnGuard : csAIAction
{
	public override void OnEnterState()
	{
		base.OnEnterState();

		m_Owner.Bugs.Animator.SetBool("OnGuard", true);
	}

	public override void OnExitState()
	{
		base.OnExitState();

		m_Owner.Bugs.Animator.SetBool("OnGuard", false);
	}

	public override void PerformAction()
	{
		
	}
}
