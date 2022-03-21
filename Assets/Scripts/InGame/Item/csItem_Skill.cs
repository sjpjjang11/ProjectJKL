using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csItem_Skill : csItem
{
	private IEnumerator m_CoCancelTime = null;

	private CountNumber m_CountNumber = null;

	public GameObject m_ItemZone = null;

	public float m_fEatTime = 1.0f;
	public float m_fCurrentEatTime = 0.0f;
	public float m_fCancelTime = 0.2f;
	public float m_fCurrentCancelTime = 0.0f;
	public float m_fMaxScale = 2.0f;
	public float m_fMinScale = 0.0f;
	public float m_fCurrentScale = 0.0f;
	public float m_fScaleRestoreTime = 0.1f;

	protected override void Awake()
	{
		base.Awake();

		m_CountNumber = new CountNumber();

		Type = csItemManager.SkillType;
		Value = Random.Range(1, 3);
	}

	public override void Spawn()
	{
		base.Spawn();

		m_ItemZone.transform.localScale = new Vector3(m_fMaxScale, m_fMaxScale, 1.0f);
	}

	protected override void HitCollider(Collider _collider)
	{
		if (_collider.tag.CompareTo("Projectile") == 0)
		{
			return;
		}

		if(m_CoCancelTime == null)
		{
			m_CoCancelTime = CoCancelTime();
			StartCoroutine(m_CoCancelTime);

			m_fCurrentScale = m_fMaxScale;

			m_CountNumber.Count(gameObject.GetInstanceID(), m_fCurrentScale, m_fMinScale, m_fEatTime, (value) =>
			{
				m_fCurrentScale = value;
				m_ItemZone.transform.localScale = new Vector3(value, value, 1.0f);
				//Debug.Log("EatItem : " + value);
			});
		}
		
		if (m_fCurrentEatTime >= m_fEatTime)
		{
			EatItem(_collider.GetComponent<csOwner_Hero>());
		}

		m_fCurrentEatTime += Time.deltaTime;
		m_fCurrentCancelTime = m_fCancelTime;
	}

	protected override void EatItem(csOwner_Hero _target)
	{
		base.EatItem(_target);

		m_CountNumber.ForceStop(gameObject.GetInstanceID());

		StopCoroutine(m_CoCancelTime);
		m_CoCancelTime = null;
	}

	private IEnumerator CoCancelTime()
	{
		m_fCurrentCancelTime = m_fCancelTime;

		while(true)
		{
			yield return null;

			if(m_fCurrentCancelTime <= 0.0f)
			{
				m_fCurrentEatTime = 0.0f;
				m_fCurrentCancelTime = 0.0f;

				m_CountNumber.ForceStop(gameObject.GetInstanceID());

				m_CountNumber.Count(gameObject.GetInstanceID(), m_fCurrentScale, m_fMaxScale, m_fScaleRestoreTime, (value) =>
				{
					m_ItemZone.transform.localScale = new Vector3(value, value, 1.0f);
					Debug.Log("CancelTime : " + value);
				});

				m_CoCancelTime = null;

				break;
			}

			m_fCurrentCancelTime -= Time.deltaTime;
		}
	}
}
