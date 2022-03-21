using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugsStateTypeCompare : IEqualityComparer<eBugsStateType>
{
	public bool Equals(eBugsStateType _x, eBugsStateType _y)
	{
		return (int)_x == (int)_y;
	}

	public int GetHashCode(eBugsStateType _obj)
	{
		return (int)_obj;
	}
}

public class TouchEventCompare : IEqualityComparer<TouchEvent>
{
	public bool Equals(TouchEvent _x, TouchEvent _y)
	{
		return (int)_x == (int)_y;
	}

	public int GetHashCode(TouchEvent _obj)
	{
		return (int)_obj;
	}
}

public class MoveActionIndexCompare : IEqualityComparer<MoveActionIndex>
{
	public bool Equals(MoveActionIndex _x, MoveActionIndex _y)
	{
		return _x.AnimationIndex == _y.AnimationIndex && _x.MoveActionInfoIndex == _y.MoveActionInfoIndex;
	}

	public int GetHashCode(MoveActionIndex _obj)
	{
		int HashCode = _obj.AnimationIndex ^ _obj.MoveActionInfoIndex;

		return HashCode.GetHashCode();
	}
}

public class FloatCompare : IEqualityComparer<float>
{
	public bool Equals(float _x, float _y)
	{
		return _x == _y;
	}

	public int GetHashCode(float _obj)
	{
		return (int)_obj;
	}
}

public class TypeCompare
{

}
