using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class csHUD_Damage : csHUD {

	private IEnumerator m_CoUIDamageControl = null;
	public TextMeshProUGUI m_Text;
	public float m_DeactivateTime = 0.0f;

	public void PlayUIDamage(Transform _damageTr, float _damage)
	{
		m_Text.text = _damage.ToString();

		Utility.Activate(m_Text.gameObject, true);

		m_CoUIDamageControl = CoUIDamageControl(_damageTr);
		StartCoroutine(m_CoUIDamageControl);

		Invoke("Deactivate", m_DeactivateTime);
	}

	private void Deactivate()
	{
		if(m_CoUIDamageControl != null)
		{
			StopCoroutine(m_CoUIDamageControl);
		}
	
		Utility.Activate(m_Text.gameObject, false);
		Utility.Activate(gameObject, false);
	}

	private IEnumerator CoUIDamageControl(Transform _damageTr)
	{
		Vector3 WorldScreen = Vector3.zero;
		Vector3 WorldPoint = Vector3.zero;

		while(true)
		{
			yield return null;

			WorldScreen = m_PlayerCamera.Camera.WorldToScreenPoint(_damageTr.position + new Vector3(1.0f, 1.0f, 0.0f));
			WorldPoint = m_UICamera.ScreenToWorldPoint(WorldScreen);
			WorldPoint.z = 0.0f;

			m_Transform.position = WorldPoint;
		}
	}
}
