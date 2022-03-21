using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class csPopulation : MonoBehaviour {

	public TextMeshProUGUI m_PopulationText = null;

	private void Awake()
	{
		Initialize();
	}

	public void Initialize()
	{
		m_PopulationText.text = "0 / " + csBattleManager.MaxPopulation;
	}

	public void IncreaseKillCount(int _killCount)
	{
		m_PopulationText.text = _killCount + " / " + csBattleManager.MaxPopulation;
	}
}
