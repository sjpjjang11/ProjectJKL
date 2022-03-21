using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*2018.07.11
* hkh
* UI를 컨트롤하기 위한 클래스
* 이 클래스를 상속받는 모든 터치 클래스들이 공통으로 사용하는 
* 필드 및 메소드가 선언된다 
*/
public class csTouch : csInput {

	protected BoxCollider m_BoxCollider = null;

	protected Camera m_UICamera = null;
	public Camera UICamera
	{
		get
		{
			if(m_UICamera == null)
			{
				m_UICamera = BattleManager.m_BattleUIManager.m_UICamera;
			}

			return m_UICamera;
		}
	}

	protected UserTouch m_CurrentTouch;

	protected int m_iLastFingerID = 0;          // 마지막으로 터치한 손가락 식별

    protected override void Awake()
    {
		base.Awake();

        Register();
		
		m_BoxCollider = GetComponent<BoxCollider>();
		
		m_iLastFingerID = -1;
    }

	protected override void Start()
	{
		base.Start();

		m_BoxCollider.size = GetComponent<RectTransform>().rect.size;
	}

	protected virtual void TouchBegan(UserTouch _touch)
    {
        if (m_iLastFingerID != -1)
        {
			return;
        }

        if (!RaycastControl(_touch))
        {
			return;
        }

		if(EventHandler_Hero.IsUsingOtherStick.Get())
		{
			return;
		}

		m_iLastFingerID = _touch.m_iFingerID;
    }

	protected virtual void TouchMoved(UserTouch _touch)
	{

	}

	protected virtual void TouchStationary(UserTouch _touch)
    {
        //Debug.Log("TouchesStationary");
    }

    protected virtual void TouchEnded(UserTouch _touch)
    {
        if (m_iLastFingerID != _touch.m_iFingerID)
        {
            return;
        }

        m_iLastFingerID = -1;
    }

    protected bool RaycastControl(UserTouch _touch)
    {
		RaycastHit Hit;

		if (Physics.Raycast(UICamera.ScreenPointToRay(_touch.m_Position), out Hit))
		{
			if (Hit.collider.name.CompareTo("PlayerInfo") == 0)
			{
				return false;
			}
		}
		//Debug.Log(name + "    " + Physics.RaycastAll(UICamera.ScreenPointToRay(_touch.m_Position)).Where(hit => hit.collider == BoxCollider).Count());
        return Physics.RaycastAll(UICamera.ScreenPointToRay(_touch.m_Position)).Where(hit => hit.collider == m_BoxCollider).Count() > 0;
    }

	public virtual void ForceTouchEnded()
	{
		//Debug.LogError("FDSA : " + name);
		if(m_iLastFingerID != -1)
		{
			//Debug.LogError("FFFFF : " + name);
			TouchEnded(m_CurrentTouch);
		}		
	}
}
