using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csBattleEtcCanvas : MonoBehaviour {

	public csResultScore m_ResultScore = null;
	public csWaitingRoomUI m_WaitingRoomUI = null;
	public csSelectHero m_SelectHero = null;
	public csHideOnScreen m_HideOnScreen = null;
	public csUI_Option_InGame UI_Option { get; private set; }
	public csUI_Result_InGame UI_Result { get; private set; }

	public GameObject m_WorldMapPanel = null;
	public GameObject m_ItemPanel = null;
	public GameObject m_SkillPanel = null;
	public GameObject m_StartBattle = null;

	public Image m_FadeImage = null;

    private void Awake()
    {
		UI_Option = GetComponentInChildren<csUI_Option_InGame>(true);
		UI_Result = GetComponentInChildren<csUI_Result_InGame>(true);
	}

	public void Settings()
    {
		UI_Option.Settings();
	}

	private void EnterOption()
    {
		UI_Option.Enter();
	}

	private void EnterResult()
    {
		UI_Result.Enter();
    }

    public void GameOver()
    {
		EnterResult();
	}

	public void Restart()
    {
		UI_Result.gameObject.SetActive(false);
	}

    #region Button Event

	public void OnClickEnterOption()
    {
		EnterOption();
    }

    #endregion
}
