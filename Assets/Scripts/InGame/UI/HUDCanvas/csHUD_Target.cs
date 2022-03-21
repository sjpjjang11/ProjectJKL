using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csHUD_Target : csHUD
{
	protected csOwner_Hero m_Owner_Hero = null;

	[SerializeField]
	protected float m_fRange = 0.0f;

	protected int m_iLayerMask_Monster = 0;
	protected int m_iLayerMask_Map = 0;

	public override void Settings(csOwner _owner)
	{
		base.Settings(_owner);

		m_Owner_Hero = (csOwner_Hero)_owner;

		RegisterEventHandler();

		Utility.Activate(m_Front, false);

		m_fRange = m_Owner.Info_Bugs.m_BugsAction[csOwner_Hero.Index_Action_Skill_1].Range;

		m_iLayerMask_Monster = m_BattleManager.m_iLayerMask_Monster;
		m_iLayerMask_Map = m_BattleManager.m_iLayerMask_Map;
	}

	protected virtual csOwner DetectViewAngle()
	{
		csOwner Target = null;

		Collider[] Colls = Physics.OverlapSphere(m_Owner.BugsCenter, m_fRange, m_iLayerMask_Monster);

		if (Colls.Length > 0)
		{
			Dictionary<float, csOwner> DicDistanceFromMeToTarget = new Dictionary<float, csOwner>();

			for (int i = 0; i < Colls.Length; i++)
			{
				Vector3 RelativePosition = Utility.RelativePosition(m_Owner.Transform.position, Colls[i].transform.position);
				Ray Ray = new Ray(m_Owner.BugsCenter, RelativePosition.normalized);

				if (Physics.SphereCast(Ray, m_Owner.Info_Bugs.m_BugsCollider.Radius, RelativePosition.magnitude, m_iLayerMask_Map))
				{
					//Debug.Log("SphereCast");
					continue;
				}

				if (!m_BattleManager.m_DicObjectColliderIndex.ContainsKey(Colls[i].GetInstanceID()))
				{
					continue;
				}

				csOwner Owner = m_BattleManager.m_DicObjectColliderIndex[Colls[i].GetInstanceID()];

				float Distance = Vector3.Distance(m_Owner.Transform.position, Colls[i].transform.position);

				if (!DicDistanceFromMeToTarget.ContainsKey(Distance))
				{
					DicDistanceFromMeToTarget.Add(Distance, Owner);
				}
			}

			if (DicDistanceFromMeToTarget.Count > 0)
			{
				List<float> ListDistance;
				ListDistance = DicDistanceFromMeToTarget.Keys.ToList();

				ListDistance.Sort();

				Target = DicDistanceFromMeToTarget[ListDistance[0]];
			}
		}

		return Target;
	}

	#region Command Method

	protected virtual void OnCommand_HUD_AimTarget(bool _isAim)
	{
		if (_isAim)
		{
			csOwner Target = m_Owner_Hero.m_EventHandler_Hero.Hero_CurrentAimTarget.Get();
			ChangeTarget(Target.Bugs.m_TargetingTr);
			m_Transform.position = GetHUDFollowTargetPosition();
		}

		Utility.Activate(m_Front, _isAim);
	}

	#endregion

	/*#region Coroutine

	protected override IEnumerator CoHUDFollowTarget()
	{
		while (true)
		{
			yield return YieldCache.WaitForEndOfFrame;
			//Debug.Log("$$$$$ ");
			//Debug.Log("!!!!!!!!! : " + m_MyOwner.name);

			csOwner Target = DetectViewAngle();

			if(Target == null)
			{
				//Debug.Log("Target == null");
				Utility.Activate(m_Front, false);

				m_FollowTarget = null;
			}
			else
			{
				//Debug.Log("Target!!!!! : " + Target.name);
				ChangeTarget(Target.Bugs.m_TargetingTr);

				m_Transform.position = GetHUDFollowTargetPosition();

				Utility.Activate(m_Front, true);
			}			
		}
	}

	#endregion*/
}
