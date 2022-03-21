using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csCameraShakeControl : MonoBehaviour
{
	private Transform m_Transform = null;

	public AnimationCurve m_ShakeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

	private csCameraShake m_CameraShake = null;	

	public float m_fDuration = 0.3f;
	public float m_fSpeed = 50.0f;
	public float m_fMagnitude = 5.0f;
	public float m_fDistanceForce = 100.0f;
	public float m_fRotationDamper = 2.0f;

	private void Awake()
	{
		m_Transform = GetComponent<Transform>();

		m_CameraShake = csBattleManager.Instance.m_PlayerCamera.GetComponent<csCameraShake>();		
	}

	private void OnEnable()
	{
		if (m_CameraShake != null)
		{
			m_CameraShake.Shake(m_ShakeCurve, m_Transform.position, m_fDuration, m_fSpeed, m_fMagnitude, m_fDistanceForce, m_fRotationDamper);
		}	
	}
}
