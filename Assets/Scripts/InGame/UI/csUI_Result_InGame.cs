using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class csUI_Result_InGame : MonoBehaviour
{
    public Button[] m_Array_Button;

    public void Enter()
    {
        Open();
    }

    private void Open()
    {
        SetActiveButton(true);
        gameObject.SetActive(true);
    }

    private void Restart()
    {
        SetActiveButton(false);
        csBattleManager.Instance.Restart();
    }

    private void ExitGame()
    {
        SceneManager.LoadScene(csProjectManager.Instance.SceneType_Lobby_InUse.ToString());
    }

    private void SetActiveButton(bool _value)
    {
        for (int i = 0; i < m_Array_Button.Length; i++)
        {
            m_Array_Button[i].SetActive(_value);
        }
    }

    #region Button Event

    public void OnClickRestart()
    {
        Restart();
    }

    public void OnClickLobby()
    {
        ExitGame();
    }

    #endregion 
}
