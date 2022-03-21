using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountNumber
{
	private IEnumerator m_CoCount = null;
	private Dictionary<int, IEnumerator> m_DicCoroutine = new Dictionary<int, IEnumerator>();

	private const int DefaultCoroutineIndex = -1;

	public void Count<T>(T _current, T _target, float _time, Action<T> _update = null, Action _complete = null)
	{
		if(_time < 0.0f)
		{
			return;
		}

		CountSettings(_current, _target, _time, _update, _complete);
	}

	public void Count<T>(int _index, T _current, T _target, float _time, Action<T> _update = null, Action _complete = null)
	{
		//Debug.Log("Count");
		CountSettings(_current, _target, _time, _update, _complete, _index);
	}

	private void CountSettings<T>(T _current, T _target, float _time, Action<T> _update = null, Action _complete = null, int _index = DefaultCoroutineIndex)
	{
		if (typeof(T) == typeof(float))
		{
			//Debug.Log("typeof(T) == typeof(float");

			float Cur = Utility.To<float>(_current);
			float Tar = Utility.To<float>(_target);

			Action<float> Update = Utility.To<Action<float>>(_update);

			if (Cur == Tar)
			{
				return;
			}

			m_CoCount = CoCount(_index, Cur, Tar, _time, Update, _complete);		
		}

		if (typeof(T) == typeof(Vector2))
		{
			//Debug.Log("typeof(T) == typeof(Vector2)");
			Vector2 Cur = Utility.To<Vector2>(_current);
			Vector2 Tar = Utility.To<Vector2>(_target);

			Action<Vector2> Update = Utility.To<Action<Vector2>>(_update);

			if (Cur == Tar)
			{
				return;
			}

			m_CoCount = CoCount(_index, Cur, Tar, _time, Update, _complete);
		}

		if (typeof(T) == typeof(Vector3))
		{
			//Debug.Log("typeof(T) == typeof(Vector3)");
			Vector3 Cur = Utility.To<Vector3>(_current);
			Vector3 Tar = Utility.To<Vector3>(_target);

			Action<Vector3> Update = Utility.To<Action<Vector3>>(_update);
			//Debug.Log(Cur + "  " + Tar);
			if (Cur == Tar)
			{
				return;
			}

			m_CoCount = CoCount(_index, Cur, Tar, _time, Update, _complete);
		}

		if(typeof(T) == typeof(Quaternion))
		{
			Quaternion Cur = Utility.To<Quaternion>(_current);
			Quaternion Tar = Utility.To<Quaternion>(_target);

			Action<Quaternion> Update = Utility.To<Action<Quaternion>>(_update);

			if (Cur == Tar)
			{
				return;
			}

			m_CoCount = CoCount(_index, Cur, Tar, _time, Update, _complete);
		}

		if (m_CoCount != null)
		{
			if (_index != DefaultCoroutineIndex)
			{
				//Debug.LogError("Add : " + _index);
				m_DicCoroutine.Add(_index, m_CoCount);
			}

			StaticCoroutine.Start(m_CoCount);
		}
	}

	public bool GetCoroutineIndexContains(int _index)
	{
		bool Result = false;
		if (m_DicCoroutine.ContainsKey(_index))
		{
			Result = true;
		}

		return Result;
	}

	public void ForceStop(int _index)
	{
		if (m_DicCoroutine.ContainsKey(_index))
		{
			StaticCoroutine.Stop(m_DicCoroutine[_index]);

			m_DicCoroutine.Remove(_index);
			//Debug.LogError("Remove : " + _index);
		}
	}

	private float GetValueToApplies(float _current, float _target, float _time)
	{		
		return (_target - _current) / _time;
	}

	private float GetMin(float _current, float _target)
	{
		return _target > _current ? _current : _target;
	}

	private float GetMax(float _current, float _target)
	{
		return _target > _current ? _target : _current;
	}

	private IEnumerator CoCount(int _index, float _current, float _target, float _time, Action<float> _update = null, Action _complete = null)
	{
		/*if (Mathf.Sign(_current) != Mathf.Sign(_target))
		{
			_current *= Mathf.Sign(_target);
		}*/

		//Debug.Log("#########CoCount : " + _current + "  " + _target + "  " + _time);
		float ValueToApplies = GetValueToApplies(_current, _target, _time);

		float Min = GetMin(_current, _target);
		float Max = GetMax(_current, _target);

		//Debug.Log("ValueToApplies : " + ValueToApplies);

		float Test = 0.0f;

		while (true)
		{
			yield return null;

			Test += Time.deltaTime;

			_current += ValueToApplies * Time.deltaTime;

			_current = Mathf.Clamp(_current, Min, Max);
			//Debug.Log(_current);
			_update?.Invoke(_current);

			if (_current == _target)
			{
				//Debug.Log("Done : " + _current + "   " + _target + "  " + Test);
				break;
			}
		}

		_complete?.Invoke();

		if (m_DicCoroutine.ContainsKey(_index))
		{
			m_DicCoroutine.Remove(_index);
		}
	}

	private IEnumerator CoCount(int _index, Vector2 _current, Vector2 _target, float _time, Action<Vector2> _update = null, Action _complete = null)
	{
		Vector2 ValueToApplies = new Vector2(
			GetValueToApplies(_current.x, _target.x, _time), 
			GetValueToApplies(_current.y, _target.y, _time));

		Vector2 Min =new Vector2(
			GetMin(_current.x, _target.x),
			GetMin(_current.y, _target.y));

		Vector2 Max = new Vector2(
			GetMax(_current.x, _target.x),
			GetMax(_current.y, _target.y));

		//Debug.Log("ValueToApplies : " + ValueToApplies);

		float Test = 0.0f;

		while (true)
		{
			yield return null;

			Test += Time.deltaTime;

			_current.x += ValueToApplies.x * Time.deltaTime;
			_current.y += ValueToApplies.y * Time.deltaTime;

			_current.x = Mathf.Clamp(_current.x, Min.x, Max.x);
			_current.y = Mathf.Clamp(_current.y, Min.y, Max.y);

			//Debug.Log(_current);
			_update?.Invoke(_current);

			if (_current == _target)
			{
				//Debug.Log("Done : " + _current + "   " + _target + "  " + Test);
				break;
			}
		}

		_complete?.Invoke();

		if (m_DicCoroutine.ContainsKey(_index))
		{
			m_DicCoroutine.Remove(_index);
		}
	}

	private IEnumerator CoCount(int _index, Vector3 _current, Vector3 _target, float _time, Action<Vector3> _update = null, Action _complete = null)
	{
		//Debug.Log("CoCount : " + _current + "  " + _target);
		Vector3 ValueToApplies = new Vector3(
			GetValueToApplies(_current.x, _target.x, _time),
			GetValueToApplies(_current.y, _target.y, _time),
			GetValueToApplies(_current.z, _target.z, _time));
		//Debug.Log("ValueToApplies : " + ValueToApplies);
		Vector3 Min = new Vector3(
			GetMin(_current.x, _target.x),
			GetMin(_current.y, _target.y),
			GetMin(_current.z, _target.z));
		//Debug.Log("Min : " + Min);
		Vector3 Max = new Vector3(
			GetMax(_current.x, _target.x),
			GetMax(_current.y, _target.y),
			GetMax(_current.z, _target.z));
		//Debug.Log("Max : " + Max);
		//Debug.Log("ValueToApplies : " + ValueToApplies);
	
		float Test = 0.0f;

		while (true)
		{
			yield return null;

			Test += Time.deltaTime;

			_current.x += ValueToApplies.x * Time.deltaTime;
			_current.y += ValueToApplies.y * Time.deltaTime;
			_current.z += ValueToApplies.z * Time.deltaTime;

			_current.x = Mathf.Clamp(_current.x, Min.x, Max.x);
			_current.y = Mathf.Clamp(_current.y, Min.y, Max.y);
			_current.z = Mathf.Clamp(_current.z, Min.z, Max.z);

			//Debug.Log(_current + " " + _target);
			_update?.Invoke(_current);

			if (_current == _target)
			{
				//Debug.Log("Done : " + _current + "   " + _target + "  " + Test);
				break;
			}
		}

		if (m_DicCoroutine.ContainsKey(_index))
		{
			//Debug.LogError("Remove : " + _index);
			m_DicCoroutine.Remove(_index);
		}

		//Debug.Log("**************** : " + _complete);
		_complete?.Invoke();
	}

	private IEnumerator CoCount(int _index, Quaternion _current, Quaternion _target, float _time, Action<Quaternion> _update = null, Action _complete = null)
	{
		Quaternion ValueToApplies = new Quaternion(
			GetValueToApplies(_current.x, _target.x, _time),
			GetValueToApplies(_current.y, _target.y, _time),
			GetValueToApplies(_current.z, _target.z, _time),
			0.0f);

		Quaternion Min = new Quaternion(
			GetMin(_current.x, _target.x),
			GetMin(_current.y, _target.y),
			GetMin(_current.z, _target.z),
			GetMin(_current.w, _target.w));

		Quaternion Max = new Quaternion(
			GetMax(_current.x, _target.x),
			GetMax(_current.y, _target.y),
			GetMax(_current.z, _target.z),
			GetMax(_current.w, _target.w));

		//Debug.Log("ValueToApplies : " + ValueToApplies);

		float Test = 0.0f;

		while (true)
		{
			yield return null;

			Test += Time.deltaTime;

			_current.x += ValueToApplies.x * Time.deltaTime;
			_current.y += ValueToApplies.y * Time.deltaTime;
			_current.z += ValueToApplies.z * Time.deltaTime;

			_current.x = Mathf.Clamp(_current.x, Min.x, Max.x);
			_current.y = Mathf.Clamp(_current.y, Min.y, Max.y);
			_current.z = Mathf.Clamp(_current.z, Min.z, Max.z);

			//Debug.Log(_current);
			_update?.Invoke(_current);

			if (_current == _target)
			{
				//Debug.Log("Done : " + _current + "   " + _target + "  " + Test);
				break;
			}
		}

		_complete?.Invoke();

		if (m_DicCoroutine.ContainsKey(_index))
		{
			m_DicCoroutine.Remove(_index);
		}
	}

	public IEnumerator CoGraduallyCount(float _current, float _target, float speed, float _time, Action<float> _update = null, Action _complete = null)
	{
		float StartDistance = Mathf.Abs(_target - _current);

		float CurrentDistance = 0.0f;

		float Test = 0.0f;

		bool IsSlow = false;

		while (true)
		{
			yield return null;

			Test += Time.deltaTime;

			_current = Mathf.MoveTowards(_current, _target, speed * Time.deltaTime);
			//Debug.Log(_current + "    " + Test + "    " + speed);

			CurrentDistance = Mathf.Abs(_target - _current);

			if (!IsSlow && CurrentDistance <= StartDistance * 0.5f)
			{
				IsSlow = true;

				StaticCoroutine.Start(CoCount(DefaultCoroutineIndex, speed, 0.0f, 1.5f, (value) =>
				{
					speed = value;
				}));
			}

			_update?.Invoke(_current);

			if (_current == _target)
			{
				break;
			}
		}

		_complete?.Invoke();
	}

	public IEnumerator CoGraduallyIncrease(float _currentValue, float _targetValue, float _time, Action<float> _update = null, Action _complete = null)
	{
		float ValueToApplies;
		float GraduallyTime = _time * 0.25f;
		float DiffValue = _targetValue - _currentValue;
		float ValueToAdd;
		float TestDiff = 0.0f;
		//Debug.Log("@@Start : " + GraduallyTime + "  " + _time);
		StaticCoroutine.Start(CoCount(DefaultCoroutineIndex, GraduallyTime, _time, GraduallyTime, (value) =>
		{
			GraduallyTime = value;
		}));
		float Test = 0.0f;
		
		while (_currentValue < _targetValue)
		{
			yield return null;

			ValueToAdd = DiffValue / GraduallyTime;

			_currentValue += ValueToAdd * Time.deltaTime;

			ValueToApplies = Mathf.Clamp(_currentValue, _currentValue, _targetValue);

			_update?.Invoke(ValueToApplies);

			Test += Time.deltaTime;
			TestDiff = ValueToApplies - TestDiff;
			//Debug.Log(TestDiff + "  " + ValueToAdd * Time.deltaTime + "   " + ValueToApplies + "   " + GraduallyTime + "     " + Test);

			TestDiff = ValueToApplies;
		}

		_complete?.Invoke();
	}
}
