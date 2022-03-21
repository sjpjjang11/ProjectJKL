using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csInfo_Pirate : csInfo_Monster
{
	public csInfo_Pirate()
	{
		BugsCollider PirateCollider;

		PirateCollider.Center = new Vector3(0.0f, 1.25f, 0.0f);
		PirateCollider.Radius = 0.4f;
		PirateCollider.Height = 2.5f;

		m_BugsCollider = PirateCollider;

		ActingPower.Max = ActingPower.Cur = 10;
		ActingPower.Use = 2; // 3

		Detection.FOVAngle = 120.0f;
		Detection.FOVRange = 15.0f;

		Settings_Attack();
	}

	private void Settings_Attack()
	{
		// BugsAction_Attack
		BugsAction BugsAction_Attack = m_BugsAction[0];

		BugsAction_Attack.AnimationName = "Attack";

		BugsAction_Attack.Range = 15.0f;
		BugsAction_Attack.ActivationTime = 0.0f;
		BugsAction_Attack.CoolTime = 2.0f;

		BugsAction_Attack.ActionIndex = 0;

		BugsAction_Attack.IsActivation = false;
		BugsAction_Attack.IsImmediatelyCoolDown = false;

		m_BugsAction[0] = BugsAction_Attack;
	}
}
