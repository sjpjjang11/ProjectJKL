using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAIActionAttack : csAIAction
{
	public float m_fAttackDelay = 2.0f;
	public float m_fCurrnetDelay = 0.0f;
	public float m_fLastAttackTime = 0.0f;

	public override void OnEnterState()
	{
		base.OnEnterState();

		if (m_Owner.IsActiveState(eBugsStateType.Attack))
		{
			m_Owner.StateStop(eBugsStateType.Attack);
		}

		if(Time.time - m_fLastAttackTime > m_fAttackDelay)
		{
			m_fCurrnetDelay = 0.0f;
		}

		//m_fCurrnetDelay = 0.0f;
	}

	public override void OnExitState()
	{
		base.OnExitState();

		m_fLastAttackTime = Time.time;

		if (m_Owner.IsActiveState(eBugsStateType.Attack))
		{
			m_Owner.StateStop(eBugsStateType.Attack);
		}
	}

	public override void PerformAction()
	{
		m_fCurrnetDelay -= Time.deltaTime;

		if (m_fCurrnetDelay <= 0.0f)
		{		
			if(m_Owner.IsActiveState(eBugsStateType.Attack))
			{
				m_Owner.StateStop(eBugsStateType.Attack);
			}

			Vector3 RelativePositionFromMeToHero = Utility.RelativePosition(m_Owner.Transform.position, Hero.Transform.position);
			RelativePositionFromMeToHero.y = 0.0f;
			m_Owner.m_EventHandler_Monster.ActionDirection.Set(RelativePositionFromMeToHero);

			m_Owner.StateStart(eBugsStateType.Attack, () =>
			{
				//Debug.Log("m_ActionCallback ");
				m_ActionCallback?.Invoke();
			});
			
			//m_Owner.BattleObject.AttackStart_Remote();

			m_fCurrnetDelay = m_fAttackDelay;
		}
	}
}
