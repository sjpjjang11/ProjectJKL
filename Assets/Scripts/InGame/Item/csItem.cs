using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;

public class csItem : MonoBehaviour {

	private IEnumerator m_CoTrackTarget;
	private IEnumerator m_CoNearbyObjectsCheck;

	protected Transform m_Transform = null;
	protected Rigidbody m_Rigidbody = null;
	protected BoxCollider m_BoxCollider = null;

	public GameObject m_EatEffectObject = null;
	protected ObjectPool m_EatEffectPool = null;

	protected csBattleManager m_BattleManager = null;

	private csOwner_Hero m_Target;

	protected csCollision m_Collision = null;

	protected int m_iCollisionLayerMask = 0;

	public int Type
	{
		get;
		protected set;
	}
	public int Value
	{
		get;
		protected set;
	}

	protected virtual void Awake()
	{		
		m_Transform = transform;
		m_Rigidbody = GetComponent<Rigidbody>();
		m_BoxCollider = GetComponent<BoxCollider>();
		m_BattleManager = csBattleManager.Instance;
		m_Collision = GetComponent<csCollision>();

		//m_iCollisionLayerMask = Layer.CollisionLayerMask(csBattleManager.CollisionLayer.Hero);
		m_iCollisionLayerMask = ((1 << LayerMask.NameToLayer(eCollisionLayerType.Floor.ToString())));
		m_Collision.RegisterCollisionLayerMask(m_iCollisionLayerMask);
		m_Collision.RegisterOnCollisionCallback(CheckGround);
		//m_Collision.RegisterOnCollisionCallback(HitCollider);

		Utility.Activate(m_EatEffectObject, false);
	}

	public virtual void Spawn()
	{
		//Rigidbody.AddForce(Vector3.up * 5.0f, ForceMode.Impulse);

		m_BoxCollider.isTrigger = false;
		m_Rigidbody.useGravity = true;		

		//StartCoroutine(CoCheckGround());

		m_EatEffectObject.transform.parent = csBattleManager.m_WaitingPool_CommonEffect;
		m_EatEffectObject.transform.position = Vector3.zero;
		m_EatEffectObject.GetComponent<csSelfDeactivate>().SetParent(csBattleManager.m_WaitingPool_CommonEffect);

		//m_CoNearbyObjectsCheck = CoNearbyObjectsCheck();
		//StartCoroutine(m_CoNearbyObjectsCheck);
	}

	protected virtual void CheckGround(Collider _collider)
	{
		m_Rigidbody.useGravity = false;

		/*Physics.IgnoreLayerCollision(LayerMask.NameToLayer(csBattleManager.CollisionLayer.Item.ToString()),
							 LayerMask.NameToLayer(csBattleManager.CollisionLayer.Map.ToString()), true);*/
		Physics.IgnoreCollision(m_BoxCollider, _collider, true);

		Invoke("Activate", 1.0f);
	}

	protected virtual void Activate()
	{
		m_iCollisionLayerMask = (1 << LayerMask.NameToLayer(eCollisionLayerType.Hero.ToString())
						| (1 << LayerMask.NameToLayer(eCollisionLayerType.Monster.ToString())));
		m_Collision.RegisterCollisionLayerMask(m_iCollisionLayerMask);
		m_Collision.RegisterOnTriggerCallback(HitCollider);
		m_Collision.UnregisterOnCollisionCallback();

		m_BoxCollider.isTrigger = true;
	}

	protected virtual void HitCollider(Collider _collider)
	{
		Debug.LogError("HitCollider : " + _collider.name);
		if(_collider.GetComponent<csOwner_Hero>() == null)
		{
			m_Collision.IgnoreCollider(_collider);

			return;
		}

		EatItem(_collider.GetComponent<csOwner_Hero>());
	}

	protected virtual void EatItem(csOwner_Hero _target)
	{
		_target.GetComponent<csOwner_Hero>().EatItem(Type, Value);
		m_EatEffectObject.transform.parent = _target.transform;
		m_EatEffectObject.transform.localPosition = Vector3.zero;
		Utility.Activate(m_EatEffectObject, true);
		Utility.Activate(gameObject, false);
	}

	public void Deactivate()
	{
		Utility.Activate(gameObject, false);
	}

	private IEnumerator CoNearbyObjectsCheck()
	{
		float leftTime = 3.0f;

		while (true)
		{
			yield return null;
			Collider[] Cols = Physics.OverlapSphere(m_Transform.position, 1.5f);

			for (int i = 0; i < Cols.Length; i++)
			{
				if (m_BattleManager.m_DicObjectColliderIndex.ContainsKey(Cols[i].GetInstanceID()))
				{
					//m_Target = Cols[i].transform;

					m_Target = (csOwner_Hero)m_BattleManager.m_DicObjectColliderIndex[Cols[i].GetInstanceID()];

					m_CoTrackTarget = CoTrackTarget();

					StartCoroutine(m_CoTrackTarget);

					StopCoroutine(m_CoNearbyObjectsCheck);
				}
			}

			if(leftTime <= 0.0f)
			{
				break;
			}

			leftTime -= Time.deltaTime;
		}

		while (true)
		{
			yield return null;
			Collider[] Cols = Physics.OverlapSphere(m_Transform.position, 5.0f);

			for(int i = 0; i < Cols.Length; i++)
			{
				if (m_BattleManager.m_DicObjectColliderIndex.ContainsKey(Cols[i].GetInstanceID()))
				{
					m_Target = (csOwner_Hero)m_BattleManager.m_DicObjectColliderIndex[Cols[i].GetInstanceID()];

					m_CoTrackTarget = CoTrackTarget();

					StartCoroutine(m_CoTrackTarget);

					StopCoroutine(m_CoNearbyObjectsCheck);
				}
			}
		}
	}

	private IEnumerator CoTrackTarget()
	{		
		float move = 0.0f;
		m_Rigidbody.useGravity = false;
		m_BoxCollider.isTrigger = true;

		while(true)
		{
			yield return null;

			m_Transform.position = Vector3.MoveTowards(m_Transform.position, m_Target.Transform.position, Mathf.Clamp(move, 0.0f, 1.0f));

			float dis = Vector3.Distance(m_Transform.position, m_Target.Transform.position);

			if(dis <= 1.0f)
			{
				m_Target.EatItem(Type, Value);
				Utility.Activate(gameObject, false);

				break;
			}

			move += Time.deltaTime * 1.0f;
		}
	}

	private IEnumerator CoCheckGround()
	{
		Vector3 PrevPosition = Vector3.zero;

		float PositionDistance = 0.0f;

		while (true)
		{
			yield return null;

			PositionDistance = Vector3.Distance(PrevPosition, m_Transform.position);

			if (PositionDistance <= 0.0f)
			{			
				m_BoxCollider.isTrigger = true;
				m_Rigidbody.useGravity = false;
				Debug.LogError("CoCheckGround");
				break;
			}

			PrevPosition = m_Transform.position;
		}
	}
}
