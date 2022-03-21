using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]

public class csDragObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[HideInInspector]
	public Transform m_Transform = null;
	private Transform m_Parent = null;
	public Transform m_ReturnToParent = null;

	protected Camera m_UICamera = null;

	private CanvasGroup m_CanvasGroup = null;

	private Vector2 m_BeginDragScale = Vector2.zero;
	private Vector2 m_DropScale = Vector2.zero;

	private void Awake()
	{
		m_Transform = GetComponent<Transform>();
		m_Parent = m_Transform.parent;

		m_UICamera = csBattleManager.Instance.m_BattleUIManager.m_UICamera;

		m_CanvasGroup = GetComponent<CanvasGroup>();

		m_ReturnToParent = m_Parent;

		m_BeginDragScale = new Vector2(1.5f, 1.5f);
		m_DropScale = m_Transform.localScale;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		m_ReturnToParent = m_Parent;

		m_Transform.localScale = m_BeginDragScale;

		m_Transform.SetParent(m_Parent.parent);

		m_Transform.SetAsLastSibling();

		m_CanvasGroup.blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector3 WorldPoint = m_UICamera.ScreenToWorldPoint(eventData.position);
		WorldPoint.z = 0.0f;

		m_Transform.position = WorldPoint;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		m_Transform.localScale = m_DropScale;

		m_Transform.SetParent(m_ReturnToParent);

		Vector3 EndDragPosition = m_Transform.position;
		EndDragPosition.z = 0.0f;
		m_Transform.position = EndDragPosition;

		m_CanvasGroup.blocksRaycasts = true;
	}
}
