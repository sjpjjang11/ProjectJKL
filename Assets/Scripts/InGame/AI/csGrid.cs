using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;

public class csGrid : MonoBehaviour {

	public delegate void Delegate();
	public static event Delegate StartCreateGrid;
	public static event Delegate EndCreateGrid;

	private Action<int, int> m_AddNeighbours = null;

	public List<csNode> m_ListPath;

	//public Transform m_Target;
	private Transform m_Transform;

	public csEventHandler_Grid m_EventHandler = null;

	[SerializeField]
	public csNode[,] m_Grid;

	public List<Vector2> m_Test;

	public LayerMask m_UnwalkbleMask;
	public Vector2 m_GridWorldSize;

	public Point m_GridSize;

	public float m_fNodeRadius = 0.5f;
	public float m_fNodeDiameter = 0.0f;
	public float m_fNodeColorAlpha = 0.5f;

	public int MaxSize
	{
		get
		{
			return m_GridSize.x * m_GridSize.y;
		}
	}

	public bool m_bIsDisplayGridGizmos;
	public bool m_bIsDisplayWalkable;

	private void Awake()
	{
		m_Transform = GetComponent<Transform>();

		m_EventHandler = GetComponent<csEventHandler_Grid>();

		CreateGrid();
	}
	public float Test;
	public void CreateGrid()
	{
		StartCreateGrid?.Invoke();

		m_fNodeDiameter = m_fNodeRadius * 2.0f;
		//m_fNodeDiameter = m_fNodeRadius;
		m_GridSize.x = Mathf.RoundToInt(m_GridWorldSize.x / m_fNodeDiameter);
		m_GridSize.y = Mathf.RoundToInt(m_GridWorldSize.y / m_fNodeDiameter);

		m_Grid = new csNode[m_GridSize.x, m_GridSize.y];

		Vector3 WorldBottomLeft = m_Transform.position - Vector3.right * m_GridWorldSize.x / 2.0f - Vector3.forward * m_GridWorldSize.y / 2.0f;

		for(int x = 0; x < m_GridSize.x; x++)
		{
			for(int y = 0; y < m_GridSize.y; y++)
			{
				Vector3 WorldPoint = WorldBottomLeft + Vector3.right * (x * m_fNodeDiameter) + Vector3.forward * (y * m_fNodeDiameter);
				//Vector3 WorldPoint = WorldBottomLeft + Vector3.right * (x * Test1) + Vector3.forward * (y * Test1);
				
				//bool IsWalkable = !(Physics.CheckSphere(WorldPoint, m_fNodeRadius, m_UnwalkbleMask));
				bool IsWalkable = !Physics.CheckBox(WorldPoint, Vector3.one * (m_fNodeRadius - Test), Quaternion.identity, m_UnwalkbleMask);

				eNodeType Type = IsWalkable ? eNodeType.Walkable : eNodeType.Obstacle;

				/*if(Type == NodeType.Obstacle)
				{
					GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);    //큐브 오브젝트 생성
					cube.transform.localScale = Vector3.one * (m_fNodeDiameter - Test);
					cube.transform.position = WorldPoint;
				}*/

				m_Grid[x, y] = new csNode(Type, WorldPoint, x, y);
				m_Test.Add(new Vector2(x, y));
				//Debug.Log(m_Grid[x, y].m_WorldPosition);
			}
		}

		GetNeighbours();

		EndCreateGrid?.Invoke();
		//m_Agent.FindPath(m_Grid);
	}

	private void GetNeighbours()
	{
		for(int x = 0; x < m_GridSize.x; x++)
		{
			for(int y = 0; y < m_GridSize.y; y++)
			{
				m_Grid[x, y].m_ListNeighbours = GetNeighbours(m_Grid[x, y], 0, 1);
			}
		}
	}

	public void ResetGrid()
	{
		csNode[,] Nodes = m_Grid;

		for (int x = 0; x < m_GridSize.x; x++)
		{
			for (int y = 0; y < m_GridSize.y; y++)
			{
				Nodes[x, y].ResetType();
			}
		}
	}

	public void ResetGrid(List<csNode> _listNodes)
	{
		//Debug.Log("ResetGrid");
		for(int i = 0; i < _listNodes.Count; i++)
		{
			_listNodes[i].ResetType();
		}
	}

	public List<csNode> GetNeighbours(csNode _node, int _start, int _unit)
	{
		//Debug.Log("GetNEighbours");
		Point CheckA = new Point(_unit, _unit);
		Point CheckB = new Point(_unit - 1, _unit);
		Point CheckC = new Point(_unit, _unit - 1);
		List<csNode> Neighbours = new List<csNode>();
		//Debug.Log("Start : " + _start + "  " + "Unit : " + _unit);
		m_AddNeighbours = (_x, _y) =>
		{
			Point Check = new Point(Mathf.Abs(_x), Mathf.Abs(_y));

			if (Check == new Point(0, 0))
			{
				return;				
			}

			if(_unit > 1)
			{
				if(Check == CheckA || Check == CheckB || Check == CheckC)
				{
					//Debug.Log("@@ : " + Check.x + "  " + Check.y);
					return;
				}				
			}

			int CheckX = _node.m_Grid.x + _x;
			int CheckY = _node.m_Grid.y + _y;

			if (CheckX >= 0 && CheckX < m_GridSize.x && CheckY >= 0 && CheckY < m_GridSize.y)
			{ 
				Neighbours.Add(m_Grid[CheckX, CheckY]);
			}
		};

		int TempIncBreak = 0;

		if (_start <= 1)
		{
			_start = 0;
			TempIncBreak = 1;
		}
		else
		{
			TempIncBreak = _start - 1;
		}

		int Sum = (_unit * 2) + 1;
		Sum -= _start;

		int Temp = 0;
		int Inc = 1;	
				
		int x = _start - 1;
		x = Mathf.Clamp(x, 0, x);
		int y = _start - 1;
		y = Mathf.Clamp(y, 0, y);

		while (Sum > 0)
		{
			if (Sum > TempIncBreak)
			{
				Temp++;
			}

			for (int i = 0; i < Temp; i++)
			{
				x += Inc;

				m_AddNeighbours(x, y);			
			}

			if(Temp == 1 && _start > 1)
			{
				Temp = (_start * 2) - 1;
			}

			if(Sum == TempIncBreak)
			{
				break;
			}

			Inc *= -1;

			for (int i = 0; i < Temp; i++)
			{
				y += Inc;

				m_AddNeighbours(x, y);
			}

			Sum--;
		}

		return Neighbours;
	}
	
	public List<csNode> GetNeighbours(csNode _node)
	{
		//Debug.Log("GetNEighbours");
		List<csNode> Neighbours = new List<csNode>();

		for(int x = -1; x <= 1; x++)
		{
			for(int y = -1; y <= 1; y++)
			{
				if(x == 0 && y == 0)
				{
					continue;
				}
				//Debug.Log("!!!!!");
				int CheckX = _node.m_Grid.x + x;
				int CheckY = _node.m_Grid.y + y;

				if(CheckX >= 0 && CheckX < m_GridSize.x && CheckY >= 0 && CheckY < m_GridSize.y)
				{
					Neighbours.Add(m_Grid[CheckX, CheckY]);
				}
			}
		}

		return Neighbours;
	}

	//public GameObject m_Player;
	//public float m_TestFloat;
	public csNode NodeFromWorldPoint(Vector3 _worldPosition)
	{
		_worldPosition.x -= m_Transform.position.x;
		_worldPosition.z -= m_Transform.position.z;
		float PercentX = (_worldPosition.x + m_GridWorldSize.x / 2.0f) / m_GridWorldSize.x;
		float PercentY = (_worldPosition.z + m_GridWorldSize.y / 2.0f) / m_GridWorldSize.y;
		//Debug.Log("PercentX : " + PercentX);
		//Debug.Log("PercentY : " + PercentY);
		PercentX = Mathf.Clamp01(PercentX);
		PercentY = Mathf.Clamp01(PercentY);
		//Debug.Log("Clamp01 PercentX : " + PercentX);
		//Debug.Log("Clamp01 PercentY : " + PercentY);
		int X = Mathf.RoundToInt(m_GridSize.x * PercentX);
		int Y = Mathf.RoundToInt(m_GridSize.y * PercentY);
        //Debug.Log("X : " + X);
        //Debug.Log("Y : " + Y);

		if(X == 158)
        {
			X = 157;
		}

		if(Y == 120)
        {
			Y = 119;
        }
        try
        {
			csNode Node = m_Grid[X, Y];
		}
		catch(IndexOutOfRangeException)
        {
			Debug.LogError("X : " + X + "  " + "Y : " + Y + "  " + _worldPosition);
        }	

		return m_Grid[X, Y];
	}
	public float m_Test2;
	private void OnDrawGizmos()
	{
		/*Gizmos.color = Color.red;

		Vector3 RelativePos = m_Target.position - m_Transform.position;
		float Distance = RelativePos.magnitude;
		Vector3 Dir = RelativePos / Distance;

		Gizmos.DrawRay(m_Transform.position, Dir * m_Test);
		Vector3 RelativePos = m_Target.position - m_Transform.position;
		float Distance = RelativePos.magnitude;
		Vector3 Dir = RelativePos / Distance;
		DebugExtension.DrawCapsule(m_Transform.position, m_Target.position);*/
		

		if (m_Grid != null && m_bIsDisplayGridGizmos)
		{
			Gizmos.DrawWireCube(m_Transform.position, new Vector3(m_GridWorldSize.x, m_fNodeRadius, m_GridWorldSize.y));		

			//csNode PlayrNode = NodeFromWorldPoint(m_Target.transform.position);
			foreach (csNode n in m_Grid)
			{
				if (!m_bIsDisplayWalkable && n.NodeType == eNodeType.Walkable)
				{
					continue;
				}

				Color NodeColor = n.Color;
				NodeColor.a = m_fNodeColorAlpha;
				Gizmos.color = NodeColor;

				/*if(PlayrNode == n)
				{
					Gizmos.color = Color.red;
				}*/

				Gizmos.DrawCube(n.m_WorldPosition, Vector3.one * (m_fNodeDiameter - m_Test2));
			}
		}
	}
}
