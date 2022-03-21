using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetection
{
	protected Dictionary<float, csOwner> m_DicDistanceAndTargetBindings = null;

	protected csBattleManager m_BattleManager = null;

	protected int m_iLayerMask_Map = 0;

	public ObjectDetection()
	{
		m_DicDistanceAndTargetBindings = new Dictionary<float, csOwner>();

		m_BattleManager = csBattleManager.Instance;

		m_iLayerMask_Map = csBattleManager.Instance.m_iLayerMask_Map;
	}

	public bool DetectionClosestObject(Vector3 _fromPoint, float _detectionRange, float _detectionRadius, out csOwner _owner, int _detectionLayerMask, bool _isCheckObstacle = true)
	{
		bool IsDetectionClosestObjects = DetectionClosestObject(_fromPoint, _detectionRange, out _owner, _detectionLayerMask);

		if (_isCheckObstacle && IsDetectionClosestObjects)
		{
			return !DetectionObstacleBetweenMeAndTarget(_fromPoint, _owner.BugsCenter, _detectionRadius);
		}
		else
		{
			return IsDetectionClosestObjects;
		}
	}

	public bool DetectionClosestObject_MoveDirection(Vector3 _fromPoint, Vector3 _direction, float _detectionRange, float _detectionRadius, out csOwner _owner, int _detectionLayerMask, bool _isCheckObstacle = true)
	{
		bool IsDetectionSphereCast = DetectionSphereCast(_fromPoint, _direction, out _owner, _detectionRange, _detectionRadius, _detectionLayerMask);
		bool IsObstacleBetweenMeAndTarget = false;
		if (_isCheckObstacle && IsDetectionSphereCast)
		{
			IsObstacleBetweenMeAndTarget = DetectionObstacleBetweenMeAndTarget(_fromPoint, _owner.BugsCenter, _detectionRadius);

			if(IsObstacleBetweenMeAndTarget)
			{
				_owner = null;
			}

			return !IsObstacleBetweenMeAndTarget;
		}
		else
		{
			return IsDetectionSphereCast;
		}
	}

	public bool DetectionClosestObject(Vector3 _fromPoint, float _detectionRange, out csOwner _owner, int _detectionLayerMask)
	{
		if (DetectionSurroundingObjects(_fromPoint, _detectionRange, out Collider[] Colliders, _detectionLayerMask))
		{
			m_DicDistanceAndTargetBindings.Clear();

			Colliders = SortByDistance(_fromPoint, Colliders);

			for (int i = 0; i < Colliders.Length; i++)
			{
				if (!m_BattleManager.m_DicObjectColliderIndex.ContainsKey(Colliders[i].GetInstanceID()))
				{
					continue;
				}

				_owner = m_BattleManager.m_DicObjectColliderIndex[Colliders[i].GetInstanceID()];

				return true;
			}
		}

		_owner = null;

		return false;
	}	

	public bool DetectionSurroundingObjects(Vector3 _detectionCenter, float _detectionRange, out Collider[] _colliders, int _detectionLayerMask)
	{
		_colliders = Physics.OverlapSphere(_detectionCenter, _detectionRange, _detectionLayerMask);

		if(_colliders.Length == 0)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public bool DetectionObstacleBetweenMeAndTarget(Vector3 _fromPoint, Vector3 _targetPoint, float _detectionRadius)
	{
		Vector3 RelativePosition = Utility.RelativePosition(_fromPoint, _targetPoint);

		Ray Ray = new Ray(_fromPoint, RelativePosition.normalized);

		return Physics.SphereCast(Ray, _detectionRadius, RelativePosition.magnitude, m_iLayerMask_Map);
	}

	public Collider[] SortByDistance(Vector3 _from, Collider[] _colliders)
	{
		Dictionary<float, Collider> DicDistanceAndTargetBindings = new Dictionary<float, Collider>();

		if(_colliders.Length == 1)
		{
			return _colliders;
		}

		for (int i = 0; i < _colliders.Length; i++)
		{
			float Distance = Vector3.Distance(_from, _colliders[i].transform.position);

			if (!DicDistanceAndTargetBindings.ContainsKey(Distance))
			{
				DicDistanceAndTargetBindings.Add(Distance, _colliders[i]);
			}
		}

		DicDistanceAndTargetBindings.OrderBy(i => i.Key);
 		
		foreach(float distance in DicDistanceAndTargetBindings.Keys)
		{
			Debug.LogError(distance);
		}

		return DicDistanceAndTargetBindings.Values.ToArray();
	}

	protected bool DetectionSphereCast(Vector3 _fromPoint, Vector3 _direction, out csOwner _owner, float _detectionRange, float _detectionRadius, int _detectionLayerMask)
	{
		Ray Ray = new Ray(_fromPoint, _direction);

		if (Physics.SphereCast(Ray, _detectionRadius, out RaycastHit Hit, _detectionRange, _detectionLayerMask))
		{
			if (m_BattleManager.m_DicObjectColliderIndex.ContainsKey(Hit.collider.GetInstanceID()))
			{
				_owner = m_BattleManager.m_DicObjectColliderIndex[Hit.collider.GetInstanceID()];

				return true;
			}		
		}

		_owner = null;

		return false;
	}
}

public class ObjectDetection_FOV : ObjectDetection
{
	public float m_fFOVAngle = 0.0f;

	public ObjectDetection_FOV(float _fieldOfViewAngle) : base()
	{
		m_fFOVAngle = _fieldOfViewAngle;
	}

	public bool DetectionWithinFOV_Target(Vector3 _fromPoint, Vector3 _targetPoint, float _detectionRadius, bool _isCheckObstacle = true)
	{
		bool IsDetectionWithinFOV = DetectionWithinFOV(_fromPoint, _targetPoint);

		if (_isCheckObstacle && IsDetectionWithinFOV)
		{
			return !DetectionObstacleBetweenMeAndTarget(_fromPoint, _targetPoint, _detectionRadius);
		}
		else
		{
			return IsDetectionWithinFOV;
		}
	}

	public bool DetectionWithinFOV_Obstacle(Vector3 _fromPoint, Vector3 _obstacleClosestPoint)
	{
		bool IsDetectionWithinFOV = DetectionWithinFOV(_fromPoint, _obstacleClosestPoint);

		return IsDetectionWithinFOV;
	}

	protected bool DetectionWithinFOV(Vector3 _fromPoint, Vector3 _targetPoint)
	{
		Vector3 RelativeToTarget = Utility.RelativePosition(_fromPoint, _targetPoint);

		if (Vector3.Dot(_fromPoint, RelativeToTarget.normalized) > Mathf.Cos((m_fFOVAngle / 2) * Mathf.Deg2Rad))
		{
			return true;
		}

		return false;
	}

	public bool ObstacleInFOV(GameObject _from, GameObject _target, Vector3 _raycastOrigin, Vector3 _fromDirection)
	{
		Vector3 FromPosition = Utility.GetComponentNoAlloc<Collider>(_from).bounds.center;
		Vector3 TargetPosition = Utility.ClosestPoint(FromPosition, Utility.GetComponentNoAlloc<Collider>(_target), _target.transform.position, _target.transform.rotation);

		bool Result = false;

		Vector3 DirToTarget = (TargetPosition - FromPosition).normalized;

		if (Vector3.Dot(_fromDirection, DirToTarget) > Mathf.Cos((m_fFOVAngle / 2) * Mathf.Deg2Rad))
		{
			Vector3 RelativePosition = Utility.RelativePosition(FromPosition, TargetPosition);

			Result = true;
		}

		return Result;
	}
}
