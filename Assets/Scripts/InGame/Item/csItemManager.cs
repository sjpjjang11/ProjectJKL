using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csItemManager : MonoBehaviour {

	protected IEnumerator m_CoSpawnItem = null;

	

	protected List<ItemName> m_ListItemName =
		new List<ItemName>()
		{			
			{ ItemName.Speed },
			{ ItemName.Power },
			{ ItemName.Heal },
			//{ ItemName.Skill }
		};

	protected GameObject[] m_Item = null;

	public List<Vector3> m_ListSpawnPosition = new List<Vector3>();
	protected ObjectPool<csItem>[] m_ItemPool = null;

	protected csBattleManager m_BattleManager = null;

	protected csGrid m_Grid = null;

	public const int SpeedType = 0;
	public const int PowerType = 1;
	public const int HealType = 2;
	public const int SkillType = 3;

	protected int m_iDefaultPoolCount = 20;
	protected int m_iItemTypeCount = 0;
	protected int m_iMaxItemPoolCount = 50;

	protected virtual void Awake()
	{
		m_BattleManager = GetComponent<csBattleManager>();

		m_iItemTypeCount = m_ListItemName.Count;
	}

	public void Settings()
	{
		m_Grid = csPathRequestManager.Instance.GetComponent<csGrid>();

		m_BattleManager.StartBattle += StartBattle;

		CreateSpawnPosition();
		CreateItem();
	}

	public void Release()
	{
		DestroyItem();
	}

	private void CreateItem()
	{
		m_Item = new GameObject[m_iItemTypeCount];
		m_ItemPool = new ObjectPool<csItem>[m_iItemTypeCount];

		for (int i = 0; i < m_iItemTypeCount; i++)
		{
			m_Item[i] = new GameObject();
			m_Item[i] = Resources.Load("Prefabs/Item/Item_" + m_ListItemName[i]) as GameObject;
			m_ItemPool[i] = new ObjectPool<csItem>();
			m_ItemPool[i].CreatePool(name, m_Item[i].gameObject, csBattleManager.m_WaitingPool_Item, m_iDefaultPoolCount);
			//Utility.Activate(m_Item[i].gameObject, false);
		}
		
		/*if (m_Item != null)
		{
			m_ItemPool = new ObjectPool<csItem>();

			m_ItemPool.CreatePool(name, m_Item, Transform, m_iMaxItemPoolCount);

			for (int i = 0; i < m_ItemPool.m_ListComponent.Count; i++)
			{
				m_ItemPool.m_ListComponent[i].Transform.localPosition = Vector3.zero;
			}
		}*/
	}

	private void DestroyItem()
	{
		for(int i = 0; i < m_iItemTypeCount; i++)
		{
			m_ItemPool[i].AllClearPool();
			m_ItemPool[i] = null;
			Destroy(m_Item[i].gameObject);
		}
	}

	private void StartBattle()
	{
		Spawn();

		m_BattleManager.StartBattle -= StartBattle;
	}

	public void Spawn()
	{
		/*CreateSpawnPosition();
		m_CoSpawnItem = CoSpawnItem();
		StartCoroutine(m_CoSpawnItem);*/

		//int ObjectIndex = m_ItemPool[_itemType].UseObject(_spawnPosition, false);
		//m_ItemPool[_itemType].m_ListComponent[ObjectIndex].Spawn();

		for(int i = 0; i < m_ListSpawnPosition.Count; i++)
		{
			int RandomItemType = UnityEngine.Random.Range(0, m_iItemTypeCount);
			int ObjectIndex = m_ItemPool[RandomItemType].UseObject(m_ListSpawnPosition[i], false);
			m_ItemPool[RandomItemType].m_ListComponent[ObjectIndex].Spawn();
		}
	}

	/*public void Deactivate(int _itemType, int _itemIndex)
	{
		for (int i = 0; i < m_ItemPool[_itemType].m_ListComponent.Count; i++)
		{
			if(m_ItemPool[_itemType].m_ListComponent[i].m_iItemIndex == _itemIndex)
			{
				Utility.Activate(m_ItemPool[_itemType].m_ListComponent[i].gameObject, false);
			}
		}
	}*/

	private void CreateSpawnPosition()
	{		
		Vector2 WorldSize = m_Grid.m_GridWorldSize;
		float WorldSizeX = WorldSize.x * 0.5f;				
		float WorldSizeZ = WorldSize.y * 0.5f;

		float Height = 4.0f;

		if (m_ListSpawnPosition.Count > 0)
		{
			m_ListSpawnPosition.Clear();
		}

		for (int i = 0; i < m_iMaxItemPoolCount; i++)
		{
			while (true)
			{
				float RandomX = UnityEngine.Random.Range(-WorldSizeX, WorldSizeX);
				RandomX += m_Grid.transform.position.x;
				float RandomZ = UnityEngine.Random.Range(-WorldSizeZ, WorldSizeZ);
				RandomZ += m_Grid.transform.position.z;
				Vector3 RandomPosition = new Vector3(RandomX, Height, RandomZ);

				if (!OverlapSpawnPositionCheck(RandomPosition, i) && m_Grid.NodeFromWorldPoint(RandomPosition).NodeType != eNodeType.Obstacle)
				{
					m_ListSpawnPosition.Add(RandomPosition);

					break;
				}
			}		
		}
	}

	private bool OverlapSpawnPositionCheck(Vector3 _pos, int _count)
	{
		//Debug.Log(_count);
		for (int i = 0; i < _count; i++)
		{
			float Distance = Vector3.Distance(m_ListSpawnPosition[i], _pos);
			//Debug.Log(Distance);
			if (Distance < 5.0f)
			{
				return true;
			}
		}

		return false;
	}

	/*private IEnumerator CoSpawnItem()
	{
		float Delay = 0.0f;
		int MaxCount = Random.Range(2, m_iMaxItemPoolCount + 1);
		int Count = 0;

		while(true)
		{
			int ObjectIndex = m_ItemPool.UseObject(m_ListSpawnPosition[Count], false);
			m_ItemPool.m_ListComponent[ObjectIndex].Spawn();

			Delay = Random.Range(0.0f, 0.3f);

			Count++;

			if (Count == MaxCount)
			{
				break;
			}

			yield return new WaitForSeconds(Delay);
		}
	}*/
}
