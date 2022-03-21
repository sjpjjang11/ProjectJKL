using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer {

	public delegate void DelayCallback();
	public delegate void TimerCallback();

	private IEnumerator m_CoStartTimer = null;
	private IEnumerator m_CoTime = null;

	private DelayCallback m_DelayCallback;
	private TimerCallback m_TimerCallback;

	private float m_fTime;

	public void OnDelay(float _delay, DelayCallback _callback = null)
	{
		if(_callback != null)
		{
			m_DelayCallback = _callback;
		}

		StaticCoroutine.Start(CoDelay(_delay));	
	}

	public void OnTime(float _time, TimerCallback _callback = null)
	{
		if (_callback != null)
		{
			m_TimerCallback = _callback;
		}

		m_CoTime = CoTime(_time);
		StaticCoroutine.Start(m_CoTime);
	}

	public void OffTime()
	{
		if(m_CoTime != null)
		{
			StaticCoroutine.Stop(m_CoTime);
			m_CoTime = null;
		}
	}

	public void StartTimer()
	{
		m_CoStartTimer = CoStartTimer();
		StaticCoroutine.Start(m_CoStartTimer);
	}

	public void StopTimer()
	{
		StaticCoroutine.Stop(m_CoStartTimer);

		m_CoStartTimer = null;
	}

	public void ResetTimer()
	{
		m_fTime = 0.0f;
	}

	public float GetTime()
	{
		return m_fTime;
	}

	public string[] SetTimerText(float _time)
	{
		string Minute = ((int)_time / 60).ToString("00");
		string Second = Mathf.FloorToInt(_time % 60.0f).ToString("00");

		return new string[2] { Minute, Second };
	}

	IEnumerator CoDelay(float _delay)
	{
		yield return YieldCache.WaitForSeconds(_delay);

		m_DelayCallback?.Invoke();
	}

	IEnumerator CoTime(float _time)
	{
		while(true)
		{
			yield return null;

			if (_time <= 0.0f)
			{
				m_TimerCallback?.Invoke();

				OffTime();

				break;
			}

			_time -= Time.deltaTime;
		}		
	}

	IEnumerator CoStartTimer()
	{
		while(true)
		{
			yield return null;

			m_fTime += Time.deltaTime;
		}
	}
}
