using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBounds : MonoBehaviour
{
	public Vector3 m_BoundsMin = Vector3.zero;
	public Vector3 m_BoundsMax = Vector3.zero;

	private void OnDrawGizmos()
	{
		Collider Collider = GetComponent<Collider>();
		Bounds Bounds = GetComponent<MeshFilter>().sharedMesh.bounds;
		float BoundsX = Bounds.size.x * transform.localScale.x;
		float BoundsY = Bounds.size.y * transform.localScale.y;
		float BoundsZ = Bounds.size.z * transform.localScale.z;

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(Collider.bounds.min, 1.0f);
		Gizmos.DrawWireSphere(new Vector3(Collider.bounds.max.x, Collider.bounds.min.y, Collider.bounds.max.z), 1.0f);
		m_BoundsMin = Collider.bounds.min;
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(Collider.bounds.max, 1.0f);
		Gizmos.DrawWireSphere(new Vector3(Collider.bounds.min.x, Collider.bounds.max.y, Collider.bounds.min.z), 1.0f);
		m_BoundsMax = Collider.bounds.max;
	}
}
