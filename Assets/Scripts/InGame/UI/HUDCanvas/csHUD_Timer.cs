using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class csHUD_Timer : MonoBehaviour {

	private Action m_TimerEndCallback = null;

	private IEnumerator m_CoStartTimer = null;

	private Timer m_Timer = new Timer();

	private bool m_bEndTimerActive = false;

	private float m_fLeftTime = 0.0f;

	private TextMeshProUGUI m_TimerText = null;
	public TextMeshProUGUI TimerText
	{
		get
		{
			if (m_TimerText == null)
			{
				m_TimerText = GetComponent<TextMeshProUGUI>();
			}

			return m_TimerText;
		}
	}

	public void Settings(bool _active, bool _endTimerActive)
	{
		TimerText.enabled = _active;

		m_bEndTimerActive = _endTimerActive;
	}

	public void SetTimerText(float _time)
	{
		string Minute = ((int)_time / 60).ToString("00");
		string Second = Mathf.FloorToInt(_time % 60.0f).ToString("00");

		TimerText.text = Minute + ":" + Second;
	}

	public void SetTimer(int _second, Action _callback = null)
	{
		if (!TimerText.isActiveAndEnabled)
		{
			TimerText.enabled = true;
		}

		SetTimerText(_second);

		if(_second == 1)
		{
			m_Timer.OnDelay(1.0f, () =>
			{
				TimerText.enabled = m_bEndTimerActive;

				_callback?.Invoke();
			});
		}
	}

	public void NonNetStartTimer(float _time, Action _callback = null)
	{
		if(_callback != null)
		{
			m_TimerEndCallback = _callback;
		}

		if(!TimerText.isActiveAndEnabled)
		{
			TimerText.enabled = true;
		}

		m_fLeftTime = _time;

		SetTimerText(_time);

		m_CoStartTimer = CoStartTimer();

		StartCoroutine(m_CoStartTimer);
	}

	private IEnumerator CoStartTimer()
	{
		float LeftTime = m_fLeftTime;
		float WaitTime = 0.0f;

		if(WaitTime >= 0.0f)
		{
			yield return YieldCache.WaitForSeconds(WaitTime);
		}

		while(LeftTime > 0.0f)
		{
			yield return null;

			LeftTime = Mathf.Clamp(LeftTime, 0.0f, LeftTime);

			SetTimerText(LeftTime);

			LeftTime -= Time.deltaTime;
		}

		TimerText.enabled = m_bEndTimerActive;

		if (m_TimerEndCallback != null)
		{
			m_TimerEndCallback();

			m_TimerEndCallback = null;
		}
	}

	public void SetBattleTimerText(int _index)
	{
		m_TimerText.enabled = true;
		switch(_index)
		{
			case 0:
				m_TimerText.text = "Buff monster appears!!";
				break;
			case 1:
			case 2:
			case 3:
			case 4:
				m_TimerText.text = "The map is reduced!!";
				break;
			default:
				break;
		}
		StartCoroutine("CoBattleTextHide");
	}

	private IEnumerator CoBattleTextHide()
	{
		yield return YieldCache.WaitForSeconds(10.0f);
		m_TimerText.enabled = false;
	}
}
