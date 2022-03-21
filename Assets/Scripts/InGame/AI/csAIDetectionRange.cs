using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAIDetectionRange : csAIDetection
{
	protected ObjectDetection m_ObjectDetection = null;

	public bool m_bIsWithinRangeHero = false;
	public bool m_bIsObstacleBetweenMeAndTarget = false;

	public override void Settings(csOwner_Monster _owner)
	{
		base.Settings(_owner);

		m_fDetectionRange = m_Owner.Info_Monster.m_BugsAction[csOwner_Monster.Index_Action_Attack].Range;

		m_ObjectDetection = new ObjectDetection();
	}

	public override bool Detect()
	{
		m_bIsWithinRangeHero = IsWithinRangeHero(m_fDetectionRange);
		m_bIsObstacleBetweenMeAndTarget = m_ObjectDetection.DetectionObstacleBetweenMeAndTarget(AICenter, Hero.BugsCenter, m_Owner.Info_Bugs.m_BugsCollider.Radius);
		return IsWithinRangeHero(m_fDetectionRange) && !m_ObjectDetection.DetectionObstacleBetweenMeAndTarget(AICenter, Hero.BugsCenter, m_Owner.Info_Bugs.m_BugsCollider.Radius);
	}
}
