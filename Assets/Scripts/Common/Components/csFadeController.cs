using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class csFadeController : MonoBehaviour
{
    private static Action m_FadeInCompleteCallback;

	private static Action m_FadeOutCompleteCallback;

	private static IEnumerator m_CoFadeIn;

	private static IEnumerator m_CoFadeOut;

    private const float m_AddAlphaValue = 2;

	private static Image m_Image_Fade = null;

	private void Awake()
	{
		m_Image_Fade = Instantiate(Resources.Load<Image>("Prefabs/FadeInOut"), transform);
	}

    public static void FadeIn(Action onComplete = null)
    {
		SetLastSibling();

		if (onComplete != null)
		{
			m_FadeInCompleteCallback = onComplete;
		}

        if(!m_Image_Fade.gameObject.IsActive())
        {
			m_Image_Fade.gameObject.SetActive(true);
		}

		if(m_CoFadeIn != null)
		{
			StaticCoroutine.Stop(m_CoFadeIn);
			m_CoFadeIn = null;
		}
		
		m_CoFadeIn = CoFadeIn();
		StaticCoroutine.Start(m_CoFadeIn);
    }

    private static IEnumerator CoFadeIn()
    {               
        Color color = m_Image_Fade.color;

        color.a = 0.0f;
		m_Image_Fade.color = color;

        while (true)
        {
			yield return null;

			color.a += m_AddAlphaValue * Time.deltaTime;

			m_Image_Fade.color = color;
           
			if (color.a >= 2.0f)
            {
				color.a = 2.0f;

				m_Image_Fade.color = color;

				if (m_FadeInCompleteCallback != null)
                {
                    m_FadeInCompleteCallback();

                    m_FadeInCompleteCallback = null;
                }

                break;
            }
        }
    }

    public static void FadeOut(Action onComplete = null)
    {
		SetLastSibling();

		if (onComplete != null)
		{
			m_FadeOutCompleteCallback = onComplete;
		}

		if (!m_Image_Fade.gameObject.IsActive())
        {
			m_Image_Fade.gameObject.SetActive(true);
		}

		if (m_CoFadeOut != null)
		{
			StaticCoroutine.Stop(m_CoFadeOut);
			m_CoFadeOut = null;
		}
		
		m_CoFadeOut = CoFadeOut();
		StaticCoroutine.Start(m_CoFadeOut);
    }

    private static IEnumerator CoFadeOut()
    {
        Color color = m_Image_Fade.color;

        color.a = 2.0f;
		m_Image_Fade.color = color;

        while (true)
        {
			yield return null;

			color.a -= m_AddAlphaValue * Time.deltaTime;

			m_Image_Fade.color = color;

			if (color.a <= 0.0f)
			{
				color.a = 0.0f;

				m_Image_Fade.color = color;

				m_Image_Fade.gameObject.SetActive(false);

				if (m_FadeOutCompleteCallback != null)
				{
					m_FadeOutCompleteCallback();

					m_FadeOutCompleteCallback = null;
				}

				break;
			}        
        }
    }

	private static void SetLastSibling()
	{
		m_Image_Fade.transform.SetAsLastSibling();
	}
}
