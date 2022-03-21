using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class csHealth : MonoBehaviour {

	private csEventHandler_Bugs m_EventHandler = null;

	[SerializeField]
	protected int m_iHealth_Max = 0;
	public int Health_Max
	{
		get
		{
			return m_iHealth_Max;
		}
		private set
		{
			m_iHealth_Max = value;
		}
	}

	[SerializeField]
	protected int m_iHealth_Cur;
	public int Health_Cur
	{
		get
		{
			return m_iHealth_Cur;
		}
		private set
		{
			
			int Val = value;
			if(Val > Health_Max)
			{
				Val = Health_Max;
			}

			//Debug.Log(Val);
			m_iHealth_Cur = Val;
		}
	}

	private void Awake()
	{
		m_EventHandler = GetComponent<csEventHandler_Bugs>();
	}

	public void Settings(BugsHealth _health)
	{
		Health_Max = _health.Max;
		Health_Cur = _health.Cur;
		//Debug.LogError(Health_Max + "  " + Health_Cur);

		m_EventHandler.SetHealth_HUD.Send();
	}

	public void Release()
	{
		
	}

	public void Initialize()
    {
		SetHealth(Health_Max);
    }

	public void HealthIncrease(int _increase)
	{
		Health_Cur += _increase;

		m_EventHandler.SetHealth_HUD.Send();
	}

	public void HealthDecrease(int _decrease, Action _callback)
	{
		Health_Cur -= _decrease;

		m_EventHandler.SetHealth_HUD.Send();

		if (Health_Cur <= 0)
		{
			_callback();
		}
	}

	public void SetHealth(int _currentHealth)
	{
		Health_Cur = _currentHealth;

		m_EventHandler.SetHealth_HUD.Send();
	}
}
