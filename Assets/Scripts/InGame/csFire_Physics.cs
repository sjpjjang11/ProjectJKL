using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProjectK;

[RequireComponent(typeof(Rigidbody))]
public class csFire_Physics : MonoBehaviour {

	private IEnumerator m_CoMaxHeightCheck = null;
	private IEnumerator m_CoLandingCheck = null;
	private IEnumerator m_CoLookAtEscape = null;
	private Action m_FireLandingCallback = null;

	protected Transform m_Transform = null;
	public Transform Transform
	{
		get
		{
			if (m_Transform == null)
			{
				m_Transform = transform;
			}

			return m_Transform;
		}
	}

	private Rigidbody m_Rigidbody = null;

	public GameObject m_FireObjectEffect = null;
	public GameObject m_FireMuzzleEffect = null;
	public GameObject m_FireExplosionEffect = null;

	private Vector3 m_PrevPosition = Vector3.zero;
	private Vector3 m_PrevRotation = Vector3.zero;

	private csCollision m_Collision = null;
	private CountNumber m_CountNumber = new CountNumber();

	private int m_iCollisionLayerMask = 0;

	public float m_fFireSpeed = 10.0f;
	public float m_fRight = 0.0f;
	public float m_fUp = 1.0f;

	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Rigidbody.isKinematic = true;
		m_Rigidbody.useGravity = true;

		m_Collision = GetComponent<csCollision>();
		m_iCollisionLayerMask = csBattleManager.Instance.m_iCollisionLayerMask_Hero;

		m_FireMuzzleEffect.transform.SetParent(csBattleManager.m_WaitingPool_CommonEffect);
		m_FireMuzzleEffect.transform.position = Vector3.zero;
		m_FireMuzzleEffect.transform.eulerAngles = Vector3.zero;

		Utility.Activate(m_FireObjectEffect, false);
		Utility.Activate(m_FireMuzzleEffect, false);
		Utility.Activate(m_FireExplosionEffect, false);
	}

	public void Settings()
	{
		m_Collision.RegisterCollisionLayerMask(m_iCollisionLayerMask);
		m_Collision.RegisterOnCollisionCallback(HitCollider);
	}

	private void Fire(Vector3 _escapeVelocity)
	{
		m_Rigidbody.isKinematic = false;
		m_Rigidbody.velocity = _escapeVelocity;

		m_FireMuzzleEffect.transform.position = Transform.position;
		m_FireMuzzleEffect.transform.rotation = Quaternion.LookRotation(_escapeVelocity);

		Utility.Activate(m_FireObjectEffect, true);
		Utility.Activate(m_FireMuzzleEffect, true);
	}

	public void FireToTarget(Vector3 _escapeVelocity, Action _callback)
	{
		m_FireLandingCallback = _callback;

		Fire(_escapeVelocity);

		m_CoLookAtEscape = CoLookAtEscape(_escapeVelocity);
		StartCoroutine(m_CoLookAtEscape);

		m_CoLandingCheck = CoLandingCheck();
		StartCoroutine(m_CoLandingCheck);
	}

	public void FireUp(Vector3 _escapeVelocity, Action _callback)
	{
		m_FireLandingCallback = _callback;

		Fire(_escapeVelocity);

		m_CoMaxHeightCheck = CoMaxHeightCheck(_escapeVelocity);
		StartCoroutine(m_CoMaxHeightCheck);
	}

	public void Fire(Action _callback)
	{
		m_FireLandingCallback = _callback;
		Vector3 Direction = -Transform.forward + (Transform.up * m_fUp) + (Transform.right * m_fRight);

		Vector3 Force = Direction * m_fFireSpeed;

		m_Rigidbody.isKinematic = false;
		m_Rigidbody.AddForce(Force, ForceMode.Impulse);

		m_FireMuzzleEffect.transform.position = Transform.position;
		m_FireMuzzleEffect.transform.rotation = Quaternion.LookRotation(Direction);
		Utility.Activate(m_FireObjectEffect, true);
		Utility.Activate(m_FireMuzzleEffect, true);

		m_CoLandingCheck = CoLandingCheck();
		StartCoroutine(m_CoLandingCheck);
	}

	/*public void Fire()
	{
		Vector3 Direction = -Transform.forward + (Transform.up * m_fUp) + (Transform.right * m_fRight);

		Vector3 Force = Direction * m_fFireSpeed;

		m_Rigidbody.isKinematic = false;
		m_Rigidbody.AddForce(Force, ForceMode.Impulse);

		m_FireMuzzleEffect.transform.position = Transform.position;
		m_FireMuzzleEffect.transform.rotation = Quaternion.LookRotation(Direction);
		Utility.Activate(m_FireObjectEffect, true);
		Utility.Activate(m_FireMuzzleEffect, true);

		m_CoLandingCheck = CoLandingCheck();
		StartCoroutine(m_CoLandingCheck);
	}*/

	private void HitCollider(Collider _collider)
	{
		Debug.LogError("HitCollider");
		if(m_FireExplosionEffect != null)
		{
			Utility.Activate(m_FireExplosionEffect, true);
		}

		if (m_CoLookAtEscape != null)
		{
			StopCoroutine(m_CoLookAtEscape);
			m_CoLookAtEscape = null;
		}

		//m_Collision.UnregisterOnCollisionCallback();
	}

	private float GetPositionDistance()
	{
		float Value = Vector3.Distance(m_PrevPosition, Transform.position);

		m_PrevPosition = Transform.position;

		return Value;
	}

	private float GetRotationDistance()
	{
		float Value = Vector3.Distance(m_PrevRotation, Transform.eulerAngles);

		m_PrevRotation = Transform.eulerAngles;

		return Value;
	}

	private void InitRotation()
	{
		m_Rigidbody.isKinematic = true;

		Vector3 Euler = Transform.eulerAngles;

		m_CountNumber.Count(Euler, Vector3.zero, 0.1f, (value) =>
		{
			//Debug.Log("@@@@@@@@@@@@@ : " + name + "  " + Transform.eulerAngles + "    " + value);
			Transform.eulerAngles = value;
		},
		() =>
		{
			if (m_FireLandingCallback != null)
			{
				m_FireLandingCallback();

				m_FireLandingCallback = null;
			}
		});
	}

	#region Coroutine
	public float m_fHeight = 10.0f;
	public float m_fPercent = 80.0f;
	public float m_fY = 0.0f;
	protected IEnumerator CoMaxHeightCheck(Vector3 _velocity)
	{
		float Y = 0.0f;

		while (true)
		{
			Y = Utility.GetPercentOfTotalValue(_velocity.y, m_fPercent);
			if(Transform.position.y >= Y)
			{
				m_fY = Y;

				m_Rigidbody.isKinematic = true;

				if (m_FireLandingCallback != null)
				{
					m_FireLandingCallback();

					m_FireLandingCallback = null;
				}
			}

			yield return YieldCache.WaitForFixedUpdate;
		}
	}

	protected IEnumerator CoLookAtEscape(Vector3 _velocity)
	{
		Vector3 Position = Vector3.zero;

		while(true)
		{
			yield return YieldCache.WaitForFixedUpdate;

			_velocity.y += Physics.gravity.y * 0.02f;
			Position += _velocity * 0.02f;

			Transform.LookAt(Position + Transform.position);
		}
	}

	protected IEnumerator CoLandingCheck()
	{
		float PositionDistance = 0.0f;
		float RotationDistance = 0.0f;

		m_PrevPosition = Vector3.zero;
		m_PrevRotation = Vector3.zero;

		while (true)
		{
			yield return YieldCache.WaitForSeconds(0.1f);

			PositionDistance = GetPositionDistance();
			RotationDistance = GetRotationDistance();

			if (PositionDistance <= 0.0f && RotationDistance <= 0.0f)
			{
				Utility.Activate(m_FireMuzzleEffect, false);
				Utility.Activate(m_FireExplosionEffect, false);
				InitRotation();

				break;
			}

			Debug.Log("POSITION : " + PositionDistance);
			Debug.Log("ROTATION : " + RotationDistance);							
		}
	}

	#endregion
}
