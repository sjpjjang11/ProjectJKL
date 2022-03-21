using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeshAngle : MonoBehaviour
{
	public MeshFilter m_MeshFilter;
	public Mesh m_Mesh;	
	public MeshRenderer m_MeshRenderer;

	private void Awake()
	{
		m_MeshFilter = GetComponent<MeshFilter>();
		m_Mesh = m_MeshFilter.mesh;
		m_MeshRenderer = GetComponent<MeshRenderer>();
	}

	private void Update()
	{
		
	}
}
