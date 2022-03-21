using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Invector;
using ProjectK;

/*2018.07.14
* hkh
* 플레이어 카메라
* 기본적으로 3인칭 시점 기능을 하며, 그외에 줌인, 줌아웃 등
* 카메라 관련 기능을 구현
*/
public class csPlayerCamera : MonoBehaviour
{
	private IEnumerator m_CoCameraShake = null;         // 카메라 흔들기 코루틴을 담을 변수

	private Transform m_Transform = null;

	protected Camera m_Camera = null;
	public Camera Camera
	{
		get
		{
			return m_Camera;
		}
	}

	private csEventHandler_Hero m_EventHandler = null;

	private csOwner_Hero m_Owner = null;

	private ObjectShake m_ObjectShake;                  // 오브젝트 흔드는 기능의 클래스
	private MotionBlur m_MotionBlur;                    // 잔상 기능 클래스

	[SerializeField]
	private Transform m_Target = null;                  // 카메라가 따라다닐 목표 Transform(영웅)
	private Vector3 m_LookPoint = Vector3.zero;         // 최종적으로 카메라가 쳐다볼 위치
	private Vector3 m_AimPoint_UI;
	private Vector3 m_AimPoint_Real;
	private Vector2 m_RotateValue = Vector2.zero;	
	private Vector2 m_ViewPoint = Vector2.zero;
	private Vector2 m_LastViewPoint = Vector2.zero;

	[HideInInspector]
	public bool m_bIsShake = false;                    // 현재 오브젝트 흔들기 진행중인지   

	[Header("Top View")]
	public float m_fTopViewDistance = 2.5f;
	public float m_fTopViewHeight = 2.5f;
	public float m_fMoveSpeed = 10.0f;
	public float m_fRotationSpeed = 10.0f;
      
	[Header("ETC")]
	public float m_fCameraHorizontalSize = 0.0f;
	public float m_fCameraVerticalSize = 0.0f;

	public Vector2 m_ScreenSize = Vector2.zero;
	public float m_OrtographicSize = 0.0f;

	private void Awake()
	{
		m_Transform = GetComponent<Transform>();

		m_Camera = GetComponent<Camera>();
	}

	public void Settings(csOwner_Hero _owner)
	{
		m_Owner = _owner;
		m_EventHandler = m_Owner.m_EventHandler_Hero;

		RegisterHandler();

		//m_Target = m_Owner.m_CameraTarget;
		m_Target = m_Owner.Transform;
		m_TargetLookAt = new GameObject("TargetLookAt").transform;

		csBattleManager.Instance.ObjectMoveToBattleScene(m_TargetLookAt.gameObject);

		m_ObjectShake = new ObjectShake();

		m_MotionBlur = GetComponent<MotionBlur>();

		// 만약 활성화 돼있으면 비활성화 
		if (m_MotionBlur != null && Utility.IsActive(m_MotionBlur))
		{
			Utility.Activate(m_MotionBlur, false);
		}

		m_fCameraHorizontalSize = m_Camera.orthographicSize * Screen.width / Screen.height;
		m_fCameraVerticalSize = m_Camera.orthographicSize;
	}

	public void RegisterHandler()
	{
		m_EventHandler.Register(this);
	}

	public void UnregisterHandler()
	{
		m_EventHandler.Unregister(this);
	}

	private void LateUpdate()
	{
		// 타겟이 없으면
		if (m_Target == null || m_EventHandler == null/* || LocalHandler.Waiting.Active*/)
		{
			return;
		}
		m_ScreenSize = new Vector2(Screen.width, Screen.height);
		m_OrtographicSize = m_Camera.orthographicSize;
		// 흔드는 중일 때 동작하지 않게
		if (m_bIsShake)
		{
			return;
		}
		TouchInput();
		FollowTarget_ThirdPerson();
		CameraRotate();
		//FollowTarget();
		//LookAtTarget();
	}

	public Vector3 m_GetPosition;
	public Vector2 TestMapSize;
	public float TestMaxPositionX;
	public float TestMaxPositionZ;
	public float TestClampX; 
	public float TestClampZ;
	public int m_TestInt = 0;
	public void FollowTarget()
	{
		Vector3 GetPosition = m_Target.position - (Vector3.forward * m_fTopViewDistance) + (Vector3.up * m_fTopViewHeight);

		Vector2 AbsoluteValue = new Vector2(Mathf.Abs(GetPosition.x), Mathf.Abs(GetPosition.z));
		//Debug.Log("!!!! : " + AbsoluteValue);
		float MaxPositionX = TestMapSize.x <= m_fCameraHorizontalSize ? 0.0f : TestMapSize.x - m_fCameraHorizontalSize;
		TestMaxPositionX = MaxPositionX;
		float MaxPositionZ = TestMapSize.y <= m_fCameraVerticalSize ? 0.0f : TestMapSize.y - m_fCameraVerticalSize * 2.0f;
		if(Mathf.Sign(GetPosition.z) == -1)
		{
			MaxPositionZ += m_Camera.orthographicSize;
		}
		TestMaxPositionZ = MaxPositionZ;
		float ClampX = Mathf.Clamp(AbsoluteValue.x, AbsoluteValue.x, MaxPositionX) * Mathf.Sign(GetPosition.x);
		TestClampX = ClampX;
		float ClampZ = Mathf.Clamp(AbsoluteValue.y, AbsoluteValue.y, MaxPositionZ) * Mathf.Sign(GetPosition.z);
		TestClampZ = ClampZ;

		GetPosition = new Vector3(ClampX, GetPosition.y, ClampZ);
		m_GetPosition = GetPosition;
		
		m_Transform.position = Vector3.MoveTowards(m_Transform.position, GetPosition, m_fMoveSpeed);	
	}

	public void LookAtTarget()
	{
		Quaternion Rotation = Quaternion.LookRotation((m_Target.position - m_Transform.position).normalized);
		switch (m_TestInt)
		{
			case 0:

				m_Transform.rotation = Rotation;

				break;

			case 1:

				m_Transform.rotation = Quaternion.Lerp(m_Transform.rotation, Rotation, m_fRotationSpeed);

				break;

			case 2:

				m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, Rotation, m_fRotationSpeed);

				break;

			case 3:

				m_Transform.LookAt(m_Target);

				break;
		}
	}

	[Header("ThirdPerson")]
	public Transform m_TargetLookAt = null;             // 카메라의 방향 전환, 이동의 기준이 되는 Transform
	public float m_fDefaultHeight = 1.5f;               // 기본이 되는 카메라의 높이   
	public float m_fDefaultDistance = 2.5f;             // 기본이 되는 카메라와 타겟의 거리
	public float m_fForward = -1.0f;                    // 값이 양수면 카메라가 전방에서 영웅을, 음수면 후방에서 영웅을 바라봄
	public float m_fRight = 0.3f;                       // 카메라를 중앙에서 왼쪽, 오른쪽으로 멀어지게 함
	[SerializeField]
	private float m_fCurrentHeight = 0.0f;              // 현재 카메라 높이값
	[SerializeField]
	private float m_fCurrentDistance = 0.0f;            // 현재 카메라와 타겟의 거리값
	public float m_fSmoothDistanceFollow = 10.0f;       // 카메라와 타겟의 거리가 변동이 생길 때의 속도값
	public float m_fSmoothCameraRotation = 12.0f;       // 카메라와 타겟이 회전하는 속도값
	[SerializeField]
	private Vector2 m_Sensitivity = new Vector2(10.0f, 10.0f);		// 카메라 회전 강도

	[SerializeField]
	private float m_fCullingDistance = 0.0f;            // 컬링 할 때에 카메라와 타겟의 거리값

	public LayerMask m_CullingLayer = 1 << 0;           // 컬링할 레이어

	public float m_fVerticalMinLimit = -40.0f;          // 세로 방향으로 회전 가능한 가장 작은 값
	public float m_fVerticalMaxLimit = 30.0f;           // 세로 방향으로 회전 가능한 가장 큰 값
	public float m_fHorizontalMinLimit = -360.0f;       // 가로 방향으로 회전 가능한 가장 작은 값
	public float m_fHorizontalMaxLimit = 360.0f;        // 가로 방향으로 회전 가능한 가장 큰 값   

	protected virtual void TouchInput()
	{
		m_ViewPoint = m_EventHandler.ViewPoint.Get();

		m_RotateValue.x += m_ViewPoint.x * m_Sensitivity.x;
		m_RotateValue.y -= m_ViewPoint.y * m_Sensitivity.y;

		m_RotateValue.x = vExtensions.ClampAngle(m_RotateValue.x, m_fHorizontalMinLimit, m_fHorizontalMaxLimit);
		m_RotateValue.y = vExtensions.ClampAngle(m_RotateValue.y, m_fVerticalMinLimit, m_fVerticalMaxLimit);
	}

	private void FollowTarget_ThirdPerson()
	{
		// 카메라 방향 변수
		Vector3 CameraDirection;

		Vector3 CameraHeighePos;

		// 타겟과의 거리값이 변동된 후 다시 기본 거리값으로 복귀시킬 때 부드럽게 복귀시킴
		m_fCurrentDistance = Mathf.Lerp(m_fCurrentDistance, m_fDefaultDistance, m_fSmoothDistanceFollow * Time.deltaTime);

		// 컬링 작업으로 인해 현재 거리값이 변동된 후 다시 현재 거리값으로 복귀시킬 때 부드럽게 복귀시킴
		//m_fCullingDistance = Mathf.Lerp(m_fCullingDistance, m_fCurrentDistance, Time.deltaTime);

		CameraDirection = (m_fForward * m_TargetLookAt.forward) + (m_fRight * m_TargetLookAt.right);

		CameraDirection = CameraDirection.normalized;

		m_fCurrentHeight = m_fDefaultHeight;

		CameraHeighePos = m_Target.position + new Vector3(0.0f, m_fCurrentHeight, 0.0f);
		RaycastHit HitInfo;

		ClipPlanePoints planePoints = Camera.NearClipPlanePoints(CameraHeighePos + (CameraDirection * (m_fDefaultDistance)));

		//Check if target position with culling height applied is not blocked
		if (CullingRayCast(CameraHeighePos, planePoints, out HitInfo, m_fCurrentDistance, m_CullingLayer, Color.cyan))
		{
			m_fCurrentDistance = Mathf.Clamp(m_fCullingDistance, 0.0f, m_fDefaultDistance);
		}

		m_LookPoint = CameraHeighePos + m_TargetLookAt.forward;
		//Debug.Log(m_LookPoint + "    " + m_CameraPos + "    " + m_TargetLookAt.forward + "   " + (m_CameraPos + m_TargetLookAt.forward));
		m_LookPoint += (m_TargetLookAt.right * Vector3.Dot(CameraDirection * (m_fCurrentDistance), m_TargetLookAt.right));

		m_Transform.position = CameraHeighePos + (CameraDirection * (m_fCurrentDistance));

		m_TargetLookAt.position = CameraHeighePos;
	}

	private void CameraRotate()
	{
		Quaternion xQuaternion = Quaternion.AngleAxis(m_RotateValue.x, Vector3.up);

		Quaternion yQuaternion = Quaternion.AngleAxis(0, Vector3.left);

		m_Target.rotation = Quaternion.Slerp(m_Target.rotation, (xQuaternion * yQuaternion), m_fSmoothCameraRotation * Time.deltaTime);

		yQuaternion = Quaternion.AngleAxis(m_RotateValue.y + 10.0f, Vector3.right);

		m_TargetLookAt.rotation = Quaternion.Slerp(m_TargetLookAt.rotation, (xQuaternion * yQuaternion), m_fSmoothCameraRotation * Time.deltaTime);

		Quaternion rotation = Quaternion.LookRotation((m_LookPoint) - m_Transform.position);

		m_Transform.rotation = rotation;
	}


	private bool CullingRayCast(Vector3 _from, ClipPlanePoints _to, out RaycastHit _hitInfo, float _distance, LayerMask _cullingLayer, Color _color)
	{
		bool Result = false;

		if (Physics.Raycast(_from, _to.LowerLeft - _from, out _hitInfo, _distance, _cullingLayer))
		{
			Result = true;

			m_fCullingDistance = _hitInfo.distance;
		}

		if (Physics.Raycast(_from, _to.LowerRight - _from, out _hitInfo, _distance, _cullingLayer))
		{
			Result = true;

			if (m_fCullingDistance > _hitInfo.distance)
			{
				m_fCullingDistance = _hitInfo.distance;
			}
		}

		if (Physics.Raycast(_from, _to.UpperLeft - _from, out _hitInfo, _distance, _cullingLayer))
		{
			Result = true;

			if (m_fCullingDistance > _hitInfo.distance)
			{
				m_fCullingDistance = _hitInfo.distance;
			}
		}

		if (Physics.Raycast(_from, _to.UpperRight - _from, out _hitInfo, _distance, _cullingLayer))
		{
			Result = true;

			if (m_fCullingDistance > _hitInfo.distance)
			{
				m_fCullingDistance = _hitInfo.distance;
			}
		}

		//Debug.Log(Result);
		return Result;
	}

	#region State Method

	#endregion

	#region Command Method

	private void OnCommand_Camera_ChangeTarget(Transform _target)
	{
		m_Target = _target;
	}

	private void OnCommand_ChangeMoveSpeed(float _speed)
	{
		m_fMoveSpeed = _speed;
	}

	private void OnCommand_ChangeRotationSpeed(float _speed)
	{
		m_fRotationSpeed = _speed;
	}

	private void OnCommand_ShakeCamera(float _intensity, float _shakeDuretion)
    {
        m_bIsShake = true;

		if(m_CoCameraShake != null)
		{
			StopCoroutine(m_CoCameraShake);
		}

		m_CoCameraShake = m_ObjectShake.CoShake(m_Transform, _intensity, _shakeDuretion, () =>
		{
			m_bIsShake = false;
		});

		StartCoroutine(m_CoCameraShake);
    }

#endregion

}
