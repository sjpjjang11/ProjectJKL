using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEventHandler_Game : csEventHandler
{
    public Pause Pause;
    public GameOver GameOver;

	protected void Awake()
	{
		// csEventHandler 타입의 이벤트 필드 얻어온 후 m_FieldList에 저장
		GetFields(GetType());
		//Debug.Log("^^^^^^^^^^^^^^ : " + GetType());
		InstantiateField();
	}
}
