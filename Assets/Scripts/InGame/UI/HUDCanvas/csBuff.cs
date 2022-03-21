using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class csBuff : MonoBehaviour {

	public TextMeshProUGUI m_BuffCountText = null;

	public int m_iCurrentCount = 0;

	private void Awake()
	{
		m_BuffCountText.text = 0.ToString();
	}

	public void Initialize()
    {
		m_iCurrentCount = 0;

		m_BuffCountText.text = m_iCurrentCount.ToString();
	}

	public void AddBuffCount()
	{
		m_iCurrentCount++;

		m_BuffCountText.text = m_iCurrentCount.ToString();
	}
}
