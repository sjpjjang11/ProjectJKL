using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csMapObject : MonoBehaviour 
{
	public GameObject[] m_objMapObject;

	public void SetMapObject(int index)
	{
		for(int i = 0; i < m_objMapObject.Length; i++)
		{
			m_objMapObject[i].SetActive(false);
			if(i == (index - 1))
			{
				m_objMapObject[i].SetActive(true);
			}
		}
	}
}
