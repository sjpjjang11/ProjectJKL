using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class TestAgent : MonoBehaviour
{
	private IEnumerator<float> m_CoFollowPath = null;
	private Action m_AgentCompleteCallback = null;
	private Action m_PathFindFailureCallback = null;

	public Transform m_Transform = null;
	public Transform m_Target = null;

	public csGrid m_Grid = null;

	private csAIPathFinding m_PathFinding = null;

	protected csRotationToTarget m_RotationToTarget = null;

	protected CharacterController m_CharacterController = null;
	public CharacterController CharacterController
	{
		get
		{
			if (m_CharacterController == null)
			{
				m_CharacterController = gameObject.GetComponent<CharacterController>();
			}

			return m_CharacterController;
		}
	}

	public csNode m_Node_WayPoint_Prev = null;
	public csNode m_Node_WayPoint_Current = null;
	public csNode m_Node_WayPoint_Next = null;

	public Vector3[] m_Path;
	public Vector3 m_LastWayPoint = Vector3.zero;
	public Vector3 m_CurrentWayPoint = Vector3.zero;

	public float m_fSpeed = 4.0f;
	public float m_fRange = 0.0f;
	public float m_fCycle = 0.0f;

	public int m_iTargetIndex;

	public bool m_bIsAgent = false;
	public bool m_bIsPathFindSuccess = false;
	public bool m_bIsAgentComplete = false;

	protected virtual void Awake()
	{
		m_Transform = GetComponent<Transform>();

		m_PathFinding = GetComponent<csAIPathFinding>();

		m_RotationToTarget = GetComponent<csRotationToTarget>();
	}

	protected virtual void Start()
	{
		m_Grid = csPathRequestManager.Instance.GetComponent<csGrid>();
		m_Node_WayPoint_Prev = m_Node_WayPoint_Current = m_Grid.NodeFromWorldPoint(m_Transform.position);
	}

	public void StartAgent(Vector3 _target, Action _success = null, Func<csNode, bool> _success_additional = null)
	{
		if(!Utility.IsActive(gameObject))
		{
			return;
		}

		m_PathFinding.Initialize();

		csPathRequestManager.RequestPath(m_PathFinding, m_Transform.position, _target, null, OnPathFound, _success_additional);

		//Debug.LogError("Start Agent : " + _target + "  " + m_Owner.CurrentState.m_BotState.ToString());
		if (_success != null)
		{
			//Debug.Log("m_AgentCompleteCallback");
			m_AgentCompleteCallback = _success;
		}

		m_bIsAgent = true;
		m_bIsAgentComplete = false;
	}

	public void StopAgent()
	{
		if (m_CoFollowPath != null)
		{ 
			StopCoroutine(m_CoFollowPath);
			m_CoFollowPath = null;
		}
	
		if (m_AgentCompleteCallback != null)
		{
			m_AgentCompleteCallback = null;
		}

		//m_Grid.ResetGrid(m_ListNodes);

		m_PathFinding.Initialize();

		//Debug.LogError("StopAgent : " + m_Owner.CurrentState.m_BotState.ToString());
		m_Path = new Vector3[0];

		m_bIsAgent = false;
	}

	public bool IsCurrentPositionLastWayPoint()
	{
		return Utility.Distance(m_Transform.position, m_LastWayPoint) == 0 && m_bIsPathFindSuccess && m_bIsAgentComplete;
	}

	private void OnPathFound(List<csNode> _listNodes, Vector3[] _newPath, bool _success)
	{
		if (!m_bIsAgent)
		{
			//Debug.Log("!m_bIsAgent");
			return;
		}

		m_bIsPathFindSuccess = _success;
		//Debug.Log("Path Find : " + _success);
		if (_success)
		{
			if (_newPath.Length == 0)
			{
				_newPath = new Vector3[1];
				_newPath[0] = m_LastWayPoint;
			}

			m_iTargetIndex = 0;
			m_Path = _newPath;
			m_LastWayPoint = m_Path.Length > 1 ? m_LastWayPoint = m_Path[m_Path.Length - 1] : m_LastWayPoint = m_Path[0];

			ChangeCurrentWayPoint(0);

			if(m_CoFollowPath == null)
			{
				m_CoFollowPath = CoFollowPath();
				Timing.RunCoroutine(m_CoFollowPath);
			}
			
			//StartCoroutine(m_CoFollowPath);
			//Debug.Log("m_bIsFollowPath = true");			
		}
		else
		{
			m_PathFindFailureCallback?.Invoke();			
		}
	}

	private void ChangeCurrentWayPoint(int _index)
	{
		m_CurrentWayPoint = m_Path[_index];

		Vector3 RelativePositionFromMeToWayPoint = m_RelativePositionFromMeToWayPoint = Utility.RelativePosition(m_Transform.position, m_CurrentWayPoint);

		Quaternion Rotation = Quaternion.LookRotation(RelativePositionFromMeToWayPoint.normalized);

		m_Transform.eulerAngles = new Vector3(0.0f, Rotation.eulerAngles.y, 0.0f);
	}

	public Vector3 m_RelativePositionFromMeToWayPoint;
	public float m_fDistanceFromMeToWayPoint;

	private void ChangeCurrentNodePosition()
	{
		csNode CurrentNode = m_Grid.NodeFromWorldPoint(m_Transform.position);

		if(m_Node_WayPoint_Current == CurrentNode || CurrentNode.NodeType == eNodeType.Obstacle)
		{
			return;
		}

		m_Node_WayPoint_Current = CurrentNode;

		if (m_Node_WayPoint_Current != m_Node_WayPoint_Prev)
		{
			//Debug.Log("RESET : " + name);
			m_Node_WayPoint_Prev.ResetType(gameObject.GetInstanceID());
			m_Node_WayPoint_Prev = m_Node_WayPoint_Current;
		}

		m_Node_WayPoint_Current.SetType(gameObject.GetInstanceID(), eNodeType.Obstacle);
	}

	private IEnumerator<float> CoFollowPath()
	{
		Vector3 PrevPosition = Vector3.zero;

		float Current = 0.0f;

		while (true)
		{
			yield return Timing.WaitForOneFrame;
			//Debug.Log("Agent : " + name);

			Current += Time.deltaTime;

			if (/*Distance <= m_fRange && */Current >= m_fCycle)
			{
				float Difference = Utility.Distance(PrevPosition, m_Transform.position);
				PrevPosition = m_Transform.position;

				if(Difference < Utility.GetPercentOfTotalValue(m_fSpeed, 50.0f) * m_fCycle)
				{
					//m_Grid.ResetGrid(m_ListNodes);

					Debug.Log(name + "  " + m_PathFinding.m_EndNode.m_WorldPosition);
					//Debug.Log(Difference);
					csPathRequestManager.RequestPath(m_PathFinding, m_Transform.position, m_Target.position, null, OnPathFound);
				}

				Current = 0.0f;			
			}

			m_fDistanceFromMeToWayPoint = Utility.Distance(m_Transform.position, m_CurrentWayPoint);

			if (m_iTargetIndex == m_Path.Length)
			{
				m_AgentCompleteCallback?.Invoke();

				m_bIsAgentComplete = true;

				StopAgent();

				break;
			}
			else
			{
				if (m_fDistanceFromMeToWayPoint == 0.0f)
				{
					m_iTargetIndex++;

					if (m_iTargetIndex < m_Path.Length)
					{
						ChangeCurrentWayPoint(m_iTargetIndex);
					}
				}

				//Vector3 MovePosition = Vector3.MoveTowards(m_Transform.position, m_CurrentWayPoint, m_fSpeed * Time.deltaTime);
				//m_Transform.position = MovePosition;
				Vector3 RelativePosition = Utility.RelativePosition(m_Transform.position, m_CurrentWayPoint);
				Vector3 Direction = RelativePosition.normalized * m_fSpeed * Time.deltaTime;
				if (Direction.sqrMagnitude < RelativePosition.sqrMagnitude)
				{
					//Debug.Log("1 : " + Direction.sqrMagnitude + "  " + RelativePosition.sqrMagnitude);
					CharacterController.Move(Direction);
				}
				else
				{
					//Debug.Log("2 : " + Direction.sqrMagnitude + "  " + RelativePosition.sqrMagnitude);
					CharacterController.Move(RelativePosition);
					//m_Transform.position = Vector3.Lerp(m_Transform.position, m_CurrentWayPoint, 1.0f);
				}
			}
		}
	}

	private void OnDrawGizmos()
	{	
		if (m_Path != null)
		{
			for (int i = m_iTargetIndex; i < m_Path.Length; i++)
			{
				Gizmos.color = Color.black;
				float NodeSize = m_Grid.m_fNodeDiameter - 0.1f;
				Gizmos.DrawCube(m_Path[i], new Vector3(m_Grid.m_fNodeRadius, m_Grid.m_fNodeRadius, m_Grid.m_fNodeRadius));

				if (i == m_iTargetIndex)
				{
					Gizmos.DrawLine(m_Transform.position, m_Path[i]);
				}
				else
				{
					Gizmos.DrawLine(m_Path[i - 1], m_Path[i]);
				}

				
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (m_Path != null)
		{
			Gizmos.color = Color.yellow;

			Gizmos.DrawWireCube(m_LastWayPoint, Vector3.one * 0.5f);
		}			
	}
}
