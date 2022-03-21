using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class csAIAction : csAI
{
	protected Action m_ActionCallback = null;

	public abstract void PerformAction();

	protected bool m_bIsPerform = false;

	public bool ActionInProgress
	{
		get;
		set;
	}

	protected override void Start()
	{
		base.Start();

		Initialization();
	}

	protected virtual void Initialization()
	{

	}

	public virtual void RegisterActionCallback(Action _callback)
	{
		m_ActionCallback = _callback;
	}

	public virtual void OnEnterState()
	{
		ActionInProgress = true;
	}

	public virtual void OnExitState()
	{
		ActionInProgress = false;
	}
}