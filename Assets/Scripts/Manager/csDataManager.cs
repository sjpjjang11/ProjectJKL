using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// JSON 파싱 라이브러리a
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

/*
2018.06.12
sjpjjang11
게임내 모든 데이터를 관리하는 매니저 클래스
*/
public partial class csDataManager 
{
	public Dictionary<int, string> m_DicHeroData;
	public Dictionary<int, string> m_DicPetData;


	/*
	2018.06.12
	sjpjjang11
	데이터 매니저 생성자 함수
	*/
	public csDataManager()
	{
		m_DicHeroData = new Dictionary<int, string>();
		m_DicPetData = new Dictionary<int, string>();
		//Test();
	}

	public void Test()
	{
		/*TextAsset json = Resources.Load("Data/HeroData") as TextAsset;
		JArray JArray = JArray.Parse(json.text);

		for(int i = 0; i < JArray.Count; i++)
		{
			JToken Data = JArray[i];
			Debug.Log(Data);
		}
		*/
	}
}
