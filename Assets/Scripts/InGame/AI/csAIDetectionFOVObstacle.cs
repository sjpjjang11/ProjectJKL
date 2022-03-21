using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAIDetectionFOVObstacle : csAIDetectionFOV
{
	private Dictionary<float, Collider> m_DicDistanceFromMeToObstacle = new Dictionary<float, Collider>();

	//public Transform m_RaycastStartTr;

	public Collider m_NearObstacle;
	public Collider[] m_Colliders;
	public Material m_Material;

	public Vector3 m_ClosestPointNearObstacle;
	public Vector3 m_DirectionFromMeToObstacle;
	public Vector3 m_RaycastStart;
	public Vector3 m_LastDestination;

	public PrevObstacle m_PrevCollider;

	public float m_fDetectionRadius = 0.0f;
	public float m_DistanceFromMeToObstacle;
	public float m_DistanceFromRaycastStartToObstacle;
	public float m_RaycastStartDistance;
	public float m_MinDistance;
	public float m_MaxDistance;

	public bool m_IsObstacleInFOV;
	public bool m_IsYellow;
	public bool m_IsRed;
	public bool m_IsBlue;
	public bool m_IsGreen;

	protected override void Awake()
	{
		base.Awake();

		m_Material = Resources.Load("Material/rangeMaterial") as Material;

		//m_PrevCollider.Obj = gameObject;
		//m_PrevCollider.Material = gameObject.GetComponent<MeshRenderer>().material;
	}

	public override void Settings(csOwner_Monster _owner)
	{
		base.Settings(_owner);

		m_fDetectionRadius = m_Owner.Info_Monster.m_BugsCollider.Radius;
	}

	public override bool Detect()
	{
		Collider[] NearColliders = GetNearObstacle();

		//Debug.Log(m_FieldOfView.TempDetection(m_Owner.gameObject, NearColliders[0].gameObject, AICenter,m_Owner.Transform.forward));
		for (int i = 0; i < NearColliders.Length; i++)
		{
			Collider NearObstacle = NearColliders[i];
			m_NearObstacle = NearObstacle;
			//Debug.LogError("Near : " + NearObstacle.name);

			SuperCollider.ClosestPointOnSurface(NearObstacle, AICenter, m_fDetectionRadius, out Vector3 ClosestPoint);
			//Vector3 ClosestPoint = Utility.ClosestPoint(AICenter, NearObstacle, NearObstacle.transform.position, NearObstacle.transform.rotation);
			m_IsObstacleInFOV = m_ObjectDetection_FOV.DetectionWithinFOV_Obstacle(AICenter, ClosestPoint);

			// 감지된 엄폐물이 자신의 시야각에 존재하는지 확인
			if (m_IsObstacleInFOV)
			{
				ChangeNearObstacle();

				//Vector3 ClosestPointNearObstacle = Utility.ClosestPoint(AICenter, NearObstacle, NearObstacle.transform.position, NearObstacle.transform.rotation);

				m_ClosestPointNearObstacle = ClosestPoint;

				float DistanceFromMeToObstacle = GetDistance(AICenter, ClosestPoint);

				m_DistanceFromMeToObstacle = DistanceFromMeToObstacle;

				Vector3 DirectionFromMeToObstacle = GetDirection(AICenter, ClosestPoint);
				DirectionFromMeToObstacle.y = 0.0f; //AICenter.y;

				m_DirectionFromMeToObstacle = DirectionFromMeToObstacle;

				Vector3 RaycastStart = Vector3.zero;

				float MinDistance = DistanceFromMeToObstacle;			
				float MaxDistance = DistanceFromMeToObstacle + 3.0f;

				Bounds Bounds = NearObstacle.GetComponent<MeshFilter>().sharedMesh.bounds;
				float BoundsX = Bounds.size.x * NearObstacle.transform.localScale.x;
				float BoundsY = Bounds.size.y * NearObstacle.transform.localScale.y;
				float BoundsZ = Bounds.size.z * NearObstacle.transform.localScale.z;

				Vector3 NearObstacleBounds = new Vector3(BoundsX, BoundsY, BoundsZ);

				if (NearObstacleBounds.x < NearObstacleBounds.z)
				{				
					MinDistance += NearObstacleBounds.x;
					MaxDistance += NearObstacleBounds.z;
				}
				else if(NearObstacleBounds.x > NearObstacleBounds.z)
				{					
					MinDistance += NearObstacleBounds.z;
					MaxDistance += NearObstacleBounds.x;
				}
				else
				{
					MinDistance += NearObstacleBounds.x;
					MaxDistance += NearObstacleBounds.x;
				}

				m_MinDistance = MinDistance;
				m_MaxDistance = MaxDistance;

;				while (true)
				{
					RaycastStart = AICenter + (DirectionFromMeToObstacle * MinDistance);

					if (m_Grid.NodeFromWorldPoint(RaycastStart).NodeType == eNodeType.Obstacle)
					{
						if(MinDistance == MaxDistance)
						{
							break;
						}

						MinDistance += 0.1f;
						MinDistance = Mathf.Clamp(MinDistance, MinDistance, MaxDistance);
						m_MinDistance = MinDistance;
						continue;
					}
					Debug.LogError("Not Obstacle");
					if (IsHeroLookAtMe(RaycastStart))
					{
						Debug.LogError("HeroLookAtMe");
						break;
					}
					
					if (MinDistance == MaxDistance)
					{
						break;
					}
					Debug.LogError("Cover True");
					MinDistance += 1.0f;
					RaycastStart = AICenter + (DirectionFromMeToObstacle * MinDistance);
					m_RaycastStart = RaycastStart;
					//m_RaycastStartTr.position = RaycastStart;
					m_Owner.m_EventHandler_Monster.TargetWayPoint.Set(RaycastStart);

					m_DicDistanceFromMeToObstacle.Clear();

					return true;
				}
			}
		}
		Debug.LogError("Cover false");
		m_NearObstacle = null;

		m_IsObstacleInFOV = false;

		if (m_PrevCollider.Obj != null)
		{
			m_PrevCollider.Obj.GetComponent<MeshRenderer>().material = m_PrevCollider.Material;
		}

		m_DicDistanceFromMeToObstacle.Clear();

		return false;
	}

	private Collider[] GetNearObstacle()
	{
		Collider[] Colls = Physics.OverlapSphere(AICenter, m_fDetectionRange, m_iLayerMask_Map);
		m_Colliders = Colls;
		for (int i = 0; i < Colls.Length; i++)
		{ 
			Vector3 ClosestPoint = Utility.ClosestPoint(AICenter, Colls[i], Colls[i].transform.position, Colls[i].transform.rotation);
			float Distance = Vector3.Distance(AICenter, ClosestPoint);

			if (!m_DicDistanceFromMeToObstacle.ContainsKey(Distance))
			{
				m_DicDistanceFromMeToObstacle.Add(Distance, Colls[i]);
			}
		}

		List<float> ListDistance; ;
		ListDistance = m_DicDistanceFromMeToObstacle.Keys.ToList();
		ListDistance.Sort();

		Collider[] NearColliders = new Collider[ListDistance.Count];

		for (int i = 0; i < ListDistance.Count; i++)
		{
			NearColliders[i] = m_DicDistanceFromMeToObstacle[ListDistance[i]];
		}

		return NearColliders;
	}

	private void ChangeNearObstacle()
	{
		if (m_PrevCollider.Obj != m_NearObstacle.gameObject)
		{
			if (m_PrevCollider.Obj != null)
			{
				m_PrevCollider.Obj.GetComponent<MeshRenderer>().material = m_PrevCollider.Material;
			}

			m_PrevCollider.Obj = m_NearObstacle.gameObject;
			m_PrevCollider.Material = m_NearObstacle.GetComponent<MeshRenderer>().material;
		}

		m_NearObstacle.GetComponent<MeshRenderer>().material = m_Material;
	}

	private void InitializeObstacle()
	{
		if(m_NearObstacle != null)
		{
			m_NearObstacle.GetComponent<MeshRenderer>().material = m_PrevCollider.Material;
		}		
	}

	protected override void OnDrawGizmosSelected()
	{
		if (m_IsObstacleInFOV)
		{
			if (m_IsYellow)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawRay(AICenter, m_DirectionFromMeToObstacle * m_DistanceFromMeToObstacle);
			}

			if (m_IsRed)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine(AICenter, m_RaycastStart);
				Gizmos.DrawWireSphere(m_RaycastStart, 1.0f);
			}

			if (m_IsBlue)
			{
				Gizmos.color = Color.blue;
			}

			if (m_IsGreen)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(m_ClosestPointNearObstacle, 1.0f);
			}
		}
	}
}
