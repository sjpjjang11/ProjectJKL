using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*2018.06.09
* hkh
* 플레이어의 이동을 담당 
*/
public class csMove_Local : csMove {

	private IEnumerator m_CoLocalMove = null;
	private IEnumerator m_CoLocalRotate = null;

	private csOwner_Hero m_Owner_Hero = null;

	private csEventHandler_Hero m_EventHandler_Hero = null;

	public override void Settings(csOwner _owner)
	{
		base.Settings(_owner);

		m_Owner_Hero = (csOwner_Hero)_owner;

		m_EventHandler_Hero = m_Owner_Hero.m_EventHandler_Hero;
	}

	public override void Release()
	{
		base.Release();

		LocalMoveStop();
		LocalRotateStop();
	}
	public float Test = 0.0f;
	protected override void MoveHandler_Local()
	{
		m_MoveDirection = m_EventHandler_Hero.MoveDirection.Get();

		bool IsInput = m_EventHandler_Hero.IsKeyMove.Get() || m_EventHandler_Hero.IsTouchMove.Get() ? true : false;

		if (m_MoveDirection != Vector3.zero)
		{
			if(!m_Owner.IsActiveState(eBugsStateType.Run) && !m_EventHandler_Hero.Run.Locked)
			{
				m_Owner.StateStart(eBugsStateType.Run);
			}

			if(!m_Owner.IsActiveState(eBugsStateType.Rotate) && !m_EventHandler_Hero.Rotate.Locked)
			{
				m_Owner.StateStart(eBugsStateType.Rotate);
			}

			Test = 0.2f;

			/*if (m_Owner.IsActiveState(eBugsStateType.LookAtTarget) && !m_Owner.IsActiveState(eBugsStateType.Walk))
			{
				//m_Owner.StateStart(eBugsStateType.Walk);
			}
			else if(!m_Owner.IsActiveState(eBugsStateType.LookAtTarget) && m_Owner.IsActiveState(eBugsStateType.Walk))
			{
				//m_Owner.StateStop(eBugsStateType.Walk);
			}*/
		}
		else
		{
			if(!IsInput || Test <= 0.0f)
			{
				if (m_Owner.IsActiveState(eBugsStateType.Run))
				{
					m_Owner.StateStop(eBugsStateType.Run);
				}

				if (m_Owner.IsActiveState(eBugsStateType.Rotate))
				{
					m_Owner.StateStop(eBugsStateType.Rotate);
				}
			}

			Test -= Time.deltaTime;
		}
		// 이동 상태 중일 때 && 입력값이 없을 때
		/*else if (m_MoveDirection == Vector3.zero && m_Owner.IsActiveState(eBugsStateType.Run))
		{
			if (IsInput)
			{
				m_Owner.StateStop(eBugsStateType.Run);
				//m_Owner.StateStop(eBugsStateType.Walk);
			}
		}
		else if (m_Owner.IsActiveState(eBugsStateType.Rotate) && m_MoveDirection == Vector3.zero)
		{
			if (IsInput)
			{
				m_Owner.StateStop(eBugsStateType.Rotate);
			}
		}*/

		//Vector3 NewMoveDirection = (Vector3.forward * m_MoveDirection.z) + (Vector3.right * m_MoveDirection.x);
		Vector3 NewMoveDirection = (Transform.forward * m_MoveDirection.z) + (Transform.right * m_MoveDirection.x);
		m_EventHandler_Hero.MoveDirection_Usable.Set(NewMoveDirection);
	}

	public override void LocalMoveStart()
	{
		if(m_CoLocalMove == null)
		{
			m_CoLocalMove = CoLocalMove();
			StartCoroutine(m_CoLocalMove);
		}		
	}

	public override void LocalMoveStop()
	{
		if (m_CoLocalMove != null)
		{
			StopCoroutine(m_CoLocalMove);
			m_CoLocalMove = null;
		}
	}

	public override void LocalRotateStart()
	{
		if (m_CoLocalRotate == null)
		{
			m_CoLocalRotate = CoLocalRotate();
			StartCoroutine(m_CoLocalRotate);
		}
	}

	public override void LocalRotateStop()
	{
		if (m_CoLocalRotate != null)
		{
			StopCoroutine(m_CoLocalRotate);
			m_CoLocalRotate = null;
		}
	}

	protected virtual void LocalMove()
	{
		Vector3 MoveDirection = m_EventHandler_Hero.MoveDirection_Usable.Get();

		m_CharacterController.Move(MoveDirection * MoveSpeed * Time.fixedDeltaTime);
	}

	protected virtual void LocalRotate()
	{
		Vector3 MoveDirection = m_EventHandler_Hero.MoveDirection_Usable.Get();

		if (MoveDirection != Vector3.zero)
		{
			Quaternion Rotation = Quaternion.LookRotation(MoveDirection.normalized);

			Transform.rotation = Quaternion.Slerp(Transform.rotation, Rotation, m_fRotationSpeed * Time.deltaTime);
		}
	}

	protected virtual void LookAtTarget()
	{
		Vector3 ActionDirection = m_EventHandler_Hero.ActionDirection.Get();

		if(ActionDirection != Vector3.zero)
		{
			Quaternion Rotation = Quaternion.LookRotation(ActionDirection.normalized);

			Transform.rotation = Quaternion.Slerp(Transform.rotation, Rotation, m_fRotationSpeed * Time.deltaTime);
		}
	}

	#region Coroutine

	private IEnumerator CoLocalMove()
	{
		Vector3 MoveDirection = Vector3.zero;
		Vector3 PrevMoveDirection = Vector3.zero;
		//float MoveAnimSpeed = 1.0f;

		while (true)
		{
			yield return YieldCache.WaitForFixedUpdate;

			MoveDirection = m_EventHandler_Hero.MoveDirection_Usable.Get();

			if(m_Owner.IsActiveState(eBugsStateType.Walk))
			{
				m_WalkDirectionCallback(m_MoveDirection);
			}

			if (MoveDirection != PrevMoveDirection)
			{
				//SendManager.MoveStart();
				PrevMoveDirection = MoveDirection;
			}

			LocalMove();
		}
	}

	private IEnumerator CoLocalRotate()
	{
		Vector3 MoveDirection = Vector3.zero;
		Vector3 PrevMoveDirection = Vector3.zero;

		while (true)
		{
			yield return YieldCache.WaitForFixedUpdate;

			MoveDirection = m_EventHandler_Hero.MoveDirection_Usable.Get();

			if (MoveDirection != PrevMoveDirection)
			{
				//SendManager.MoveStart();
				PrevMoveDirection = MoveDirection;
			}

			//LocalRotate();
		}
	}

	private IEnumerator CoLookAtTarget()
	{
		while(true)
		{
			yield return YieldCache.WaitForFixedUpdate;
			//Debug.LogError("@@@@@@@@@@");
			LookAtTarget();
		}
	}

	#endregion

	/*void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Color color;
			color = Color.white;

			DrawHelperAtCenter(Test, color, 4f);

			//color = Color.yellow;
			//DrawHelperAtCenter(m_MoveDir, color, 4f);
		}
	}

	private void DrawHelperAtCenter(
					   Vector3 direction, Color color, float scale)
	{
		Gizmos.color = color;
		Vector3 destination = transform.position + direction * scale;
		Gizmos.DrawLine(transform.position, destination);
	}*/
}
