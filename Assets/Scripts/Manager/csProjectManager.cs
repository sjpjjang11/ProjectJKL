using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
2018.06.12
sjpjjang11
게임내 모든 매니저들과 상태를 관리하는 전체 프로젝트 매니저 클래스
*/

// sealed 한정자를 통해서 해당 클래스가 상속이 불가능하도록 조치.
public sealed class csProjectManager 
{
	private static volatile csProjectManager uniqueInstance;		// 싱글톤 인스턴스를 저장
	private static object _lock = new System.Object();              // 접근 한정자

	public csDataManager m_pDataManager;							// 데이터 매니저 접근
	public csNetworkManager m_pNetworkManager;						// 네트웍 매니저 접근
	public csUIManager m_pUIManager;                                // 유아이 매니저 접근
	public csLobbyManager m_pLobbyManager;                          // 로비 매니저 접근

	public LobbySceneType SceneType_Lobby_InUse { get; private set; }
	public LoadingSceneType SceneType_Loding_InUse { get; private set; }
	public InGameSceneType SceneType_InGame_InUse { get; private set; }
	public MapSceneType SceneType_Map_InUse { get; private set; }

	public bool m_bLoadedData = false;                                 // 로딩 완료 체크

	public int m_iHeroSelect = 0;
	public int m_iPetSelect = 0;							

	/*
	2018.06.12
	sjpjjang11
	프로젝트 매니저 생성자 함수
	*/
	private csProjectManager()
	{
		ProjectSetting();
	}

	/*
	2018.06.12
	sjpjjang11
	프로젝트에서 시작시 셋팅해야 하는 값들 세팅하는 함수
	*/
	void ProjectSetting()
	{
		
	}

	/*
	2018.06.12
	sjpjjang11
	외부에서 접근할 수 있도록 하는 함수
	*/
	public static csProjectManager Instance
	{
		get
		{
			if(uniqueInstance == null)
			{
				// lock으로 지정된 블록안의 코드를 하나의 쓰레드만 접근하도록 한다.
				lock(_lock)
				{
					if(uniqueInstance == null)
					{
						uniqueInstance = new csProjectManager();
					}
				}
			}

			return uniqueInstance;
		}
	}

	public void Initialize()
    {
		SceneType_Lobby_InUse = Utility.EnumParse<LobbySceneType>(m_pLobbyManager.gameObject.scene.name); 
		SceneType_Loding_InUse = m_pLobbyManager.m_SceneType_Loading;
		SceneType_InGame_InUse = m_pLobbyManager.m_SceneType_InGame;
		SceneType_Map_InUse = m_pLobbyManager.m_SceneType_Map;

		CreatePrimitiveClass();
	}

	private void CreatePrimitiveClass()
	{
		m_pDataManager = new csDataManager();
	}

	public void DataLoadComplete()
	{
		if (m_bLoadedData.Equals(false))
		{
			m_bLoadedData = true;
		}
	}

}


