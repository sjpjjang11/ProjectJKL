using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csMove_Bot : csMove {

	private void OnEnable()
	{
		if (csBattleManager.Instance.m_GameState == csBattleManager.GameState.Battle)
		{
			StartBattle();
		}
	}

	protected override void StartBattle()
	{
		base.StartBattle();

		if (Utility.IsActive(gameObject))
		{			
			m_CoMoveHandler = CoMoveHandler();
			StartCoroutine(m_CoMoveHandler);
		}		
	}
}
