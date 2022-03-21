using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEventHandler_Hero : csEventHandler_Bugs
{
	#region 플레이어의 상태 관리(시작, 종료, 현재 상태 얻어오기 등)

	public Waiting Waiting;
	public Rotate Rotate;
	public Attack_Ready Attack_Ready;
	public Attack_Action Attack_Action;
	public Attack Attack;
	public Skill_1_Ready Skill_1_Ready;
	public Skill_1_Action Skill_1_Action;
	public Skill_1 Skill_1;
	public Skill_2_Ready Skill_2_Ready;
	public Skill_2_Action Skill_2_Action;
	public Skill_2 Skill_2;
	public Skill_3_Ready Skill_3_Ready;
	public Skill_3_Action Skill_3_Action;
	public Skill_3 Skill_3;
	public Skill_Default_Ready Skill_Default_Ready;
	public Skill_Default_Action Skill_Default_Action;
	public Skill_Default Skill_Default;
	public Hero Hero;
	public Pet Pet;
	public Groggy Groggy;
	public Escape_Ready Escape_Ready;
	public Escape_Action Escape_Action;
	public Escape Escape;
	public Revival Revival;
	
	#endregion

	#region 명령
	public csCommand<Transform> Camera_ChangeTarget;	
	public csCommand<float, float> ShakeCamera;     // 카메라 흔들기
	public csCommand<float> ChangeMoveSpeed;
	public csCommand<float> ChangeRotationSpeed;
	public csCommand<int> UIGetBuff;
	public csCommand<int> ForceTouchEnded;
	public csCommand<int, int> SetBuff;
	public csCommand<bool> IsUsableBullet;
	public csCommand<bool> IsAttackSettings;
	public csCommand<bool> HUD_AimTarget;
	public csCommand AnimationEndCallback;

	#endregion

	#region 값 저장 및 읽기

	public csValue<csOwner> Hero_CurrentAimTarget;
	public csValue<Vector3> ActionDirection_Enter;
	public csValue<Vector3> ActionDirection_Original;
	public csValue<Vector3> EscapeVelocity;
	public csValue<Vector3> MoveDirection;           // 이동에 사용될 입력값 저장 및 읽기	
	public csValue<Vector2> ViewPoint;
	public csValue<UserTouch> Touch;
	public csValue<float> AttackAnimationSpeed;     // 공격 애니메이션 속도값 저장 및 읽기
	public csValue<float> MoveAnimationSpeed;       // 이동 애니메이션 속도값 저장 및 읽기	
	public csValue<bool> IsTouchMove;               // 터치 이동 조작중인지 체크
	public csValue<bool> IsKeyMove;                 // 키보드 이동 조작중인지 체크	
	public csValue<bool> IsUsedStick;
	public csValue<bool> IsUsingOtherStick;
	public csValue<bool> IsAttack;

	#endregion
}
