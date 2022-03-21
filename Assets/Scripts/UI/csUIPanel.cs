using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
2018.06.12
sjpjjang11
패널형태의 팝업들의 기본 세팅 클래스a - 팝업 , 뷰
*/

public class csUIPanel : csUICommon 
{
	protected string m_strPanelName;					// 기본 패널 이름a
	protected RectTransform m_pRectTransform;			// 패널의 RectTransform 
	public string strPanelName { get { return m_strPanelName; } set { m_strPanelName = value; } }		// 기본 패널 이름 get, set

	public virtual void SetNavigationViewInfo() { } //Awake전에 호출할수도 있음

	protected override void OnAwake()
	{
		base.OnAwake();

		if(m_pRectTransform == null)
		{
			m_pRectTransform = this.gameObject.GetComponent<RectTransform>();
		}
	}

	/*
	2018.06.12
	sjpjjang11
	패널이 종료 될때 실행되는 함수a
	*/
	protected override void OnDestroied()
	{
		base.OnDestroied();
	}

	/*
	2018.06.12
	sjpjjang11
	팝업에 필요한 데이터 설정
	*/
	public virtual void SetData(object _param) { }

	/*
	2018.06.12
	sjpjjang11
	앵커 중심 조절 및 전체 화면 
	*/
	public void InitializeCenterStretchTransform()
	{
		if(m_pRectTransform == null)
		{
			m_pRectTransform = this.gameObject.GetComponent<RectTransform>();
		}
		m_pRectTransform.anchorMin = Vector2.zero;
		m_pRectTransform.anchorMax = Vector2.one;

		m_pRectTransform.sizeDelta = Vector2.zero;
		transform.localScale = Vector3.one;
	}

	/*
	2018.07.09
	sjpjjang11
	팝업창 삭제시 호출a
	*/
	protected virtual void Remove()
	{
		//UINavigationManager.Instance.RemovePopup(this);
	}

	/*
	2018.06.12
	sjpjjang11
	팝업창에 버튼 세팅
	*/
	public virtual void Visible(bool _bShow)
	{
		this.gameObject.SetActive(_bShow);
	}

	public bool IsVisible()
	{
		return this.gameObject.activeSelf;
	}

	/*
	2018.06.12
	sjpjjang11
	팝업창에 버튼 세팅
	*/
	protected virtual void SetButtons()
	{
	}
}
