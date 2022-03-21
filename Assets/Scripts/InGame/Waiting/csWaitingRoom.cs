using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class csWaitingRoom : MonoBehaviour
{
	private Action m_AnimationEndCallback = null;

	public Transform m_SpawnParentTr = null;
	public Transform[] m_SpawnPoint = null;

	private Animator m_Animator = null;

	private Vector3[] m_WaitingSpawnPos;

	public Vector3 m_MinSpawnPos = Vector3.zero;
	public Vector3 m_MaxSpawnPos = Vector3.zero;

	public float m_fWaitingTime = 0.0f;
	public float m_fSpawnDelay = 0.0f;

	private void Awake()
	{
		m_Animator = GetComponent<Animator>();
		//SetWaitingSpawnPos();
	}

	public void Spawn(csOwner _user, int _index = 0)
	{
		_user.Transform.SetParent(m_SpawnParentTr);

		_user.Transform.localPosition = m_SpawnPoint[_index].localPosition;
	}	

	public void PlayAnimation(Action _callback = null)
	{
		Utility.Activate(m_Animator, true);

		if(_callback != null)
		{
			m_AnimationEndCallback = _callback;
		}
	}

	private void SetWaitingSpawnPos()
	{
		m_WaitingSpawnPos = new Vector3[3];

		for (int i = 0; i < m_WaitingSpawnPos.Length; i++)
		{
			while (true)
			{
				float x = UnityEngine.Random.Range(m_MinSpawnPos.x, m_MaxSpawnPos.x);

				float y = UnityEngine.Random.Range(m_MinSpawnPos.y, m_MaxSpawnPos.y);

				float z = UnityEngine.Random.Range(m_MinSpawnPos.z, m_MaxSpawnPos.z);

				m_WaitingSpawnPos[i] = new Vector3(x, y, z);

				if (!IsOverlapSpawnPos(m_WaitingSpawnPos[i], i))
				{
					break;
				}
			}
		}
	}

	private bool IsOverlapSpawnPos(Vector3 _pos, int _count)
	{
		for (int i = 0; i < _count; i++)
		{
			Debug.Log("OverlapSpawnPos : " + Vector3.Distance(m_WaitingSpawnPos[i], _pos));
			if (Vector3.Distance(m_WaitingSpawnPos[i], _pos) < 10.0f)
			{
				return true;
			}
		}

		return false;
	}

	private void AnimationEndEvent()
	{
		if(m_AnimationEndCallback != null)
		{
			m_AnimationEndCallback();

			m_AnimationEndCallback = null;
		}
	}
}
