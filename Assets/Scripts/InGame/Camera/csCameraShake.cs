using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csCameraShake : MonoBehaviour
{
	private Transform m_Transform;

	public AnimationCurve m_ShakeCurve = null;

	private csPlayerCamera m_PlayerCamera = null;

	private CountNumber m_CountNumber = null;

	[SerializeField]
	private Vector3 m_InitRotation = Vector3.zero;
	private Vector3 m_OriginalCamRotation = Vector3.zero;
	private Vector3 m_Direction = Vector3.zero;
	private Vector3 m_OldRotation = Vector3.zero;

	private float m_fElapsed = 0.0f;
	private float m_fDuration = 0.0f;
	private float m_fTime = 0.0f;
	private float m_fSpeed = 0.0f;
	private float m_fMagnitude = 0.0f;
	private float m_fRandomStart = 0.0f;
	private float m_fDistanceDamper = 0.0f;
	private float m_RotationDamper = 0.0f;

	public float m_fTestCurrentTimeScale = 0.0f;

	private void Awake()
	{
		m_Transform = GetComponent<Transform>();
		m_PlayerCamera = GetComponent<csPlayerCamera>();
		m_CountNumber = new CountNumber();
	}

	private void Start()
	{
		m_InitRotation = m_Transform.eulerAngles;

		if(Utility.IsActive(this))
		{
			Utility.Activate(this, false);
		}
	}

	private void Update()
	{
		m_fTestCurrentTimeScale = Time.timeScale;

		if (m_fElapsed > m_fDuration)
		{
			Utility.Activate(this, false);

			m_PlayerCamera.m_bIsShake = false;

			return;
		}

		m_PlayerCamera.FollowTarget();

		m_fElapsed += Time.deltaTime;
		float percentComplete = m_fElapsed / m_fDuration;
		float damper = m_ShakeCurve.Evaluate(percentComplete) * m_fDistanceDamper;
		m_fTime += Time.deltaTime * damper;
		m_Transform.position -= m_Direction * Time.deltaTime * Mathf.Sin(m_fTime * m_fSpeed) * damper * m_fMagnitude / 2;

		float alpha = m_fRandomStart + m_fSpeed * percentComplete / 10;
		float x = Mathf.PerlinNoise(alpha, 0.0f) * 2.0f - 1.0f;
		float y = Mathf.PerlinNoise(1000 + alpha, alpha + 1000) * 2.0f - 1.0f;
		float z = Mathf.PerlinNoise(0.0f, alpha) * 2.0f - 1.0f;

		if (Quaternion.Euler(m_OriginalCamRotation + m_OldRotation) != m_Transform.rotation)
		{
			m_OriginalCamRotation = m_Transform.rotation.eulerAngles;
		}
			
		m_OldRotation = Mathf.Sin(m_fTime * m_fSpeed) * damper * m_fMagnitude * new Vector3(0.5f + y, 0.3f + x, 0.3f + z) * m_RotationDamper;
		m_Transform.rotation = Quaternion.Euler(m_OriginalCamRotation + m_OldRotation);
	}

	public float m_fSlowScale = 0.1f;
	public float m_fWatingTime = 0.25f;
	public float m_fRestoreTime = 0.5f;
	public void Shake(AnimationCurve _shakeCurve, Vector3 _from, float _duration, float _speed, float _magnitude, float _distanceForce, float _rotationDemper)
	{
		//Time.timeScale = m_fSlowScale;

		//Invoke("Slow", m_fWatingTime);

		m_PlayerCamera.FollowTarget();

		m_ShakeCurve = _shakeCurve;
		m_OriginalCamRotation = m_Transform.eulerAngles;
		//m_OriginalCamRotation = m_InitRotation;
		m_Transform.eulerAngles = m_InitRotation;
		m_Direction = (_from - m_Transform.position).normalized;
		m_OldRotation = Vector3.zero;
		m_fElapsed = 0.0f;
		m_fDuration = _duration;
		m_fTime = 0.0f;
		m_fSpeed = _speed;
		m_fMagnitude = _magnitude;
		m_RotationDamper = _rotationDemper;
		m_fRandomStart = Random.Range(-1000.0f, 1000.0f);
		m_fDistanceDamper = 1 - Mathf.Clamp01((m_Transform.position - transform.position).magnitude / _distanceForce);
	
		m_PlayerCamera.m_bIsShake = true;

		Utility.Activate(this, true);
	}

	private void Slow()
	{
		m_CountNumber.Count(Time.timeScale, 1.0f, m_fRestoreTime, (value) =>
		{
			Time.timeScale = value;
		}, null);
	}
}
