using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csStun : csCrowdControl
{
	public override void StartCrowdControl(csOwner _target, Action _update = null, Action _complete = null)
	{
		if(_update != null)
		{
			m_UpdateCallback = _update;
		}

		if(_complete != null)
		{
			m_CompleteCallback = _complete;
		}

		m_Target = _target;

		m_CoCrowdControl = CoCrowdControl(_target);
		StartCoroutine(m_CoCrowdControl);
	}

	protected override IEnumerator CoCrowdControl(csOwner _target)
	{
		float StunTime = m_fCrowdControlTime;

		yield return YieldCache.WaitForSeconds(StunTime);

		m_CompleteCallback?.Invoke();

		m_Target = null;
	}
}
