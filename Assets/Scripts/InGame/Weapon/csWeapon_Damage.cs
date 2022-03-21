using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;

public class csWeapon_Damage : csWeapon {

	public Transform m_HitEffectSpawnPoint = null;

	public GameObject m_HitEffectObject = null;

	public Collider m_Collider = null;

	protected csCollision m_Collision = null;

	protected ObjectPool m_HitEffectPool = null;

	protected int m_iCollisionLayerMask = 0;

	public override void Settings(csOwner _owner, int _actionIndex)
	{
		base.Settings(_owner, _actionIndex);

		m_Collider = GetComponent<Collider>();
		m_Collision = GetComponent<csCollision>();

		m_iCollisionLayerMask = LayerMask.LayerToName(m_Owner.gameObject.layer) == eCollisionLayerType.Hero.ToString() ?
			csBattleManager.Instance.m_iCollisionLayerMask_Hero : csBattleManager.Instance.m_iCollisionLayerMask_Monster;

		m_Collision.RegisterOnTriggerCallback(HitCollider);
		m_Collision.RegisterCollisionLayerMask(m_iCollisionLayerMask);
	}

	public override void Release()
	{
		base.Release();

		if (m_HitEffectPool != null)
		{
			m_HitEffectPool.AllClearPool();
		}

		m_Collision.Release();
	}

	public virtual void StartAttack(Vector3 _direction)
	{

	}

	public virtual void StartAttack()
	{

	}

	public virtual void StopAttack()
	{
		ClearIgnore();
	}

	protected virtual void AfterHit()
	{

	}

	public virtual void ClearIgnore()
	{
		m_Collision.ClearIgnore();
	}

	protected virtual void HitCollider(Collider _collider)
	{
		if (BattleManager.m_DicObjectColliderIndex.ContainsKey(_collider.GetInstanceID()))
		{
			csOwner Obj = BattleManager.m_DicObjectColliderIndex[_collider.GetInstanceID()];

			if(Obj == m_Owner)
			{
				m_Collision.IgnoreCollider(_collider);

				return;
			}

			Obj.HitMe(m_Owner.OwnerType, m_Owner.OwnerIndex, m_Owner.m_Power.Power_Cur, m_Owner.Info_Bugs.m_BugsAction[m_iActionIndex]);

			Debug.Log(Obj.name);
		}

		if (m_HitEffectPool != null)
		{
			m_HitEffectPool.UseObject(m_HitEffectSpawnPoint.position, false);
		}

		m_Collision.IgnoreCollider(_collider);

		AfterHit();
	}

	public virtual void ForceHitCollider(Collider _collider)
	{
		RemoveCollider(_collider);
		HitCollider(_collider);
	}

	public virtual void RemoveCollider(Collider _collider)
	{
		m_Collision.RemoveIgnore(_collider);
	}
}
