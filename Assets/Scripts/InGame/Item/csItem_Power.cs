using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csItem_Power : csItem
{
	protected override void Awake()
	{
		base.Awake();

		Type = csItemManager.PowerType;
		Value = 10;
	}
}
