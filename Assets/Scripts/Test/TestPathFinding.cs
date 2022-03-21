using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;

public class TestPathFinding : MonoBehaviour
{
	private IEnumerator m_CoFindPath = null;

	private List<csNode> m_ListExceptionNode = null;

	private csPathRequestManager m_RequestManager;

	private csGrid m_Grid = null;
	private csNode m_StartNode = null;
	public csNode m_EndNode = null;

	public Vector3 m_EndPosition;

	//public bool m_bIsTest;

	private void Awake()
	{
		m_ListExceptionNode = new List<csNode>();
	}

	protected virtual void Start()
	{
		m_Grid = csPathRequestManager.Instance.GetComponent<csGrid>();
		m_RequestManager = csPathRequestManager.Instance;
	}

	public void Initialize()
	{
		m_ListExceptionNode.Clear();
	}

	public void StartFindPath(Vector3 _startPosition, Vector3 _endPosition, csNode _exceptionNode)
	{
		//m_Grid.CreateGrid();
		if (m_CoFindPath != null)
		{
			//Debug.LogError("StopCoroutine");
			StopCoroutine(m_CoFindPath);
		}

		m_CoFindPath = CoFindPath(_startPosition, _endPosition, _exceptionNode);
		StartCoroutine(m_CoFindPath);
	}

	private IEnumerator CoFindPath(Vector3 _startPosition, Vector3 _endPosition, csNode _exceptionNode)
	{
		//m_Grid.ResetGrid();

		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		sw.Start();

		if (_exceptionNode != null)
		{
			if (!m_ListExceptionNode.Contains(_exceptionNode))
			{
				//Debug.Log("Exception ADD");
				m_ListExceptionNode.Add(_exceptionNode);
			}
		}

		List<csNode> ListPathNodes = new List<csNode>();
		Vector3[] WayPoints = new Vector3[0];
		bool IsPathSuccess = false;

		if (m_StartNode != null)
		{
			m_StartNode.ResetType(gameObject.GetInstanceID());
		}

		if (m_EndNode != null)
		{
			m_EndNode.ResetType(gameObject.GetInstanceID());
		}

		m_StartNode = m_Grid.NodeFromWorldPoint(_startPosition);

		int DefaultCount = 3;
		int Count = (DefaultCount * DefaultCount) - 1;

		List<csNode> List = m_Grid.NodeFromWorldPoint(_endPosition).m_ListNeighbours;
		m_EndPosition = Utility.RelativePosition(m_Grid.NodeFromWorldPoint(_endPosition).m_WorldPosition, _endPosition);
		m_EndPosition.y = 0.0f;

		List.Sort(delegate (csNode _a, csNode _b)
		{
			float DistanceA = Utility.Distance(_a.m_WorldPosition, _startPosition);
			float DistanceB = Utility.Distance(_b.m_WorldPosition, _startPosition);

			return DistanceA.CompareTo(DistanceB);
		});

		for (int i = 0; i < List.Count; i++)
		{
			//Debug.Log(Utility.Distance(List[i].m_WorldPosition, _startPosition));
			/*GameObject Obj = Instantiate(Resources.Load("Prefabs/Cube") as GameObject);
			Obj.transform.position = List[i].m_WorldPosition;
			Obj.name = i.ToString() + "  " + List[i].m_Grid.x.ToString() + "  " + List[i].m_Grid.y.ToString();*/

			if (m_ListExceptionNode.Contains(List[i]))
			{
				//Debug.LogError("@@@@@@@@@@@ : " + List[i].m_WorldPosition);
				continue;
			}

			if (List[i].NodeType == eNodeType.Walkable)
			{
				Debug.Log("End : " + i + "  " + List[i].m_WorldPosition + "  " + name);
				m_EndNode = List[i];
				m_EndNode.SetType(gameObject.GetInstanceID(), eNodeType.End);

				break;
			}
		}

		//Debug.Log(m_StartNode.NodeType + "  ||  " + m_EndNode.NodeType);
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

				for (int i = 0; i < Node.m_ListNeighbours.Count; i++)
				{
					csNode neighbour = Node.m_ListNeighbours[i];
					/*if (m_bIsTest)
					{
						GameObject Obj = Instantiate(Resources.Load("Prefabs/Cube") as GameObject);
						Obj.transform.position = neighbour.m_WorldPosition;
					}*/

					if (CloseSet.Contains(neighbour))
					{
						continue;
					}

					eNodeType Type = neighbour.NodeType;

					if (Type == eNodeType.Obstacle)
					{
						CloseSet.Add(neighbour);

						continue;
					}

					if (Type == eNodeType.End && neighbour.m_iOwner != gameObject.GetInstanceID())
					{
						CloseSet.Add(neighbour);

						continue;
					}

					csNode NodeX = m_Grid.m_Grid[neighbour.m_Grid.x, Node.m_Grid.y];
					csNode NodeY = m_Grid.m_Grid[Node.m_Grid.x, neighbour.m_Grid.y];

					if (NodeX.NodeType != eNodeType.Walkable && NodeY.NodeType != eNodeType.Walkable)
					{
						if (Node.m_Grid.x != neighbour.m_Grid.x && Node.m_Grid.y != neighbour.m_Grid.y)
						{
							/*Debug.LogError("!!!! : " + name + "  " + new Vector2(neighbour.m_Grid.x, neighbour.m_Grid.y) + "  " + new Vector2(NodeX.m_Grid.x, NodeX.m_Grid.y));
							Debug.LogError("!!!! : " + name + "  " + new Vector2(Node.m_Grid.x, Node.m_Grid.y) + "  " + new Vector2(NodeY.m_Grid.x, NodeY.m_Grid.y));
							GameObject Obj = Instantiate(Resources.Load("Prefabs/Cube") as GameObject);
							Obj.transform.position = neighbour.m_WorldPosition;
							Obj = Instantiate(Resources.Load("Prefabs/Cube") as GameObject);
							Obj.transform.position = Node.m_WorldPosition;*/
							CloseSet.Add(neighbour);

							continue;
						}
					}

					/*int LayerMask = Layer.CollisionLayerMask(csBattleManager.CollisionLayer.Monster);

					if(Physics.CheckBox(neighbour.m_WorldPosition, Vector3.one * 0.5f, Quaternion.identity, LayerMask))
					{
						CloseSet.Add(neighbour);
						Debug.Log("!!!!!!");
						continue;
					}*/

					if (csPathRequestManager.Instance.AdditionalSuccessCallback(neighbour))
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
		if (IsPathSuccess)
		{
			ListPathNodes = RetracePath(m_StartNode, m_EndNode);

			List<Vector3> ListWayPoint = new List<Vector3>();
			for (int i = 0; i < ListPathNodes.Count; i++)
			{
				/*if (m_bIsTest)
				{
					ListPathNodes[i].SetType(gameObject.GetInstanceID(), eNodeType.Path);
				}*/

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

		/*if (m_bIsTest)
		{
			EditorApplication.isPaused = true;
		}*/

		csPathRequestManager.Instance.FinishedProcessingPath(ListPathNodes, WayPoints, IsPathSuccess);

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

		while (CurrentNode != _startNode)
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

		for (int i = 1; i < _path.Count; i++)
		{
			Vector2 DirectionNew = new Vector2(_path[i - 1].m_Grid.x - _path[i].m_Grid.x, _path[i - 1].m_Grid.y - _path[i].m_Grid.y);

			if (DirectionNew != DirectionOld)
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

		if (DistanceX > DistanceY)
		{
			return 14 * DistanceY + 10 * (DistanceX - DistanceY);
		}

		return 14 * DistanceX + 10 * (DistanceY - DistanceX);

	}
}

