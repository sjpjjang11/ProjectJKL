using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csMove : MonoBehaviour {

	protected IEnumerator m_CoMoveHandler = null;
	protected Action<Vector3> m_WalkDirectionCallback = null;

	protected delegate void MoveHandler();
	protected MoveHandler m_MoveHandler;

	protected csOwner m_Owner = null;

	protected CharacterController m_CharacterController = null;

	protected Transform m_Transform = null;
	public Transform Transform
	{
		get
		{
			if (m_Transform == null)
			{
				m_Transform = transform;
			}

			return m_Transform;
		}
	}

	[SerializeField]
	protected Vector3 m_MoveDirection = Vector3.zero;
	public Vector3 m_Velocity = Vector3.zero;

	protected float m_fWalkValue = 0.5f;

	protected float m_fDefaultMoveSpeed;
	public float m_fGravity = 5.0f;                     // 중력값 /*JsonData*/

	protected bool m_bCurrentIsRun = false;
	protected bool m_bPrevIsRun = false;

	public float m_fRotationSpeed = 0.0f;
	public float m_fMoveSpeed = 0.0f;                          // 최종적으로 쓰일 이동 속도값
	public float MoveSpeed
	{
		get
		{
			return m_fMoveSpeed;
		}
		set
		{
			if (value != m_fMoveSpeed)
			{
				/*float DiffValue = value - m_fDefaultMoveSpeed;
				float Val = Mathf.Sign(DiffValue) == 1 ? 0.035f : 0.2f;
                float AnimSpeed = 1.0f + (DiffValue * Val);
				//Debug.Log(m_fDefaultMoveSpeed + "  " + value + "   " + DiffValue + "   " + (DiffValue * 0.035f) + "  " + AnimSpeed);
				MyHandler.MoveAnimationSpeed.Set(AnimSpeed);

                m_fMoveSpeed = value;*/
			}

			m_fMoveSpeed = value;
		}
	}

	protected virtual void Awake()
	{
		m_CharacterController = GetComponent<CharacterController>();
		m_fDefaultMoveSpeed = m_fMoveSpeed;
	}

	public virtual void RegisterWalkDirectionCallback(Action<Vector3> _callback)
	{
		m_WalkDirectionCallback = _callback;
	}

	public virtual void Settings(csOwner _owner)
	{
		m_Owner = _owner;

		switch (m_Owner.OwnerType)
		{
			case csBattleManager.HeroType:

				if (csBattleManager.Instance.IsHero(m_Owner.Bugs))
				{
					m_MoveHandler = Local;
				}
				else
				{
					m_MoveHandler = Remote;
				}
		
				break;

			case csBattleManager.MonsterType:

				m_MoveHandler = Monster;

				break;
		}

		csBattleManager.Instance.StartBattle += StartBattle;

		if (Utility.IsActive(gameObject))
		{
			m_CoMoveHandler = CoMoveHandler();
			StartCoroutine(m_CoMoveHandler);
		}
	}

	public virtual void Release()
	{
		if(m_CoMoveHandler != null)
		{
			StopCoroutine(m_CoMoveHandler);
		}
		Debug.Log("Release");
		m_MoveHandler = null;
	}

	protected virtual void StartBattle()
	{
		csBattleManager.Instance.StartBattle -= StartBattle;

		/*if (Utility.IsActive(gameObject))
		{			
			m_CoMoveHandler = CoMoveHandler();
			StartCoroutine(m_CoMoveHandler);
		}		*/
	}

	/*protected virtual void Update()
	{		
		Test();
	}
	Vector3 PrevPosition = Vector3.zero;
	float TestTime = 1.0f;
	private void Test()
	{
		TestTime -= Time.deltaTime;

		if(TestTime <= 0.0f)
		{
			float Distance = Vector3.Distance(Transform.position, PrevPosition);
			Debug.Log("###### : " + Distance);
			PrevPosition = Transform.position;
			TestTime = 1.0f;
		}
	}*/

	private void Local()
	{
		SetGravity();

		MoveHandler_Local();
	}

	private void Remote()
	{
		SetGravity();

		MoveHandler_Remote();
	}

	private void Monster()
	{
		SetGravity();
	}

	private void SetGravity()
	{
		if(m_CharacterController.isGrounded && m_Velocity.y < 0.0f)
		{
			//Debug.Log("Graound");
			m_Velocity.y = 0.0f;
		}

		m_Velocity.y -= m_fGravity * Time.deltaTime;

		if(Utility.IsActive(m_CharacterController))
		{
			m_CharacterController.Move(m_Velocity * Time.deltaTime);
		}	
	}

	protected virtual void MoveHandler_Local()
	{
		
	}

	protected virtual void MoveHandler_Remote()
	{

	}

	public virtual void LocalMoveStart()
	{

	}

	public virtual void LocalMoveStop()
	{

	}

	public virtual void LocalRotateStart()
	{

	}

	public virtual void LocalRotateStop()
	{

	}

	public virtual void RemoteMoveStart()
	{

	}

	public virtual void RemoteMoveStop()
	{

	}

	public virtual void RemoteInterpolation()
	{

	}

	public virtual void RemoteChangeAim(Vector2 _moveDirection)
	{

	}

	/*2018.09.02
    * hkh
    * 방향값 한계 설정
    */
	protected virtual void SetForwardDirectionLimit(float _maxValue)
	{
		if (Mathf.Sign(m_MoveDirection.x) == 1)
		{
			m_MoveDirection.x = Mathf.Clamp(m_MoveDirection.x, m_MoveDirection.x, _maxValue);
		}
		else
		{
			m_MoveDirection.x = Mathf.Clamp(m_MoveDirection.x, -_maxValue, m_MoveDirection.x);
		}

		if (Mathf.Sign(m_MoveDirection.z) == 1)
		{
			m_MoveDirection.z = Mathf.Clamp(m_MoveDirection.z, m_MoveDirection.z, _maxValue);
		}
		else
		{
			m_MoveDirection.z = Mathf.Clamp(m_MoveDirection.z, -_maxValue, m_MoveDirection.z);
		}
	}

	#region Coroutine

	protected IEnumerator CoMoveHandler()
	{
		while(true)
		{
			yield return YieldCache.WaitForFixedUpdate;

			m_MoveHandler?.Invoke();
		}
	}

	#endregion
}
