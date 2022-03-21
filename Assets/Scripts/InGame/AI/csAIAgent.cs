using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

/// <summary>
/// AI 이동 관련 처리 클래스
/// </summary>
public class csAIAgent : MonoBehaviour {

	private IEnumerator<float> m_CoFollowPath = null;   // 기존 길찾기 기능. 대체 예정
	private Action m_AgentCompleteCallback = null;	// 이동 완료 콜백
	private Action m_PathFindFailureCallback = null;	// 길찾기 실패 콜백

	private Transform m_Transform = null;

	protected csOwner_Monster m_Owner = null;

	protected csGrid m_Grid = null; // 기존 길찾기 기능. 대체 예정

	private csAIPathFinding m_PathFinding = null;   // 기존 길찾기 기능. 대체 예정

	protected csRotationToTarget m_RotationToTarget = null;

	private csNode m_Node_WayPoint_Prev = null; // 기존 길찾기 기능. 대체 예정
	private csNode m_Node_WayPoint_Current = null;  // 기존 길찾기 기능. 대체 예정
	private csNode m_Node_WayPoint_Next = null; // 기존 길찾기 기능. 대체 예정

	public Vector3[] m_Path = null;
	public Vector3 m_Target = Vector3.zero;
	public Vector3 m_LastWayPoint = Vector3.zero;
	protected Vector3 m_CurrentWayPoint = Vector3.zero;

	private eBugsStateType m_PrevMoveType;
	private eBugsStateType m_CurMoveType;

	public float m_fSpeed = 4.0f;
	public float m_fCycle = 0.0f;

	public int m_iTargetIndex;

	public bool m_bIsAgent = false;
	public bool m_bIsPathFindSuccess = false;
	public bool m_bIsAgentComplete = false;

	protected virtual void Awake()
	{
		m_Transform = GetComponent<Transform>();
		m_Grid = csPathRequestManager.Instance.GetComponent<csGrid>();
		m_PathFinding = GetComponent<csAIPathFinding>();
		m_RotationToTarget = GetComponent<csRotationToTarget>();

		m_Node_WayPoint_Prev = m_Node_WayPoint_Current = m_Grid.NodeFromWorldPoint(m_Transform.position);

		m_PrevMoveType = m_CurMoveType = eBugsStateType.Walk;
	}

	public void Settings(csOwner_Monster _owner)
	{
		m_Owner = _owner;
	}

	// 경로 찾기 실패 콜백 등록
	public void RegisterPathFindFailureCallback(Action _callback)
	{
		m_PathFindFailureCallback = _callback;
	}

	public void UnregisterPathFindFailureCallback()
	{
		m_PathFindFailureCallback = null;
	}

	/// <summary>
	/// AI 이동 시작
	/// </summary>
	/// <param name="_target">목표 위치</param>
	/// <param name="_moveType">이동 종류(걷기, 달리기)</param>
	/// <param name="_success">이동 완료 콜백</param>
	public void StartAgent(Vector3 _target, eBugsStateType _moveType, Action _complete = null)
	{
		m_Target = _target;

		// 기존 길찾기 기능. 대체 예정
		csPathRequestManager.RequestPath(m_PathFinding, m_Transform.position, _target, null, OnPathFound);

		if (_complete != null)
		{
			m_AgentCompleteCallback = _complete;
		}

		m_CurMoveType = _moveType;

		m_bIsAgent = true;
		m_bIsAgentComplete = false;
	}

	/// <summary>
	/// AI 이동 정지
	/// </summary>
	public void StopAgent()
	{
		// 기존 길찾기 기능. 대체 예정
		if (m_CoFollowPath != null)
		{
			StopCoroutine(m_CoFollowPath);
			m_CoFollowPath = null;
		}
		//

		if(m_AgentCompleteCallback != null)
		{
			m_AgentCompleteCallback = null;
		}

		// 기존 길찾기 기능. 대체 예정
		m_PathFinding.m_EndNode.ResetType(gameObject.GetInstanceID());
		csNode EndNode = m_Grid.NodeFromWorldPoint(m_Owner.Transform.position);
		EndNode.SetType(gameObject.GetInstanceID(), eNodeType.End);
		m_PathFinding.m_EndNode = EndNode;
		//

		m_Path = new Vector3[0];
		m_Owner.StateStop(m_CurMoveType);

		m_bIsAgent = false;
	}

	public bool IsCurrentPositionLastWayPoint()
	{
		return Utility.Distance(m_Transform.position, m_LastWayPoint) == 0.0f && m_bIsPathFindSuccess && m_bIsAgentComplete;
	}

	/// <summary>
	/// 기존 길찾기 기능. 대체 예정
	/// </summary>
	/// <param name="_listNodes"></param>
	/// <param name="_newPath"></param>
	/// <param name="_success"></param>
	private void OnPathFound(List<csNode> _listNodes, Vector3[] _newPath, bool _success)
	{		
		if(!m_bIsAgent)
		{
			//Debug.Log("!m_bIsAgent");
			return;
		}

		m_bIsPathFindSuccess = _success;
		//Debug.Log("Path Find : " + _success);
		if (_success)
		{
			if(_newPath.Length == 0)
			{
				_newPath = new Vector3[1];
				_newPath[0] = m_LastWayPoint;				
			}
			
			m_iTargetIndex = 0;
			m_Path = _newPath;

			m_LastWayPoint = m_Path.Length > 1 ? m_LastWayPoint = m_Path[m_Path.Length - 1] : m_LastWayPoint = m_Path[0];

			if(m_Owner != null)
			{
				m_Owner.m_EventHandler_Monster.TargetWayPoint.Set(m_LastWayPoint);
			}

			ChangeCurrentWayPoint(0);

			if (m_CoFollowPath == null)
			{
				m_CoFollowPath = CoFollowPath();
				StartCoroutine(m_CoFollowPath);
			}
			//Debug.Log("m_bIsFollowPath = true : " + m_Owner.CurrentState.m_BotState.ToString());			
		}
		else
		{
			m_PathFindFailureCallback?.Invoke();
		}
	}

	/// <summary>
	/// 기존 길찾기 기능. 대체 예정
	/// </summary>
	/// <param name="_index"></param>
	private void ChangeCurrentWayPoint(int _index)
	{
		m_CurrentWayPoint = m_Path[_index];

		Vector3 RelativePositionFromMeToWayPoint = m_RelativePositionFromMeToWayPoint = Utility.RelativePosition(m_Owner.Transform.position, m_CurrentWayPoint);

		m_Owner.Bugs.RotateToTarget(m_Owner.Transform, RelativePositionFromMeToWayPoint);
	}
	public Vector3 m_RelativePositionFromMeToWayPoint;
	public float m_fDistanceFromMeToWayPoint;

	/// <summary>
	/// 기존 길찾기 기능. 대체 예정
	/// </summary>
	/// <returns></returns>
	private IEnumerator<float> CoFollowPath()
	{
		Vector3 PrevPosition = Vector3.zero;

		float Current = 0.0f;

		while (true)
		{
			yield return Timing.WaitForOneFrame;

			//Debug.Log("Agent");

			Current += Time.deltaTime;

			if (/*Distance <= m_fRange && */Current >= m_fCycle)
			{
				float Difference = Utility.Distance(PrevPosition, m_Transform.position);
				PrevPosition = m_Transform.position;

				if (Difference < Utility.GetPercentOfTotalValue(m_fSpeed, 50.0f) * m_fCycle)
				{
					//m_Grid.ResetGrid(m_ListNodes);

					Debug.Log(name + "  " + m_PathFinding.m_EndNode.m_WorldPosition);
					//Debug.Log(Difference);
					csPathRequestManager.RequestPath(m_PathFinding, m_Transform.position, m_Target, null, OnPathFound);
				}

				Current = 0.0f;
			}

			m_fDistanceFromMeToWayPoint = Utility.Distance(m_Owner.Transform.position, m_CurrentWayPoint);

			if (m_iTargetIndex == m_Path.Length)
			{
				//Debug.Log("!!!!!!!!!!!!!! : " + m_Transform.position + "  " + m_CurrentWayPoint);
				m_AgentCompleteCallback?.Invoke();

				m_bIsAgentComplete = true;

				m_Owner.StateStop(m_CurMoveType);

				StopAgent();

				break;
			}
			else
			{
				if (m_CurMoveType != m_PrevMoveType)
				{
					m_Owner.StateStop(m_PrevMoveType);
					m_PrevMoveType = m_CurMoveType;
				}

				if(m_Owner.m_EventHandler_Bugs.GetState(m_CurMoveType).Locked)
				{
					continue;
				}

				if(!m_Owner.IsActiveState(m_CurMoveType))
				{
					m_Owner.StateStart(m_CurMoveType);
					m_fSpeed = m_Owner.m_Move.MoveSpeed;
				}
				
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
					m_Owner.CharacterController.Move(Direction);
				}
				else
				{
					//Debug.Log("2 : " + Direction.sqrMagnitude + "  " + RelativePosition.sqrMagnitude);
					m_Owner.CharacterController.Move(RelativePosition);
					//m_Transform.position = Vector3.Lerp(m_Transform.position, m_CurrentWayPoint, 1.0f);
				}
			}
		}
	}

	/// <summary>
	/// 기존 길찾기 기능. 대체 예정
	/// </summary>
	private void OnDrawGizmos()
	{
		if (m_Path != null && m_Grid.m_bIsDisplayGridGizmos)
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
}
