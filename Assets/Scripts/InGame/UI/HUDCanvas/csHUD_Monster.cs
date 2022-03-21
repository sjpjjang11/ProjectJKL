using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csHUD_Monster : csHUD_Bugs
{
	public csHUD_ActingPower m_ActingPower_HUD = null;

	public override void Settings(csOwner _owner)
	{
		base.Settings(_owner);

		RegisterEventHandler();
	}
}
