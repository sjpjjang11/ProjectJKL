using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEventHandler_Monster : csEventHandler_Bugs {

	#region 상태 관리

	public Attack Attack;

	#endregion

	#region 명령

	public csCommand StartFillActingPower;
	public csCommand StopFillActingPower;
	public csCommand SetActingPower_HUD;

	#endregion

	#region 값 저장 및 읽기

	public csValue<csEventHandler_Grid> Grid;
	public csValue<Vector3> TargetWayPoint;
	
	#endregion
}
