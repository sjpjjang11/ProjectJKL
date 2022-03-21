using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class csCrowdControl : MonoBehaviour {

	protected Action m_UpdateCallback = null;
	protected Action m_CompleteCallback = null;
	public Action<csOwner> m_Callback = null;

	protected IEnumerator m_CoCrowdControl = null;

	public eBugsStateType m_CrowdControlType;

	public csOwner m_Target = null;

	public csCrowdControl m_AddCrowdControl = null;

	public float m_fCrowdControlTime = 0.0f;

	//public int m_iCrowdControlIndex = 0;

	public virtual void RegisterCallback(Action<csOwner> _callback)
	{
		m_Callback = _callback;
	}

	public virtual void AddCrowdControl(csCrowdControl _crowdControl)
	{
		m_AddCrowdControl = _crowdControl;
	}

	public virtual void CrowdControlReady(csOwner _target)
	{

	}

	public virtual void StartCrowdControl(csOwner _target, Action _update = null, Action _complete = null)
	{ 

	}

	public virtual void StopCrowdControl(csOwner _target)
	{

	}

	protected virtual IEnumerator CoCrowdControl(csOwner _target)
	{
		yield return null;
	}
}
