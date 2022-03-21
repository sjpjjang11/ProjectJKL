using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;

public class csKnockBack : csCrowdControl
{
	public Vector3 m_KnockBackDirection;
	public Vector3 m_KnockBackTargetPosition;

	public CountNumber m_CountNumber = new CountNumber();

	public const float m_fInitPercentOfRemainingTargetDistance = 100.0f;
	public float m_fKnockBackTime;
	public float m_fPercentOfRemainingTargetDistance = 0.0f;

	private void Awake()
	{
		m_fPercentOfRemainingTargetDistance = m_fInitPercentOfRemainingTargetDistance;
	}

	public virtual void SetTargetPosition(Vector3 _direction, Vector3 _target)
	{
		//m_fKnockBackTime = m_fInitKnockBackTime;

		m_KnockBackDirection = _direction;
		m_KnockBackTargetPosition = _target;
	}

	public virtual void SetPercentOfRemainingTargetDistance(float _percentOfRemainingTargetDistance)
	{
		m_fPercentOfRemainingTargetDistance = _percentOfRemainingTargetDistance;
		//Debug.Log("+++++++++++ : " + m_fPercentOfRemainingTargetDistance);
	}

	public override void CrowdControlReady(csOwner _target)
	{		
		_target.CharacterController.Move(m_KnockBackDirection);
	}
	
	public override void StartCrowdControl(csOwner _target, Action _update = null, Action _complete = null)
	{
		m_Target = _target;

		if (_update != null)
		{
			m_UpdateCallback = _update;
		}

		if (_complete != null)
		{
			m_CompleteCallback = _complete;
		}

		float TargetDiameter = (_target.CharacterController.radius * 2.0f) + 0.2f;
		float Distance = Vector3.Distance(transform.position, m_KnockBackTargetPosition);
		
		m_KnockBackTargetPosition = m_KnockBackTargetPosition + (m_KnockBackDirection * TargetDiameter);

		float KnockBackTime = Utility.GetPercentOfTotalValue(m_fKnockBackTime, m_fPercentOfRemainingTargetDistance);
		Debug.Log("=============== : " + m_Target + "  " + KnockBackTime + "   [" + m_fKnockBackTime + " / " + m_fPercentOfRemainingTargetDistance + "]"); ;
		if (KnockBackTime <= 0.0f)
		{
			KnockBackTime = 0.001f;
		}

		m_CountNumber.Count(_target.GetInstanceID(), _target.Transform.position, m_KnockBackTargetPosition, KnockBackTime, (value) =>
		{
			m_UpdateCallback?.Invoke();

			_target.CharacterController.Move(m_KnockBackDirection);
			_target.Transform.position = value;			
		},
		() =>
		{
			if (m_CompleteCallback != null)
			{
				Debug.Log("m_CompleteCallback != null");
				m_CompleteCallback();
			}

			m_fPercentOfRemainingTargetDistance = m_fInitPercentOfRemainingTargetDistance;

			m_Target = null;
		});		
	}

	public override void StopCrowdControl(csOwner _target)
	{
		base.StopCrowdControl(_target);

		m_CountNumber.ForceStop(_target.GetInstanceID());
		Debug.Log("StopCrowdControl");
		m_Target = null;

		m_CompleteCallback?.Invoke();
	}
}
