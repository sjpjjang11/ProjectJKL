using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csProgress : MonoBehaviour {

	private IEnumerator m_CoFillProgress = null;

	private Action m_CompelteCallback = null;

	public Image m_Progress = null;

	public Color m_FillColor;
	public Color m_EmptyColor;

	public float m_fFillColorValue = 0.0f;

	public void Settings(float _fillColorValue)
	{		
		m_fFillColorValue = _fillColorValue;
	}

	public void SetFillAmount(float _value)
	{
		m_Progress.fillAmount = _value;
		//Debug.Log(m_Progress.fillAmount);
		if(m_Progress.fillAmount >= m_fFillColorValue)
		{
			m_Progress.color = m_FillColor;
		}
		else
		{
			m_Progress.color = m_EmptyColor;
		}
	}

	public void EmptyProgress()
	{
		m_Progress.fillAmount = 0.0f;
	}

	public bool CheckProgress()
	{
		bool Result = false;

		if(m_Progress.fillAmount == 1.0f)
		{
			Result = true;
		}

		return Result;
	}
}
