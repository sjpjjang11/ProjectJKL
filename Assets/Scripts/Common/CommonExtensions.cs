using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class ActivationExtensions
{
	public static void SetActive(this Behaviour _behaviour, bool _value)
	{
		if (IsActive(_behaviour) == _value)
		{
			return;
		}

		_behaviour.enabled = _value;
	}

	public static void SetActive(this Renderer _renderer, bool _value)
	{
		if (IsActive(_renderer) == _value)
		{
			return;
		}

		_renderer.enabled = _value;
	}

	public static void SetActive(this Collider _collider, bool _value)
	{
		if (IsActive(_collider) == _value)
		{
			return;
		}

		_collider.enabled = _value;
	}

	public static bool IsActive(this GameObject _gameObject)
	{
		return _gameObject.activeSelf;
	}

	public static bool IsActive(this Behaviour _behaviour)
	{
		return _behaviour.enabled;
	}

	public static bool IsActive(this Renderer _renderer)
	{
		return _renderer.isVisible;
	}

	public static bool IsActive(this Collider _collider)
	{
		return _collider.enabled;
	}	
}

public static class RectTransformExtensions
{
	public static void SetLeft(this RectTransform rt, float left)
	{
		rt.offsetMin = new Vector2(left, rt.offsetMin.y);
	}

	public static void SetRight(this RectTransform rt, float right)
	{
		rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
	}

	public static void SetTop(this RectTransform rt, float top)
	{
		rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
	}

	public static void SetBottom(this RectTransform rt, float bottom)
	{
		rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
	}
}

public static class EnumerableExtension
{
	private static readonly System.Random Random = new System.Random();

	public static IEnumerable<int> GetRange(int _start, int _count)
	{
		IEnumerable<int> Range = Enumerable.Range(_start, _count);
		
		return Range;
	}

	public static IEnumerable<int> GetRandomRange(int _start, int _count)
	{
		IEnumerable<int> Enumerable = GetRange(_start, _count);

		return Enumerable.Shuffle();
	}

	public static int GetRandom(this int[] _range)
	{
		int Index = Random.Next(0, _range.Length);

		return _range[Index];
	}

	public static int[] GetRandomRange(int _start, int _end, int _count)
	{
		List<int> List_Range = GetRange(_start, _end - _start).ToList();

		List<int> List_Result = new List<int>();

		for(int i = 0; i < _count; i++)
        {
			int Index = Random.Next(_start, List_Range.Count);

			List_Result.Add(List_Range[Index]);

			List_Range.RemoveAt(Index);
		}

		return List_Result.ToArray();
	}



	public static IEnumerable<int> Shuffle(this IEnumerable<int> _enumerable)
	{
		System.Random Random = new System.Random();

		_enumerable = _enumerable.OrderBy(x => Random.Next());

		return _enumerable;
	}

	///<summary>
	/// 생성한 범위 내에서 값을 제외한 후 무작위 값 추출
	/// </summary>
	/// <param name="_start">생성할 범위의 시작값</param>
	/// <param name="_count">생성할 범위의 개수</param>
	/// <param name="_exclusion">제외할 값</param>
	/// <returns></returns>
	public static int GetRandom_Exclusion(int _start, int _count, params int[] _exclusion)
	{
		int[] Range = GetRange(_start, _count).Where(Value => !_exclusion.Contains(Value)).ToArray();
		
		int Index = Random.Next(0, Range.Length);

		return Range[Index];
	}

	/// <summary>
	/// 제공 받는 범위에서 값을 제외한 후 무작위 값 추출
	/// </summary>
	/// <param name="_range">값을 추출할 범위</param>
	/// <param name="_exclusion">제외할 값</param>
	/// <returns></returns>
	public static int GetRandom_Exclusion(this IEnumerable<int> _range, params int[] _exclusion)
	{
		int[] Range = _range.Where(Value => !_exclusion.Contains(Value)).ToArray();

		int Index = Random.Next(0, Range.Length);

		return Range[Index];
	}

	/// <summary>
	/// 생성한 범위 내에서 값을 제외한 후 반환
	/// </summary>
	/// <param name="_start">생성할 범위의 시작값</param>
	/// <param name="_count">생성할 범위의 개수</param>
	/// <param name="_exclusion">제외할 값</param>
	/// <returns></returns>
	public static int[] GetRange_Exclusion(int _start, int _count, params int[] _exclusion)
	{
		return GetRange(_start, _count).Where(Value => !_exclusion.Contains(Value)).ToArray();
	}

	/// <summary>
	/// 제공 받는 범위에서 값을 제외한 후 반환
	/// </summary>
	/// <param name="_range">값을 추출할 범위</param>
	/// <param name="_exclusion">제외할 값</param>
	/// <returns></returns>
	public static int[] GetRange_Exclusion(this IEnumerable<int> _range, params int[] _exclusion)
	{
		int[] Range = _range.Where(Value => !_exclusion.Contains(Value)).ToArray();

		return Range;
	}

	public static void Shuffle<T>(this List<T> _list)
	{
		System.Random Random = new System.Random();

		int Count = _list.Count;

		while (Count > 1)
		{
			Count--;
			int i = Random.Next(Count + 1);
			T Value = _list[i];
			_list[i] = _list[Count];
			_list[Count] = Value;
		}
	}

	public static void Shuffle<T>(this T[] _array)
	{
		System.Random Random = new System.Random();

		int Length = _array.Length;

		while (Length > 1)
		{
			Length--;
			int i = Random.Next(Length + 1);
			T value = _array[i];
			_array[i] = _array[Length];
			_array[Length] = value;
		}
	}
}

public static class DictionaryExtensions
{
	public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
	{
		return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
	}

	public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValueProvider)
	{
		return dictionary.TryGetValue(key, out TValue value) ? value : defaultValueProvider();
	}
}

public static class StaticExtensions
{
	public static bool Contains(this string _string, char _value)
	{
		return _string.IndexOf(_value) >= 0;
	}
}
