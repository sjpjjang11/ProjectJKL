using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAIActionBattleMove : csAIAction
{
	public csAIAction m_BotActionCallback = null;

	protected override void Awake()
	{
		base.Awake();

		//m_BotActionCallback.RegisterActionCallback(BattleMove);
	}

	public override void OnExitState()
	{
		base.OnExitState();

		m_Agent.StopAgent();
	}

	private void BattleMove()
	{
		m_Agent.StartAgent(GetBattleMoveTarget(), eBugsStateType.Run, () =>
		{
			if(GetComponent<csAIActionAttack>().m_fCurrnetDelay > 0.0f)
			{
				BattleMove();
			}
		});
	}

	private Vector3 GetBattleMoveTarget()
	{
		Vector3 Center = (Hero.Transform.position + m_Owner.Transform.position) * 0.5f;
		Vector3 RandomPosition = Vector3.zero;

		while (true)
		{
			RandomPosition = Center + Random.insideUnitSphere * m_Owner.Info_Monster.m_BugsAction[csOwner_Monster.Index_Action_Attack].Range;
			RandomPosition.y = m_Owner.Transform.position.y;

			if (m_Grid.NodeFromWorldPoint(RandomPosition).NodeType != eNodeType.Obstacle)
			{
				break;
			}
		}
		
		//m_BattleMoveTarget = RandomPosition;
		//Debug.LogError("m_BattleMoveTarget : " + RandomPosition);

		return RandomPosition;
	}

	public override void PerformAction()
	{
		if (m_Owner.IsActiveState(eBugsStateType.Attack))
		{
			//Debug.Log("IsState : " + m_Owner.IsActiveState(BugsState.Attack));
			if(m_Owner.IsActiveState(eBugsStateType.Run))
			{
				Debug.Log("m_Owner.IsActiveState(BugsState.Run)");
				m_Agent.StopAgent();
			}			
		}
	}
}
