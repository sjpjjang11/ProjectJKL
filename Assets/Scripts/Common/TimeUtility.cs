using UnityEngine;
using System.Collections;

public static class TimeUtility
{
    private static bool m_Paused = false;

    public static bool Paused
    {
        get
        {
            return m_Paused;
        }
        set
        {
            if(value)
            {
                if (m_Paused)
                {
                    return;                    
                }

                m_Paused = true;

                Time.timeScale = 0.0f;
            }            
            else
            {
                if(!m_Paused)
                {
                    return;
                }

                m_Paused = false;
                Time.timeScale = 1.0f;
            }
        }
    }
}