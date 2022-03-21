using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csRalph : csHero {

	/*public csMeleeWeapon m_Sword = null;
	public csMeleeWeapon m_Shield = null;
	
	private csKnockBack m_KnockBack = null;
	private csStun m_Stun = null;

	public override void Settings()
	{
		base.Settings();

		//ObjectIndex = 4;	
		ObjectIndex = m_iTempObjectIndex;
	}

	public override void Release()
	{
		base.Release();

		if (m_ActionRangeRenderer != null)
		{
			Destroy(m_ActionRangeRenderer.gameObject);
		}
	}

	public override void SelectObject(csOwner _owner)
	{
		base.SelectObject(_owner);

		m_KnockBack = GetComponent<csKnockBack>();
		m_Stun = GetComponent<csStun>();

		ColliderInfo RalphColliderInfo;
	
		RalphColliderInfo.Center = new Vector3(0.0f, 1.3f, 0.0f);
		RalphColliderInfo.Radius = 0.7f;
		RalphColliderInfo.Height = 2.5f;

		m_Owner.HeroInfo.ColliderInfo = RalphColliderInfo;

		if (m_BattleManager.IsLocal(this))
		{
			m_ActionRangeRenderer = Instantiate(Resources.Load("Prefabs/HeroRelated/Ralph/ActionRangeRenderer_Ralph") as GameObject).GetComponent<csActionRangeRenderer>();
			m_ActionRangeRenderer.name = m_ActionRangeRenderer.name + "_" + m_Owner.BattleObject.name;
			m_BattleManager.ObjectMoveToBattleScene(m_ActionRangeRenderer.gameObject);

			m_ActionRangeRenderer.Settings(m_Owner, csInfo_Hero.m_iActionCount);
		}

		m_Sword.Settings(m_Owner, m_Attack_ActionInfo.ActionIndex);
		m_Shield.Settings(m_Owner, m_Skill_1_ActionInfo.ActionIndex);

		//m_KnockBack.AddCrowdControl(m_Stun);
		m_KnockBack.RegisterCallback((target) =>
		{
			float Diameter = (m_Owner.CharacterController.radius * 2.0f) + 0.2f;
			//float Radius = m_Owner.CharacterController.radius;
			Vector3 Position = target.Transform.position - (m_Owner.Transform.forward * Diameter);
			MoveAction.Stop(m_Owner.GetInstanceID());
			m_Owner.Transform.position = Position;
			Debug.Log("KnockBackCollisionMap : " + Position);
			Debug.Log("Target : " + target);
			
			m_Skill_1_ActionInfo.CrowdControl = m_Stun;
			UpdateHeroInfo(m_Skill_1_ActionInfo);
			Debug.Log("@@@@@@@@@@@@ : " + m_Owner.HeroInfo.m_ActionInfo[m_Skill_1_ActionInfo.ActionIndex].CrowdControl);
			m_Shield.ForceHitCollider(target.CharacterController);
		});

		//임시
		m_Owner.m_Move.MoveSpeed = 10.0f;
	}

	#region Local Hero Action

	public override void Attack_ReadyStart_Local()
	{
		m_ActionRangeRenderer.RenderStart(m_Attack_ActionInfo.ActionIndex);
	}

	public override void Attack_ReadyStop_Local()
	{
		Debug.Log("Attack_ReadyStop_Local");

		m_ActionRangeRenderer.RenderStop();
	}

	public override void Attack_ActionStart_Local()
	{
		Debug.Log("Attack_ActionStart_Local");

		m_Owner.StateStart(BugsState.LookAtTarget);

		Vector3 Rotation = Transform.localEulerAngles;

		float TargetEulerY;

		if(LocalHandler.Run.Active)
		{
			TargetEulerY = -40.0f;
		}
		else
		{
			TargetEulerY = -30.0f;
		}

		m_CountNumber.Count(Rotation.y, TargetEulerY, 0.2f, (value) =>
		{
			Rotation.y = value;

			Transform.localEulerAngles = Rotation;
		});

		HeroActionEnter_Local(m_Attack_ActionInfo);

		Vector3 AimDirection = LocalHandler.RenderDirection.Get()[m_Attack_ActionInfo.ActionIndex];
		Debug.Log("Attack AimAimAimAimAimAim : " + AimDirection);
		LocalHandler.AimDirection.Set(AimDirection);

		m_AnimationEventCallback = () =>
		{
			m_Sword.StartAttack();
		};

		m_EventHandler.ActionDirection.Set(Vector3.zero);
	}

	public override void Attack_ActionStop_Local()
	{
		Debug.Log("Attack_ActionStop_Local");
		m_Owner.StateStop(BugsState.LookAtTarget);

		Vector3 Rotation = Transform.localEulerAngles;
		Rotation.y = Utility.GetNegativeAngleValue(Rotation.y);

		m_CountNumber.Count(Rotation.y, 0.0f, 0.2f, (value) =>
		{
			Rotation.y = value;

			Transform.localEulerAngles = Rotation;
		});

		m_Sword.StopAttack();

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
		m_ActionRangeRenderer.RenderStart(m_Skill_1_ActionInfo.ActionIndex);
	}

	public override void Skill_1_ReadyStop_Local()
	{
		m_ActionRangeRenderer.RenderStop();
	}

	public override void Skill_1_ActionStart_Local()
	{
		Debug.Log("Skill_1_ActionStart_Local");

		HeroActionEnter_Local(m_Skill_1_ActionInfo);

		m_Skill_1_ActionInfo.CrowdControl = m_KnockBack;
		UpdateHeroInfo(m_Skill_1_ActionInfo);
	
		Vector3 AimDirection = LocalHandler.RenderDirection.Get()[m_Skill_1_ActionInfo.ActionIndex];

		Vector3 Target = m_Owner.Transform.position + AimDirection * m_Skill_1_ActionInfo.Range;
		Debug.Log("AimDirection : " + AimDirection);
		//Vector3 Target = m_Owner.Transform.position + AimDirection * m_TestRange;
		LocalHandler.AimDirection.Set(AimDirection);

		m_Shield.CheckHitMap();

		m_AnimationEventCallback = () =>
		{
			m_Shield.StartAttack();
			//Debug.Log("@@@@@@@@@@@@@ : " + m_Shield_KnockBack.m_bIsCollisionMap);
			if (!m_Shield.m_bIsCollisionMap)
			{
				MoveAction.StartMoveToTheDirection(m_Owner.Transform, AimDirection, m_Skill_1_ActionInfo.Range, 0.7f, () =>
				{
					float Distance = Vector3.Distance(m_Owner.Transform.position, Target);
					float PercentOfRemainingTargetDistance = Utility.GetSomeValueOfTotalValue(m_Skill_1_ActionInfo.Range, Distance);

					m_KnockBack.SetPercentOfRemainingTargetDistance(PercentOfRemainingTargetDistance);

					if (m_Shield.m_bIsCollisionMap)
					{
						Debug.Log("m_Shield_KnockBack.m_bIsCollisionMap");

						MoveAction.Stop(m_Owner.GetInstanceID());
					}
				});
			}
		};

		m_KnockBack.SetTargetPosition(AimDirection, Target);

		m_Owner.StateStart(BugsState.LookAtTarget);

		m_EventHandler.ActionDirection.Set(Vector3.zero);
	}

	public override void Skill_1_ActionStop_Local()
	{
		Debug.Log("Skill_1_ActionStop_Local");
		m_Owner.StateStop(BugsState.LookAtTarget);

		m_Shield.StopAttack();
	}

	public override void Skill_1_Start_Local()
	{
		base.Skill_1_Start_Local();
	}

	public override void Skill_1_Stop_Local()
	{
		base.Skill_1_Stop_Local();
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

		m_EventHandler.LookAtTarget.Start();

		Vector3 Rotation = Transform.localEulerAngles;

		float TargetEulerY;

		if (m_EventHandler.Run.Active)
		{
			TargetEulerY = -40.0f;
		}
		else
		{
			TargetEulerY = -30.0f;
		}

		m_CountNumber.Count(Rotation.y, TargetEulerY, 0.2f, (value) =>
		{
			Rotation.y = value;

			Transform.localEulerAngles = Rotation;
		});

		HeroActionEnter_Remote(m_Attack_ActionInfo);

		m_AnimationEventCallback = () =>
		{
			m_Sword.StartAttack();
		};

		m_EventHandler.ActionDirection.Set(Vector3.zero);
	}

	public override void Attack_ActionStop_Remote()
	{
		Debug.Log("Attack_ActionStop_Remote");
		m_EventHandler.LookAtTarget.Stop();

		Vector3 Rotation = Transform.localEulerAngles;
		Rotation.y = Utility.GetNegativeAngleValue(Rotation.y);

		m_CountNumber.Count(Rotation.y, 0.0f, 0.2f, (value) =>
		{
			Rotation.y = value;

			Transform.localEulerAngles = Rotation;
		});

		m_Sword.StopAttack();

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
		Debug.Log("Skill_1_ActionStart_Remote");

		Vector3 AimDirection = m_EventHandler.AimDirection.Get();
		Vector3 Target = m_Owner.Transform.position + AimDirection * m_Skill_1_ActionInfo.Range;

		HeroActionEnter_Remote(m_Skill_1_ActionInfo);

		m_Skill_1_ActionInfo.CrowdControl = m_KnockBack;
		UpdateHeroInfo(m_Skill_1_ActionInfo);

		m_KnockBack.SetTargetPosition(AimDirection, Target);

		m_EventHandler.LookAtTarget.Start();

		m_Shield.CheckHitMap();

		m_AnimationEventCallback = () =>
		{
			m_Shield.StartAttack();

			if (!m_Shield.m_bIsCollisionMap)
			{
				MoveAction.StartMoveToTheDirection(m_Owner.Transform, AimDirection, m_Skill_1_ActionInfo.Range, 0.7f, () =>
				{
					float Distance = Vector3.Distance(m_Owner.Transform.position, Target);
					float PercentOfRemainingTargetDistance = Utility.GetSomeValueOfTotalValue(m_Skill_1_ActionInfo.Range, Distance);

					m_KnockBack.SetPercentOfRemainingTargetDistance(PercentOfRemainingTargetDistance);

					if (m_Shield.m_bIsCollisionMap)
					{
						Debug.Log("!!!!!!!!!!! Stop");

						MoveAction.Stop(m_Owner.GetInstanceID());
					}
				});
			}
		};
	}

	public override void Skill_1_ActionStop_Remote()
	{
		Debug.Log("Skill_1_ActionStop_Remote");

		m_EventHandler.LookAtTarget.Stop();

		m_Shield.StopAttack();
	}

	public override void Skill_1_Start_Remote()
	{
		base.Skill_1_Start_Remote();
	}

	public override void Skill_1_Stop_Remote()
	{
		base.Skill_1_Stop_Remote();
	}

	#endregion*/
}
