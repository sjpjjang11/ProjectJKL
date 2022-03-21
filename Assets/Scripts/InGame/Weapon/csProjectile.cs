using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csProjectile : csWeapon_Damage
{
	public GameObject m_SpawnEffect = null;

	protected Transform m_SpawnEffectSpawnPoint = null;

	public Vector3 m_Target = Vector3.zero;
	public Vector3 m_StartFirePosition = Vector3.zero;
	public Vector3 m_TempLocalScale = Vector3.zero;

	protected csWeapon_Projectile m_Weapon_Projectile = null;

	public float m_fFireSpeed = 0.0f;
	public float m_fDistanceToTarget = 0.0f;

	protected virtual void FixedUpdate()
	{
		if(Utility.IsActive(gameObject))
		{
			Fire();
		}
	}

	protected virtual void SetSpawnEffect()
	{
		m_Transform.LookAt(m_Target);
		m_SpawnEffect.transform.SetParent(m_SpawnEffectSpawnPoint);		
		m_SpawnEffect.transform.localPosition = Vector3.zero;
		m_SpawnEffect.transform.localRotation = Quaternion.identity;

		if (Utility.IsActive(m_SpawnEffect))
		{
			Utility.Activate(m_SpawnEffect, false);
		}

		Utility.Activate(m_SpawnEffect, true);
	}

	protected virtual void Fire()
	{
		m_Transform.position = Vector3.MoveTowards(m_Transform.position, m_Target, m_fFireSpeed * Time.deltaTime);

		m_fDistanceToTarget = Vector3.Distance(m_Transform.position, m_Target);

		if (m_fDistanceToTarget == 0.0f)
		{
			StopAttack();
		}
	}

	public override void Settings(csOwner _owner, int _actionIndex)
	{
		base.Settings(_owner, _actionIndex);

		if (m_HitEffectObject != null)
		{
			m_HitEffectPool = new ObjectPool();
			m_HitEffectPool.CreatePool(m_Owner.name, m_HitEffectObject, m_Owner.m_WaitingPool_Effect, m_iDefaultPoolCount);
		}

		if (m_SpawnEffect != null)
		{
			m_TempLocalScale = m_SpawnEffect.transform.localScale;
			m_SpawnEffect.transform.SetParent(m_Owner.m_WaitingPool_Effect);
			m_SpawnEffect.transform.localScale = m_TempLocalScale;
			Utility.Activate(m_SpawnEffect, false);
		}	
	}

	public virtual void Settings(csWeapon_Projectile _weapon)
	{
		m_Weapon_Projectile = _weapon;

		m_SpawnEffectSpawnPoint = m_Weapon_Projectile.m_SpawnPoint;
	}

	public override void StartAttack(Vector3 _direction = default(Vector3))
	{
		base.StartAttack(_direction);
		
		m_StartFirePosition = m_Transform.position;

		m_Owner.m_EventHandler_Bugs.ActivatedWeapon.Get().Add(this);

		Vector3 Target = Vector3.zero;

		if (_direction == Vector3.zero)
		{
			Target = m_Transform.position + m_Weapon_Projectile.m_Transform.forward * m_Owner.Info_Bugs.m_BugsAction[m_iActionIndex].Range;			
		}
		else
		{
			Target = m_Transform.position + _direction * m_Owner.Info_Bugs.m_BugsAction[m_iActionIndex].Range;			
		}
		
		Target.y = m_Transform.position.y;
		Debug.Log(_direction + "  " + Target);
		m_Target = Target;

		if(m_SpawnEffect != null)
		{
			SetSpawnEffect();
		}		
	}

	public override void StopAttack()
	{
		base.StopAttack();

		if(m_SpawnEffect != null)
		{
			m_SpawnEffect.transform.SetParent(m_Owner.m_WaitingPool_Effect);
			m_SpawnEffect.transform.localScale = m_TempLocalScale;
		}
		
		transform.localPosition = Vector3.zero;

		m_Owner.m_EventHandler_Bugs.ActivatedWeapon.Get().Remove(this);

		Deactivate();
	}

	protected override void AfterHit()
	{
		base.AfterHit();

		StopAttack();
	}

	private void Deactivate()
	{
		if (Utility.IsActive(gameObject))
		{
			Utility.Activate(gameObject, false);
		}
	}
}
