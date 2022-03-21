using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;

public class csWeapon_Melee : csWeapon_Damage
{
	public Transform m_OverlapCapsulePoint1 = null;
	public Transform m_OverlapCapsulePoint2 = null;

	public float m_fDetectRadius = 0.5f;

	public bool m_bIsDebug = false;

	public override void Settings(csOwner _owner, int _actionIndex)
	{
		base.Settings(_owner, _actionIndex);

		if (m_HitEffectObject != null)
		{
			m_HitEffectPool = new ObjectPool();
			m_HitEffectPool.CreatePool(name, m_HitEffectObject, m_Owner.m_WaitingPool_Effect, m_iDefaultPoolCount);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();

		Collider[] Colliders = Physics.OverlapCapsule(m_OverlapCapsulePoint1.position, m_OverlapCapsulePoint2.position, m_fDetectRadius, m_iCollisionLayerMask);
		//Debug.Log(Colliders.Length);
		for (int i = 0; i < Colliders.Length; i++)
		{
			//Debug.Log(Colliders[i].name);
			if (BattleManager.m_DicObjectColliderIndex.ContainsKey(Colliders[i].GetInstanceID()))
			{
				//Debug.Log(Colliders[i].name);
				csOwner Obj = BattleManager.m_DicObjectColliderIndex[Colliders[i].GetInstanceID()];

				Obj.HitMe(m_Owner.OwnerType, m_Owner.OwnerIndex, m_Owner.m_Power.Power_Cur, m_Owner.Info_Bugs.m_BugsAction[m_iActionIndex]);

				//Debug.Log(Obj.name);

				if (m_HitEffectPool != null)
				{
					m_HitEffectPool.UseObject(m_HitEffectSpawnPoint.position, false);
				}
			}
		}
	}

	protected virtual void OnDrawGizmosSelected()
	{
		if(BattleManager != null && m_bIsDebug)
		{
			Gizmos.color = Color.red;

			Gizmos.DrawWireSphere(m_OverlapCapsulePoint1.position, m_fDetectRadius);
			Gizmos.DrawWireSphere(m_OverlapCapsulePoint2.position, m_fDetectRadius);
		}
	}
}
