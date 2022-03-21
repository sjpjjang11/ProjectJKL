using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ProjectK;

public class csGunner : csHero {

	private Dictionary<MoveActionIndex, MoveActionInfo> m_DicTumbleIndexInfoBindings;
	public csWeapon_Projectile_Normal m_Weapon_Projectile_Normal_Left = null;
	public csWeapon_Projectile_Normal m_Weapon_Projectile_Normal_Right = null;
	public csWeapon_Projectile_Spike m_Weapon_Projectile_Spike_Left = null;
	public csWeapon_Projectile_Spike m_Weapon_Projectile_Spike_Right = null;
	public csWeapon_Lightsaber m_Weapon_Lightsaber = null;

	private int m_iComboIndex = 0;

	public override void Settings(csOwner _owner)
	{
		ObjectIndex = m_iTempObjectIndex;

		base.Settings(_owner);

		m_ActionRangeRenderer = Instantiate(Resources.Load("Prefabs/HeroRelated/Gunner/ActionRangeRenderer_Gunner") as GameObject).GetComponent<csActionRangeRenderer>();
		m_ActionRangeRenderer.name = m_ActionRangeRenderer.name + "_" + m_Owner_Hero.Bugs.name;
		m_BattleManager.ObjectMoveToBattleScene(m_ActionRangeRenderer.gameObject);

		m_ActionRangeRenderer.Settings(m_Owner_Hero, csInfo_Hero.ActionCount);

		m_Weapon_Projectile_Normal_Left.Settings(m_Owner_Hero, m_BugsAction_Attack.ActionIndex);
		m_Weapon_Projectile_Normal_Right.Settings(m_Owner_Hero, m_BugsAction_Attack.ActionIndex);
		m_Weapon_Projectile_Spike_Left.Settings(m_Owner_Hero, m_BugsAction_Attack.ActionIndex);
		m_Weapon_Projectile_Spike_Right.Settings(m_Owner_Hero, m_BugsAction_Attack.ActionIndex);
		m_Weapon_Lightsaber.Settings(m_Owner_Hero, m_BugsAction_Skill_1.ActionIndex);

		m_DicTumbleIndexInfoBindings = new Dictionary<MoveActionIndex, MoveActionInfo>(new MoveActionIndexCompare())
		{
			{ new MoveActionIndex(0, 0), new MoveActionInfo(eDirectionType.Forward, 0.5f, 0.27f) },
			{ new MoveActionIndex(0, 1), new MoveActionInfo(eDirectionType.Forward, 1.2f, 0.15f) },
			{ new MoveActionIndex(0, 2), new MoveActionInfo(eDirectionType.Forward, 1.0f, 0.15f) },
			/*{ new MoveActionIndex(0, 0), new MoveActionInfo(Direction.Forward, 0.65f, 0.15f) },
			{ new MoveActionIndex(0, 1), new MoveActionInfo(Direction.Forward, 0.35f, 0.24f) },
			{ new MoveActionIndex(1, 0), new MoveActionInfo(Direction.Back, 1.0f, 0.27f) },
			{ new MoveActionIndex(1, 1), new MoveActionInfo(Direction.Forward, 1.0f, 0.3f) },
			{ new MoveActionIndex(2, 0), new MoveActionInfo(Direction.Forward, 0.5f, 0.27f) },
			{ new MoveActionIndex(2, 1), new MoveActionInfo(Direction.Forward, 1.2f, 0.15f) },
			{ new MoveActionIndex(2, 2), new MoveActionInfo(Direction.Forward, 1.0f, 0.15f) }*/
			//{ new MoveActionIndex(), new MoveActionInfo() }
		};
	}

	public override void Release()
	{
		base.Release();

		if (m_ActionRangeRenderer != null)
		{
			Destroy(m_ActionRangeRenderer.gameObject);
		}

		m_Weapon_Projectile_Normal_Left.Release();
		m_Weapon_Projectile_Normal_Right.Release();
	}

	#region Animation Event Mothod

	protected void Event_MoveAction(int _moveActionInfoIndex)
	{
		if(m_EventHandler_Hero.Hero_CurrentAimTarget.Get() != null)
		{
			return;
		}

		MoveActionIndex MoveActionIndex = new MoveActionIndex(m_iComboIndex, _moveActionInfoIndex);
		MoveActionInfo MoveActionInfo = m_DicTumbleIndexInfoBindings[MoveActionIndex];

		Vector3 MoveDirection = Vector3.zero;

		switch (MoveActionInfo.DirectionType)
		{
			case eDirectionType.Forward:

				MoveDirection = m_Owner_Hero.Transform.forward;

				break;

			case eDirectionType.Back:

				MoveDirection = -m_Owner_Hero.Transform.forward;

				break;

			case eDirectionType.Right:

				MoveDirection = m_Owner_Hero.Transform.right;

				break;

			case eDirectionType.Left:

				MoveDirection = -m_Owner_Hero.Transform.right;

				break;
		}
		//Debug.LogError("Action!!! : " + m_iComboIndex + "  " + _moveActionInfoIndex);
		m_Activity.MoveStart(MoveDirection, MoveActionInfo.Distance, MoveActionInfo.Time, false);
	}

	#region Shoot

	protected virtual void Shoot_Normal_ActionDirection_Left()
	{
		if (!m_Owner.IsActiveState(eBugsStateType.Attack))
		{
			return;
		}

		CrowdControl_KnockBack KnockBack = (CrowdControl_KnockBack)m_BugsAction_Attack.CrowdControl[0];
		KnockBack.KnockBacDirection = Vector3.zero;

		LookAt_Target();
		m_Weapon_Projectile_Normal_Left.NormalShoot_ActionDirection();
	}

	protected virtual void Shoot_Normal_ActionDirection_Right()
	{
		if (!m_Owner.IsActiveState(eBugsStateType.Attack))
		{
			return;
		}

		CrowdControl_KnockBack KnockBack = (CrowdControl_KnockBack)m_BugsAction_Attack.CrowdControl[0];
		KnockBack.KnockBacDirection = Vector3.zero;

		LookAt_Target();
		m_Weapon_Projectile_Normal_Right.NormalShoot_ActionDirection();
	}

	protected virtual void Shoot_Normal_Forward_Left()
	{
		LookAt_Target();
		m_Weapon_Projectile_Normal_Left.NormalShoot_Forward();
	}

	protected virtual void Shoot_Normal_Forward_Right()
	{
		LookAt_Target();
		m_Weapon_Projectile_Normal_Right.NormalShoot_Forward();
	}

	protected virtual void Shoot_Spike_ActionDirection_Left()
	{
		if (!m_Owner.IsActiveState(eBugsStateType.Attack))
		{
			return;
		}

		CrowdControl_KnockBack KnockBack = (CrowdControl_KnockBack)m_BugsAction_Attack.CrowdControl[0];
		KnockBack.KnockBacDirection = m_Owner.Transform.forward;

		LookAt_Target();
		m_Weapon_Projectile_Spike_Left.NormalShoot_ActionDirection();
	}

	protected virtual void Shoot_Spike_ActionDirection_Right()
	{
		if (!m_Owner.IsActiveState(eBugsStateType.Attack))
		{
			return;
		}

		CrowdControl_KnockBack KnockBack = (CrowdControl_KnockBack)m_BugsAction_Attack.CrowdControl[0];
		KnockBack.KnockBacDirection = m_Owner.Transform.forward;

		LookAt_Target();
		m_Weapon_Projectile_Spike_Right.NormalShoot_ActionDirection();
	}

	protected virtual void Shoot_Spike_Forward_Left()
	{
		LookAt_Target();
		m_Weapon_Projectile_Spike_Left.NormalShoot_Forward();
	}

	protected virtual void Shoot_Spike_Forward_Right()
	{
		LookAt_Target();
		m_Weapon_Projectile_Spike_Right.NormalShoot_Forward();
	}

	#endregion

	protected virtual void Lightsaber_On()
	{
		if (!m_Owner.IsActiveState(eBugsStateType.Attack))
		{
			return;
		}

		m_Weapon_Lightsaber.Lightsaber_On();
	}

	protected virtual void Lightsaber_Off()
	{
		m_Weapon_Lightsaber.StopAttack();
	}

	protected virtual void Lightsaber_Attack()
	{
		if(m_Owner.IsActiveState(eBugsStateType.Attack))
		{
			Dash_Target_Start();

			CrowdControl_KnockBack KnockBack = (CrowdControl_KnockBack)m_BugsAction_Skill_1.CrowdControl[0];
			KnockBack.KnockBacDirection = m_Owner.Transform.forward;

			m_Weapon_Lightsaber.ClearIgnore();
			//m_Weapon_Lightsaber.StartAttack();
		}
	}

	/*protected virtual void ClearIgnore()
	{		
		m_Weapon_Lightsaber.ClearIgnore();
	}*/

	protected virtual void ComboTransition()
	{
		if(m_bIsAttack)
		{
			m_iComboIndex++;
			//Debug.LogError("NextCombo : " + m_bIsCheckCombo);
			Animator.SetTrigger("NextCombo");
		}		
		else
		{
			//Debug.LogError("StopCombo");
			m_Owner.StateStop(eBugsStateType.Attack);
		}
	}

	#endregion

	#region Local Hero Action

	public override void RunStart()
	{
		base.RunStart();
	}

	public override void RunStop()
	{
		base.RunStop();
	}

	public override void Attack_ReadyStart()
	{
		Debug.Log("Attack_ReadyStart");

		m_ActionRangeRenderer.RenderStart(m_BugsAction_Attack.ActionIndex);
	}

	public override void Attack_ReadyStop()
	{
		Debug.Log("Attack_ReadyStop");

		m_ActionRangeRenderer.RenderStop();
	}

	public override void Attack_Start()
	{
		Debug.Log("Attack_Start");

		//m_EventHandler_Hero.IsAttackSettings.Send(false);

		HeroActionEnter(m_BugsAction_Attack);
	}

	protected override void SoundOn(int _index)
	{
		if (!m_Owner.IsActiveState(eBugsStateType.Attack))
		{
			return;
		}

		base.SoundOn(_index);
	}

	public override void Attack_Stop()
	{
		base.Attack_Stop();

		Dash_Target_Stop();

		m_iComboIndex = 0;
		Debug.Log("Attack_Stop");

		Initialize();

		if (Utility.IsActive(m_Weapon_Lightsaber.gameObject))
		{
			//Lightsaber_Off();
			m_Weapon_Lightsaber.Lightsaber_ForceOff();
		}

		SoundOff(0);
	}

	public override void Attack_ForceStop()
	{
		base.Attack_ForceStop();

		Dash_Target_Stop();
		
		m_iComboIndex = 0;
		Debug.Log("Attack_ForceStop");

		Animator.SetTrigger("ForceStop");

		m_Weapon_Lightsaber.Lightsaber_ForceOff();

		SoundOff(0);
	}

	public override void Skill_1_ReadyStart()
	{
		Debug.Log("Skill_1_ReadyStart");
		
		m_ActionRangeRenderer.RenderStart(m_BugsAction_Skill_1.ActionIndex);
	}

	public override void Skill_1_ReadyStop()
	{
		m_ActionRangeRenderer.RenderStop();
	}

	public override void Skill_1_ActionStart()
	{
		Debug.Log("Skill_1_ActionStart");		
	}

	public override void Skill_1_ActionStop()
	{
		Debug.Log("Skill_1_ActionStop_Local");

		//m_AnimationEventCallback = null;

		//m_EventHandler_Hero.Index_Action_Skill_Item.Set(csOwner_Hero.Index_Action_None);
		//AAAAA
		//m_Owner_Hero_Local.JoystickHandlerSettings(csOwner_User.Index_Button_Skill_Item, csOwner_User.Index_Action_None);
	}

	public override void Skill_1_Start()
	{
		base.Skill_1_Start();
	}

	public override void Skill_1_Stop()
	{
		base.Skill_1_Stop();

		//m_EventHandler_Hero.Index_Action_Skill_Item.Set(csOwner_Hero.Index_Action_None);
		//m_Owner_Hero_Local.JoystickHandlerSettings(csOwner_User.Index_Button_Skill_Item, csOwner_User.Index_Action_None);
	}

	public override void Skill_2_ReadyStart()
	{
		m_ActionRangeRenderer.RenderStart(m_BugsAction_Skill_2.ActionIndex);
	}

	public override void Skill_2_ReadyStop()
	{
		m_ActionRangeRenderer.RenderStop();
	}

	public override void Skill_2_ActionStart()
	{
		Debug.Log("Skill_2_ActionStart");
	}

	public override void Skill_2_ActionStop()
	{
		Debug.Log("Skill_2_ActionStop");

		//m_AnimationEventCallback = null;

		//m_EventHandler_Hero.Index_Action_Skill_Item.Set(csOwner_Hero.Index_Action_None);
		//m_Owner_Hero_Local.JoystickHandlerSettings(csOwner_User.Index_Button_Skill_Item, csOwner_User.Index_Action_None);
	}

	public override void Skill_2_Start()
	{
		base.Skill_2_Start();
	}

	public override void Skill_2_Stop()
	{
		base.Skill_2_Stop();

		//m_EventHandler_Hero.Index_Action_Skill_Item.Set(csOwner_Hero.Index_Action_None);
		//m_Owner_Hero_Local.JoystickHandlerSettings(csOwner_User.Index_Button_Skill_Item, csOwner_User.Index_Action_None);
	}

	public override void Skill_3_ReadyStart()
	{
		Debug.Log("Skill_3_ReadyStart");
		//m_ActionRangeRenderer.RenderStart(m_BugsAction_Skill_3.ActionIndex);
	}

	public override void Skill_3_ReadyStop()
	{
		Debug.Log("Skill_3_ReadyStop");
		m_ActionRangeRenderer.RenderStop();
	}

	public override void Skill_3_ActionStart()
	{
		Debug.Log("Skill_3_ActionStart");
		Tumble_Start();
	}

	public override void Skill_3_ActionStop()
	{
		Debug.Log("Skill_3_ActionStop");
		//TumbleAction_Stop_Local();
	}

	public override void Skill_3_Start()
	{
		Debug.Log("Skill_3_Start");
		Tumble_Start();
	}

	public override void Skill_3_Stop()
	{
		Debug.Log("Skill_3_Stop");
		//TumbleAction_Stop_Local();
	}

	public override void Skill_Default_ReadyStart()
	{
		Debug.Log("Skill_Default_ReadyStart");
		m_ActionRangeRenderer.RenderStart(m_BugsAction_Skill_Default.ActionIndex);
	}

	public override void Skill_Default_ReadyStop()
	{
		Debug.Log("Skill_Default_ReadyStop");
		m_ActionRangeRenderer.RenderStop();
	}

	public override void Skill_Default_ActionStart()
	{
		Debug.Log("Skill_Default_ActionStart");
		Tumble_Start();
	}

	public override void Skill_Default_ActionStop()
	{
		Debug.Log("Skill_Default_ActionStop");
		//TumbleAction_Stop_Local();
		//Animator.SetTrigger("Stop");
	}

	public override void Skill_Default_Start()
	{
		Debug.Log("Skill_Default_Start");
		Tumble_Start();
	}

	public override void Skill_Default_Stop()
	{
		Debug.Log("Skill_Default_Stop");
		//TumbleAction_Stop_Local();
		//Animator.SetTrigger("Stop");
	}

	#endregion

	protected virtual void OnDrawGizmosSelected()
	{
		if (m_BattleManager != null)
		{
			/*Vector3 LeftBoundary = DirFromAngle(-m_fFOVAngle / 2.0f);
			Vector3 RightBoundary = DirFromAngle(m_fFOVAngle / 2.0f);

			Gizmos.DrawLine(m_Owner_Hero.Transform.position, Transform.position + LeftBoundary * m_fViewDistance);
			Gizmos.DrawLine(m_Owner_Hero.Transform.position, Transform.position + RightBoundary * m_fViewDistance);*/

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(m_Owner_Hero.Transform.position, m_fFOVDistance);

			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(m_Owner_Hero.Transform.position, m_BugsAction_Skill_1.Range);
		}
	}
}
