using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csSpawnManager : MonoBehaviour {

	public Transform[] m_SpawnPoint;

	private csBattleManager m_BattleManager = null;
	private csBattleUIManager m_BattleUIManager = null;
	private csGrid m_Grid = null;

	private csOwner_Monster m_MonsterPoolObject = null;

	private ObjectPool<csOwner_Monster> m_MonsterPool = null;

	private csHUD_Timer m_WaitingTimer = null;

	public string m_strOwnerLoadPath = "";
	public string m_strLocalUserOwnerPrefabName = "";
	public string m_strMonsterOwnerPrefabName = "";

	public const int LimitedPopulation = 9;
	public int m_iDefaultPoolCount = 3;
	public int m_iCurrentPopulation = 0;	
	public int m_CurrentSpawnIndex = 1;
	public int CurrentSpawnIndex
	{
		get
		{
			int Value = m_CurrentSpawnIndex;
			m_CurrentSpawnIndex++;

			return Value;
		}
	}
	
	private void Awake()
	{
		m_BattleManager = GetComponent<csBattleManager>();
		m_BattleUIManager = m_BattleManager.m_BattleUIManager;

		m_MonsterPool = new ObjectPool<csOwner_Monster>();

		m_WaitingTimer = m_BattleUIManager.BattleHUD.m_WaitingTimer;
		m_WaitingTimer.Settings(false, false);	
	}

	private void Start()
	{
		m_Grid = m_BattleManager.m_Grid;
	}

	public void CreateMonster()
	{
		GameObject Monster;

		Monster = Resources.Load(m_strOwnerLoadPath + m_strMonsterOwnerPrefabName) as GameObject;

		m_MonsterPool.CreatePool(Monster.gameObject, csBattleManager.m_WaitingPool_Monster, m_iDefaultPoolCount, false);

		for (int i = 0; i < m_MonsterPool.m_ListComponent.Count; i++)
		{
			MonsterSettings(i);
		}
	}

	private void MonsterSettings(int _index)
	{	
		int MonsterIndex = _index + 1;
		csOwner_Monster Monster = m_MonsterPool.m_ListComponent[_index];
		//Monster.name = Monster.name + "_" + MonsterIndex + "_emg";
		m_BattleManager.OwnerSettings(csBattleManager.MonsterType, MonsterIndex, Monster);
	}

	public void Initialize()
    {
		m_iCurrentPopulation = 0;
	}

	public void SpawnMonster()
	{
		Vector3 SpawnPoint = Vector3.zero;

		for(int i = 0; i < m_SpawnPoint.Length; i++)
		{
			SpawnPoint = m_SpawnPoint[i].position;

			bool IsMonster = Physics.CheckSphere(SpawnPoint, 5.0f, m_BattleManager.m_iLayerMask_Monster);
			bool IsHero = Physics.CheckSphere(SpawnPoint, 5.0f, m_BattleManager.m_iLayerMask_Hero);

			if (!IsMonster && !IsHero)
			{
				break;
			}

			if(i == m_SpawnPoint.Length - 1)
			{
				Debug.LogError("!!!!!!!!!!!!!!!!!");
				float MinX = m_Grid.transform.position.x - 50.0f;
				float MaxX = m_Grid.transform.position.x + 20.0f;

				float MinZ = m_Grid.transform.position.z - 40.0f;
				float MaxZ = m_Grid.transform.position.z + 30.0f;

				while (true)
				{
					SpawnPoint = new Vector3(UnityEngine.Random.Range(MinX, MaxX), 0.0f, UnityEngine.Random.Range(MinZ, MaxZ));

					IsMonster = Physics.CheckSphere(SpawnPoint, 5.0f, m_BattleManager.m_iLayerMask_Monster);
					IsHero = Physics.CheckSphere(SpawnPoint, 5.0f, m_BattleManager.m_iLayerMask_Hero);
					bool IsObstacle = m_Grid.NodeFromWorldPoint(SpawnPoint).NodeType == eNodeType.Obstacle;

					if (!IsMonster && !IsHero && !IsObstacle)
					{
						break;
					}
				}
			}
		}

		int ObjectIndex = m_MonsterPool.UseObject(SpawnPoint, false);

		if (ObjectIndex == m_iDefaultPoolCount)
		{
			MonsterSettings(ObjectIndex);
			m_iDefaultPoolCount++;
		}

		m_iCurrentPopulation++;
	}

	public void RespawnMonster()
	{
		if(m_iCurrentPopulation < csBattleManager.MaxPopulation)
		{
			SpawnMonster();
		}		
	}

	public void CreateUser(Action<csOwner_Hero> _callback = null)
	{
		csOwner_Hero User;
		User = Instantiate(Resources.Load(m_strOwnerLoadPath + m_strLocalUserOwnerPrefabName) as GameObject).GetComponent<csOwner_Hero>();
		int RandNum = UnityEngine.Random.Range(0, m_SpawnPoint.Length);
		User.Transform.position = m_SpawnPoint[RandNum].position;
		m_SpawnPoint = m_SpawnPoint.Where(condition => condition != m_SpawnPoint[RandNum]).ToArray();
		m_BattleManager.ObjectMoveToBattleScene(User.gameObject);

		_callback?.Invoke(User);
	}

	/*public void CreateMonster(Action<csOwner_Monster, int> _callback)
	{
		csOwner_Monster Monster;

		Monster = Instantiate(Resources.Load(m_strOwnerLoadPath + m_strMonsterOwnerPrefabName) as GameObject).GetComponent<csOwner_Monster>();
		Monster.name = Monster.name + "_" + m_MonsterIndex;
		m_BattleManager.ObjectMoveToBattleScene(Monster.gameObject);

		int RandNum = UnityEngine.Random.Range(0, m_SpawnPoint.Length);
		Monster.Transform.position = m_SpawnPoint[RandNum].position;

		if (_callback != null)
		{
			_callback(Monster, m_MonsterIndex);
		}

		m_MonsterIndex++;
	}*/

	public void BattleStartTimer(int _second, Action _callback = null)
	{
		m_WaitingTimer.NonNetStartTimer(_second, () =>
		{
			_callback?.Invoke();
		});
	}
}
