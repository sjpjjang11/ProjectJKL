using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csPirate : csMonster
{
	public csWeapon_Projectile_Normal m_Weapon_Projectile_Left = null;

	public override void Settings(csOwner _owner)
	{
		base.Settings(_owner);

		m_Weapon_Projectile_Left.Settings(m_Owner_Monster, m_BugsAction_Attack.ActionIndex);
	}

	public override void Release()
	{
		base.Release();

		if (m_Weapon_Projectile_Left != null)
		{
			m_Weapon_Projectile_Left.Release();
		}
	}

	public virtual void NormalShoot_ActionDirection_Left()
	{
		m_Weapon_Projectile_Left.NormalShoot_ActionDirection(m_Owner_Monster.m_EventHandler_Monster.ActionDirection.Get());
	}

	public override void Attack_Stop()
	{
		base.Attack_Stop();
		Debug.Log("Attack_Stop");
		if (m_AttackStopCallback != null)
		{
			m_AttackStopCallback();

			m_AttackStopCallback = null;
		}
	}
}
