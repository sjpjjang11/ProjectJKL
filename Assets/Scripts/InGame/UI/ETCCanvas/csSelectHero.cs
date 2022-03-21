using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csSelectHero : MonoBehaviour {

	private bool m_IsLock = false;

	private csBattleManager m_BattleManager = null;
	private csBattleUIManager m_BattleUIManager = null;

	private void Awake()
	{
		m_BattleManager = csBattleManager.Instance;
		m_BattleUIManager = m_BattleManager.m_BattleUIManager;
	}

	public void OnClickOpenTheSelect()
	{
		if(!m_IsLock && !Utility.IsActive(gameObject))
		{
			Utility.Activate(gameObject, true);
			m_BattleUIManager.BattleETC.m_WaitingRoomUI.CloseWaitingRoomUI();
		}		
	}

	public void CloseSelectHero()
	{
		if (Utility.IsActive(gameObject))
		{
			Utility.Activate(gameObject, false);
		}

		m_BattleUIManager.BattleETC.m_WaitingRoomUI.OpenWaitingRoomUI();
	}

	public bool IsOpen()
	{
		return Utility.IsActive(gameObject);
	}

	public void Lock()
	{
		m_IsLock = true;
	}
}
