using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csBeadManager : MonoBehaviour {

	private IEnumerator m_CoSpawnBead = null;

	protected ObjectPool<csBead> m_Beadpool = null;
	protected Transform m_WaitingPool_Bead = null;
	protected GameObject m_Bead = null;

	public float m_fSpawnCycle = 0.5f;
	public int m_iDefaultPoolCount = 100;

	public virtual void Settings()
	{
		csBattleManager.Instance.StartBattle += StartBattle;

		CreateBead();
		CreateWaitingPool();
	}

	protected virtual void CreateBead()
	{
		m_Bead = Instantiate(Resources.Load("Prefabs/Bead/Bead") as GameObject);

		if(m_Bead != null)
		{
			m_Beadpool = new ObjectPool<csBead>();
			m_Beadpool.CreatePool(name, m_Bead, m_WaitingPool_Bead, m_iDefaultPoolCount);

			for(int i = 0; i < m_Beadpool.m_ListComponent.Count; i++)
			{

			}
		}
	}

	protected virtual void CreateWaitingPool()
	{
		m_WaitingPool_Bead = new GameObject(Utility.DetermineObjectPoolName("Bead")).transform;
	}

	protected virtual void StartBattle()
	{
		csBattleManager.Instance.StartBattle -= StartBattle;
	}

	#region Coroutine

	protected virtual IEnumerator CoSpawnBead()
	{
		while(true)
		{
			yield return null;

			//m_Grid.NodeFromWorldPoint(m_PathTarget).m_NodeType != NodeType.Obstacle
		}
	}

	#endregion
}
