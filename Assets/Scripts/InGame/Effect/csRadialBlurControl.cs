using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csRadialBlurControl : MonoBehaviour
{
	private csRadialBlur m_RadialBlur = null;

	public float m_fBlurStrength = 3.0f;

	private void Awake()
	{
		m_RadialBlur = csBattleManager.Instance.m_PlayerCamera.GetComponent<csRadialBlur>();
	}

	private void OnEnable()
	{
		if (m_RadialBlur != null)
		{
			m_RadialBlur.AddStrength(m_fBlurStrength);
		}		
	}
}
