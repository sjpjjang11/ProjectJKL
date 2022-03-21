using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class csCollision : MonoBehaviour {

	private Action<Collider> m_OnCollision = null;
	private Action<Collider> m_OnTrigger = null;

	private List<Collider> m_ListIgnoredColliders = null;

	private Collider m_Collider = null;

	private int m_iTargetLayerMask = 0;

	private void Awake()
	{
		m_ListIgnoredColliders = new List<Collider>();
	}

	public void Release()
	{
		UnregisterOnCollisionCallback();
		UnregisterOnTriggerCallback();
		ClearIgnore();
		m_iTargetLayerMask = 0;
	}

	public void RegisterOnCollisionCallback(Action<Collider> _callback)
	{	
		m_OnCollision = _callback;
	}

	public void RegisterOnTriggerCallback(Action<Collider> _callback)
	{
		m_OnTrigger = _callback;
	}

	public void UnregisterOnCollisionCallback()
	{
		m_OnCollision = null;
	}

	public void UnregisterOnTriggerCallback()
	{
		m_OnTrigger = null;
	}

	public void RegisterCollisionLayerMask(int _targetLayerMask)
	{		
		m_iTargetLayerMask = _targetLayerMask;
	}

	public void IgnoreCollider(Collider _collider)
	{
		//Debug.Log("IgnoreCollider");
		if(!m_ListIgnoredColliders.Contains(_collider))
		{
			m_ListIgnoredColliders.Add(_collider);
		}		
	}

	public bool CheckIgnore(Collider _collider)
	{
		bool Result = false;

		if (m_ListIgnoredColliders.Contains(_collider))
		{
			Result = true;
		}

		return Result;
	}

	public void RemoveIgnore(Collider _collider)
	{
		m_ListIgnoredColliders.Remove(_collider);
	}

	public void ClearIgnore()
	{
		//Debug.Log("ClearIgnore");
		if(m_ListIgnoredColliders != null)
		{
			m_ListIgnoredColliders.Clear();
		}		
	}

	private void OnCollisionEnter(Collision _collision)
	{
		//Debug.Log("OnCollisionEnter : " + _collision.gameObject.name + "  " + name);
		Colliding(_collision);
	}

	private void OnCollisionStay(Collision _collision)
	{
		//Debug.Log("OnCollisionStay : " + _collision.gameObject.name + "  " + name);
		Colliding(_collision);
	}

	private void OnTriggerEnter(Collider _collider)
	{
		//Debug.Log("OnTriggerEnter : " + _collider.gameObject.name + "  " + name);
		Colliding(_collider);
	}

	private void OnTriggerStay(Collider _collider)
	{
		//Debug.Log("OnTriggerStay : " + _collider.gameObject.name + "  " + name);
		Colliding(_collider);
	}

	private void OnControllerColliderHit(ControllerColliderHit _hit)
	{
		//Debug.Log("######### OnControllerColliderHit : " + _hit.collider.name + "  " + name);
		Colliding(_hit.collider);
	}

	private void Colliding(Collision _collision)
	{
		//Debug.Log("@@@@@@@");
		if(m_ListIgnoredColliders.Contains(_collision.collider))
		{
			//Debug.Log("m_ListIgnoredColliders.Contains(_collision.collider) : " + _collision.collider.name);
			return;
		}

		int Test = ((1 << _collision.gameObject.layer) & m_iTargetLayerMask);
		//Debug.Log("~~~~~~~~~~~~ : " + Test + "    " + _collision.gameObject.layer + "   " + m_iTargetLayerMask);
		if (((1 << _collision.gameObject.layer) & m_iTargetLayerMask) == 0)
		{
			//Debug.Log("_collision.gameObject.layer != m_iTargetLayerMask : " + _collision.gameObject.layer);
			return;
		}

		if (_collision.gameObject.layer != m_iTargetLayerMask)
		{
			
		}

		//m_OnCollision(_collision.collider, _collision.contacts[0].point);
		if(m_OnCollision == null)
		{
			return;			
		}

		m_OnCollision(_collision.collider);
	}

	private void Colliding(Collider _collider)
	{
		// 이미 충돌 처리가 된 콜라이더일 경우 무시한다
		if (m_ListIgnoredColliders.Contains(_collider))
		{
			//Debug.Log("m_ListIgnoredColliders.Contains(_collider) : " + _collider.name);
			return;
		}

		int Test = ((1 << _collider.gameObject.layer) & m_iTargetLayerMask);
		//Debug.Log("~~~~~~~~~~~~ : " + Test + "    " + _collider.gameObject.layer + "   " + m_iTargetLayerMask);
		if (((1 << _collider.gameObject.layer) & m_iTargetLayerMask) == 0)
		{
			//Debug.Log("__collider.gameObject.layer != m_iTargetLayerMask : " + _collider.gameObject.layer);
			return;
		}
		
		if(m_OnTrigger == null)
		{
			return;
		}

		m_OnTrigger(_collider);
		//Debug.Log("OnTrigger!!!!!!! : " + _collider.name + "   " + name);
	}
}
