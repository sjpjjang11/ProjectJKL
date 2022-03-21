using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class csAIDetection : csAITransitionCondition
{
	public abstract bool Detect();

	public float m_fDetectionRange = 0.0f;

	public override bool Check()
	{
		return Detect();
	}
}
