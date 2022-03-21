using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class csPathRequestManager : MonoBehaviour
{
	private Queue<PathRequest> m_QueuePathRequest = new Queue<PathRequest>();
	private PathRequest m_CurrentPathRequest;

	public static csPathRequestManager Instance;
	private csPathFinding m_PathFinding;

	private bool m_bIsProcessingPath;

	private void Awake()
	{
		Instance = this;
		m_PathFinding = GetComponent<csPathFinding>();
	}

	public static void RequestPath(csAIPathFinding _pathFinding, Vector3 _pathStart, Vector3 _pathEnd, csNode _exceptionNode, Action<List<csNode>, Vector3[], bool> _success, Func<csNode, bool> _success_additional = null)
	{
		//Debug.LogError("RequestPath");
		PathRequest NewRequest = new PathRequest(_pathFinding, _pathStart, _pathEnd, _exceptionNode, _success, _success_additional);
		Instance.m_QueuePathRequest.Enqueue(NewRequest);
		Instance.TryProcessNext();
	}

	private void TryProcessNext()
	{
		if(!m_bIsProcessingPath && m_QueuePathRequest.Count > 0)
		{
			//Debug.Log("!m_bIsProcessingPath && m_QueuePathRequest.Count > 0");
			m_CurrentPathRequest = m_QueuePathRequest.Dequeue();

			m_bIsProcessingPath = true;

			m_CurrentPathRequest.PathFinding.StartFindPath(m_CurrentPathRequest.PathStart, m_CurrentPathRequest.PathEnd, m_CurrentPathRequest.ExceptionNode);
		}
	}

	public bool AdditionalSuccessCallback(csNode _node)
	{
		if (m_CurrentPathRequest.SuccessCallback_Additional != null)
		{
			return m_CurrentPathRequest.SuccessCallback_Additional(_node);
		}

		return false;
	}

	public void FinishedProcessingPath(List<csNode> _listNodes, Vector3[] _path, bool _success)
	{
		m_CurrentPathRequest.SuccessCallback(_listNodes, _path, _success);
		m_bIsProcessingPath = false;
		TryProcessNext();
	}
}
