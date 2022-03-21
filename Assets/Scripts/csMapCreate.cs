using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class csMapCreate : MonoBehaviour 
{
	public GameObject m_prefab;
	// Use this for initialization
	void Start () 
	{
		TextAsset data = Resources.Load<TextAsset>("map");
		StringReader sr = new StringReader(data.text);
		string source = sr.ReadLine();
		string[] values;
		int y = 0;

		while(source != null)
		{
			values = source.Split(',');
			for(int i = 0; i < values.Length; i++)
			{
				GameObject obj = Instantiate(m_prefab);
				Vector3 vec = obj.transform.localPosition;
				obj.transform.localPosition = new Vector3(vec.x + i, vec.y, vec.z - y);
				int cnt = int.Parse(values[i]);
				obj.name = string.Format("{0}_{1}_{2}", i, y, cnt);
				obj.GetComponent<csMapObject>().SetMapObject(cnt);
			}

			y++;
			if(values.Length == 0)
			{
				sr.Close();
				return;
			}

			source = sr.ReadLine();
		}

	}
	

}
