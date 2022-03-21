using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csPower : MonoBehaviour {

	private csEventHandler_Bugs m_EventHandler = null;

	private int m_Power_Min_Limit = 0;
	private int m_Power_Max_Limit = 0;

	[SerializeField]
	protected int m_iPower_Cur = 0;
	public int Power_Cur
	{
		get
		{
			return m_iPower_Cur;
		}
		private set
		{
			m_iPower_Cur = value;
		}
	}

	[SerializeField]
	protected int m_iPower_Max = 0;
	public int Power_Max
	{
		get
		{
			return m_iPower_Max;
		}
		protected set
		{
			int Val = value;
			if (Val > m_Power_Max_Limit)
			{
				Val = m_Power_Max_Limit;
			}
			m_iPower_Max = Val;
		}
	}

	[SerializeField]
	protected int m_iPower_Min = 0;
	public int Power_Min
	{
		get
		{
			return m_iPower_Min;
		}
		protected set
		{
			int Val = value;
			if (Val > m_Power_Min_Limit)
			{			
				Val = m_Power_Min_Limit;
			}
			m_iPower_Min = Val;
		}
	}

	private void Awake()
	{
		m_EventHandler = GetComponent<csEventHandler_Bugs>();
	}

	public void Settings(BugsPower _power)
	{
		m_Power_Min_Limit = _power.Limit_Min;
		m_Power_Max_Limit = _power.Limit_Max;
		Power_Min = _power.Min;
		Power_Max = _power.Max;	
		Power_Cur = Power_Min;
	}

	public void Release()
	{

	}

	public void PowerIncrease(int _increase)
	{
		Power_Cur += _increase;
	}

	public void SetPower(int _power)
	{
		Power_Cur = _power;
	}

	public void Initialize()
	{
		Power_Cur = Power_Min;
	}
}
