using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class csLoadingManager : Singleton<csLoadingManager>
{
	public static string m_strNextSceneName;

	public static string m_strMapSceneName;

	public Image m_ProgressBarImage;

	private void Start()
	{
		StartCoroutine(LoadScene());
	}

	public static void LoadScene(string _battleSceneName, string _mapSceneName, string _loadingSceneName)
	{
		m_strNextSceneName = _battleSceneName;

		m_strMapSceneName = _mapSceneName;

		SceneManager.LoadScene(_loadingSceneName);
	}

	private IEnumerator LoadScene()
	{
		yield return null;

		AsyncOperation Op = SceneManager.LoadSceneAsync(m_strMapSceneName, LoadSceneMode.Additive);
		AsyncOperation AsyncMap = SceneManager.LoadSceneAsync(m_strNextSceneName, LoadSceneMode.Additive);
		Op.allowSceneActivation = false;

		//Debug.Log("op : " + op);
		//float progressComplete = 0.1f;
		float Timer = 0.0f;

		while (true)
		{
			yield return null;

			Timer += Time.deltaTime;

			m_ProgressBarImage.fillAmount = Mathf.Lerp(m_ProgressBarImage.fillAmount, AsyncMap.progress, Timer);

			if (AsyncMap.progress >= 0.9f)
			{
				if (!Op.allowSceneActivation)
				{
					Op.allowSceneActivation = true;
					AsyncMap.allowSceneActivation = true;
				}
			}
		}
	}
}
