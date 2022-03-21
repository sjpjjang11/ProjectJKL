using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectShake
{
	public enum VectorType
	{
		Vector2,
		Vector3
	}

	private VectorType m_VectorType = VectorType.Vector2;

	public void ShakeSettings(VectorType _vectorType)
	{
		m_VectorType = _vectorType;
	}

	public IEnumerator CoShake(Transform _target, float _intensity, float _shakeDuration, Action _callback = null)
	{
		Vector3 OriginPosition = _target.localPosition;

		Vector3 RandomPosition = Vector3.zero;

		while (_shakeDuration > 0.0f)
		{
			RandomPosition = OriginPosition + UnityEngine.Random.insideUnitSphere * _intensity;

			if (m_VectorType.Equals(VectorType.Vector2))
			{
				Vector3 Vector2Position = new Vector3(RandomPosition.x, RandomPosition.y, OriginPosition.z);

				_target.localPosition = Vector3.Lerp(_target.localPosition, Vector2Position, 0.05f);
			}
			else
			{
				_target.localPosition = Vector3.Lerp(_target.position, RandomPosition, 0.05f);
			}

			_shakeDuration -= Time.deltaTime;

			yield return null;
		}

		_target.localPosition = OriginPosition;

		_callback?.Invoke();
	}
}
