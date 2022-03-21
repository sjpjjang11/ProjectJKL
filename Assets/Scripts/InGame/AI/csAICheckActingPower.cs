using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAICheckActingPower : csAITransitionCondition
{
	public eAIActionableCriterion m_ActionableCriterion;

	public bool m_bIsActionable = false;

	public override void Settings(csOwner_Monster _owner)
	{
		base.Settings(_owner);

		m_ActionableCriterion = eAIActionableCriterion.Consumption;
	}

	public override bool Check()
	{
		bool IsActionable = false;

		switch (m_ActionableCriterion)
		{
			case eAIActionableCriterion.Consumption:

				IsActionable = m_Owner.m_ActingPower.IsActionable_Consumption();

				if(!IsActionable)
				{
					m_ActionableCriterion = eAIActionableCriterion.Full;
				}

				break;

			case eAIActionableCriterion.Full:

				IsActionable = m_Owner.m_ActingPower.IsActionable_Full();

				if (IsActionable)
				{
					m_ActionableCriterion = eAIActionableCriterion.Consumption;
				}

				break;
		}

		m_bIsActionable = IsActionable;

		if (!IsActionable && !m_Owner.IsActiveState(eBugsStateType.Attack))
		{
			return false;
		}	
		else
		{
			return true;
		}
	}
}
