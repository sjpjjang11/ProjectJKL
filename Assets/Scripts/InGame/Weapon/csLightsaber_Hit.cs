using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csLightsaber_Hit : MonoBehaviour
{
	[HideInInspector]
	public Transform m_Transform = null;

	public ParticleSystem m_Particle = null;

	public float m_fDeactivateTime = 2.0f;

	private void Awake()
	{
		m_Transform = transform;
	}

	public void ParticlePlay()
	{
		m_Particle.Play();
	}

	public void ParticleStop()
	{
		//m_Particle.Stop();

		Invoke("EffectDeactivate", m_fDeactivateTime);
	}

	public void SetParticleForce(Vector3 _force)
	{
		ParticleSystem.ForceOverLifetimeModule ParticleForceOverLifetime = m_Particle.forceOverLifetime;
		ParticleForceOverLifetime.xMultiplier = _force.x;
		ParticleForceOverLifetime.yMultiplier = _force.y;
		ParticleForceOverLifetime.zMultiplier = _force.z;
	}

	private void EffectDeactivate()
	{
		Utility.Activate(gameObject, false);
	}
}
