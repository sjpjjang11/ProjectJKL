using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class csCharge : MonoBehaviour {

	protected csOwner m_Owner;

	protected CountNumber m_CountNumber = null;

	public float m_fMaxChargeTime = 0.0f;

	public void Settings(csOwner _owner)
	{
		m_Owner = _owner;

		m_CountNumber = new CountNumber();

		m_fMaxChargeTime = 0.4f;
		//RegisterHandler();
	}

	public void Release()
	{
		//UnregisterHandler();
	}

	public void StartCharge()
	{
		int MinPower = m_Owner.m_Power.Power_Min;
		int MaxPower = m_Owner.m_Power.Power_Max;

		m_CountNumber.Count(gameObject.GetInstanceID(), MinPower, MaxPower, m_fMaxChargeTime, (value) =>
		{
			m_Owner.m_Power.SetPower((int)value);
		});
	}

	public void StopCharge()
	{
		m_Owner.m_Power.Initialize();
		m_CountNumber.ForceStop(gameObject.GetInstanceID());		
	}

	/*public void RegisterHandler()
	{
		m_Owner.m_EventHandler.Register(this);
	}

	public void UnregisterHandler()
	{
		m_Owner.m_EventHandler.Unregister(this);
	}*/

	/*#region State Method

	protected virtual void OnStart_Charge(Action _callback = null)
	{
		if (_callback != null)
		{
			_callback();
		}
	}

	protected virtual void OnStop_Charge(Action _callback = null)
	{
		if (_callback != null)
		{
			_callback();
		}
	}

	#endregion*/
}
