using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csHealthBar : csHUD
{
	public Image m_HpProgressImage = null;

	public override void Settings(csOwner _owner)
	{
		base.Settings(_owner);

		RegisterEventHandler();
	}

	private void SetHealth()
	{
		//Debug.LogWarning(name + " q " + m_ObjectInfo);
		//Debug.LogWarning(name + " q " + m_ObjectInfo.Health);
		Debug.LogWarning(name + " q " + m_Owner.m_Health.Health_Max + "   " + m_Owner.m_Health.Health_Cur + "    " + Utility.GetSomeValueOfTotalValue(m_Owner.m_Health.Health_Max, m_Owner.m_Health.Health_Cur) * 0.01f);
		
		m_HpProgressImage.fillAmount = Utility.GetSomeValueOfTotalValue(m_Owner.m_Health.Health_Max, m_Owner.m_Health.Health_Cur) * 0.01f;
	}

	#region Command Method

	private void OnCommand_SetHealth_HUD()
	{
		SetHealth();
	}

	#endregion
}