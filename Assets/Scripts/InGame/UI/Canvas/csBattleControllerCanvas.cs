using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csBattleControllerCanvas : MonoBehaviour {

	public csJoystickHandler_Move m_MoveController = null;
	public csJoystickHandler_Action[] m_JoystickHandler = null;

	/*private void Awake()
	{
		// 2020.11.09
		// 에디터에서는 정상
		// 빌드에서 구르기(csJoystickHandler_ByState) 버튼 두개 중 하나가 활성화 돼있음에도 불구하고 Awake가 호출이 안 되어 에러
		// 반복문 비활성화 코드를 아래 Settings 함수로 옮겨 이 기능을 늦춤으로서 임시로 해결. 원인 모름
		for(int i = 0; i < m_JoystickHandler.Length; i++)
		{
			Utility.Activate(m_JoystickHandler[i].gameObject, false);
		}
	}*/

	public void Settings(int _heroIndex, bool _isThird = false)
	{
		// Multi
		//Utility.Activate(m_JoystickHandler[csOwner_Hero.Index_Button_Multi].gameObject.gameObject, true);
		//JoystickHandlerSettings(_heroIndex, csOwner_Hero.Index_Button_Multi, csOwner_Hero.Index_Action_None);

		for (int i = 0; i < m_JoystickHandler.Length; i++)
		{
			Debug.LogError("DeActivate : " + m_JoystickHandler[i].name + "   " + m_JoystickHandler[i].gameObject.IsActive());
			Utility.Activate(m_JoystickHandler[i].gameObject, false);
		}

		// 현재 사용
		//JoystickHandlerSettings(_heroIndex, csOwner_Hero.Index_Button_Skill_Default, csOwner_Hero.Index_Action_Skill_Default);
		//JoystickHandlerSettings(_heroIndex, csOwner_Hero.Index_Button_Skill_Default + 2, csOwner_Hero.Index_Action_Skill_Default);
		m_MoveController.Settings(_heroIndex, 0);
		//JoystickHandlerSettings(_heroIndex, csOwner_Hero.Index_Button_Escape, csOwner_Hero.Index_Action_Escape);
		//m_JoystickHandler[csOwner_Hero.Index_Button_Skill_Default].Activate();
		//m_JoystickHandler[csOwner_Hero.Index_Button_Skill_Default + 2].Activate();

		csBattleManager.Instance.m_EventHandler_Hero.Register(this);		
	}

	public void JoystickHandlerSettings(int _heroIndex, int _joyStickIndex, int _actionIndex)
	{
		m_JoystickHandler[_joyStickIndex].Settings(_heroIndex, _actionIndex);
	}

	protected virtual void OnCommand_ForceTouchEnded(int _index)
	{
		m_JoystickHandler[_index].GetJoystick().ForceTouchEnded();
	}
}
