using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;

public class TestAgentManager : MonoBehaviour
{
	private Transform m_Transform = null;
	public Transform m_TargetTr = null;

	public GameObject m_AgentObject = null;

	public csWeapon_Lightsaber m_LightSaber = null;
	
	public TestAgent[] m_Agents = null;
	private csGrid m_Grid = null;

	protected Vector3 m_Target = Vector3.zero;

	public float m_fAllowPathUpdate = 1.0f;
	public float m_fTestRange = 0.0f;
	public float m_fGizmosRadius;

	public int m_iLayerMask = 0;

	private void Awake()
	{
		m_Transform = GetComponent<Transform>();
		m_iLayerMask = Layer.CollisionLayerMask(eCollisionLayerType.Floor);

		if(m_AgentObject != null)
		{
			m_Agents = m_AgentObject.GetComponentsInChildren<TestAgent>();
		}
	}

	private void Start()
	{
		m_Grid = csPathRequestManager.Instance.GetComponent<csGrid>();
	}

	private void Update()
	{
		if(Input.GetMouseButtonUp(1))
		{
			//Debug.Log(Input.mousePosition);
			Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit Hit;
			if(Physics.Raycast(Ray, out Hit, Mathf.Infinity, m_iLayerMask))
			{
				m_Grid.NodeFromWorldPoint(m_Target).ResetType(gameObject.GetInstanceID());

				m_Target = Hit.point;
				m_TargetTr.position = new Vector3(Hit.point.x, m_TargetTr.position.y, Hit.point.z);

				m_Grid.NodeFromWorldPoint(m_Target).SetType(gameObject.GetInstanceID(), eNodeType.Obstacle);

				StartAgent();
			}
		}

		if(Input.GetKey(KeyCode.Space))
		{
			m_Grid.NodeFromWorldPoint(m_Target).ResetType(gameObject.GetInstanceID());

			m_Target = m_TargetTr.position;

			m_Grid.NodeFromWorldPoint(m_Target).SetType(gameObject.GetInstanceID(), eNodeType.Obstacle);

			StartAgent();
		}

		for (int i = 0; i < m_Agents.Length; i++)
		{
			csNode CurrentNode = m_Grid.NodeFromWorldPoint(m_Agents[i].m_Transform.position);

			m_Agents[i].m_Node_WayPoint_Prev.ResetType(gameObject.GetInstanceID());
			m_Agents[i].m_Node_WayPoint_Prev = CurrentNode;

			CurrentNode.SetType(gameObject.GetInstanceID(), eNodeType.Obstacle);
		}

		if(Input.GetKeyUp(KeyCode.Q))
		{
			if (!m_LightSaber.m_bIsSaberOn)
			{
				m_LightSaber.StartAttack();
			}
			else
			{
				m_LightSaber.StopAttack();
			}
		}
	}

	private void StartAgent()
	{
		for(int i = 0; i < m_Agents.Length; i++)
		{
			m_Agents[i].StartAgent(m_Target);
		}
		//StartCoroutine(CoStartAgent());
	}

	private IEnumerator CoStartAgent()
	{
		int Index = 0;

		while(true)
		{
			yield return new WaitForSeconds(0.1f);

			m_Agents[Index].StartAgent(m_Agents[Index].m_Target.position);
			Index++;

			if(Index == m_Agents.Length)
			{
				break;
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Gizmos.DrawWireSphere(m_Transform.position, m_fGizmosRadius);
		}
	}
}
