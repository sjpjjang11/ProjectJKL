using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csHUD_ActingPower : csHUD
{
	private csOwner_Monster m_Owner_Monster = null;

	public csProgress m_Progress = null;

	public override void Settings(csOwner _owner)
	{
		base.Settings(_owner);

		m_Owner_Monster = (csOwner_Monster)m_Owner;

		m_Progress.Settings((float)m_Owner_Monster.Info_Monster.ActingPower.Use / m_Owner_Monster.Info_Monster.ActingPower.Max);
		RegisterEventHandler();
		//Utility.Activate(gameObject, true);
	}

	#region Command Method

	private void OnCommand_SetActingPower_HUD()
	{
		m_Progress.SetFillAmount(m_Owner_Monster.m_ActingPower.ActingPower_Cur / m_Owner_Monster.m_ActingPower.ActingPower_Max);
	}

	#endregion
}
