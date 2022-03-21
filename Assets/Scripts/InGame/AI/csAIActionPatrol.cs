using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAIActionPatrol : csAIAction {

	protected IEnumerator m_CoPathTargetDecision = null;

	public Vector3 m_PathTarget = Vector3.zero;
	public eBugsStateType m_MoveType;

	public float m_fPathTargetDecisionCycle = 3.0f;
	public float m_fCurrentPathTargetDecisionCycle = 0.0f;

	protected override void Awake()
	{
		base.Awake();

		//m_Agent = GetComponent<csAIAgent>();
	}

	public override void OnEnterState()
	{
		base.OnEnterState();

		PatrolPointDecision();
	}

	public override void OnExitState()
	{
		base.OnExitState();

		//m_Agent.StopAgent();
		m_Agent.StopAgent();
	}
	//public float m_fDistance;
	public override void PerformAction()
	{
		if (m_fCurrentPathTargetDecisionCycle <= 0.0f || m_Agent.IsCurrentPositionLastWayPoint())
		{
			m_fCurrentPathTargetDecisionCycle = m_fPathTargetDecisionCycle;
			PatrolPointDecision();
		}

		m_fCurrentPathTargetDecisionCycle -= Time.deltaTime;
		//Debug.Log(Vector3.Distance(m_PathTarget, transform.position));
		/*m_fDistance = Utility.Distance(transform.position, m_PathTarget);
		if (m_fDistance == 0)
		{
			PathTargetDecision();
		}*/
	}

	private void PatrolPointDecision()
	{
		while(true)
		{
			//Debug.Log("While");
			Transform[] SpawnPoint = csBattleManager.Instance.m_SpawnManager.m_SpawnPoint;

			int RandNum = Random.Range(0, SpawnPoint.Length);
			m_PathTarget = SpawnPoint[RandNum].position;
			//Debug.Log(m_PathTarget);

			if(Utility.Distance(transform.position, m_PathTarget) != 0 && m_Grid.NodeFromWorldPoint(m_PathTarget).NodeType != eNodeType.Obstacle)
			{
				break;
			}

			//m_PathTarget.y = transform.position.y;
		}

		//m_Agent.StartAgent(m_PathTarget);
		m_Agent.StartAgent(m_PathTarget, m_MoveType);
		
	}
}
