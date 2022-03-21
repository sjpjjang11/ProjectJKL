using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FadeInFadeOut : MonoBehaviour 
{
    private static Action m_FadeInCompleteCallback;
    private static Action m_FadeOutCompleteCallback;

	public static IEnumerator CoFadeIn(Image img, float smooth, Action onComplete = null)
	{
        m_FadeInCompleteCallback = onComplete;

        smooth = smooth / 255f;
		Color c = img.color;
		c.a = 0.1f;
		img.color = c;
		float tempAlpha = 0.0f;

        while ( true )
		{
			tempAlpha = Mathf.Lerp ( 0.0f, 1.0f, c.a + smooth );
			c.a = tempAlpha;
			img.color = c;

			if( tempAlpha == 1.0f )
			{
                if (m_FadeInCompleteCallback != null)
                {
                    yield return YieldCache.WaitForSeconds(1.0f);
                    m_FadeInCompleteCallback();

                    m_FadeInCompleteCallback = null;
                }
                break;
			}

			yield return null;
		}
	}

	public static IEnumerator CoFadeOut(Image img, float smooth, Action onComplete = null)
	{
        m_FadeOutCompleteCallback = onComplete;

		img.raycastTarget = true;

        smooth = smooth / 255f;
		Color c = img.color;
		c.a = 1.0f;
		img.color = c;
		float tempAlpha = 0.0f;

		while( true )
		{
			tempAlpha = Mathf.Lerp ( 0.0f, 1.0f, c.a - smooth );
			c.a = tempAlpha;
			img.color = c;

			if( tempAlpha == 0.0f )
			{
                if(m_FadeOutCompleteCallback != null)
                {
                    yield return YieldCache.WaitForSeconds(1.0f);
                    m_FadeOutCompleteCallback();

                    m_FadeOutCompleteCallback = null;
                }

				img.raycastTarget = false;

				break;
			}
			yield return null;
		}
	}
}
