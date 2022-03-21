using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class csStateHandler_Multi : csStateHandler
{ 
	private Vector3 ActionDirection
	{
		get
		{
			return Owner.m_EventHandler_Hero.ActionDirection.Get();
		}
	}

	public float m_fDistanceFromPrevToCurrentActionDirection = 0.0f;
	public float m_fActionableSkill_DefaultValue = 0.0f;
	public float m_fAttackStartTime = 0.2f;
	public float m_fAttackCurrentStartTime = 0.0f;

	public bool m_bIsAttack;
	public bool IsAttack
	{
		set
		{
			m_bIsAttack = value;
			Owner.m_EventHandler_Hero.IsAttack.Set(value);
		}
	}

	public bool IsStickNeutral
	{
		get
		{
			if(ActionDirection == Vector3.zero)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();

#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)

		m_fActionableSkill_DefaultValue = 1.0f;

#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX

		m_fActionableSkill_DefaultValue = 1.0f;

#elif UNITY_ANDROID || UNITY_IOS

        m_fActionableSkill_DefaultValue = 70.0f;

#endif
	}

	public override void Settings(csInfo_Hero _info)
	{
		base.Settings(_info);
		
		Owner.m_EventHandler_Hero.Register(this);
	}

	#region Command Method

	private void OnCommand_IsAttackSettings(bool _isAttack)
	{
		IsAttack = _isAttack;
	}

	#endregion

	protected virtual void TouchBegan()
	{
		IsAttack = true;
	}

	protected virtual void TouchMoved()
	{
		if(Owner.IsActiveState(eBugsStateType.Attack))
		{
			Owner.StateForceStop(eBugsStateType.Attack);
		}

		UserTouch Touch = Owner.m_EventHandler_Hero.Touch.Get();

		m_fDistanceFromPrevToCurrentActionDirection = Touch.m_DeltaPosition.magnitude;
	}

	protected virtual void TouchStationary()
	{
		if (IsStickNeutral)
		{
			if (m_fAttackCurrentStartTime >= m_fAttackStartTime)
			{
				if (!Owner.IsActiveState(eBugsStateType.Attack))
				{
					Owner.StateStart(eBugsStateType.Attack);
				}

				IsAttack = true;
			}

			m_fAttackCurrentStartTime += Time.deltaTime;
		}
		else
		{
			IsAttack = false;
			m_fAttackCurrentStartTime = 0.0f;
		}
	}

	protected virtual void TouchEnded()
	{
		if(IsStickNeutral)
		{
			Owner.StateStart(eBugsStateType.Attack);
		}

		if (m_fDistanceFromPrevToCurrentActionDirection >= m_fActionableSkill_DefaultValue)
		{
			Owner.m_EventHandler_Hero.ActionDirection.Set(ActionDirection);

			Owner.StateStart(eBugsStateType.Skill_Default);
		}

		m_fAttackCurrentStartTime = 0.0f;
		m_fDistanceFromPrevToCurrentActionDirection = 0.0f;
	}
}
