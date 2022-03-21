using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GridLayoutGroup))]

public class csDropSlot : MonoBehaviour, IDropHandler
{
	private Transform m_Transform = null;

	private void Awake()
	{
		m_Transform = GetComponent<Transform>();
	}

	public csDragObject Item
	{
		get
		{
			if(m_Transform.childCount > 0)
			{
				return m_Transform.GetChild(0).GetComponent<csDragObject>();
			}

			return null;
		}
	}

	public void OnDrop(PointerEventData eventData)
	{		
		csDragObject D = eventData.pointerDrag.GetComponent<csDragObject>();

		if(Item != null)
		{
			Item.m_Transform.SetParent(D.m_ReturnToParent);
		}

		D.m_ReturnToParent = m_Transform;
	}

}
