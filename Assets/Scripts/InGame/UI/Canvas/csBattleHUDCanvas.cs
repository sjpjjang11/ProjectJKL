using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csBattleHUDCanvas : MonoBehaviour {

	public Transform m_DamagePopupPanel = null;
	public Transform m_HUD_HeroPanel = null;
	public Transform m_HUD_MonsterPanel = null;
	public Transform m_IndicatorPanel = null;
	public Transform m_TargetPanel = null;

	public GameObject m_NormalCrossHair = null;

	public CanvasScaler m_CanvasScaler = null;

	public csHUD_Timer m_WaitingTimer = null;

	public void EnableCrossHair(bool _enable)
	{
		if (Utility.IsActive(m_NormalCrossHair) != _enable)
		{
			Utility.Activate(m_NormalCrossHair, _enable);
		}
	}
}
