using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class csPet : csBugs
{
	private IEnumerator m_CoGroundCheck = null;

	protected csOwner_Hero m_Owner_Hero;

	protected csFire_Physics m_Fire_Physics = null;

	public override void Settings(csOwner _owner)
	{
		base.Settings(_owner);

		m_Owner_Hero = (csOwner_Hero)m_Owner;

		m_RotationToTarget.Settings(m_Owner_Hero);

		m_Fire_Physics = GetComponent<csFire_Physics>();
		m_Fire_Physics.Settings();
	}

	public void Escape(Action _callback, Vector3 _escapeVelocity = default(Vector3))
	{
		Utility.Activate(gameObject, true);

		if(_escapeVelocity != Vector3.zero)
		{
			if (_escapeVelocity.x == 0.0f && _escapeVelocity.z == 0.0f)
			{
				m_Fire_Physics.FireUp(_escapeVelocity, _callback);
			}
			else
			{
				m_Fire_Physics.FireToTarget(_escapeVelocity, _callback);
			}			
		}
		else
		{
			m_Fire_Physics.Fire(_callback);
		}	
	}

	public override void RunStart()
	{
		StartCoroutine(CoGroundCheck());
	}

	#region Coroutine

	private IEnumerator CoGroundCheck()
	{
		while(true)
		{
			yield return null;

			if(m_Owner_Hero.CharacterController.isGrounded)
			{
				if(m_Owner_Hero.IsActiveState(eBugsStateType.Run))
				{
					Animator.SetBool("Fall", false);
					Animator.SetBool("Run", true);
				}

				break;
			}
			else
			{
				if (m_Owner_Hero.IsActiveState(eBugsStateType.Run))
				{
					Animator.SetBool("Fall", true);
				}
				else
				{
					Animator.SetBool("Fall", false);
				}
			}
		}
	}

	#endregion
}
