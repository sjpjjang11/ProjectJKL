using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csBattleUIManager : Singleton<csBattleUIManager> {

    public Camera m_UICamera = null;

	private csBattleControllerCanvas m_BattleController = null;
	public csBattleControllerCanvas BattleController
	{
		get
		{
			if(m_BattleController == null)
			{
				m_BattleController = GetComponentInChildren<csBattleControllerCanvas>();
			}

			return m_BattleController;
		}
	}

	private csBattleTopCanvas m_BattleTop = null;
	public csBattleTopCanvas BattleTop
	{
		get
		{
			if (m_BattleTop == null)
			{
				m_BattleTop = GetComponentInChildren<csBattleTopCanvas>();
			}

			return m_BattleTop;
		}
	}

	private csBattleEtcCanvas m_BattleETC = null;
	public csBattleEtcCanvas BattleETC
	{
		get
		{
			if (m_BattleETC == null)
			{
				m_BattleETC = GetComponentInChildren<csBattleEtcCanvas>();
			}

			return m_BattleETC;
		}
	}

	private csBattleHUDCanvas m_BattleHUD = null;
	public csBattleHUDCanvas BattleHUD
	{
		get
		{
			if (m_BattleHUD == null)
			{
				m_BattleHUD = GetComponentInChildren<csBattleHUDCanvas>();
			}

			return m_BattleHUD;
		}
	}

	public void Settings()
    {
		BattleETC.Settings();
    }
}
