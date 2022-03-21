using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;

public class csWeapon_Lightsaber : csWeapon_Damage
{
	protected Transform m_WaitingPool = null;

	protected Collider m_CurrentHitCollider = null;

	protected ObjectPool<csLightsaber_Hit> m_HitEffectPool_Lightsaber = null;

	public csLightsaberTrail m_Trail = null;

	public Vector3 m_Point1 = Vector3.zero;
	public Vector3 m_Point2 = Vector3.zero;
	public Vector3 m_Center = Vector3.zero;

	public float m_fOnSize = 5.0f;
	public float m_fOnSize_Convert = 0.0f;
	public float m_fOffSize = 0.01f;
	public float m_fUpdateSizeSpeed = 50.0f;
	public float m_fUpdateSizeSpeed_Convert = 0.0f;
	public float m_fPercent = 0.0f;
	public float m_fDetectRadius = 0.5f;
	public float m_fWaitingTime = 0.15f;

	public bool m_bIsSaberOn = false;

	protected override void Awake()
	{
		base.Awake();

		m_fPercent = Utility.GetSomeValueOfTotalValue(m_Transform.lossyScale.x, m_Transform.localScale.x);
		m_fOnSize_Convert = Utility.GetPercentOfTotalValue(m_fOnSize, m_fPercent);
		m_fUpdateSizeSpeed_Convert = Utility.GetPercentOfTotalValue(m_fUpdateSizeSpeed, m_fPercent);
	}

	public override void Settings(csOwner _owner, int _actionIndex)
	{
		base.Settings(_owner, _actionIndex);

		m_iCollisionLayerMask = Layer.CollisionLayerMask(eCollisionLayerType.Monster);
		m_Collision.RegisterCollisionLayerMask(m_iCollisionLayerMask);

		if (m_HitEffectObject != null)
		{
			m_HitEffectPool = new ObjectPool();
			m_HitEffectPool.CreatePool(name, m_HitEffectObject, m_Owner.m_WaitingPool_Effect, m_iDefaultPoolCount);
		}
	}

	private void FixedUpdate()
	{
		//Debug.Log("Fixed");
		m_Transform.eulerAngles = new Vector3(m_Transform.eulerAngles.x, m_Transform.eulerAngles.y, 90.0f);
		m_Trail.Iterate(Time.time);
		m_Trail.UpdateTrail(Time.time, 0f);

		UpdateSize();

		if(Utility.IsActive(m_Collider))
		{
			HitCollider();
		}		
	}

	public virtual void Lightsaber_On()
	{
		Utility.Activate(m_Collider, true);

		Utility.Activate(gameObject, true);

		if (Mathf.Sign(m_fUpdateSizeSpeed_Convert) == -1.0f)
		{
			m_fUpdateSizeSpeed_Convert *= -1.0f;
		}
	}

	public virtual void Lightsaber_Off()
	{
		if (Mathf.Sign(m_fUpdateSizeSpeed_Convert) == 1.0f)
		{
			m_fUpdateSizeSpeed_Convert *= -1.0f;
		}
	}

	public virtual void Lightsaber_ForceOff()
	{
		//Debug.LogError("Lightsaber_ForceOff");
		Utility.Activate(gameObject, false);

		Vector3 LocalScale = m_Transform.localScale;
		m_Transform.localScale = new Vector3(LocalScale.x, m_fOffSize, LocalScale.z);
	}

	public override void StartAttack()
	{
		base.StartAttack();

		//m_fOnSize_Convert = Utility.GetPercentOfTotalValue(m_fOnSize, m_fPercent);		
	}

	public override void StopAttack()
	{
		Utility.Activate(m_Collider, false);

		Lightsaber_Off();

		base.StopAttack();		
	}

	public override void ClearIgnore()
	{
		base.ClearIgnore();

		m_CurrentHitCollider = null;
	}

	private void UpdateSize()
	{
		Vector3 LocalScale = m_Transform.localScale;
		float CurrentSize = LocalScale.y;

		m_fOnSize_Convert = Utility.GetPercentOfTotalValue(m_fOnSize, m_fPercent);

		CurrentSize += m_fUpdateSizeSpeed_Convert * Time.fixedDeltaTime;
		CurrentSize = Mathf.Clamp(CurrentSize, m_fOffSize, m_fOnSize_Convert);

		m_Transform.localScale = new Vector3(LocalScale.x, CurrentSize, LocalScale.z);

		m_bIsSaberOn = CurrentSize > m_fOffSize;
		m_Trail.height = Utility.GetPercentOfTotalValue(m_fOnSize, 91.0f);

		if (m_bIsSaberOn && !Utility.IsActive(gameObject))
		{
			Utility.Activate(gameObject, true);
		}
		else if (!m_bIsSaberOn && Utility.IsActive(gameObject))
		{
			Utility.Activate(gameObject, false);
		}
	}

	protected override void HitCollider(Collider _collider)
	{

	}

	private void HitCollider()
	{
		Vector3 Center = m_Transform.position + (m_Transform.up * (m_Transform.lossyScale.y - 0.3f) / 2);
		m_Center = Center;
		float DistansToPoints = m_Transform.lossyScale.y / 2 - m_fDetectRadius;
		m_Point1 = Center - m_Transform.up * (DistansToPoints + 1.0f);
		m_Point2 = Center + m_Transform.up * DistansToPoints;

		Collider[] Colls = Physics.OverlapCapsule(m_Point1, m_Point2, m_fDetectRadius, m_iCollisionLayerMask);

		if (Colls.Length > 0)
		{
			for (int i = 0; i < Colls.Length; i++)
			{
				if (!m_Collision.CheckIgnore(Colls[i]))
				{
					m_Collision.IgnoreCollider(Colls[i]);

					if (BattleManager.m_DicObjectColliderIndex.ContainsKey(Colls[i].GetInstanceID()))
					{
						//m_Owner.Bugs.Animator.speed = 0.0f;

						//StaticCoroutine.Start(CoSlowEffect(Colls[i]));

						csOwner Obj = BattleManager.m_DicObjectColliderIndex[Colls[i].GetInstanceID()];

						Obj.HitMe(m_Owner.OwnerType, m_Owner.OwnerIndex, m_Owner.m_Power.Power_Cur, m_Owner.Info_Bugs.m_BugsAction[m_iActionIndex]);
					}

					if (m_HitEffectPool != null)
					{
						int ObjectIndex = m_HitEffectPool.UseObject(Colls[i].bounds.center, false);
						m_HitEffectPool.m_ListObject[ObjectIndex].transform.rotation = Quaternion.LookRotation(Utility.RelativePosition(m_Transform.position, Colls[i].bounds.center));
					}
				}				
			}
		}
	}

	#region Coroutine

	private IEnumerator CoSlowEffect(Collider _collider)
	{
		yield return YieldCache.WaitForSeconds(m_fWaitingTime);

		m_Owner.Bugs.Animator.speed = 1.0f;

		csOwner Obj = BattleManager.m_DicObjectColliderIndex[_collider.GetInstanceID()];

		Obj.HitMe(m_Owner.OwnerType, m_Owner.OwnerIndex, m_Owner.m_Power.Power_Cur, m_Owner.Info_Bugs.m_BugsAction[m_iActionIndex]);
	}

	#endregion

	private void OnDrawGizmos()
	{
		if (BattleManager != null)
		{
			Gizmos.color = Color.red;

			Gizmos.DrawWireSphere(m_Point1, m_fDetectRadius);
			Gizmos.DrawWireSphere(m_Point2, m_fDetectRadius);
		}
	}
}
