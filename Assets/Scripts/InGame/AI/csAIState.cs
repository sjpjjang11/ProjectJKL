using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectK
{
	[System.Serializable]
	public class AIActionsList : ReorderableArray<csAIAction>
	{

	}

	[System.Serializable]
	public class AITransitionsList : ReorderableArray<csAITransition>
	{

	}
	
	[System.Serializable]
	public class csAIState
	{
		public eBotStateType m_BotState;

		[Reorderable(null, "Action", null)]
		public AIActionsList m_Actions;
		[Reorderable(null, "Transition", null)]
		public AITransitionsList m_Transitions;

		protected csOwner_Monster m_Owner;

		public void Settings(csOwner_Monster _owner)
		{
			m_Owner = _owner;
		}

		public virtual void UpdateState()
		{
			PerformActions();
			EvaluateTransitions();
		}

		public virtual void EnterState()
		{
			if(m_Owner.Info_Monster.ListRecoverableStateType.Contains(m_BotState))
			{
				m_Owner.m_ActingPower.StartFillActingPower();
			}
			else
			{
				m_Owner.m_ActingPower.StopFillActingPower();
			}

			foreach (csAIAction action in m_Actions)
			{
				action.OnEnterState();
			}

			foreach(csAITransition Transitions in m_Transitions)
			{
				Transitions.m_Condition.OnEnterState();
			}
		}

		public virtual void ExitState()
		{
			foreach (csAIAction action in m_Actions)
			{
				action.OnExitState();
			}

			foreach (csAITransition Transitions in m_Transitions)
			{
				Transitions.m_Condition.OnExitState();
			}
		}

		protected virtual void PerformActions()
		{
			if (m_Actions.Count == 0)
			{
				return;
			}

			for (int i = 0; i < m_Actions.Count; i++)
			{
				if (m_Actions[i] != null)
				{
					m_Actions[i].PerformAction();
					//Debug.Log(m_Actions.Count + "      " + m_Actions[i]);
				}
			}
		}

		protected virtual void EvaluateTransitions()
		{
			if(m_Transitions.Count == 0)
			{
				return;
			}

			for(int i = 0; i < m_Transitions.Count; i++)
			{
				if(m_Transitions[i].m_Condition != null)
				{
					if(m_Transitions[i].m_Condition.Check())
					{
						if(m_Transitions[i].m_ConditionTrue != eBotStateType.None)
						{
							Debug.Log("true : " + m_Transitions[i].m_Condition.GetType().Name + "   " + m_Transitions[i].m_ConditionTrue);
							m_Owner.Monster.TransitionToState(m_Transitions[i].m_ConditionTrue);

							return;
						}
					}
					else
					{
						if(m_Transitions[i].m_ConditionFalse != eBotStateType.None)
						{
							Debug.Log("false : " + m_Transitions[i].m_Condition.GetType().Name + "   " + m_Transitions[i].m_ConditionFalse);
							m_Owner.Monster.TransitionToState(m_Transitions[i].m_ConditionFalse);

							return;
						}
					}
				}
			}
		}
	}
}
