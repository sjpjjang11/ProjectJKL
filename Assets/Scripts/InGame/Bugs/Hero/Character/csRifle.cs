using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csRifle : csHero
{
	/*public Material m_BodyMaterial;
	public Material m_WeaponMaterial;

	public csProjectileWeapon_Normal m_ProjectileWeapon_Normal = null;
	public csProjectileWeapon_Broad m_ProjectileWeapon_Broad = null;

	public float m_fInitHideOnValue = 1.0f;
	public float m_fTargetHideOnValue_Local = 0.3f;
	public float m_fTargetHideOnValue_Remote = 0.0f;
	public float m_fHideOnTime = 1.0f;

	public override void Settings()
	{
		base.Settings();

		//ObjectIndex = 2;
		ObjectIndex = m_iTempObjectIndex;
	}

	public override void Release()
	{
		base.Release();

		if (m_ActionRangeRenderer != null)
		{
			Destroy(m_ActionRangeRenderer.gameObject);
		}

		m_ProjectileWeapon_Normal.Release();
		m_ProjectileWeapon_Broad.Release();
	}

	public override void SelectHero(csOwner _owenr)
	{
		base.SelectHero(_owenr);

		if(m_BattleManager.IsLocal(this))
		{
			m_BodyMaterial = Resources.Load("Material/Skeleton_Local") as Material;
			m_WeaponMaterial = Resources.Load("Material/Weapon_Local") as Material;
		}
		else
		{
			m_BodyMaterial = Resources.Load("Material/Skeleton_Remote") as Material;
			m_WeaponMaterial = Resources.Load("Material/Weapon_Remote") as Material;
		}

		Color TargetColor = new Color
		{
			r = m_fInitHideOnValue,
			g = m_fInitHideOnValue,
			b = m_fInitHideOnValue,
			a = m_fInitHideOnValue
		};

		Color ColorToApplies = new Color();
		ColorToApplies = TargetColor;
		m_BodyMaterial.color = ColorToApplies;
		m_WeaponMaterial.color = ColorToApplies;

		Material[] SkinMaterials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
		Material[] TempMaterials = SkinMaterials;

		for(int i = 0; i < TempMaterials.Length; i++)
		{
			TempMaterials[i] = m_BodyMaterial;
		}

		//SkinMaterials = TempMaterials;
		GetComponentInChildren<SkinnedMeshRenderer>().materials = TempMaterials;

		MeshRenderer[] WeaponMesh = m_ProjectileWeapon_Normal.GetComponentsInChildren<MeshRenderer>();

		for(int i = 0; i < WeaponMesh.Length; i++)
		{
			WeaponMesh[i].material = m_WeaponMaterial;
		}

		ColliderInfo RifleSuitColliderInfo;

		RifleSuitColliderInfo.Center = new Vector3(0.0f, 1.3f, 0.0f);
		RifleSuitColliderInfo.Radius = 0.7f;
		RifleSuitColliderInfo.Height = 2.5f;

		m_Owner.HeroInfo.ColliderInfo = RifleSuitColliderInfo;

		if (m_BattleManager.IsLocal(this))
		{
			m_ActionRangeRenderer = Instantiate(Resources.Load("Prefabs/HeroRelated/Rifle/ActionRangeRenderer_Rifle") as GameObject).GetComponent<csActionRangeRenderer>();
			m_ActionRangeRenderer.name = m_ActionRangeRenderer.name + "_" + m_Owner.BattleObject.name;
			m_BattleManager.ObjectMoveToBattleScene(m_ActionRangeRenderer.gameObject);

			m_ActionRangeRenderer.Settings(m_Owner, csInfo_Hero.m_iActionCount);
		}		

		m_ProjectileWeapon_Normal.Settings(m_Owner, m_Attack_ActionInfo.ActionIndex);
		m_ProjectileWeapon_Broad.Settings(m_Owner, m_Skill_2_ActionInfo.ActionIndex);
	}

	#region Animation Event

	protected override void EffectOn(int _index)
	{
		switch (_index)
		{
			case 0:

				break;

			case 1:



				break;

			case 2:



				break;

			case 3:



				break;
		}

		if (m_Effect.Length > _index)
		{
			Utility.Activate(m_Effect[_index], true);
		}
	}

	protected override void EffectOff(int _index)
	{
		base.EffectOff(_index);
	}

	#endregion

	#region Local Hero Action

	public override void Attack_ReadyStart_Local()
	{
		m_ActionRangeRenderer.RenderStart(m_Attack_ActionInfo.ActionIndex);
	}

	public override void Attack_ReadyStop_Local()
	{
		m_ActionRangeRenderer.RenderStop();
	}

	public override void Attack_ActionStart_Local()
	{
		Debug.Log("Attack_ActionStart_Local");

		if (LocalHandler.Skill_1.Active)
		{
			m_Owner.StateStop(BugsState.Skill_1);
		}

		m_Owner.StateStart(BugsState.LookAtTarget);

		HeroActionEnter_Local(m_Attack_ActionInfo);

		Vector3 AimDirection = LocalHandler.RenderDirection.Get()[m_Attack_ActionInfo.ActionIndex];

		LocalHandler.AimDirection.Set(AimDirection);

		m_AnimationEventCallback = () =>
		{
			m_ProjectileWeapon_Normal.NormalShoot_RenderDirection();
		};

		m_EventHandler.ActionDirection.Set(Vector3.zero);
	}

	public override void Attack_ActionStop_Local()
	{
		Debug.Log("Attack_ActionStop_Local");
		m_Owner.StateStop(BugsState.LookAtTarget);

		//m_AnimationEventCallback = null;
	}

	public override void Attack_Start_Local()
	{
		base.Attack_Start_Local();
	}

	public override void Attack_Stop_Local()
	{
		base.Attack_Stop_Local();
	}

	public override void Skill_1_ReadyStart_Local()
	{
		base.Skill_1_ReadyStart_Local();
	}

	public override void Skill_1_ReadyStop_Local()
	{
		base.Skill_1_ReadyStop_Local();
	}

	public override void Skill_1_ActionStart_Local()
	{
		base.Skill_1_ActionStart_Local();
	}

	public override void Skill_1_ActionStop_Local()
	{
		base.Skill_1_ActionStop_Local();
	}

	public override void Skill_1_Start_Local()
	{
		m_BattleManager.m_BattleUIManager.BattleETC.m_HideOnScreen.HideOnScreen(m_fTargetHideOnValue_Local, m_fHideOnTime);

		Color TargetColor = new Color
		{
			a = m_fTargetHideOnValue_Local
		};

		Color ColorToApplies = new Color();

		ColorToApplies = m_BodyMaterial.color;

		m_CountNumber.Count(m_BodyMaterial.color.a, TargetColor.a, m_fHideOnTime, (value) =>
		{
			ColorToApplies.a = value;
			m_BodyMaterial.color = ColorToApplies;
		});

		ColorToApplies = m_WeaponMaterial.color;

		m_CountNumber.Count(m_WeaponMaterial.color.a, TargetColor.a, m_fHideOnTime, (value) =>
		{
			ColorToApplies.a = value;
			m_WeaponMaterial.color = ColorToApplies;
		});
	}

	public override void Skill_1_Stop_Local()
	{
		m_BattleManager.m_BattleUIManager.BattleETC.m_HideOnScreen.HideOnScreen(0.0f, m_fHideOnTime);

		Color TargetColor = new Color
		{
			a = m_fInitHideOnValue
		};

		Color ColorToApplies = new Color();

		ColorToApplies = m_BodyMaterial.color;

		m_CountNumber.Count(m_BodyMaterial.color.a, TargetColor.a, m_fHideOnTime, (value) =>
		{
			ColorToApplies.a = value;
			m_BodyMaterial.color = ColorToApplies;
		});

		ColorToApplies = m_WeaponMaterial.color;

		m_CountNumber.Count(m_WeaponMaterial.color.a, TargetColor.a, m_fHideOnTime, (value) =>
		{
			ColorToApplies.a = value;
			m_WeaponMaterial.color = ColorToApplies;
		});
	}

	public override void Skill_2_ReadyStart_Local()
	{
		m_ProjectileWeapon_Broad.BroadShootReadyStart();

		m_ActionRangeRenderer.RenderStart(m_Skill_2_ActionInfo.ActionIndex);
	}

	public override void Skill_2_ReadyStop_Local()
	{
		m_ProjectileWeapon_Broad.BroadShootReadyStop();

		m_ActionRangeRenderer.RenderStop();
	}

	public override void Skill_2_ActionStart_Local()
	{
		if (LocalHandler.Skill_1.Active)
		{
			m_Owner.StateStop(BugsState.Skill_1);
		}

		m_Owner.StateStart(BugsState.LookAtTarget);

		HeroActionEnter_Local(m_Skill_2_ActionInfo);

		Vector3 AimDirection = LocalHandler.RenderDirection.Get()[m_Skill_2_ActionInfo.ActionIndex];

		LocalHandler.AimDirection.Set(AimDirection);

		m_AnimationEventCallback = () =>
		{
			m_ProjectileWeapon_Broad.BroadShoot();
		};

		m_EventHandler.ActionDirection.Set(Vector3.zero);
	}

	public override void Skill_2_ActionStop_Local()
	{
		m_Owner.StateStop(BugsState.LookAtTarget);

		//m_AnimationEventCallback = null;
	}

	public override void Skill_2_Start_Local()
	{
		base.Skill_2_Start_Local();
	}

	public override void Skill_2_Stop_Local()
	{
		base.Skill_2_Stop_Local();
	}

	public override void Skill_3_ReadyStart_Local()
	{
		m_ActionRangeRenderer.RenderStart(m_Skill_3_ActionInfo.ActionIndex);
	}

	public override void Skill_3_ReadyStop_Local()
	{
		m_ActionRangeRenderer.RenderStop();
	}

	public override void Skill_3_ActionStart_Local()
	{
		if (LocalHandler.Skill_1.Active)
		{
			m_Owner.StateStop(BugsState.Skill_1);
		}

		TumbleAction_Start_Local();
	}

	public override void Skill_3_ActionStop_Local()
	{
		TumbleAction_Stop_Local();
	}

	public override void Skill_3_Start_Local()
	{
		base.Skill_3_Start_Local();
	}

	public override void Skill_3_Stop_Local()
	{
		base.Skill_3_Stop_Local();
	}

	#endregion

	#region Remote Hero Action

	public override void Attack_ReadyStart_Remote()
	{
		base.Attack_ReadyStart_Remote();
	}

	public override void Attack_ReadyStop_Remote()
	{
		base.Attack_ReadyStop_Remote();
	}

	public override void Attack_ActionStart_Remote()
	{
		Debug.Log("Attack_ActionStart_Remote");
		Vector3 AimDirection = m_EventHandler.AimDirection.Get();

		m_EventHandler.LookAtTarget.Start();

		HeroActionEnter_Remote(m_Attack_ActionInfo);

		m_AnimationEventCallback = () =>
		{
			m_ProjectileWeapon_Normal.NormalShoot_RenderDirection(AimDirection);
		};
	}

	public override void Attack_ActionStop_Remote()
	{
		Debug.Log("Attack_ActionStop_Remote");

		m_EventHandler.LookAtTarget.Stop();

		//m_AnimationEventCallback = null;
	}

	public override void Attack_Start_Remote()
	{
		base.Attack_Start_Remote();
	}

	public override void Attack_Stop_Remote()
	{
		base.Attack_Stop_Remote();
	}

	public override void Skill_1_ReadyStart_Remote()
	{
		base.Skill_1_ReadyStart_Remote();
	}

	public override void Skill_1_ReadyStop_Remote()
	{
		base.Skill_1_ReadyStop_Remote();
	}

	public override void Skill_1_ActionStart_Remote()
	{
		base.Skill_1_ActionStart_Remote();
	}

	public override void Skill_1_ActionStop_Remote()
	{
		base.Skill_1_ActionStop_Remote();
	}

	public override void Skill_1_Start_Remote()
	{
		m_EventHandler.HUD_ActivateFront.Send(false);

		Color TargetColor = new Color
		{
			a = m_fTargetHideOnValue_Remote
		};

		Color ColorToApplies = new Color();

		ColorToApplies = m_BodyMaterial.color;

		m_CountNumber.Count(m_BodyMaterial.color.a, TargetColor.a, m_fHideOnTime, (value) =>
		{
			ColorToApplies.a = value;
			m_BodyMaterial.color = ColorToApplies;
		});

		ColorToApplies = m_WeaponMaterial.color;

		m_CountNumber.Count(m_WeaponMaterial.color.a, TargetColor.a, m_fHideOnTime, (value) =>
		{
			ColorToApplies.a = value;
			m_WeaponMaterial.color = ColorToApplies;
		});
	}

	public override void Skill_1_Stop_Remote()
	{
		m_EventHandler.HUD_ActivateFront.Send(true);

		Color TargetColor = new Color
		{
			a = m_fInitHideOnValue
		};

		Color ColorToApplies = new Color();

		ColorToApplies = m_BodyMaterial.color;

		m_CountNumber.Count(m_BodyMaterial.color.a, TargetColor.a, m_fHideOnTime, (value) =>
		{
			ColorToApplies.a = value;
			m_BodyMaterial.color = ColorToApplies;
		});

		ColorToApplies = m_WeaponMaterial.color;

		m_CountNumber.Count(m_WeaponMaterial.color.a, TargetColor.a, m_fHideOnTime, (value) =>
		{
			ColorToApplies.a = value;
			m_WeaponMaterial.color = ColorToApplies;
		});
	}

	public override void Skill_2_ReadyStart_Remote()
	{
		base.Skill_2_ReadyStart_Remote();
	}

	public override void Skill_2_ReadyStop_Remote()
	{
		base.Skill_2_ReadyStop_Remote();
	}

	public override void Skill_2_ActionStart_Remote()
	{
		m_EventHandler.LookAtTarget.Start();

		HeroActionEnter_Remote(m_Skill_2_ActionInfo);

		Vector3 AimDirection = m_EventHandler.AimDirection.Get();

		m_AnimationEventCallback = () =>
		{
			m_ProjectileWeapon_Broad.BroadShoot(AimDirection);
		};
	}

	public override void Skill_2_ActionStop_Remote()
	{
		m_EventHandler.LookAtTarget.Stop();

		//m_AnimationEventCallback = null;
	}

	public override void Skill_2_Start_Remote()
	{
		base.Skill_2_Start_Remote();
	}

	public override void Skill_2_Stop_Remote()
	{
		base.Skill_2_Stop_Remote();
	}

	public override void Skill_3_ReadyStart_Remote()
	{
		base.Skill_3_ReadyStart_Remote();
	}

	public override void Skill_3_ReadyStop_Remote()
	{
		base.Skill_3_ReadyStop_Remote();
	}

	public override void Skill_3_ActionStart_Remote()
	{
		TumbleAction_Start_Remote();
	}

	public override void Skill_3_ActionStop_Remote()
	{
		TumbleAction_Stop_Remote();
	}

	public override void Skill_3_Start_Remote()
	{
		base.Skill_3_Start_Remote();
	}

	public override void Skill_3_Stop_Remote()
	{
		base.Skill_3_Stop_Remote();
	}

	#endregion*/
}
