using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csObstacle : MonoBehaviour
{
	private MeshCollider m_MeshCollider = null;

	private void Awake()
	{
		m_MeshCollider = GetComponent<MeshCollider>();

		csGrid.StartCreateGrid += EnableConvex;
		csGrid.EndCreateGrid += DisableConvex;
	}

	private void EnableConvex()
	{
		if(m_MeshCollider != null)
		{
			m_MeshCollider.convex = true;
		}		
	}

	private void DisableConvex()
	{
		if (m_MeshCollider != null)
		{
			m_MeshCollider.convex = false;
		}			
	}
}
