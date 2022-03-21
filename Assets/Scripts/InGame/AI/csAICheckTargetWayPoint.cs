using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAICheckTargetWayPoint : csAITransitionCondition
{
	public override bool Check()
	{
		//Debug.Log("Decision");
		if (m_Agent.IsCurrentPositionLastWayPoint())
		{
			return true;
		}

		return false;
	}
}
