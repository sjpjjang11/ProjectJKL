using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class csWaitingIconPrefab : MonoBehaviour 
{
	public Image m_CharImgae;
	public TextMeshProUGUI m_CharNameText;

	public int m_UserIndex = -1;

	public void SetCharInfo(int _index, string _name)
	{
		m_CharImgae.sprite = Resources.Load<Sprite>(GetCharName(_index));
		//m_CharNameText.text = _name;
	}

	protected string GetCharName(int _index)
	{
		string strName = "";

		switch(_index)
		{
			case 1:
				strName = "UI/portraitBattle_katrina";
				m_CharNameText.text = "KATRINA";
				break;
			case 2:
				strName = "UI/portraitBattle_garou";
				m_CharNameText.text = "FURY";
				break;
			case 3:
				strName = "UI/Ghost";
				m_CharNameText.text = "Ghost";
				break;
			default:
				strName = "UI/KnightHat";
				m_CharNameText.text = "KnightHat";
				break;
		}

		return strName;
	}
}
