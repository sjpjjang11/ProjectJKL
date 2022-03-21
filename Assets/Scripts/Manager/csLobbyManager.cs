using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
2018.06.12
sjpjjang11
타이틀을 제외한 로비에서 쓰이는 UI를 관리하는 로비 매니저
*/
public class csLobbyManager : MonoBehaviour
{
	public LoadingSceneType m_SceneType_Loading;
	public InGameSceneType m_SceneType_InGame;
	public MapSceneType m_SceneType_Map;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		Initialize();
		StartDataLoading();		
	}

	protected void Initialize()
	{
		csProjectManager.Instance.m_pLobbyManager = this;
		csProjectManager.Instance.Initialize();
	}

	public void StartDataLoading()
	{
		StartCoroutine(IE_DataLoadWork());
	}

	IEnumerator IE_DataLoadWork()
	{
		if (csProjectManager.Instance.m_bLoadedData == false)
		{
			yield return StartCoroutine(csProjectManager.Instance.m_pDataManager.CoHeroData());
			yield return StartCoroutine(csProjectManager.Instance.m_pDataManager.CoPetData());
		}

		StartGame();//
	}

	public void StartGame()
	{
		csLoadingManager.LoadScene(m_SceneType_InGame.ToString(), m_SceneType_Map.ToString(), m_SceneType_Loading.ToString());
	}
}
