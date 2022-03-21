using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCoroutine : MonoBehaviour {

    private static StaticCoroutine m_Instance = null;

    private static StaticCoroutine Instance
    {
        get
        {
			if (m_Instance == null)
			{
				m_Instance = new GameObject("StaticCoroutine").AddComponent<StaticCoroutine>();
			}

			return m_Instance;
        }
    }

    private void Awake()
    {
        if(m_Instance == null)
        {
            m_Instance = this as StaticCoroutine;
        }
    }

    IEnumerator Perform(IEnumerator coroutine)
    {
        yield return StartCoroutine(coroutine);
    }

	public static void Start(IEnumerator coroutine)
	{
		Instance.StartCoroutine(Instance.Perform(coroutine));
	}

	public static void Stop(IEnumerator coroutine)
    {
        Instance.StopCoroutine(coroutine);        
    }

    private void Die()
    {
        m_Instance = null;
        Destroy(gameObject);
    }

    private void OnApplicationQuit()
    {
		Die();
	}
}
