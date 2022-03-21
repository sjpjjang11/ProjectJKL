using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csItem_Speed : csItem
{
	protected override void Awake()
	{
		base.Awake();

		Type = csItemManager.SpeedType;
		Value = 1;
	}
}
