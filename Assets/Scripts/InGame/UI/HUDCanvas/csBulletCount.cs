using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csBulletCount : MonoBehaviour {

	private IEnumerator m_CoFillBullet = null;
	private List<float> m_ListTemp =
		new List<float>()
		{
			{ 15.0f},
			{ 20.0f},
			{ 25.0f},
			{ 30.0f},
		};
	
	public float m_fReloadSpeed = 0.0f;
	public float m_fBulletProgress = 0.0f;

	public const int MinUsableBullet = 2;
	public const int MaxUsableBullet = 6;

	public int m_iTempTimeIndex = 0;
	public int m_iCurrentUsableBullet = 0;

	public int CurrentBulletIndex
	{
		get
		{
			float Value = m_fBulletProgress;
			Value = Mathf.Clamp(Value, m_fBulletProgress, MaxUsableBullet - 1);

			return (int)Value;
		}
	}

	public bool m_bIsReloading = false;

	public csProgress[] m_Progress = null;

	private void Awake()
	{
		m_fBulletProgress = m_iCurrentUsableBullet = MinUsableBullet;

		for(int i = 0; i < MaxUsableBullet; i++)
		{
			m_Progress[i].Settings(1.0f);
			m_Progress[i].SetFillAmount(1.0f);

			if (i >= MinUsableBullet)
			{
				Utility.Activate(m_Progress[i].gameObject, false);
			}
		}
	}

	public void Settings(csBugs _owner)
	{
		if (!csBattleManager.Instance.IsHero(_owner))
		{
			Utility.Activate(gameObject, false);
		}
		else
		{
			//StartCoroutine(CoTempTimer());
		}
	}

	public bool CheckShoot()
	{
		bool Result = false;

		if (m_fBulletProgress - 1.0f >= 0.0f)
		{
			Result = true;

			m_Progress[CurrentBulletIndex].SetFillAmount(0.0f);

			m_fBulletProgress -= 1.0f;
			if (m_CoFillBullet == null)
			{			
				m_CoFillBullet = CoFillBullet();
				StartCoroutine(m_CoFillBullet);
			}
		}

		return Result;
	}

	private IEnumerator CoFillBullet()
	{
		while(true)
		{
			yield return null;

			m_fBulletProgress += m_fReloadSpeed * Time.deltaTime;

			m_fBulletProgress = Mathf.Clamp(m_fBulletProgress, m_fBulletProgress, m_iCurrentUsableBullet);

			m_Progress[CurrentBulletIndex].SetFillAmount(m_fBulletProgress - CurrentBulletIndex);
			//Debug.Log(m_fBulletProgress + "     " + CurrentBulletIndex);

			if (m_fBulletProgress == m_iCurrentUsableBullet)
			{
				m_CoFillBullet = null;

				break;
			}
		}
	}

	private IEnumerator CoTempTimer()
	{
		float TempTime = 0.0f;
		while(true)
		{
			yield return null;

			if (m_iCurrentUsableBullet == MaxUsableBullet)
			{
				break;
			}

			TempTime += Time.deltaTime;

			if (TempTime >= m_ListTemp[m_iTempTimeIndex])
			{				
				m_iCurrentUsableBullet++;
				m_iTempTimeIndex++;

				if (m_fBulletProgress < m_iCurrentUsableBullet - 1)
				{
					m_Progress[m_iCurrentUsableBullet - 1].SetFillAmount(0.0f);

					if (m_CoFillBullet == null)
					{
						m_CoFillBullet = CoFillBullet();
						StartCoroutine(m_CoFillBullet);
					}
				}
				else
				{
					m_Progress[m_iCurrentUsableBullet - 1].SetFillAmount(1.0f);

					m_fBulletProgress += 1.0f;
				}

				Utility.Activate(m_Progress[m_iCurrentUsableBullet - 1].gameObject, true);				
			}			
		}
	}
}
