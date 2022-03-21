using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csHitEffect : MonoBehaviour
{
	protected Transform m_Waiting_DamagePopup = null;               // 데미지 UI 생성시 부모가 되는 오브젝트
	protected Transform m_EffectParentTr = null;                    // 공격 관련 이펙트 생성시 부모가 되는 오브젝트

	protected GameObject m_NormalHitCartoon = null;                 // 기본 공격 타격 카툰 이펙트 오브젝트 
	protected GameObject m_CriticalHitCartoon = null;               // 크리티컬 공격 타격 카툰 이펙트 오브젝트
	protected GameObject m_NormalUIDamage = null;                   // 기본 피격 데미지 UI 오브젝트
	protected GameObject m_CriticalUIDamage = null;                 // 크리티컬 피격 데미지 UI 오브젝트

	private csEventHandler_Bugs m_EventHandler = null;
	private csOwner m_Owner = null;

	public ObjectPool m_NormalHitCartoonPool;                           // 기본 공격 타격 카툰 이펙트 풀
	public ObjectPool m_CriticalHitCartoonPool;                     // 크리티컬 공격 타격 카툰 이펙트 풀
	public ObjectPool<csHUD_Damage> m_NormalUIDamagePool;          // 기본 피격 데미지 UI 풀
	public ObjectPool<csHUD_Damage> m_CriticalUIDamagePool;        // 크리티컬 피격 데미지 UI 풀

	private int m_iDefaultPoolCount = 10;                         // 풀 생성시 기본적인 개수

	public void Settings(csOwner _owner)
	{
		m_Owner = _owner;

		CreateUIDamage();
		CreateWeaponEffect();
	}

	public void Release()
	{
		DestroyUIDamage();
		DestroyWeaponEffect();
	}

	private void CreateUIDamage()
	{
		m_Waiting_DamagePopup = csBattleUIManager.Instance.BattleHUD.m_DamagePopupPanel;

		m_NormalUIDamage = Resources.Load("Prefabs/UI/NormalDamage") as GameObject;

		if (m_NormalUIDamage != null)
		{
			// 기본 피격 데미지 UI 풀 인스턴스화
			m_NormalUIDamagePool = new ObjectPool<csHUD_Damage>();

			// 기본 피격 데미지 UI 풀 생성
			m_NormalUIDamagePool.CreatePool(name, m_NormalUIDamage, m_Waiting_DamagePopup, m_iDefaultPoolCount);
		}

		for (int i = 0; i < m_NormalUIDamagePool.m_ListComponent.Count; i++)
		{
			m_NormalUIDamagePool.m_ListComponent[i].Settings(m_Owner);
		}

		m_CriticalUIDamage = Resources.Load("Prefabs/UI/CriticalDamage") as GameObject;

		if (m_CriticalUIDamage != null)
		{
			// 크리티컬 피격 데미지 UI 풀 인스턴스화
			m_CriticalUIDamagePool = new ObjectPool<csHUD_Damage>();

			// 크리티컬 데미지 UI 풀 생성
			m_CriticalUIDamagePool.CreatePool(name, m_CriticalUIDamage, m_Waiting_DamagePopup, m_iDefaultPoolCount);
		}

		for (int i = 0; i < m_CriticalUIDamagePool.m_ListComponent.Count; i++)
		{
			m_CriticalUIDamagePool.m_ListComponent[i].Settings(m_Owner);
		}
	}

	private void CreateWeaponEffect()
	{
		m_EffectParentTr = csBattleManager.m_WaitingPool_CommonEffect;

		m_NormalHitCartoon = Resources.Load("Prefabs/HeroRelated/Effect_Common/NormalHitCartoon") as GameObject;

		m_CriticalHitCartoon = Resources.Load("Prefabs/HeroRelated/Effect_Common/CriticalHitCartoon") as GameObject;

		// 기본 공격 타격 이펙트 오브젝트가 존재한다면
		if (m_NormalHitCartoon != null)
		{
			// 기본 공격 타격 이펙트 풀 인스턴스화
			m_NormalHitCartoonPool = new ObjectPool();

			// 기본 공격 타격 이펙트 풀 생성
			m_NormalHitCartoonPool.CreatePool(name, m_NormalHitCartoon, m_EffectParentTr, m_iDefaultPoolCount);
		}

		if (m_CriticalHitCartoon != null)
		{
			m_CriticalHitCartoonPool = new ObjectPool();

			m_CriticalHitCartoonPool.CreatePool(name, m_CriticalHitCartoon, m_EffectParentTr, m_iDefaultPoolCount);
		}
	}

	private void DestroyUIDamage()
	{
		m_Waiting_DamagePopup = null;

		if (m_NormalUIDamagePool != null)
		{
			m_NormalUIDamagePool.AllClearPool();
			m_NormalUIDamagePool = null;
		}

		if (m_NormalUIDamage != null)
		{
			m_NormalUIDamage = null;
		}

		if (m_CriticalUIDamagePool != null)
		{
			m_CriticalUIDamagePool.AllClearPool();
			m_CriticalUIDamagePool = null;
		}

		if (m_CriticalUIDamage != null)
		{
			m_CriticalUIDamage = null;
		}
	}

	private void DestroyWeaponEffect()
	{
		m_EffectParentTr = null;

		if (m_NormalHitCartoonPool != null)
		{
			m_NormalHitCartoonPool.AllClearPool();
			m_NormalHitCartoonPool = null;
		}

		if (m_NormalHitCartoon != null)
		{
			m_NormalHitCartoon = null;
		}

		if (m_CriticalHitCartoonPool != null)
		{
			m_CriticalHitCartoonPool.AllClearPool();
			m_CriticalHitCartoonPool = null;
		}

		if (m_CriticalHitCartoon != null)
		{
			m_CriticalHitCartoon = null;
		}
	}

	public void PlayEffect(float _damage)
	{
		Vector3 RandomPos = transform.position + UnityEngine.Random.insideUnitSphere * 0.8f;

		RandomPos = new Vector3(RandomPos.x, m_Owner.Bugs.m_UIDamageTr.position.y, transform.position.z);

		if (UnityEngine.Random.Range(0, 4) == 0)
		{
			//PlayEffect(m_CriticalUIDamagePool, _damage);
			m_CriticalHitCartoonPool.UseObject(RandomPos, false);
			//m_LocalEventHandler.ShakeCamera.Send(1.0f, 0.1f);
		}
		else
		{
			//PlayEffect(m_NormalUIDamagePool, _damage);
			m_NormalHitCartoonPool.UseObject(RandomPos, false);
		}
	}

	private void PlayEffect(ObjectPool<csHUD_Damage> _damagePool, float _damage)
	{
		int ObjectIndex;

		ObjectIndex = _damagePool.UseObject(Vector3.zero, false);

		if(ObjectIndex == m_iDefaultPoolCount)
		{
			_damagePool.m_ListComponent[ObjectIndex].Settings(m_Owner);
			_damagePool.m_ListComponent[ObjectIndex].PlayUIDamage(m_Owner.Bugs.m_UIDamageTr, _damage);
			m_iDefaultPoolCount++;
		}
	}
}

