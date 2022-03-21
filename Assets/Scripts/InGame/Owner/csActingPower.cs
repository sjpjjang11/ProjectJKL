using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csActingPower : MonoBehaviour
{
	private IEnumerator m_CoFillActingPower = null;

	private csEventHandler_Monster m_EventHandler = null;

	public float m_fRecoverySpeed = 10.0f;

	[SerializeField]
	private int m_iActingPower_Max = 0;
	public int ActingPower_Max
	{
		get
		{
			return m_iActingPower_Max;
		}
		private set
		{
			m_iActingPower_Max = value;
		}
	}

	[SerializeField]
	private float m_iActingPower_Cur = 0;
	public float ActingPower_Cur
	{
		get
		{
			return m_iActingPower_Cur;
		}
		private set
		{
			m_iActingPower_Cur = value;
		}
	}

	[SerializeField]
	private int m_iActingPower_Use = 0;
	public int ActingPower_Use
	{
		get
		{
			return m_iActingPower_Use;
		}
		private set
		{
			m_iActingPower_Use = value;
		}
	}

	private void Awake()
	{
		m_EventHandler = GetComponent<csEventHandler_Monster>();
	}

	public void Settings(AIActingPower _actingPower)
	{
		ActingPower_Max = _actingPower.Max;
		ActingPower_Cur = _actingPower.Cur;
		ActingPower_Use = _actingPower.Use;

		m_EventHandler.SetActingPower_HUD.Send();

		//StartFillActingPower();
	}

	public void StartFillActingPower()
	{
		if(m_CoFillActingPower == null)
		{
			m_CoFillActingPower = CoFillActingPower();
			StartCoroutine(m_CoFillActingPower);
		}
	}

	public void StopFillActingPower()
	{
		if(m_CoFillActingPower != null)
		{
			StopCoroutine(m_CoFillActingPower);
			m_CoFillActingPower = null;
		}
	}

	public bool IsActionable_Consumption()
	{
		bool Result = false;

		float Remaining = ActingPower_Cur - ActingPower_Use;

		if (Remaining >= 0.0f)
		{
			Result = true;
		}

		return Result;
	}

	public bool IsActionable_Full()
	{
		return ActingPower_Cur == ActingPower_Max;
	}

	public void ActingPowerDecrease()
	{
		ActingPower_Cur -= ActingPower_Use;

		m_EventHandler.SetActingPower_HUD.Send();
	}

	#region Coroutine

	private IEnumerator CoFillActingPower()
	{
		while (true)
		{
			yield return null;

			ActingPower_Cur += m_fRecoverySpeed * Time.deltaTime;

			ActingPower_Cur = Mathf.Clamp(ActingPower_Cur, 0.0f, ActingPower_Max);

			m_EventHandler.SetActingPower_HUD.Send();

			if (ActingPower_Cur == ActingPower_Max)
			{
				StopFillActingPower();

				break;
			}
		}
	}

	#endregion
}
