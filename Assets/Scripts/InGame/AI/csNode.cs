using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class csNode : IHeapItem<csNode> {

	public List<csNode> m_ListNeighbours = null;

	public csNode m_Parent;

	public eNodeType NodeType
	{
		get;
		private set;
	}

	private eNodeType m_ResetType;
	private eNodeType m_PrevType;

	public Vector3 m_WorldPosition;

	public Point m_Grid;

	public Color Color
	{
		get
		{
			Color Color;

			switch(NodeType)
			{
				case eNodeType.Start:

					Color = Color.green;

					break;

				case eNodeType.End:

					Color = Color.red;

					break;

				case eNodeType.Obstacle:

					Color = Color.blue;

					break;

				case eNodeType.Current:

					Color = Color.cyan;

					break;

				case eNodeType.Path:

					Color = Color.yellow;

					break;

				default:

					Color = Color.white;

					break;
			}

			return Color;
		}
	}

	public int m_HeapIndex;

	public int G;
	public int H;

	public int F
	{
		get
		{
			return G + H;
		}
	}

	public int m_iOwner;

	public csNode(eNodeType _type, Vector3 _worldPosition, int _gridX, int _gridY)
	{
		m_ListNeighbours = new List<csNode>();

		NodeType = _type;
		m_ResetType = NodeType;
		m_WorldPosition = _worldPosition;
		m_Grid.x = _gridX;
		m_Grid.y = _gridY;
	}	

	public int HeapIndex
	{
		get
		{
			return m_HeapIndex;
		}
		set
		{
			m_HeapIndex = value;
		}
	}

	public int CompareTo(csNode _nodeToCompare)
	{
		int Compare = F.CompareTo(_nodeToCompare.F);

		if(Compare == 0)
		{
			Compare = H.CompareTo(_nodeToCompare.H);
		}

		return -Compare;
	}

	public void SetType(int _index, eNodeType _type)
	{
		if(m_iOwner != 0 && m_iOwner != _index)
		{
			return;
		}

		m_iOwner = _index;
		NodeType = _type;
	}

	public void ResetType()
	{
		if (NodeType == eNodeType.None)
		{
			return;
		}

		NodeType = m_ResetType;
	}

	public void ResetType(int _index)
	{
		if (NodeType == eNodeType.None)
		{
			return;
		}

		if (m_iOwner != _index)
		{
			return;
		}

		m_iOwner = 0;

		NodeType = m_ResetType;
	}
}
