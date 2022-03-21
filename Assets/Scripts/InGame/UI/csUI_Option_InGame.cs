using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csUI_Option_InGame : MonoBehaviour, IUIBack
{
	public void Settings()
    {
		//csHandler_Back_UI.Add(this);
    }

	public void Enter()
    {
		Pause();
	}

	public void Pause()
    {
		TimeUtility.Paused = !TimeUtility.Paused;

		if(csBattleManager.Instance.EventHandler_Game.Pause.Active)
        {
			csBattleManager.Instance.EventHandler_Game.Pause.Stop();
		}
		else
        {
			csBattleManager.Instance.EventHandler_Game.Pause.Start();
		}

		gameObject.SetActive(!gameObject.IsActive());
	}

	public void Back()
	{
		Pause();
	}

	private void Resume()
    {
		//csHandler_Back_UI.Remove();

		Pause();
	}

	private void Exit()
    {
		Resume();

		csBattleManager.Instance.GameOver();
	}

	#region Button Event

	public void OnClickResume()
    {
		Resume();
	}

	public void OnClickExit()
    {
		Exit();
    }

	#endregion
}
