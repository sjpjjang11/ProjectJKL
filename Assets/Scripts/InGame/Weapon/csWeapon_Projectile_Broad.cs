using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csWeapon_Projectile_Broad : csWeapon_Projectile
{
	public Vector3[] m_BroadSpawnPoint = null;

	public float m_fBroad = 0.0f;

	public int m_iBroadShotCount = 0;
	public int m_iBroadPerShot = 0;

	public int m_iCurrentShotCount = 0;
	public int CurrentShotCount
	{
		get
		{
			m_iCurrentShotCount++;

			if(m_iCurrentShotCount == m_iBroadShotCount)
			{
				m_iCurrentShotCount = 0;
			}

			return m_iCurrentShotCount;
		}
		set
		{
			m_iCurrentShotCount = value;
		}
	}

	protected override void Start()
	{
		base.Start();

		m_BroadSpawnPoint = new Vector3[m_iBroadShotCount];
	}

	public Vector3 Test()
	{
		Vector3 P = m_SpawnPoint.position;

		float R = Remap(CurrentShotCount, 0, m_iBroadShotCount - 1, -m_fBroad, m_fBroad);

		//m_BroadSpawnPoint[CurrentShotCount] = new Vector3(P.x + R, P.y, P.z);

		return new Vector3(P.x + R, P.y, P.z);
	}

	protected override csProjectile SpawnProjectile()
	{
		int ObjectIndex;
		csProjectile Projectile;

		ObjectIndex = m_ProjectilePool.UseObject(Test(), false);
		
		if (ObjectIndex == m_iDefaultPoolCount)
		{
			m_ProjectilePool.m_ListComponent[ObjectIndex].Settings(m_Owner, m_iActionIndex);
			m_iDefaultPoolCount++;
		}

		Projectile = m_ProjectilePool.m_ListComponent[ObjectIndex];

		return Projectile;
	}

	public void BroadShootReadyStart()
	{
		CurrentShotCount = 0;
	}

	public void BroadShootReadyStop()
	{

	}

	public void BroadShoot(Vector3 _aim = default(Vector3))
	{
		if (_aim != Vector3.zero)
		{
			SpawnProjectile().StartAttack(_aim);
		}
		else
		{
			//SetTargetPoint(0, m_AttackRangeRenderer.m_RenderStartPoint.forward);
			SpawnProjectile().StartAttack(RenderDirection);
		}
	}

	/*private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(m_SpawnPoint.position, 0.05f);

		if (m_AttackRangeRenderer != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawRay(m_AttackRangeRenderer.m_RenderStartPoint.position, m_AttackRangeRenderer.m_RenderStartPoint.forward * m_OwnerInfo.AttackRange);
		}
	}*/
}
