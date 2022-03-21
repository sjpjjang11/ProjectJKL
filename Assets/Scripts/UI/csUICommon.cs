using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
2018.06.12
sjpjjang11
UI 에서 사용하는 기본 기능들을 정의한 클래스 - 팝업창 , 전체팝업창 , 버튼 , 등등 

*/
public class csUICommon : MonoBehaviour 
{
	/*
	2018.06.12
	sjpjjang11
	데이터 클래스 get 변수와 함수
	*/
	protected csDataManager m_pDataManager;
	public csDataManager pDataManager
	{
		get
		{
			if(m_pDataManager == null)
			{
				m_pDataManager = csProjectManager.Instance.m_pDataManager;
			}
			return m_pDataManager;
		}
	}


	void Awake()
	{
		OnAwake();
	}

	void Start()
	{
		OnStart();
	}

	void OnEnable()
	{
		OnEnabled();
	}

	void OnDestroy()
	{
		OnDestroied();
	}

	/*
	2018.06.12
	sjpjjang11
	각 매니저들 할당하는 함수a
	*/
	protected void SetManagers()
	{
		if(m_pDataManager == null)
		{
			m_pDataManager = csProjectManager.Instance.m_pDataManager;
		}

	}



	/*
	2018.06.12
	sjpjjang11
	가장 먼저 실행되어야할 함수들 Awake 함수에서 실행 - 데이터 세팅
	*/
	protected virtual void OnAwake()
	{
		SetManagers();
	}

	/*
	2018.06.12
	sjpjjang11	
	UI 관련 정보들 세팅 - 언어 , 이벤트 등등
	*/
	protected virtual void OnStart()
	{
	}

	/*
	2018.06.12
	sjpjjang11
	오브젝트가 활성화 되었을 때 실행하는 함수
	*/
	protected virtual void OnEnabled()
	{
	}

	/*
	2018.06.12
	sjpjjang11
	오브젝트가 종료됐을때 실행하는 함수
	*/
	protected virtual void OnDestroied()
	{
	}

	/*
	2018.06.12
	sjpjjang11
	객체내용 다시 표시할때 사용하는 함수
	*/
	public virtual void OnUpdate()
	{

	}

}
