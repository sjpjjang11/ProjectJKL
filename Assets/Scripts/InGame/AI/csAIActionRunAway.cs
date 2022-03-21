using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAIActionRunAway : csAIAction
{
	public Vector3[] m_InescapableAreaPoints = new Vector3[4];
	private Vector3 m_RunAwayPoint = Vector3.zero;

	private float m_fFront = 0.0f;
	public float m_fHorizontal = 10.0f;
	private float m_fPadding = 2.0f;

	public override void OnEnterState()
	{
		base.OnEnterState();

		m_Agent.RegisterPathFindFailureCallback(FailureRunAwayPoint);
		m_Owner.m_ActingPower.StartFillActingPower();
		//Debug.Log("RunAway OnEnterState");
	}

	public override void OnExitState()
	{
		base.OnExitState();

		m_Agent.StopAgent();
		m_Agent.UnregisterPathFindFailureCallback();
		m_Owner.m_ActingPower.StopFillActingPower();

		m_bIsPerform = false;
	}

	public override void PerformAction()
	{
		//Debug.Log("RunAway PerForAction");
		if (!m_bIsPerform)
		{
			m_bIsPerform = true;
			RunAway();
		}
	}

	private void RunAway()
	{
		m_fFront = GetDistance(Hero.Transform.position, m_Owner.Transform.position) - m_fPadding;
		//m_fHorizontal = 20.0f;

		InescapableAreaDecision(m_fFront, m_fHorizontal, 20.0f);

		bool IsInescapableInside = false;
		bool IsObstacle = false;

		while (true)
		{
			m_RunAwayPoint = RunAwayPointDecision();

			IsInescapableInside = IsContainsInside(m_InescapableAreaPoints, m_RunAwayPoint);

			IsObstacle = m_Grid.NodeFromWorldPoint(m_RunAwayPoint).NodeType == eNodeType.Obstacle;

			if(!IsInescapableInside && !IsObstacle)
			{
				break;
			}
		}

		// Temp
		/*m_Agent.StartAgent(m_RunAwayPoint, eBugsStateType.Run, () =>
		{
			ActionInProgress = false;

		}, IsContainsInside);*/
	}

	private Vector3 RunAwayPointDecision()
	{
		/*float MinX = m_Grid.transform.position.x - (m_Grid.m_GridWorldSize.x * 0.5f);
		float MaxX = m_Grid.transform.position.x + (m_Grid.m_GridWorldSize.x * 0.5f);

		float MinZ = m_Grid.transform.position.z - (m_Grid.m_GridWorldSize.y * 0.5f);
		float MaxZ = m_Grid.transform.position.z + (m_Grid.m_GridWorldSize.y * 0.5f);*/

		float MinX = m_Grid.transform.position.x - 50.0f;
		float MaxX = m_Grid.transform.position.x + 20.0f;

		float MinZ = m_Grid.transform.position.z - 40.0f;
		float MaxZ = m_Grid.transform.position.z + 30.0f;

		Vector3 RunAwayPoint = new Vector3(Random.Range(MinX, MaxX), m_Owner.Transform.position.y, Random.Range(MinZ, MaxZ));

		return RunAwayPoint;
	}

	private void InescapableAreaDecision(float _front, float _horizontal, float _back)
	{
		Vector3 RelativePositionFromTargetToMe = GetRelativePosition(Hero.Transform.position, m_Owner.Transform.position);
		Quaternion RotationFromTargetToMe = Quaternion.LookRotation(RelativePositionFromTargetToMe);

		Vector3 LookAtMeForward = RotationFromTargetToMe * Vector3.forward;
		Vector3 LookAtMeRight = RotationFromTargetToMe * Vector3.right;

		// Left Front
		m_InescapableAreaPoints[0] = Hero.Transform.position + (LookAtMeForward * _front) + (-LookAtMeRight * _horizontal);
		// Right Front
		m_InescapableAreaPoints[1] = Hero.Transform.position + (LookAtMeForward * _front) + (LookAtMeRight * _horizontal);
		// Right Back
		m_InescapableAreaPoints[2] = Hero.Transform.position + (-LookAtMeForward * _back) + (LookAtMeRight * _horizontal);
		// Left Back
		m_InescapableAreaPoints[3] = Hero.Transform.position + (-LookAtMeForward * _back) + (-LookAtMeRight * _horizontal);

		for (int i = 0; i < m_InescapableAreaPoints.Length; i++)
		{
			m_InescapableAreaPoints[i].y = 0.0f;
		}
	}

	private void FailureRunAwayPoint()
	{
		Debug.Log("FailureRunAwayPoint");

		m_fFront -= m_fPadding;
		m_fHorizontal -= m_fPadding;

		InescapableAreaDecision(m_fFront, m_fHorizontal, 20.0f);

		m_Agent.StartAgent(m_RunAwayPoint, eBugsStateType.Run);
	}

	public bool IsContainsInside(Vector3[] _points, Vector3 _checkInside)
	{
		int j = _points.Length - 1;

		bool inside = false;

		for (int i = 0; i < _points.Length; j = i++)
		{
			Vector3 PointI = _points[i];
			Vector3 PointJ = _points[j];

			if (((PointI.z <= _checkInside.z && _checkInside.z < PointJ.z) || (PointJ.z <= _checkInside.z && _checkInside.z < PointI.z)) &&
				(_checkInside.x < (PointJ.x - PointI.x) * (_checkInside.z - PointI.z) / (PointJ.z - PointI.z) + PointI.x))
			{
				inside = !inside;
			}
		}

		return inside;
	}

	private bool IsContainsInside(csNode _node)
	{
		//Debug.Log("ContainsInside");
		return IsContainsInside(m_InescapableAreaPoints, _node.m_WorldPosition);
	}

	private void OnDrawGizmos()
	{
		if(Application.isPlaying && Hero != null)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawLine(m_InescapableAreaPoints[0], m_InescapableAreaPoints[1]);
			Gizmos.DrawLine(m_InescapableAreaPoints[1], m_InescapableAreaPoints[2]);
			Gizmos.DrawLine(m_InescapableAreaPoints[2], m_InescapableAreaPoints[3]);
			Gizmos.DrawLine(m_InescapableAreaPoints[3], m_InescapableAreaPoints[0]);
		}
	}
}
