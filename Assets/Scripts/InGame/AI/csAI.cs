using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAI : MonoBehaviour {
	
	protected csOwner_Monster m_Owner = null;
	protected csOwner_Hero Hero
	{
		get
		{
			return csBattleManager.Instance.m_Hero;
		}
	}

	protected List<csWeapon> m_ListHeroActivatedWeapon;

	protected csGrid m_Grid = null;
	protected csAIAgent m_Agent = null;

	protected virtual void Awake()
	{
		//m_Owner = GetComponent<csOwner_Monster>();
		//m_Grid = GetComponent<csGrid>();
		m_Grid = csPathRequestManager.Instance.GetComponent<csGrid>();
	}

	protected virtual void Start()
	{

	}

	public virtual void Settings(csOwner_Monster _owner)
	{
		m_Owner = _owner;
		m_Agent = _owner.Monster.GetComponent<csAIAgent>();

		m_ListHeroActivatedWeapon = Hero.m_EventHandler_Hero.ActivatedWeapon.Get();
	}

	protected virtual Vector3 GetRelativePosition(Vector3 _from, Vector3 _to)
	{
		return _to - _from;
	}

	protected virtual float GetDistanceFromMeToHero()
	{
		return Utility.Distance(m_Owner.Transform.position, Hero.Transform.position);
	}

	protected virtual float GetDistance(Vector3 _from, Vector3 _to)
	{
		return Utility.Distance(_from, _to);
	}

	protected virtual Vector3 GetDirection(Vector3 _from, Vector3 _to)
	{
		return GetRelativePosition(_from, _to).normalized;
	}

	protected virtual bool IsWithinRangeHero(float _range)
	{
		return GetDistanceFromMeToHero() < _range;
	}

	protected virtual csWeapon IsWithinRangeAttack(float _range)
	{
		csWeapon Weapon = null;

		if(m_ListHeroActivatedWeapon.Count == 0)
		{
			return Weapon;
		}

		m_ListHeroActivatedWeapon.Sort(delegate (csWeapon _a, csWeapon _b)
		{
			float DistanceA = Utility.Distance(_a.m_Transform.position, m_Owner.Transform.position);
			float DistanceB = Utility.Distance(_b.m_Transform.position, m_Owner.Transform.position);

			return DistanceA.CompareTo(DistanceB);
		});

		for (int i = 0; i < m_ListHeroActivatedWeapon.Count; i++)
		{
			float DistanceFromMeToWeapon = Utility.Distance(m_Owner.Transform.position, m_ListHeroActivatedWeapon[i].m_Transform.position);

			if(DistanceFromMeToWeapon < _range)
			{
				return m_ListHeroActivatedWeapon[i];
			}
		}

		return Weapon;
	}
}
