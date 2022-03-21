using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
2018.06.12
sjpjjang11
전체화면형태의 뷰 화면을 보여주는 클래스
*/
public class csUIView : csUIPanel 
{
	public string viewName;

	public override void SetNavigationViewInfo()
	{
		base.SetNavigationViewInfo();
		//m_pNavigationViewInfo.eUiType = eUIType.UIVIEW;
	}

	protected override void OnAwake()
	{
		base.OnAwake();
	}

	protected void SetViewName()
	{
		if(!string.IsNullOrEmpty(viewName))
		{
		}
	}

	protected override void Remove()
	{
		base.Remove();
	}
}
