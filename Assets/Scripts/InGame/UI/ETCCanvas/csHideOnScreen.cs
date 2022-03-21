using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csHideOnScreen : MonoBehaviour {

	private CountNumber m_CountNumber = null;

	public Image m_HideOnImage = null;

	private void Awake()
	{
		m_CountNumber = new CountNumber();
	}

	public void HideOnScreen(float _targetAlpha, float _hideOnTime)
	{
		Color TargetColor = new Color
		{
			a = _targetAlpha
		};

		Color ColorToApplies = new Color();
		ColorToApplies = m_HideOnImage.color;

		m_CountNumber.Count(m_HideOnImage.color.a, TargetColor.a, _hideOnTime, (value) =>
		{
			ColorToApplies.a = value;
			m_HideOnImage.color = ColorToApplies;
		});
	}
}
