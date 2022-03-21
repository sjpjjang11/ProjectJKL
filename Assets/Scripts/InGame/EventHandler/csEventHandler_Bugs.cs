using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEventHandler_Bugs : csEventHandler {

	#region 상태 관리(시작, 종료, 현재 상태 얻어오기 등)

	public Walk Walk;
	public Run Run;
	public Slow Slow;                           // 슬로우
	public Stun Stun;                           // 기절	
	public KnockBack KnockBack;

	public Die Die;

	#endregion

	#region 명령

	public csCommand UpdateAbilities;
	public csCommand<bool> HUD_ActivateFront;
	public csCommand SetHealth_HUD;

	#endregion

	#region 값 저장 및 읽기
	
	public csValue<List<csWeapon>> ActivatedWeapon;
	public csValue<Vector3[]> RenderDirection;
	public csValue<Vector3> MoveDirection_Usable;             // 최종적으로 사용될 이동값 저장 및 읽기	
	public csValue<Vector3> ActionDirection;
	public csValue<CrowdControl> CrowdControl;

	#endregion

	protected void Awake()
	{
		// csEventHandler 타입의 이벤트 필드 얻어온 후 m_FieldList에 저장
		GetFields(GetType());
		//Debug.Log("^^^^^^^^^^^^^^ : " + GetType());
		InstantiateField();
	}

	public csEvent GetEvent(string _name)
	{
		csEvent Event = null;
		//Debug.Log("GetEvent : " + _name + "    " + m_DicEvents.ContainsKey(_name));
		if(m_DicEvents.ContainsKey(_name))
		{
			Event = m_DicEvents[_name];
		}

		return Event;
	}

	public csState GetState(eBugsStateType _stateType)
	{
		csState State = null;
		//Debug.Log("GetEvent : " + _name + "    " + m_DicEvents.ContainsKey(_name));
		if (DicStates.ContainsKey(_stateType))
		{
			DicStates.TryGetValue(_stateType, out State);
			//State = DicStates[_name];
		}

		return State;
	}
}
