﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csInfo_Goblin : csInfo_Monster
{
	public csInfo_Goblin()
	{
		BugsCollider GoblinCollider;

		GoblinCollider.Center = new Vector3(0.0f, 1.5f, 0.0f);
		GoblinCollider.Radius = 0.6f; //0.4
		GoblinCollider.Height = 3.0f;

		m_BugsCollider = GoblinCollider;

		ListRecoverableStateType = new List<eBotStateType>()
		{
			{ eBotStateType.Waiting },
			{ eBotStateType.Chase },
			{ eBotStateType.Battle },
		};

		ActingPower.Max = 10;
		ActingPower.Cur = 0;
		ActingPower.Use = 3; // 5

		Detection.FOVAngle = 120.0f;
		Detection.FOVRange = 15.0f;

		IsKnockbackAnimationCancel = false;

		Settings_Attack();
	}

	private void Settings_Attack()
	{
		// BugsAction_Attack
		BugsAction BugsAction_Attack = m_BugsAction[0];

		BugsAction_Attack.StateType = eBugsStateType.Attack;

		BugsAction_Attack.CrowdControl = null;

		BugsAction_Attack.AnimationName = "Attack";

		BugsAction_Attack.Range = 3.0f;
		BugsAction_Attack.ActivationTime = 0.0f;
		BugsAction_Attack.CoolTime = 2.0f;

		BugsAction_Attack.ActionIndex = 0;

		BugsAction_Attack.IsActivation = false;
		BugsAction_Attack.IsImmediatelyCoolDown = false;

		m_BugsAction[0] = BugsAction_Attack;
	}
}
