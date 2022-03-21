using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAIDetectionFOV : csAIDetection
{
	protected ObjectDetection_FOV m_ObjectDetection_FOV = null;

	public float m_fFOVAngle = 0.0f;

	public override void Settings(csOwner_Monster _owner)
	{
		base.Settings(_owner);

		m_fFOVAngle = m_Owner.Info_Monster.Detection.FOVAngle;
		m_fDetectionRange = m_Owner.Info_Monster.Detection.FOVRange;

		m_ObjectDetection_FOV = new ObjectDetection_FOV(m_fFOVAngle);
	}

	public override bool Detect()
	{
		if(!IsWithinRangeHero(m_fDetectionRange) || Hero.IsActiveState(eBugsStateType.Die))
		{
			return false;
		}

		return DetectionWithinFOV();
	}

	protected bool DetectionWithinFOV(bool _isCheckObstacle = true)
	{
		return m_ObjectDetection_FOV.DetectionWithinFOV_Target(AICenter, Hero.BugsCenter, m_Owner.Info_Bugs.m_BugsCollider.Radius, _isCheckObstacle);
	}

	public Vector3 DirFromAngle(float _angleInDegrees)
	{
		_angleInDegrees += m_Owner.Transform.eulerAngles.y;

		return new Vector3(Mathf.Sin(_angleInDegrees * Mathf.Deg2Rad), 0.0f, Mathf.Cos(_angleInDegrees * Mathf.Deg2Rad));
	}

	protected virtual void OnDrawGizmosSelected()
	{
		if(Application.isPlaying)
		{
			Vector3 LeftBoundary = DirFromAngle(-m_fFOVAngle / 2.0f);
			Vector3 RightBoundary = DirFromAngle(m_fFOVAngle / 2.0f);

			Gizmos.DrawLine(AICenter, m_Owner.Transform.position + LeftBoundary * m_fDetectionRange);
			Gizmos.DrawLine(AICenter, m_Owner.Transform.position + RightBoundary * m_fDetectionRange);

			Gizmos.DrawWireSphere(AICenter, m_fDetectionRange);			
		}		
	}
}
