using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csSelfDeactivate : MonoBehaviour {

	public Transform m_Parent = null;
	public float m_fDeactivateTime = 4.0f;

	private void OnEnable()
	{
		Invoke("EffectDeactivate", m_fDeactivateTime);
	}

	public void SetParent(Transform _parent)
	{
		m_Parent = _parent;
	}

	private void EffectDeactivate()
	{
		Utility.Activate(gameObject, false);

		if(m_Parent != null)
		{
			transform.parent = m_Parent;
		}		
	}
}
