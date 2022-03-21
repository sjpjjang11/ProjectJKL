using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectK;
using System;

public class csRotationToTarget : MonoBehaviour
{
	private IEnumerator m_CoRotationToTarget = null;

	private csBattleManager m_BattleManager = null;

	private csOwner m_Owner = null;

	public float m_fLookRotationSpeed = 50.0f;

	private void Start()
	{
		m_BattleManager = csBattleManager.Instance;
	}

	public void Settings(csOwner _owner)
	{
		if(_owner.GetType().Equals(typeof(csOwner_Monster)))
		{
			m_fLookRotationSpeed = 10.0f;
		}

		m_Owner = _owner;
	}

	public void Rotation(Transform _tr, Vector3 _target, float _speed = 0.0f, Action _callback = null)
	{
		//Debug.Log("Rotation");
		Quaternion Rotation;

		float RotationSpeed = 0.0f;

		Rotation = Quaternion.LookRotation(_target.normalized);
		//Rotation = new Quaternion(0.0f, Mathf.Abs(Rotation.y), 0.0f, Mathf.Abs(Rotation.w));

		if (_speed == 0.0f)
		{
			RotationSpeed = m_fLookRotationSpeed;
		}
		else
		{
			RotationSpeed = _speed;
		}

		if (m_CoRotationToTarget != null)
		{
			StopCoroutine(m_CoRotationToTarget);
		}

		m_CoRotationToTarget = CoRotationToTarget(_tr, Rotation, RotationSpeed, () =>
		{
			_callback?.Invoke();
		});

		StartCoroutine(m_CoRotationToTarget);
	}

	private void Convert(float _from, float _target)
	{
		float From = Mathf.Sign(_from);
		float Target = Mathf.Sign(_target);

		if(From != Target)
		{

		}
	}

	//public Quaternion m_Tr;
	//public Quaternion m_Target;
	//public float m_Distance;
	//public float m_FloorDistance;
	//public float m_Angle;
	private IEnumerator CoRotationToTarget(Transform _tr, Quaternion _target, float _speed, Action _complete = null)
	{
		float Angle = 0.0f;
		//m_Target = _target;
		//float LifeTime = 3.0f;
		while (true)
		{
			yield return null;
			_tr.rotation = Quaternion.Lerp(_tr.rotation, _target, _speed * Time.deltaTime);
			Angle = Quaternion.Angle(_tr.rotation, _target);
			//m_Angle = Angle;
			//m_Tr = _tr.rotation;
			//m_Distance = _tr.rotation.eulerAngles.y - _target.eulerAngles.y;
			float FAngle = Mathf.Floor(Angle);
			//m_FloorDistance = Mathf.Floor(m_Distance);
			//Debug.Log(_tr.name + "  (" + _tr.rotation.y + ", " + _tr.rotation.w + ") (" + _target.y + ", " + _target.w + ")  " + Angle + "  " +  FAngle);
			//Debug.Log(_tr.name + "  " + _tr.rotation.eulerAngles.y + "  " + _target.eulerAngles.y + "  " + Angle + "  " + FAngle);
			//Debug.Log(_tr.rotation + "  " + _target + "  " + Quaternion.Dot(_tr.rotation, _target));
			/*if (_tr.rotation.eulerAngles.y == _target.eulerAngles.y)
			{
				_complete?.Invoke();

				break;
			}*/
			if (FAngle == 0.0f)
			{
				_complete?.Invoke();

				break;
			}

			//LifeTime -= Time.deltaTime;
		}
	}
}

