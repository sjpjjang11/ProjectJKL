using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csTrailManager : MonoBehaviour {

	public TrailRenderer[] m_TrailRenderer = null;

	public void ClearTrailAll()
	{
		for (int i = 0; i < m_TrailRenderer.Length; i++)
		{
			m_TrailRenderer[i].Clear();
		}
	}

	public void ClearTrailIndex(int _index)
	{
		m_TrailRenderer[_index].Clear();
	}

	public void ActivateGameObjectAll(bool _active)
	{
		for(int i = 0; i < m_TrailRenderer.Length; i++)
		{
			if (Utility.IsActive(m_TrailRenderer[i].gameObject) != _active)
			{
				Utility.Activate(m_TrailRenderer[i].gameObject, _active);
			}			
		}		
	}

	public void ActivateGameObjectIndex(int _index, bool _active)
	{
		if(Utility.IsActive(m_TrailRenderer[_index].gameObject) != _active)
		{
			Utility.Activate(m_TrailRenderer[_index].gameObject, _active);
		}		
	}

	public void ActivateRendererAll(bool _active)
	{
		for(int i = 0; i < m_TrailRenderer.Length; i++)
		{
			if(Utility.IsActive(m_TrailRenderer[i]) != _active)
			{
				Utility.Activate(m_TrailRenderer[i], _active);
			}		
		}	
	}

	public void ActivateRendererIndex(int _index, bool _active)
	{
		if (Utility.IsActive(m_TrailRenderer[_index]) != _active)
		{
			Utility.Activate(m_TrailRenderer[_index], _active);
		}			
	}
}
