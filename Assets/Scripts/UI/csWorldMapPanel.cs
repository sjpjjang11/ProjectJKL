using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csWorldMapPanel : MonoBehaviour
{

	public RectTransform m_HeroMarker;
	private Transform m_HeroTr;

	private Vector2 m_TerrainSize = new Vector2(160.0f, 160.0f);
	private Vector2 m_WorldMapSize;
	private Vector2 m_Percent;

	protected void Awake()
	{
        m_WorldMapSize = new Vector2(900.0f, 900.0f);
		m_Percent = new Vector2(Utility.GetSomeValueOfTotalValue(m_TerrainSize.x, m_WorldMapSize.x), Utility.GetSomeValueOfTotalValue(m_TerrainSize.y, m_WorldMapSize.y));
	}

	protected void Start()
	{
		m_HeroTr = csBattleManager.Instance.m_EventHandler_Hero.transform;

		MarkerSettings();
	}

	private void MarkerSettings()
	{	
		//BotSpawn[] spawns = BattleManager.instance.GetBotSpawnManager().GetSpawnPointTr();
		GameObject obj = Resources.Load("Prefabs/BotMarker") as GameObject;
		Vector2 pos;

		//for(int i = 0; i < spawns.Length; i++)
		{
			//pos = new Vector2(Utility.GetPercentOfTotalValue(spawns[i].Transform.position.x, m_Percent.x), Utility.GetPercentOfTotalValue(spawns[i].Transform.position.z, m_Percent.y));
		}
	}

	protected void Update ()
	{
		Vector2 pos = new Vector2(Utility.GetPercentOfTotalValue(m_HeroTr.position.x, m_Percent.x), Utility.GetPercentOfTotalValue(m_HeroTr.position.z, m_Percent.y));
        Vector3 rot = new Vector3(0.0f, 0.0f, -m_HeroTr.eulerAngles.y);

        m_HeroMarker.anchoredPosition = pos;
        m_HeroMarker.eulerAngles = rot;
    }
}
