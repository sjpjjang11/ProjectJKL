using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/*2018.07.07
* hkh
*
*/

public class csWeapon_Projectile : csWeapon
{
	[Header("Projectile Spawn")]
	public Transform m_SpawnPoint = null;	
	protected ObjectPool<csProjectile> m_ProjectilePool = null;              // 원거리 발사체 풀
	public GameObject m_Projectile = null;            // 원거리 발사체 오브젝트	

	public override void Settings(csOwner _owner, int _actionIndex)
	{
		base.Settings(_owner, _actionIndex);

		CreateProjectile();		
	}

	public override void Release()
	{
		DestroyProjectile();
	}

	protected void CreateProjectile()
	{
		// 발사체 오브젝트 존재한다면
		if (m_Projectile != null)
		{
			// 원거리 발사체 풀 인스턴스화
			m_ProjectilePool = new ObjectPool<csProjectile>();

			// 원거리 발사체 풀 생성
			m_ProjectilePool.CreatePool(m_Projectile, m_Owner.m_WaitingPool_Bullet, m_iDefaultPoolCount);
			
			for (int i = 0; i < m_ProjectilePool.m_ListComponent.Count; i++)
			{
				m_ProjectilePool.m_ListComponent[i].Settings(m_Owner, m_iActionIndex);
				m_ProjectilePool.m_ListComponent[i].Settings(this);
			}
		}
	}

	public void DestroyProjectile()
	{
		Debug.LogError(" DestroyProjectile");
		if (m_ProjectilePool != null)
		{
			for (int i = 0; i < m_ProjectilePool.m_ListComponent.Count; i++)
			{
				//m_ProjectilePool.m_ListComponent[i].Release();
			}

			m_ProjectilePool.AllClearPool();
			m_ProjectilePool = null;
		}
	}

	protected float Remap(float x, float A, float B, float C, float D)
	{
		//(projectileIndex, 0, totalProjectiles - 1, -Spread.x, Spread.x);
		float RemappedValue = C + (x - A) / (B - A) * (D - C);
		//Debug.Log(C + " + (" + x + " - " + A + ") / (" + B + " - " + A + ") * (" + D + " - " + C + ")" + "=========  " + RemappedValue);
		return RemappedValue;
	}

	public void NormalShoot_RenderDirection(Vector3 _direction = default)
	{
		if (_direction == Vector3.zero)
		{
			_direction = RenderDirection;
		}

		Shoot(_direction);
	}

	public void NormalShoot_ActionDirection(Vector3 _direction = default)
	{
		if (_direction == Vector3.zero)
		{
			_direction = ActionDirection;
		}

		Shoot(_direction);
	}

	public void NormalShoot_Forward()
	{
		Shoot(Vector3.zero);
	}

	protected virtual csProjectile SpawnProjectile()
	{
		int ObjectIndex;
		csProjectile Projectile;

		ObjectIndex = m_ProjectilePool.UseObject(m_SpawnPoint.position, false);

		if (ObjectIndex == m_iDefaultPoolCount)
		{
			m_ProjectilePool.m_ListComponent[ObjectIndex].Settings(m_Owner, m_iActionIndex);
			m_ProjectilePool.m_ListComponent[ObjectIndex].Settings(this);
			m_iDefaultPoolCount++;
		}

		Projectile = m_ProjectilePool.m_ListComponent[ObjectIndex];
		//Debug.LogError("!!!!!!!!!!!!!!!!!!!!!!!!! : " + Projectile.transform.position);
		return Projectile;
	}

	protected virtual void Shoot(Vector3 _direction = default)
	{
		SpawnProjectile().StartAttack(_direction);
	}
}
