using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// JSON 파싱 라이브러리a
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

/*
2018.06.12
sjpjjang11
데이터를 파싱하고 데이터 매니저에 넣는 데이터 매니저의 파셜 클래스
*/
public partial class csDataManager
{
	public IEnumerator CoPetData()
	{
		m_DicPetData.Add(0, "Bear");
		m_DicPetData.Add(1, "Bunny");
		/*m_DicPetData.Add(2, "Cat");
		m_DicPetData.Add(3, "Cow");
		m_DicPetData.Add(4, "Deer");
		m_DicPetData.Add(5, "Dog");
		m_DicPetData.Add(6, "Fox");
		m_DicPetData.Add(7, "Goat");
		m_DicPetData.Add(8, "Horse");
		m_DicPetData.Add(9, "Lion");
		m_DicPetData.Add(10, "Lizard");
		m_DicPetData.Add(11, "Panda");
		m_DicPetData.Add(12, "Pig");
		m_DicPetData.Add(13, "Sheep");
		m_DicPetData.Add(14, "Turtle");
		m_DicPetData.Add(15, "Wolf");
		m_DicPetData.Add(16, "Zebra");*/

		yield return null;
	}

	public IEnumerator CoHeroData()
	{
		m_DicHeroData.Add(0, "PANDA");
		m_DicHeroData.Add(1, "SKELETON");
		m_DicHeroData.Add(2, "CHICKEN");
		m_DicHeroData.Add(3, "CROCODILE");
		m_DicHeroData.Add(4, "DEER");

		yield return null;
	}
}
