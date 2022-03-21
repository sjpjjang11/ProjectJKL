using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;
using UnityEditor;

public class csActivity : MonoBehaviour {

	private IEnumerator m_CoMoveStart = null;

	private eDirectionType m_OrthogonalDirectionType;

	private Action m_CompleteCallback = null;

	protected csOwner m_Owner;

	public Vector3 DetectionCenter_Origin
	{
		get
		{
			return m_Owner.BugsCenter + m_Direction_Origin * 0.5f;
		}
	}

	public Vector3 DetectionCenter_Current
	{
		get
		{
			return m_Owner.BugsCenter + m_Direction_Current * 0.5f;
		}
	}

	public Vector3 DetectionCenter_Obstacle
	{
		get
		{
			return m_Owner.BugsCenter + m_Direction_Obstacle * m_fDetectionDistance_Obstacle;
		}
	}
	
	public Vector3 DetectionCenter_Gizmos
	{
		get
		{
			return m_Owner.BugsCenter + m_Owner.Transform.forward * 0.5f;
		}
	}

	public Vector3 m_Direction_Origin = Vector3.zero;
	public Vector3 m_Direction_Current = Vector3.zero;	
	public Vector3 m_Direction_Obstacle = Vector3.zero;
	public Vector3 m_HitPoint = Vector3.zero;
	public Vector3 m_Orthogonal_Left = Vector3.zero;
	public Vector3 m_Orthogonal_Right = Vector3.zero;

	private Vector3 m_OverlapCapsulePoint1 = Vector3.zero;
	private Vector3 m_OverlapCapsulePoint2 = Vector3.zero;

	public float m_fOrthogonalAngle_Left = 0.0f;
	public float m_fOrthogonalAngle_Right = 0.0f;

	public float m_fDistanceToTarget = 0.0f;
	public float m_fDetectionRadius_Current = 0.0f;
	public float m_fDetectionRadius_Obstacle = 0.5f;
	public float m_fDetectionRadius_Center = 0.6f;
	public float m_fDetectionDistance_Obstacle = 0.5f;
	public float m_fGizmosRadius = 0.1f;
	public float m_fValueToBackwardWhenColliding = 1.0f;
	private float m_fDistanceToOverlapCapsulePoints = 0.0f;

	private int m_iCollisionLayerMask;

	public bool m_bIsTest = false;
	public bool m_bIsDebug = false;
	private bool m_bIsSlip = false;

	public virtual void Settings(csOwner _owner)
	{
		m_Owner = _owner;

		BugsCollider BugsCollider = m_Owner.Info_Bugs.m_BugsCollider;
		m_fDetectionRadius_Current = BugsCollider.Radius - 0.1f;
		m_fDistanceToOverlapCapsulePoints = (BugsCollider.Height / 2) - BugsCollider.Radius;

		m_iCollisionLayerMask = LayerMask.LayerToName(m_Owner.gameObject.layer) == eCollisionLayerType.Hero.ToString() ?
			Layer.CollisionLayerMask(eCollisionLayerType.Monster, eCollisionLayerType.Map) : Layer.CollisionLayerMask(eCollisionLayerType.Hero, eCollisionLayerType.Map);
	}

	public virtual void MoveStart(Vector3 _direction, float _distance, float _moveTime, bool _isSlip, Action _updateCallback = null, Action _completeCallback = null)
	{
		m_CompleteCallback = _completeCallback;

		MoveStop();

		m_bIsSlip = _isSlip;

		m_OrthogonalDirectionType = eDirectionType.None;

		m_Direction_Origin = m_Direction_Current = _direction;
		m_Direction_Obstacle = Vector3.zero;

		Vector3 Target = m_Owner.Transform.position + _direction * _distance;

		m_CoMoveStart = CoMoveStart(Target, _moveTime, m_CompleteCallback);
		StartCoroutine(m_CoMoveStart);
	}

	public void MoveStop()
	{
		if (m_CoMoveStart != null)
		{
			StopCoroutine(m_CoMoveStart);
		}
	}

	private IEnumerator CoMoveStart(Vector3 _target, float _moveTime, Action _completeCallback = null)
	{
		if(m_bIsTest)
		{
#if UNITY_EDITOR
			EditorApplication.isPaused = true;
#endif
		}

		if(!m_bIsSlip)
		{
			//Debug.Log("No Slip");
			TempCheckObstacle(ref _target);
		}
		
		Vector3 RelativeToTarget = Utility.RelativePosition(m_Owner.Transform.position, _target);

		m_fDistanceToTarget = RelativeToTarget.magnitude;

		float ValueToApplies = (RelativeToTarget.magnitude / _moveTime) * Time.fixedDeltaTime;

		//float TestTime = 0.0f;
		while (true)
		{
			//Debug.Log("!!!! : " + m_Owner.Transform.name);
			//TestTime += Time.fixedDeltaTime;
			yield return YieldCache.WaitForFixedUpdate;

			//CurrentValue += ValueToApplies;

			if(m_bIsSlip)
			{
				CheckObstacle(ref _target);
			}
			
			m_Owner.Transform.position = Vector3.MoveTowards(m_Owner.Transform.position, _target, ValueToApplies);
			m_fDistanceToTarget -= ValueToApplies;
			m_fDistanceToTarget = Mathf.Clamp(m_fDistanceToTarget, 0.0f, m_fDistanceToTarget);
			//m_Owner.Transform.position = Vector3.Lerp(StartPosition, _target, CurrentValue);

			if (m_fDistanceToTarget == 0.0f)
			{
				m_CoMoveStart = null;
				_completeCallback?.Invoke();

				break;
			}
		}
	}

	private void CheckObstacle(ref Vector3 _target)
	{
		bool IsClosestPoint = false;

		Collider[] Colliders = Physics.OverlapSphere(DetectionCenter_Current, m_fDetectionRadius_Current, m_iCollisionLayerMask);

		if (Colliders.Length > 0)
		{
			IsClosestPoint = SuperCollider.ClosestPointOnSurface(Colliders[0], DetectionCenter_Current, m_fDetectionRadius_Current, out m_HitPoint);
			//Debug.LogError("Tumble : " + Colliders[0].name);
			if (!IsClosestPoint)
			{
				//Debug.LogError("Tumble : " + "!IsClosestPoint");
				m_HitPoint = Colliders[0].ClosestPoint(DetectionCenter_Current);
				/*// 충돌한 장애물이 맵 오브젝트가 아닐 때(예 : 몬스터) 미끄러지지 않고 멈춤
				m_HitPoint = Colliders[0].ClosestPoint(DetectionCenter_Current);
				//Debug.LogError("Tumble");
				_target = m_HitPoint - m_Direction_Current * m_fValueToBackwardWhenColliding;
				_target.y = m_Owner.Transform.position.y;

				StopMove();

				return;*/
			}

			m_Direction_Obstacle = Utility.RelativePosition(DetectionCenter_Current, m_HitPoint).normalized;
			m_Direction_Obstacle.y = 0.0f;
			Quaternion Rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
			m_Orthogonal_Left = Rotation * -m_Direction_Obstacle;
			m_Orthogonal_Right = Rotation * m_Direction_Obstacle;
			m_fOrthogonalAngle_Left = Vector3.Angle(m_Direction_Current, m_Orthogonal_Left);
			m_fOrthogonalAngle_Right = Vector3.Angle(m_Direction_Current, m_Orthogonal_Right);

			// 양쪽의 각도가 같을 때는 미끄러지지 않고 멈춤
			if (m_fOrthogonalAngle_Left == m_fOrthogonalAngle_Right)
			{
				MoveStop();
				//Debug.LogError("Tumble : " + "m_fAngle_Left == m_fAngle_Right");
				return;
			}

			// 최초로 미끄러지는 방향 설정 후 이동이 끝날 때까지 동일한 방향 유지
			if (m_OrthogonalDirectionType == eDirectionType.None)
			{
				m_OrthogonalDirectionType = m_fOrthogonalAngle_Left < m_fOrthogonalAngle_Right ? eDirectionType.Left : eDirectionType.Right;
			}

			switch (m_OrthogonalDirectionType)
			{
				case eDirectionType.Left:

					//Debug.LogError("Tumble : " + "Collision Left");
					m_Direction_Current = m_Orthogonal_Left;

					break;

				case eDirectionType.Right:

					//Debug.LogError("Tumble : " + "Collision Right");
					m_Direction_Current = m_Orthogonal_Right;

					break;
			}

			float Angle_Origin_And_Current = Vector3.Angle(m_Direction_Current, m_Direction_Origin);

			if (Angle_Origin_And_Current > 90.0f)
			{
				//Debug.LogError("Tumble : " + "Angle_Origin_And_Current > 90.0f");
				MoveStop();

				return;
			}

			// 미끄러지는 각도를 얻어 최종 목표 위치을 얻었지만 해당 위치에 장애물이 있을 때를 대비하여
			// 리턴하지 않고 아래의 SphereCast를 통해 목표 위치에 장애물을 감지
			//Debug.LogError("Tumble : " + "GetTarget");
			_target = m_Owner.Transform.position + m_Direction_Current * m_fDistanceToTarget;			
		}
		// 장애물에 막히지 않으면 기존 방향으로 이동 
		else if(m_Direction_Obstacle != Vector3.zero)
		{
			m_OverlapCapsulePoint1 = DetectionCenter_Obstacle + (Vector3.up * m_fDistanceToOverlapCapsulePoints);
			m_OverlapCapsulePoint2 = DetectionCenter_Obstacle - (Vector3.up * m_fDistanceToOverlapCapsulePoints);

			//Debug.LogError("Tumble : " + "m_Direction_Obstacle != Vector3.zero");
			
			if (Physics.OverlapSphere(DetectionCenter_Obstacle, m_fDetectionRadius_Obstacle, m_iCollisionLayerMask).Length == 0 && Physics.OverlapSphere(m_Owner.BugsCenter, m_fDetectionRadius_Center, m_iCollisionLayerMask).Length == 0)
			{
				//Debug.LogError("Tumble : " + "Collision Target Change");

				m_Direction_Current = m_Direction_Origin;
				_target = m_Owner.Transform.position + m_Direction_Current * m_fDistanceToTarget;
			}
		}
		
		if (Physics.SphereCast(m_Owner.BugsCenter, m_fDetectionRadius_Current, m_Direction_Current, out RaycastHit Hit, m_fDistanceToTarget + m_fDetectionRadius_Current, m_iCollisionLayerMask))
		{
			//Debug.LogError("Tumble : " + "SphereCast");
			_target = Hit.point - m_Direction_Current * m_fDetectionRadius_Current;// m_fValueToBackwardWhenColliding가 아닌 이유는 SphereCast로 감지하기 때문
			_target.y = m_Owner.Transform.position.y;
			Test = Vector3.Distance(m_Owner.Transform.position, _target);
			m_Hit = _target;
		}
	}
	public Vector3 m_Hit;
	public float Test = 0.2f;
	private void TempCheckObstacle(ref Vector3 _target)
	{
		Collider[] Colliders = Physics.OverlapSphere(DetectionCenter_Current, m_fDetectionRadius_Current, m_iCollisionLayerMask);

		if (Colliders.Length > 0)
		{
			bool IsClosestPoint = SuperCollider.ClosestPointOnSurface(Colliders[0], DetectionCenter_Current, m_fDetectionRadius_Current, out m_HitPoint);
			//Debug.LogError("Tumble : " + Colliders[0].name);
			if (IsClosestPoint)
			{
				//Debug.LogError("Tumble : " + "IsClosestPoint");
				_target = m_HitPoint - m_Direction_Current * m_fValueToBackwardWhenColliding;
				_target.y = m_Owner.Transform.position.y;

				return;
			}
			else
			{
				if (m_bIsTest)
				{
#if UNITY_EDITOR
					EditorApplication.isPaused = true;
#endif
				}
				m_HitPoint = Colliders[0].ClosestPoint(DetectionCenter_Current);
				//Debug.LogError("Tumble");
				_target = m_HitPoint - m_Direction_Current * m_fValueToBackwardWhenColliding;
				_target.y = m_Owner.Transform.position.y;

				return;
			}
		}
		//Debug.LogError("Tumble : " + "SphereCast");
		Vector3 RelativePosition = Utility.RelativePosition(m_Owner.BugsCenter, _target);

		if (Physics.SphereCast(m_Owner.BugsCenter, m_fDetectionRadius_Current, m_Direction_Current, out RaycastHit Hit, RelativePosition.magnitude + m_fDetectionRadius_Current, m_iCollisionLayerMask))
		{
			//Debug.LogError("SphereCast");
			_target = Hit.point - m_Direction_Current * m_fValueToBackwardWhenColliding;// m_fValueToBackwardWhenColliding가 아닌 이유는 SphereCast로 감지하기 때문
			_target.y = m_Owner.Transform.position.y;
		}
	}

	protected virtual void OnDrawGizmos()
	{
		if (m_Owner != null && m_bIsDebug)
		{
			//Gizmos.color = Color.black;
			//Gizmos.DrawWireSphere(m_OverlapCapsulePoint1, m_fDetectionRadius);
			//Gizmos.DrawWireSphere(m_OverlapCapsulePoint2, m_fDetectionRadius);
			Gizmos.color = Color.red;
			Gizmos.DrawRay(m_Owner.BugsCenter, m_Direction_Current * 0.5f);
			Gizmos.DrawWireSphere(DetectionCenter_Current, m_fDetectionRadius_Current);
			Gizmos.color = Color.white;
			Gizmos.DrawRay(m_Owner.BugsCenter, m_Direction_Obstacle * 0.5f);
			Gizmos.DrawWireSphere(DetectionCenter_Obstacle, m_fDetectionRadius_Obstacle);
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(m_HitPoint, m_fGizmosRadius);
			Gizmos.color = Color.green;
			Gizmos.DrawRay(m_Owner.BugsCenter, m_Direction_Current * (m_fDistanceToTarget + m_fDetectionRadius_Current));
			Gizmos.DrawWireSphere(m_Owner.BugsCenter + m_Direction_Current * (m_fDistanceToTarget + m_fDetectionRadius_Current), m_fDetectionRadius_Current);
			Gizmos.color = Color.black;
			Gizmos.DrawWireSphere(m_Hit, m_fGizmosRadius);
			//Gizmos.color = Color.green;
			//Gizmos.DrawLine(m_Owner.BugsCenter, m_Owner.BugsCenter + m_Direction_Obstacle * 10.0f);
			//Gizmos.color = Color.yellow;
			//Gizmos.DrawLine(m_Owner.BugsCenter, m_Owner.BugsCenter + m_Orthogonal_Left * 10.0f);
			//Gizmos.color = Color.white;
			//Gizmos.DrawLine(m_Owner.BugsCenter, m_Owner.BugsCenter + m_Orthogonal_Right * 10.0f);
		}		
	}
}
