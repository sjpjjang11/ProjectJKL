using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProjectK;
using MEC;

public class Test : MonoBehaviour {

	Collider m_Collider;
	public Vector3 m_Center;
	public Vector3 m_Size;
	public float m_fDistance;
	public const int m_iNumberOfItems = 1;

	public bool[] m_bBools = new bool[m_iNumberOfItems];

	private void Start()
	{
		m_Collider = GetComponent<Collider>();
		
		/*for (int i = 0; i < m_iNumberOfItems; i++)
		{
			StartCoroutine(CoUnityTest(i));
		}*/

		/*for (int i = 0; i < m_iNumberOfItems; i++)
		{
			Timing.RunCoroutine(CoMECTest(i));
		}*/
	}

	private void Update()
	{
		if(Physics.BoxCast(m_Collider.bounds.center, m_Collider.bounds.size * 0.5f, transform.forward, out RaycastHit Hit, Quaternion.identity, m_fDistance))
		{
			Debug.Log("@@@@ : " + Hit.collider.name);
		}
	}

	private IEnumerator CoUnityTest(int _index)
	{
		int Num = _index;

		while (Num >= 0)
		{
			m_bBools[_index] = Num % 2 == 0;

			Num++;

			yield return null;
		}
	}

	private IEnumerator<float> CoMECTest(int _index)
	{
		int Num = _index;

		while (Num >= 0)
		{
			m_bBools[_index] = Num % 2 == 0;

			Num++;

			yield return Timing.WaitForOneFrame;
		}
	}

	private void OnDrawGizmos()
	{
		//Gizmos.color = Color.black;
		//Gizmos.DrawRay(m_Transform.position, m_Transform.up * m_fOnSize);
		//m_Center = m_Collider.bounds.center;
		//m_Size = m_Collider.bounds.size;
		//Gizmos.DrawCube(m_Collider.bounds.center, m_Size);
	}
}

