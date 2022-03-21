using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class csProjectileWeapon_Spread : csWeapon_Projectile
{
	public Vector3 m_Spread = Vector3.zero;
	public Vector3[] m_SpreadDirection;

	protected csPlayerCamera m_PlayerCamera = null;
	public csPlayerCamera PlayerCamera
	{
		get
		{
			if (m_PlayerCamera == null)
			{
				m_PlayerCamera = BattleManager.m_PlayerCamera;
			}

			return m_PlayerCamera;
		}
	}

	public int m_iSpreadPerShot = 5;

	protected override void Start()
	{
		m_SpreadDirection = new Vector3[m_iSpreadPerShot];
	}

	public void SpreadShootReadyStart()
	{
		//m_TargetDirection = m_AttackRangeRenderer.m_RenderStartPoint.forward;

		if (m_iSpreadPerShot != m_SpreadDirection.Length)
		{
			Array.Resize(ref m_SpreadDirection, m_iSpreadPerShot);
		}
	}

	public void SpreadShootReadyStop()
	{

	}

	private void SetSpreadDirection(int _index, Vector3 _aim = default(Vector3))
	{
		Vector3 GetSpread = Vector3.zero;
		Vector3 Forward = Vector3.zero;
		//Debug.Log("==================  : " + _aim);
		if (_aim != Vector3.zero)
		{
			Forward = _aim;
			//Debug.Log("====================== Forward : " + Forward);
		}
		else
		{
			Forward = RenderDirection;
		}

		GetSpread.x = Remap(_index, 0, m_iSpreadPerShot - 1, -m_Spread.x, m_Spread.x);
		GetSpread.y = Remap(_index, 0, m_iSpreadPerShot - 1, -m_Spread.y, m_Spread.y);
		GetSpread.z = Remap(_index, 0, m_iSpreadPerShot - 1, -m_Spread.z, m_Spread.z);
		Debug.Log(Forward);
		Quaternion Spread = Quaternion.Euler(GetSpread);
		//Vector3 Direction = Spread * m_OwnerInfo.Owner.Transform.forward;
		Vector3 Direction = Spread * Forward;
		Direction.y = Forward.y;
		m_SpreadDirection[_index] = Direction;//m_AttackRangeRenderer.m_RenderStartPoint.position + Direction * m_OwnerInfo.AttackRange;
	}

	public void SpreadShoot(Vector3 _aim = default(Vector3))
	{
		for (int i = 0; i < m_iSpreadPerShot; i++)
		{
			if(_aim != Vector3.zero)
			{
				SetSpreadDirection(i, _aim);
			}
			else
			{
				SetSpreadDirection(i);
			}

			SpawnProjectile().StartAttack(m_SpreadDirection[i]);
		}
	}

	/*private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(m_SpawnPoint.position, 0.05f);

		if (m_AttackRangeRenderer != null)
		{
			for (int i = 0; i < m_iSpreadPerShot; i++)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawRay(m_AttackRangeRenderer.m_RenderStartPoint.position, m_SpreadDirection[i] * m_OwnerInfo.AttackRange);
			}
		}
	}*/
}
