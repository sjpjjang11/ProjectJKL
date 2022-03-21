using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csItem_Heal : csItem
{
	protected override void Awake()
	{
		base.Awake();

		Type = csItemManager.HealType;
		Value = 30;
	}
}
