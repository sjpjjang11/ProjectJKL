using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csEscapeController : csJoystickHandler_ByState
{
	private IEnumerator m_CoEmptiedDependingOnTheTime = null;

	public Image m_FilledImage = null;

	public Animator m_Animator = null;

	public override void Activate()
	{
		base.Activate();

		m_FilledImage.fillAmount = 1.0f;

		float EscapeableTime = m_Info_Hero.m_BugsAction[csOwner_Hero.Index_Action_Escape].ActivationTime;

		m_CoEmptiedDependingOnTheTime = CoEmptiedDependingOnTheTime(EscapeableTime);
		StartCoroutine(m_CoEmptiedDependingOnTheTime);
	}

	public override void Deactivate()
	{
		base.Deactivate();

		if (m_CoEmptiedDependingOnTheTime != null)
		{
			StopCoroutine(m_CoEmptiedDependingOnTheTime);
			m_CoEmptiedDependingOnTheTime = null;
		}
	}

	protected override void TouchBegan()
	{
		base.TouchBegan();

		m_Animator.enabled = false;
	}

	protected override void TouchEnded()
	{
		base.TouchEnded();

		m_Animator.enabled = true;
	}

	#region Coroutine

	public float m_OriginTime = 0.0f;
	public float m_CurrentTime = 0.0f;
	public float m_Filled = 0.0f;

	private IEnumerator CoEmptiedDependingOnTheTime(float _time)
	{
		float OriginTime = _time;
		float CurrentTime = _time;
		m_OriginTime = OriginTime;

		while (true)
		{
			yield return null;

			if(CurrentTime <= 0.0f)
			{
				break;
			}

			float Percent = Utility.GetSomeValueOfTotalValue(OriginTime, CurrentTime);
			float Filled = Utility.GetPercentOfTotalValue(1.0f, Percent);
			m_Filled = Filled;
			m_FilledImage.fillAmount = Filled;

			CurrentTime -= Time.deltaTime;
			m_CurrentTime = CurrentTime;
		}
	}

	#endregion
}
