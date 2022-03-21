using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csHUD_Hero : csHUD_Bugs
{	
	public csBulletCount m_BulletCount = null;
	public csBuff[] m_Buff = null;

	#region Command Method

	public override void Settings(csOwner _owner)
	{
		base.Settings(_owner);

		m_BulletCount.Settings(m_Owner.Bugs);

		RegisterEventHandler();
	}

	public void Restart()
    {
		Initialize();
	}

	private void Initialize()
    {
		for(int i = 0; i < m_Buff.Length; i++)
        {
			m_Buff[i].Initialize();
		}
    }

	protected virtual bool OnCommandReturn_IsUsableBullet(bool _bool)
	{
		return m_BulletCount.CheckShoot();
	}

	private void OnCommand_UIGetBuff(int _buffIndex)
	{
		switch (_buffIndex)
		{
			case csItemManager.SpeedType:

				m_Buff[_buffIndex].AddBuffCount();
				//m_Owner.m_CommonInfo.MoveSpeed = 

				break;

			case csItemManager.PowerType:

				m_Buff[_buffIndex].AddBuffCount();

				break;

			case csItemManager.HealType:

				//m_Owner.m_CommonInfo.Health = 

				break;
		}
	}

	#endregion
}