using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
	public CharacterController m_CC;

	public Vector3 m_Point1 = Vector3.zero;
	public Vector3 m_Point2 = Vector3.zero;
	public Vector3 m_Center = Vector3.zero;
	public Vector3 m_HitPoint = Vector3.zero;
	public Vector3 m_OrthogonalVector = Vector3.zero;
	public Vector3 m_CollisionDirection_Left = Vector3.zero;
	public Vector3 m_CollisionDirection_Right = Vector3.zero;	

	public float m_fCollisionAngle = 0.0f;
	public float m_fAngle_Left = 0.0f;
	public float m_fAngle_Right = 0.0f;
	public float m_fGizmosRadius;
	public float m_fTest;

	private void Awake()
	{
		m_CC = GetComponent<CharacterController>();
	}

	private void FixedUpdate()
	{
		HitCollider();	
	}

	private void HitCollider()
	{
		/*Vector3 Center = transform.position + (transform.up * (transform.lossyScale.y - m_fTest) / 2);
		m_Center = Center;
		float DistansToPoints = transform.lossyScale.y / 2 - m_fGizmosRadius;
		m_Point1 = Center - transform.up * DistansToPoints;
		m_Point2 = Center + transform.up * DistansToPoints;

		Collider[] Colls = Physics.OverlapCapsule(m_Point1, m_Point2, m_fGizmosRadius);*/
		Collider[] Colls = Physics.OverlapSphere(m_CC.bounds.center, m_fGizmosRadius);
		Debug.Log(Colls.Length);
		for(int i = 0; i < Colls.Length; i++)
		{
			Debug.Log(Colls[i].name);
		}
		if (Colls.Length > 0)
		{
			BSPTree BSPTree = Colls[0].GetComponent<BSPTree>();
			if(BSPTree != null)
			{
				Debug.Log("@@@@ : " + Colls[0].name);
				m_HitPoint = BSPTree.ClosestPointOn(m_CC.bounds.center, m_fGizmosRadius);

				//m_OrthogonalVector = m_HitPoint - m_CC.bounds.center;
				m_OrthogonalVector = Utility.RelativePosition(m_CC.bounds.center, m_HitPoint).normalized;
				//m_fCollisionAngle = Vector3.Angle(m_OrthogonalVector, transform.forward);
				//m_fCollisionAngle = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
				Quaternion Rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
				m_CollisionDirection_Left = Rotation * -m_OrthogonalVector;
				//Rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
				m_CollisionDirection_Right = Rotation * m_OrthogonalVector;
				m_fAngle_Left = Vector3.Angle(transform.forward, m_CollisionDirection_Left);
				m_fAngle_Right = Vector3.Angle(transform.forward, m_CollisionDirection_Right);				
			}
		}
		else
		{
			m_HitPoint = Vector3.zero;
		}

		/*if (Physics.CapsuleCast(m_Point1 - (transform.up * transform.lossyScale.y), m_Point2 - (transform.up * transform.lossyScale.y), m_fGizmosRadius, transform.up, out RaycastHit Hit, transform.lossyScale.y))
		{
			Debug.LogError("Hit!!!! : " + Hit.collider.name);
			m_HitPoint = Hit.point;
		}
		else
		{
			m_HitPoint = Vector3.zero;
		}*/

		/*if (Physics.SphereCast(transform.position, m_fGizmosRadius, transform.up, out RaycastHit Hit, transform.lossyScale.y))
		{
			Debug.LogError("Hit!!!! : " + Hit.collider.name);
			m_HitPoint = Hit.point;
		}
		else
		{
			m_HitPoint = Vector3.zero;
		}*/
	}

	/*private void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		Gizmos.DrawSphere(m_Point1, m_fGizmosRadius);
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(m_Point2, m_fGizmosRadius);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(m_Center, m_fGizmosRadius);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(m_CC.bounds.center, m_fGizmosRadius);
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(m_HitPoint, m_fGizmosRadius);

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(m_CC.bounds.center, m_CC.bounds.center + m_CollisionDirection_Left * 10.0f);
		Gizmos.color = Color.white;
		Gizmos.DrawLine(m_CC.bounds.center, m_CC.bounds.center + m_CollisionDirection_Right * 10.0f);		
	}*/
}
