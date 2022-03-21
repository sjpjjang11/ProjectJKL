using System;
using System.Collections.Generic;
using UnityEngine;

public class csBugsInfoManager : MonoBehaviour{

	public struct BugsClassification
	{
		public int Type;
		public int Index;

		public BugsClassification(int _type, int _index)
		{
			Type = _type;
			Index = _index;
		}
	}

	private Dictionary<BugsClassification, csInfo_Bugs> DicInfo_Bugs = new Dictionary<BugsClassification, csInfo_Bugs>();

	protected void Awake()
	{
		Utility.InstantiateField(this, GetType(), false);
		AddBugsInfo();
	}

	protected virtual void AddBugsInfo()
	{
		DicInfo_Bugs.Add(new BugsClassification(csBattleManager.HeroType, 0), new csInfo_Gunner());
		DicInfo_Bugs.Add(new BugsClassification(csBattleManager.HeroType, 1), new csInfo_Gunner());
		DicInfo_Bugs.Add(new BugsClassification(csBattleManager.HeroType, 2), new csInfo_Gunner());
		DicInfo_Bugs.Add(new BugsClassification(csBattleManager.HeroType, 3), new csInfo_Gunner());
		DicInfo_Bugs.Add(new BugsClassification(csBattleManager.HeroType, 4), new csInfo_Gunner());

		DicInfo_Bugs.Add(new BugsClassification(csBattleManager.MonsterType, 0), new csInfo_Wolf());	
		DicInfo_Bugs.Add(new BugsClassification(csBattleManager.MonsterType, 1), new csInfo_Wraith());
		DicInfo_Bugs.Add(new BugsClassification(csBattleManager.MonsterType, 2), new csInfo_Goblin());
		DicInfo_Bugs.Add(new BugsClassification(csBattleManager.MonsterType, 4), new csInfo_One_Eyed());	
	}

	public csInfo_Bugs GetBugsInfo(int _type, int _index)
	{
		BugsClassification BugsClassification;
		BugsClassification.Type = _type;
		BugsClassification.Index = _index;

		if (DicInfo_Bugs.ContainsKey(BugsClassification))
		{
			return DicInfo_Bugs[BugsClassification];
		}

		return null;
	}
}
