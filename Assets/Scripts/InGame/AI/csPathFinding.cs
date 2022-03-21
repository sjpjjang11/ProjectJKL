using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class csPathFinding : MonoBehaviour
{
	private IEnumerator m_CoFindPath = null;
	private csPathRequestManager m_RequestManager;

	private csGrid m_Grid = null;
	private csNode m_StartNode = null;
	private csNode m_EndNode = null;

	public Vector3 m_EndPosition;

	private void Awake()
	{
		m_RequestManager = GetComponent<csPathRequestManager>();
		m_Grid = GetComponent<csGrid>();
	}

	public void StartFindPath(Vector3 _startPosition, Vector3 _endPosition, csNode _exceptionNode)
	{
		//m_Grid.CreateGrid();
		if(m_CoFindPath != null)
		{
			//Debug.LogError("StopCoroutine");
			StopCoroutine(m_CoFindPath);
		}

		m_CoFindPath = CoFindPath(_startPosition, _endPosition, _exceptionNode);
		StartCoroutine(m_CoFindPath);
	}

	private Vector3 RandomPointOnCircleEdge(Vector3 _endPosition)
	{
		Vector2 RandomCircle = UnityEngine.Random.insideUnitCircle * 1.0f;
		return new Vector3(RandomCircle.x, _endPosition.y, RandomCircle.y) + _endPosition;
	}

	public int m_Start;
	public int m_Unit;
	private IEnumerator CoFindPath(Vector3 _startPosition, Vector3 _endPosition, csNode _exceptionNode)
	{
		//m_Grid.ResetGrid();

		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		sw.Start();

		List<csNode> ListPathNodes = new List<csNode>();
		Vector3[] WayPoints = new Vector3[0];
		bool IsPathSuccess = false;

		if(m_StartNode != null)
		{
			//m_StartNode.ResetType();
		}

		if (m_EndNode != null)
		{
			//m_EndNode.ResetType();
		}

		m_StartNode = m_Grid.NodeFromWorldPoint(_startPosition);

		//_endPosition = RandomPointOnCircleEdge(_endPosition);
		//List<csNode> List = m_Grid.GetNeighbours(m_Grid.NodeFromWorldPoint(_endPosition), m_Start, m_Unit);
		List<csNode> List = m_Grid.NodeFromWorldPoint(_endPosition).m_ListNeighbours;
		m_EndPosition = Utility.RelativePosition(m_Grid.NodeFromWorldPoint(_endPosition).m_WorldPosition, _endPosition);
		m_EndPosition.y = 0.0f;
		
		for(int i = 0; i < List.Count; i++)
		{
			/*GameObject Obj = Instantiate(Resources.Load("Prefabs/Cube") as GameObject);
			Obj.transform.position = List[i].m_WorldPosition;
			Obj.name = i.ToString() + "  " + List[i].m_Grid.x.ToString() + "  " + List[i].m_Grid.y.ToString();*/

			/*if(List[i] != _exceptionNode && List[i].NodeType != NodeType.Current && List[i].NodeType != NodeType.End && List[i].NodeType != NodeType.Obstacle && List[i].NodeType != NodeType.Path)
			{
				//Debug.Log("End : " + i + "  " + List[i].NodeType);
				m_EndNode = List[i];

				break;
			}*/
		}
		//m_EndNode = List.FirstOrDefault(x => x.NodeType != NodeType.Current);

		/*if (m_Grid.NodeFromWorldPoint(_endPosition).NodeType != NodeType.Walkable)
		{
			while (true)
			{
				Debug.LogError("RandomPointOnCircleEdge");
				Vector3 RandomPosition = RandomPointOnCircleEdge(_endPosition);
				
				if (m_Grid.NodeFromWorldPoint(RandomPosition).NodeType == NodeType.Walkable)
				{
					_endPosition = RandomPosition;
					break;
				}
			} 
		}*/

		/*m_EndNode = m_Grid.NodeFromWorldPoint(_endPosition);

		if(m_EndNode.NodeType == NodeType.Current)
		{
			List<csNode> List = m_Grid.GetNeighbours(m_Grid.NodeFromWorldPoint(_endPosition), 2);
			m_EndNode = List.FirstOrDefault(x => x.NodeType != NodeType.Current);
		}*/

		Debug.Log(m_StartNode.NodeType + "  ||  " + m_EndNode.NodeType);
		if (m_EndNode.NodeType != eNodeType.Obstacle)
		{
			//m_StartNode.SetType(NodeType.Start);
			//m_EndNode.SetType(NodeType.End); 

			Heap<csNode> OpenList = new Heap<csNode>(m_Grid.MaxSize);
			HashSet<csNode> CloseSet = new HashSet<csNode>();

			OpenList.Add(m_StartNode);
			//Debug.Log("@@@@ : " + OpenList.Count);
			while (OpenList.Count > 0)
			{
				//.Debug.Log("@@@@");
				csNode Node = OpenList.RemoveFirst();

				CloseSet.Add(Node);

				if (Node == m_EndNode)
				{
					sw.Stop();
					//Debug.Log("Path Found : " + sw.ElapsedMilliseconds + " ms");
					IsPathSuccess = true;
					//RetracePath(StartNode, EndNode);

					break;
				}
				
				for(int i = 0; i < 8; i++)
				{
					csNode neighbour = Node.m_ListNeighbours[i];

					/*if (Node.NodeType == NodeType.Start)
					{
						GameObject Obj = Instantiate(Resources.Load("Prefabs/Cube") as GameObject);
						Obj.transform.position = neighbour.m_WorldPosition;
						Debug.Log("!!!!!!Current : " + i + "  " + neighbour.NodeType + "  " + neighbour.m_WorldPosition);
					}*/

					//i++;
					if (neighbour.NodeType == eNodeType.Obstacle || CloseSet.Contains(neighbour))
					{
						continue;
					}

					if (m_RequestManager.AdditionalSuccessCallback(neighbour))
					{
						continue;
					}

					//neighbour.SetType(NodeType.Path);

					int NewCostNeighbour = Node.G + GetDistance(Node, neighbour);

					if (NewCostNeighbour < neighbour.G || !OpenList.Contains(neighbour))
					{
						neighbour.G = NewCostNeighbour;
						neighbour.H = GetDistance(neighbour, m_EndNode);
						neighbour.m_Parent = Node;

						if (!OpenList.Contains(neighbour))
						{
							OpenList.Add(neighbour);
						}
					}
				}
			}			
		}

		//Debug.Log("ContainsInside : " + i + "   " + IsPathSuccess);

		yield return null;
		Debug.LogError("IsPathSuccess : " + IsPathSuccess);
		if(IsPathSuccess)
		{
			ListPathNodes = RetracePath(m_StartNode, m_EndNode);

			List<Vector3> ListWayPoint = new List<Vector3>();
			for (int i = 0; i < ListPathNodes.Count; i++)
			{
				//Debug.Log(i + "  " + ListPathNodes[i].NodeType);
				//ListPathNodes[0].SetType(NodeType.Path);
				ListWayPoint.Add(ListPathNodes[i].m_WorldPosition);
			}
			WayPoints = ListWayPoint.ToArray();

			//WayPoints = SimplifyPath(ListPathNodes);

			if (WayPoints.Length > 0)
			{
				WayPoints[0] += m_EndPosition;
			}

			Array.Reverse(WayPoints);

			//WayPoints = RetracePath(m_StartNode, m_EndNode);
		}

		m_RequestManager.FinishedProcessingPath(ListPathNodes, WayPoints, IsPathSuccess);

		m_CoFindPath = null;
	}

	/*private Vector3[] RetracePath(csNode _startNode, csNode _endNode)
	{
		List<csNode> PathList = new List<csNode>();

		csNode CurrentNode = _endNode;

		while (CurrentNode != _startNode)
		{
			PathList.Add(CurrentNode);
			//CurrentNode.SetType(NodeType.Path);
			CurrentNode = CurrentNode.m_Parent;
		}

		Vector3[] WayPoints = SimplifyPath(PathList);
		Array.Reverse(WayPoints);
		//WayPoints.Reverse();
		//PathList.Reverse();

		m_Grid.m_ListPath = PathList;

		return WayPoints;
	}*/

	private List<csNode> RetracePath(csNode _startNode, csNode _endNode)
	{
		List<csNode> ListPath = new List<csNode>();

		csNode CurrentNode = _endNode;

		while(CurrentNode != _startNode)
		{
			ListPath.Add(CurrentNode);

			CurrentNode = CurrentNode.m_Parent;
		}

		//Vector3[] WayPoints = SimplifyPath(PathList);
		//Array.Reverse(WayPoints);

		return ListPath;
	}

	private Vector3[] SimplifyPath(List<csNode> _path)
	{
		List<Vector3> WayPoints = new List<Vector3>();
		Vector2 DirectionOld = Vector2.zero;

		for(int i = 1; i < _path.Count; i++)
		{
			Vector2 DirectionNew = new Vector2(_path[i - 1].m_Grid.x - _path[i].m_Grid.x, _path[i - 1].m_Grid.y - _path[i].m_Grid.y);

			if(DirectionNew != DirectionOld)
			{
				WayPoints.Add(_path[i].m_WorldPosition);
			}

			DirectionOld = DirectionNew;
		}

		return WayPoints.ToArray();
	}

	private int GetDistance(csNode _nodeA, csNode _nodeB)
	{
		int DistanceX = Mathf.Abs(_nodeA.m_Grid.x - _nodeB.m_Grid.x);
		int DistanceY = Mathf.Abs(_nodeA.m_Grid.y - _nodeB.m_Grid.y);

		if(DistanceX > DistanceY)
		{
			return 14 * DistanceY + 10 * (DistanceX - DistanceY);
		}

		return 14 * DistanceX + 10 * (DistanceY - DistanceX);
	}
}