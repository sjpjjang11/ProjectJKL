using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;

[System.Serializable]
public class csAITransition
{
	public csAITransitionCondition m_Condition;
	public eBotStateType m_ConditionTrue;
	public eBotStateType m_ConditionFalse;
}


