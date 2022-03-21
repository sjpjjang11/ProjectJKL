using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAICheckHeroLookAtMe : csAITransitionCondition
{
	public override bool Check()
	{		
		//Debug.Log("Decision");
		if(m_Agent.IsCurrentPositionLastWayPoint() && !IsHeroLookAtMe(AICenter))
		{
			return false;
		}

		return true;
	}
}
