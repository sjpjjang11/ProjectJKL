using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csHUD_Bugs : csHUD
{
	public csHealthBar m_HealthBar = null;

	public override void Settings(csOwner _owner)
	{
		base.Settings(_owner);

		m_HealthBar.Settings(m_Owner);

		m_FollowTarget = m_Owner.Bugs.m_HpBarTr;
	}

	#region Command Method

	protected virtual void OnCommand_HUD_ActivateFront(bool _active)
	{
		if (_active)
		{
			m_Transform.position = GetHUDFollowTargetPosition();
		}

		Utility.Activate(m_Front, _active);
	}

	#endregion
}
