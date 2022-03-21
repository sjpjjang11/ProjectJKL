using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csWraith : csMonster
{
	public csWeapon_Melee m_Weapon_Melee = null;

	public override void Settings(csOwner _owner)
	{
		//ObjectIndex = 1;

		base.Settings(_owner);

		m_Weapon_Melee.Settings(m_Owner_Monster, m_BugsAction_Attack.ActionIndex);
	}

	public void Attack()
	{
		m_Weapon_Melee.StartAttack();
	}
}
