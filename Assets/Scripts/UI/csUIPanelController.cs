using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
2018.06.12
sjpjjang11
패널 컨트롤러 한개의 패널에서 여러개의 패널을 관리하도록 함a
*/
public class csUIPanelController  
{
	protected csUIPanel m_pRootView;						// 기본 부모 패널
	protected List<csUIPanel> m_pSubViews;					// 자식 패널들

	public csUIPanel rootView { get { return m_pRootView; } }				// 부모패널 get
	public List<csUIPanel> subViews { get { return m_pSubViews; } }			// 자식 패널들 get
	public int subViewCount { get { return m_pSubViews.Count; } }			// 자식 패널들 갯수 get

	#region 생성자/소멸자 
	/*
	2018.06.12
	sjpjjang11
	생성자
	*/
	public csUIPanelController(csUIPanel _rootView)
	{
		m_pRootView = _rootView;
		m_pSubViews = new List<csUIPanel>();
	}

	/*
	2018.06.12
	sjpjjang11
	소멸자
	*/
	public void Dispose()
	{
		if(m_pSubViews != null)
		{
			m_pSubViews.Clear();
			m_pSubViews = null;
		}
		m_pRootView = null;
	}
	#endregion

	#region 제어 
	/*
	2018.06.12
	sjpjjang11
	패널을 서브뷰리스트에 추가하는 함수
	*/
	public void Push(csUIPanel _panel)
	{
		m_pSubViews.Add(_panel);
	}

	/*
	2018.06.12
	sjpjjang11
	맨 마지막 패널을 제거하는 함수
	*/
	public csUIPanel Pop()
	{
		csUIPanel p = null;
		int c = m_pSubViews.Count;
		if(c > 0)
		{
			p = m_pSubViews[c - 1];
			m_pSubViews.Remove(p);
		}
		return p;
	}

	/*
	2018.06.12
	sjpjjang11
	패널을 서브뷰리스트에서 제거하는 함수
	*/
	public csUIPanel Pop(csUIPanel _p)
	{
		m_pSubViews.Remove(_p);
		return _p;
	}

	/*
	2018.06.12
	sjpjjang11
	맨 앞 패널을 반환하는 함수
	*/
	public csUIPanel Head()
	{
		csUIPanel p = null;
		int c = m_pSubViews.Count;
		if(c > 0)
		{
			p = m_pSubViews[0];
		}
		return p;
	}

	/*
	2018.06.12
	sjpjjang11
	맨 뒤 패널을 반환하는 함수a
	*/
	public csUIPanel Tail()
	{
		csUIPanel p = null;
		int c = m_pSubViews.Count;
		if(c > 0)
		{
			p = m_pSubViews[c - 1];
		}
		return p;
	}

	/*
	2018.06.12
	sjpjjang11
	최상단 패널을 반환
	*/
	public csUIPanel TopMost()
	{
		csUIPanel p = Tail();
		if(p == null)
		{
			p = m_pRootView;
		}
		return p;
	}

	/*
	2018.06.12
	sjpjjang11
	컨트롤러에 있는 모든 패널을 보이거나 감추는 함수
	*/
	public void Visible(bool _bShow)
	{
		if(m_pRootView != null)
		{
			bool activeSelf = m_pRootView.IsVisible();
			m_pRootView.Visible(_bShow);

			if(_bShow && !activeSelf)
			{
				UpdatePanel();
			}
		}

		for(int i = 0; i < m_pSubViews.Count; i++)
		{
			csUIPanel p = m_pSubViews[i];
			p.Visible(_bShow);
		}
	}

	/*
	2018.06.12
	sjpjjang11
	컨트롤러에 있는 패널 찾는 함수
	*/
	public csUIPanel FindPanel(string _strName)
	{
		for(int i = 0; i < subViewCount; i++)
		{
			if(subViews[i].strPanelName.Equals(_strName))
			{
				return subViews[i];
			}
		}

		return null;
	}

	/*
	2018.06.12
	sjpjjang11
	컨트롤러에 있는 모든 패널 업데이트a
	*/
	public void UpdatePanel()
	{
		if(m_pRootView != null)
		{
			m_pRootView.OnUpdate();
		}

		for(int i = 0; i < m_pSubViews.Count; i++)
		{
			csUIPanel p = m_pSubViews[i];
			p.OnUpdate();
		}
	}
	#endregion
}
