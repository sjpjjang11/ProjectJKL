using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csActionRangeRenderer : MonoBehaviour {

	private IEnumerator m_CoAttackRangeRender = null;
	private IEnumerator m_CoEscapeRangeRender = null;

	public LineRenderer[] m_LineRenderer = null;

	protected Transform m_Transform = null;
	public Transform m_RenderStartPoint = null;

	private Vector3[] m_TargetDirection = null;

	private CountNumber m_CountNumber = null;

	protected csOwner m_Owner;

	public float[] m_fCurrentDistance;
	public float m_fRenderRotationSpeed = 30.0f;
	public float m_fRenderTime = 0.05f;
	
	[SerializeField]
	private int m_iCurrentIndex = 0;

	protected csEventHandler_Hero m_EventHandler_Hero = null;
	public csEventHandler_Hero EventHandler_Hero
	{
		get
		{
			if (m_EventHandler_Hero == null)
			{
				m_EventHandler_Hero = csBattleManager.Instance.m_EventHandler_Hero;
			}

			return m_EventHandler_Hero;
		}
	}

	private void Awake()
	{
		m_Transform = GetComponent<Transform>();
		m_CountNumber = new CountNumber();
		m_fCurrentDistance = new float[m_LineRenderer.Length];
	}

	public void Settings(csOwner _owner, int _actionCount)
	{
		m_Owner = _owner;

		m_TargetDirection = new Vector3[_actionCount];
	}

	public void SetTargetDirection()
	{
		//Debug.Log("m_TargetDirection : " + m_TargetDirection.Length + "   " + m_iCurrentIndex);
		m_TargetDirection[m_iCurrentIndex] = m_RenderStartPoint.forward;
		EventHandler_Hero.RenderDirection.Set(m_TargetDirection);
	}

	public void SetCurrentIndex(int _index)
	{
		m_iCurrentIndex = _index;
	}

	public void RenderStart(int _index)
	{
		//Debug.Log("RenderStart : " + _index);
		m_iCurrentIndex = _index;

		m_Transform.eulerAngles = m_Owner.Transform.eulerAngles;

		m_CoAttackRangeRender = CoAttackRangeRender();
		StartCoroutine(m_CoAttackRangeRender);
	}

	public void RenderStart_Escape(int _index)
	{
		m_iCurrentIndex = _index;

		m_Transform.eulerAngles = Vector3.zero;

		m_CoEscapeRangeRender = CoEscapeRangeRender();
		StartCoroutine(m_CoEscapeRangeRender);
	}

	public void RenderStop()
	{
		Initialize();

		//SetTargetDirection();

		if (m_CoAttackRangeRender != null)
		{
			StopCoroutine(m_CoAttackRangeRender);
			m_CoAttackRangeRender = null;
		}

		//m_iCurrentIndex = -1;
	}

	public void RenderStop_Escape()
	{
		Initialize_Escape();

		if(m_CoEscapeRangeRender != null)
		{
			StopCoroutine(m_CoEscapeRangeRender);
			m_CoEscapeRangeRender = null;
		}
	}

	public void SetRangeRender(Vector3 _direction, float _targetDistance)
	{
		LineRenderer LineRenderer = m_LineRenderer[m_iCurrentIndex];
		LineRenderer.SetPosition(0, m_RenderStartPoint.localPosition);
		//Debug.Log("@@@@@@@@@@@@@@ : " + m_iCurrentIndex);
		m_CountNumber.Count(m_iCurrentIndex, m_fCurrentDistance[m_iCurrentIndex], _targetDistance, m_fRenderTime, (value) =>
		{
			m_fCurrentDistance[m_iCurrentIndex] = value;
			LineRenderer.SetPosition(1, new Vector3(m_RenderStartPoint.localPosition.x, m_RenderStartPoint.localPosition.y - 0.1f, m_fCurrentDistance[m_iCurrentIndex]));
		});
	}

	public void Initialize(Action _callback = null)
	{	
		LineRenderer LineRenderer = m_LineRenderer[m_iCurrentIndex];
		Vector3 CurrentPosition = LineRenderer.GetPosition(1);
		Vector3 TargetPosition = new Vector3(m_RenderStartPoint.localPosition.x, m_RenderStartPoint.localPosition.y, 0.0f);
		//Debug.Log("############ : " + m_iCurrentIndex);
		m_CountNumber.ForceStop(m_iCurrentIndex);

		m_fCurrentDistance[m_iCurrentIndex] = 0.0f;

		LineRenderer.SetPosition(0, m_RenderStartPoint.localPosition);
		LineRenderer.SetPosition(1, TargetPosition);
	}

	public void Initialize_Escape()
	{
		LineRenderer LineRenderer = m_LineRenderer[m_iCurrentIndex];
		LineRenderer.positionCount = 0;
	}

	#region Coroutine

	protected IEnumerator CoAttackRangeRender()
	{
		//Debug.Log("~~~~~~~~~~~~~~~~~");
		bool IsRendering = false;

		while (true)
		{
			yield return null;

			Vector3 ActionDirection = EventHandler_Hero.ActionDirection.Get();

			m_Transform.position = m_Owner.Transform.position;
			//Debug.LogError("$$$$$$$$$$$$");
			if (ActionDirection == Vector3.zero)
			{
				//Debug.LogError("Zero : " + IsRendering);
				if (!IsRendering)
				{
					m_Transform.rotation = m_Owner.Transform.rotation;
				}
									
				if(IsRendering)
				{
					/*Initialize(() =>
					{
						IsRendering = false;
					});*/

					IsRendering = false;
					//Debug.Log("!!!!!!!!!!!!!!!!! : " + IsRendering + "  " + Time.time);
					Initialize();
				}
				
				//Debug.Log("!!!!!!!!!!!!! : " + ActionDirection);
			}
			else
			{
				//Debug.LogError("NOTZero : " + IsRendering);
				Quaternion Rotation = Quaternion.LookRotation(ActionDirection.normalized);
				m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, Rotation, m_fRenderRotationSpeed * Time.deltaTime);

				if(!IsRendering)
				{
					//Debug.Log("$$$$$$$$$$$$$$$$$ : " + IsRendering + "  " + Time.time);
					IsRendering = true;
					//Debug.Log("$$$$$$$$$$$$$$$$$ : " + IsRendering + "  " + Time.time);
					//Debug.Log("VVVVVVVVV : " + m_Owner.HeroInfo.m_ActionInfo[m_iCurrentIndex].Range + "    " + m_Owner.HeroInfo);
					SetRangeRender(m_RenderStartPoint.forward, m_Owner.Info_Bugs.m_BugsAction[m_iCurrentIndex].Range);
				}

				//Debug.Log("@@@@@@@@@@@@ : " + ActionDirection);
			}
		}
	}
	public Vector3 m_Velocity = Vector3.zero;
	public float m_fHeight = 10.0f;
	protected IEnumerator CoEscapeRangeRender()
	{
		LineRenderer LineRenderer = m_LineRenderer[m_iCurrentIndex];

		List<Vector3> ListPoint = new List<Vector3>();

		Vector3 ActionDirection = Vector3.zero;
		Vector3 ActionDirection_Original = Vector3.zero;
		Vector3 StartPoint = Vector3.zero;
		Vector3 TargetPoint = Vector3.zero;
		Vector3 Velocity = Vector3.zero;

		bool IsRendering = false;

		while (true)
		{
			yield return null;

			ActionDirection = EventHandler_Hero.ActionDirection.Get();

			if(ActionDirection != Vector3.zero)
			{
				IsRendering = true;

				ActionDirection_Original = EventHandler_Hero.ActionDirection_Original.Get();
				m_Transform.position = m_Owner.Transform.position;

				StartPoint = m_RenderStartPoint.localPosition;

				TargetPoint = ActionDirection_Original * m_Owner.Info_Bugs.m_BugsAction[m_iCurrentIndex].Range;

				Velocity = GetVelocity(StartPoint, TargetPoint, 45.0f);
				m_Velocity = Velocity;
				EventHandler_Hero.EscapeVelocity.Set(Velocity);

				ListPoint.Clear();

				for (int i = 0; i < 500; i++)
				{
					Velocity.y += Physics.gravity.y * 0.02f;
					StartPoint += Velocity * 0.02f;

					ListPoint.Add(StartPoint);

					if (StartPoint.y <= TargetPoint.y)
					{
						break;
					}
				}

				LineRenderer.positionCount = ListPoint.Count;
				LineRenderer.SetPositions(ListPoint.ToArray());
			}	
			else
			{
				if (IsRendering)
				{
					IsRendering = false;

					Initialize_Escape();
				}

				Velocity = m_Transform.up * m_fHeight;
				m_Velocity = Velocity;
				EventHandler_Hero.EscapeVelocity.Set(Velocity);
			}
		}
	}

	#endregion

	public Vector3 GetVelocity(Vector3 _currentPos, Vector3 _targetPos, float _initialAngle)
	{
		float Gravity = Physics.gravity.magnitude;
		float Angle = _initialAngle * Mathf.Deg2Rad;

		Vector3 PlanarTarget = new Vector3(_targetPos.x, 0, _targetPos.z);
		Vector3 PlanarPosition = new Vector3(_currentPos.x, 0, _currentPos.z);

		float Distance = Vector3.Distance(PlanarTarget, PlanarPosition);
		float YOffset = _currentPos.y - _targetPos.y;

		float InitialVelocity = (1 / Mathf.Cos(Angle)) * Mathf.Sqrt((0.5f * Gravity * Mathf.Pow(Distance, 2)) / (Distance * Mathf.Tan(Angle) + YOffset));
		Vector3 Velocity = new Vector3(0f, InitialVelocity * Mathf.Sin(Angle), InitialVelocity * Mathf.Cos(Angle));
		float AngleBetweenObjects = Vector3.Angle(Vector3.forward, PlanarTarget - PlanarPosition) * (_targetPos.x > _currentPos.x ? 1 : -1);
		Vector3 Result = Quaternion.AngleAxis(AngleBetweenObjects, Vector3.up) * Velocity;

		return Result;
	}
}
