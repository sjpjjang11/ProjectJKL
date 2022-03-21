using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csActivity_Dash : csActivity
{
	private ObjectPool m_DashEffectPool = null;

	private int m_iDefaultPoolCount = 3;

	public override void Settings(csOwner _owner)
	{
		base.Settings(_owner);

		m_DashEffectPool = new ObjectPool();
		GameObject DashEffect = Resources.Load("Prefabs/HeroRelated/Effect_Common/DashBlue") as GameObject;
		m_DashEffectPool.CreatePool(DashEffect, m_Owner.m_WaitingPool_Effect, m_iDefaultPoolCount);
	}

	public override void MoveStart(Vector3 _direction, float _distance, float _moveTime, bool _isSlip, Action _updateCallback = null, Action _completeCallback = null)
	{
		int ObjectIndex = m_DashEffectPool.UseObject(m_Owner.Transform.position + Vector3.up + m_Owner.Transform.forward * 4.0f, false);

		Quaternion Rotation = Quaternion.LookRotation(-m_Owner.Transform.forward);
		m_DashEffectPool.m_ListObject[ObjectIndex].transform.rotation = Rotation;
		base.MoveStart(_direction, _distance, _moveTime, _isSlip, _updateCallback, _completeCallback);
	}
}
