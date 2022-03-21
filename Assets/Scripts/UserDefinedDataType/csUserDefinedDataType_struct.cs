using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BugsCollider
{
	public Vector3 Center;
	public float Radius;
	public float Height;
}

[Serializable]
public struct BugsHealth
{
	public int Max;
	public int Cur;
}

[Serializable]
public struct BugsPower
{
	public int Min;
	public int Max;
	public int Limit_Min;
	public int Limit_Max;
}

public struct AIActingPower
{
	public int Max;
	public int Cur;
	public int Use;
}

public struct AIDetection
{
	public float FOVAngle;
	public float FOVRange;
}

[Serializable]
public struct Point : IEquatable<Point>
{
	public int x;
	public int y;

	public Point(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override bool Equals(object obj)
	{
		if (obj is Point)
		{
			return this.Equals((Point)obj);
		}
		return false;
	}

	public bool Equals(Point p)
	{
		return (x == p.x) && (y == p.y);
	}

	public override int GetHashCode()
	{
		return x ^ y;
	}

	public static bool operator ==(Point lhs, Point rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Point lhs, Point rhs)
	{
		return !(lhs.Equals(rhs));
	}
}

public struct PathRequest
{
	public Vector3 PathStart;
	public Vector3 PathEnd;

	public csAIPathFinding PathFinding;
	public csNode ExceptionNode;

	public Action<List<csNode>, Vector3[], bool> SuccessCallback;
	public Func<csNode, bool> SuccessCallback_Additional;

	public PathRequest(csAIPathFinding _pathFinding, Vector3 _start, Vector3 _end, csNode _exceptionNode, Action<List<csNode>, Vector3[], bool> _success, Func<csNode, bool> _success_additional = null)
	{
		PathFinding = _pathFinding;
		PathStart = _start;
		PathEnd = _end;
		ExceptionNode = _exceptionNode;
		SuccessCallback = _success;
		SuccessCallback_Additional = _success_additional;
	}
}

[Serializable]
public struct PrevObstacle
{
	public GameObject Obj;
	public Material Material;
}

public struct MoveActionIndex
{
	public int AnimationIndex;
	public int MoveActionInfoIndex;

	public MoveActionIndex(int _animationIndex, int _moveActionInfoIndex)
	{
		AnimationIndex = _animationIndex;
		MoveActionInfoIndex = _moveActionInfoIndex;
	}
}

public struct MoveActionInfo
{
	public eDirectionType DirectionType;
	public float Distance;
	public float Time;

	public MoveActionInfo(eDirectionType _directionType, float _distance, float _time)
	{
		DirectionType = _directionType;
		Distance = _distance;
		Time = _time;
	}
}

public struct CameraViewInfo
{
	public float DefaultDistance;
	public float Forward;
	public float Right;
	public float DefaultHeight;
}

public struct BugsAction
{
	public eBugsStateType StateType;

	public CrowdControl[] CrowdControl;

	public string AnimationName;

	public float Range;
	public float ActivationTime;
	public float CoolTime;

	public int ActionIndex;

	public bool IsActivation;
	public bool IsImmediatelyCoolDown;
}

[Serializable]
public struct JoystickHandlerSettings
{
	public JoystickType Type;
	public bool IsFade;

	public JoystickHandlerSettings(JoystickType _type, bool _isFade)
	{
		Type = _type;
		IsFade = _isFade;
	}
}
