using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csInfo_Monster : csInfo_Bugs {

	public List<eBotStateType> ListRecoverableStateType;
	public AIActingPower ActingPower;	
	public AIDetection Detection;
	public bool IsKnockbackAnimationCancel;
	//public eAIActionableCriterion ActionableCriterion;

	public csInfo_Monster()
	{
		m_Health.Max = m_Health.Cur = 100;
		m_Power.Min = 5; //5
		m_Power.Max = 30; //30
		m_Power.Limit_Min = m_Power.Max;
		m_Power.Limit_Max = m_Health.Max;
		m_WalkSpeed = 2.0f; // 2.0f
		m_RunSpeed = 4.0f; //4.0f

		m_BugsAction = new BugsAction[1];
	}
}
