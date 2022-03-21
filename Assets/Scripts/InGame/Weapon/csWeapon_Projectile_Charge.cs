using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csWeapon_Projectile_Charge : csWeapon_Projectile
{
	public GameObject m_ChargeEffect = null;

	protected override void Awake()
	{
		base.Awake();

		Utility.Activate(m_ChargeEffect, false);
	}

	public void Charge()
	{
		Utility.Activate(m_ChargeEffect, true);
	}

	public void Shoot(Vector3 _direction = default(Vector3))
	{
		Utility.Activate(m_ChargeEffect, false);
		SpawnProjectile().StartAttack(RenderDirection);
	}
}
