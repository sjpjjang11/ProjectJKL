using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;

public class csMonster : csBugs
{
	protected Action m_AttackStopCallback = null;
	protected IEnumerator m_CoState = null;

	public eBotStateType m_FirstState;

	public List<csAIState> m_States;

	protected csEventHandler_Monster m_EventHandler_Monster = null;

	protected csOwner_Monster m_Owner_Monster;

	[SerializeField]
	private csAIState m_CurrentState;
	public csAIState CurrentState
	{
		get
		{
			return m_CurrentState;
		}
	}

	[SerializeField]
	private csInfo_Monster m_Info_Monster;
	public csInfo_Monster Info_Monster
	{
		get
		{
			if(m_Info_Monster == null)
			{
				m_Info_Monster = (csInfo_Monster)m_Owner_Monster.Info_Bugs;
			}
			return m_Info_Monster;
		}
	}

	protected csAIAgent m_Agent = null;

	protected BugsAction m_BugsAction_Attack;

	protected override void Start()
	{
		base.Start();
	}

	public override void Settings(csOwner _owner)
	{
		base.Settings(_owner);

		//ObjectIndex = 1;
		ObjectIndex = m_iTempObjectIndex;

		m_Owner_Monster = (csOwner_Monster)m_Owner;

		for (int i = 0; i < m_States.Count; i++)
		{
			m_States[i].Settings(m_Owner_Monster);
		}

		m_Agent = GetComponent<csAIAgent>();

		m_Agent.Settings(m_Owner_Monster);

		GetBugsAction();

		csAI[] Bot = GetComponents<csAI>();
		for (int i = 0; i < Bot.Length; i++)
		{
			//Debug.LogError("Bot : " + Bot[i].GetType());
			Bot[i].Settings(m_Owner_Monster);
		}

		m_RotationToTarget.Settings(m_Owner_Monster);
		m_Activity.Settings(m_Owner_Monster);

		m_EventHandler_Monster = m_Owner_Monster.m_EventHandler_Monster;

		BugsCollider BugsCollider = Info_Monster.m_BugsCollider;

		m_Owner.CharacterController.center = BugsCollider.Center;
		m_Owner.CharacterController.radius = BugsCollider.Radius;
		m_Owner.CharacterController.height = BugsCollider.Height;
	}


	public override void Release()
	{
		base.Release();	
	}

	protected override void GetBugsAction()
	{
		m_BugsAction_Attack = Info_Monster.m_BugsAction[csOwner_Hero.Index_Action_Attack];
	}

	public void StartAI()
	{
		TransitionToState(m_FirstState);

		m_CoState = CoState();
		StartCoroutine(m_CoState);
	}

	public void StopAI()
	{
		m_CurrentState.ExitState();
		//m_CurrentState.m_BotState = BotState.None;

		if (m_CoState != null)
		{
			StopCoroutine(m_CoState);
		}
	}

	protected csAIState FindState(eBotStateType _stateName)
	{
		foreach (csAIState State in m_States)
		{
			if (State.m_BotState == _stateName)
			{
				return State;
			}
		}

		return null;
	}

	public virtual void TransitionToState(eBotStateType _newStateName)
	{
		if (_newStateName != m_CurrentState.m_BotState)
		{			
			m_CurrentState.ExitState();

			m_CurrentState = FindState(_newStateName);

			if (m_CurrentState != null)
			{
				//Debug.LogError("TransitionToState : " + m_CurrentState.m_BotState);
				m_CurrentState.EnterState();
			}
		}
		else
		{
			for(int i = 0; i < m_CurrentState.m_Actions.Count; i++)
			{
				if(!m_CurrentState.m_Actions[i].ActionInProgress)
				{
					//Debug.LogError("ReTransitionToState : " + m_CurrentState.m_BotState);
					m_CurrentState.ExitState();
					m_CurrentState.EnterState();
				}
			}
		}
	}

	public override void Attack_Start()
	{
		base.Attack_Start();
	}

	public override void Attack_Start(Action _callback = null)
	{
		base.Attack_Start(_callback);

		//Debug.Log("Attack_Start");

		Vector3 ActionDirection = m_Owner_Monster.m_EventHandler_Monster.ActionDirection.Get();

		RotateToTarget(m_Owner_Monster.Transform, ActionDirection, 0.0f, () =>
		{
			Animator.SetTrigger("Attack");

			m_Owner_Monster.m_ActingPower.ActingPowerDecrease();
		});
	}

	public override void Attack_Stop()
	{
		base.Attack_Stop();

		//Debug.Log("Attack_Stop");
	}

	#region Coroutine

	protected IEnumerator CoState()
	{
		//Debug.Log("CoState");
		while(true)
		{
			yield return null;
			m_CurrentState.UpdateState();
			//Debug.Log("@@@@@@@@@@Current State");
		}
	}

	#endregion
}
