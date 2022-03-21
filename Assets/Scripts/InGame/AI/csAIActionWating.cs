using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAIActionWating : csAIAction
{
	public override void OnEnterState()
	{
		base.OnEnterState();

		m_Owner.m_ActingPower.StartFillActingPower();
	}

	public override void PerformAction()
	{
		
	}
}
