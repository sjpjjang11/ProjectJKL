using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 버튼 타입
public enum ButtonType
{
	None,
	EventTrigger,
	Button
}

[RequireComponent(typeof(EventTrigger))]
/*2018.07.16
* hkh
* 전체적인 영웅 행동들을 UI를 조작하여 처리.
* UI 버튼에 현재 플레이어 영웅에 해당하는 행동을 등록
*/

public class csStateHandler_ByState_UI : csStateHandler_ByState/*, IPointerDownHandler, IPointerUpHandler, IUpdateSelectedHandler, 
	IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler, ISelectHandler, IDeselectHandler,
	IMoveHandler, IInitializePotentialDragHandler, ISubmitHandler, ICancelHandler*/
{
	protected BoxCollider m_BoxCollider = null;

	private EventTrigger m_EventTrigger = null;

	private Button m_ActionButton = null;

	public Image m_CoolDownCircleImage;
	public Image m_ActivationImage;
	public Image m_DisabledImage;

	public TextMeshProUGUI m_CoolDownText;

	/*private Dictionary<int, StateEventTriggerType> m_DicStateEventTriggerType = new Dictionary<int, StateEventTriggerType>
	{
		{ 0, StateEventTriggerType.Test },
		{ 1, StateEventTriggerType.Test },
		{ 2, StateEventTriggerType.Test },
		{ 3, StateEventTriggerType.Test },
		{ 4, StateEventTriggerType.Test },
		{ 5, StateEventTriggerType.Test },
		{ 6, StateEventTriggerType.Test },
		{ 7, StateEventTriggerType.Test },
		{ 8, StateEventTriggerType.Test },
		{ 9, StateEventTriggerType.Test },
		{ 10, StateEventTriggerType.Test },
		{ 11, StateEventTriggerType.Test },
		{ 12, StateEventTriggerType.Test },
		{ 13, StateEventTriggerType.Test },
		{ 14, StateEventTriggerType.Test },
		{ 15, StateEventTriggerType.Test },
		{ 16, StateEventTriggerType.Test },
		{ 2, StateEventTriggerType.StateReady },
		{ 3, StateEventTriggerType.StateStart },
		//{ 8, StateEventTriggerType.StateUpdate }
	};*/

	protected override void Awake()
    {
		base.Awake();

		m_BoxCollider = GetComponent<BoxCollider>();

		m_EventTrigger = GetComponent<EventTrigger>();

		m_StateHandler_ByState_UI = GetComponent<csStateHandler_ByState_UI>();

		Utility.Activate(m_CoolDownText.gameObject, false);
		Utility.Activate(m_ActivationImage.gameObject, false);
		m_CoolDownCircleImage.fillAmount = 0.0f;
	}

	public override void Settings(csInfo_Hero _info, int _actionIndex)
	{
		base.Settings(_info, _actionIndex);

		if (_info.m_StateHandlerSettings_ByState[_actionIndex].JoystickHandlerSettings.Type == JoystickType.None)
		{
			if(m_DisabledImage != null)
			{
				Utility.Activate(m_DisabledImage.gameObject, true);
			}
			
			return;
		}
		else
		{
			if(m_DisabledImage != null)
			{
				Utility.Activate(m_DisabledImage.gameObject, false);
			}		
		}
	}

	public void Test()
	{

	}
    /*protected virtual void RegisterEventTrigger(Dictionary<int, StateEventTriggerType> _eventTriggerType)
    {
		foreach (KeyValuePair<int, StateEventTriggerType> e in _eventTriggerType)
        {
			if(e.Value == StateEventTriggerType.StateButton)
			{
				continue;
			}

            EventTrigger.Entry Entry = new EventTrigger.Entry
            {
                eventID = ((EventTriggerType)e.Key),
                callback = new EventTrigger.TriggerEvent()
            };

            Entry.callback.AddListener((eventData) =>
            {
                MethodInfo Method = typeof(csStateHandler_ByState_UI).GetMethod(e.Value.ToString(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                Debug.LogWarning("Invoke : " + Method.Name + "   " + Entry.eventID + "   " + name);
                Method.Invoke(this, null);
            });

            m_EventTrigger.triggers.Add(Entry);
        }
    }*/

	protected override IEnumerator CoStateActivation(csState _state, Action _callback = null)
	{
		float StateActivationTime = m_fStateActivationTime;

		while (true)
		{
			yield return null;

			StateActivationTime -= Time.deltaTime;

			m_ActivationImage.fillAmount = StateActivationTime / m_fStateActivationTime;

			if (!_state.Active || StateActivationTime <= 0.0f)
			{
				_callback?.Invoke();

				break;
			}
		}
	}

	protected override IEnumerator CoStateCoolDown(Action _callback = null)
	{
		float StateCoolTime = m_fStateCoolTime;

		Utility.Activate(m_BoxCollider, false);

		while (true)
		{
			yield return null;

			StateCoolTime -= Time.deltaTime;

			m_CoolDownCircleImage.fillAmount = StateCoolTime / m_fStateCoolTime;

			//m_CoolDownText.text = ((int)CurrentCoolDown).ToString();
			m_CoolDownText.text = ((float)(Math.Truncate(StateCoolTime * 10.0f) / 10.0f)).ToString();
			//Debug.Log(StateCoolTime);
			if (StateCoolTime <= 0.0f)
			{
				//Debug.Log("Break");
				Utility.Activate(m_BoxCollider, true);

				_callback?.Invoke();

				break;
			}
		}
	}

	/*public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			//Debug.LogError("OnPointerDown : " + name + "    " + eventData.selectedObject.name);
		}

		if (m_ButtonType != ButtonType.Joystick)
		{
			return;
		}

		if(LocalHandler.IsUsingOtherStick.Get())
		{
			Debug.Log("LocalHandler.IsUsingStick : " + LocalHandler.IsUsingOtherStick.Get() + "  " + name);
			return;
		}

		//LocalHandler.IsUsingOtherStick.Set(true);

		m_bIsPointDown = true;

		StateReady();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			//Debug.LogError("OnPointerUp : " + name + "    " + eventData.selectedObject.name);			
		}

		if (m_ButtonType != ButtonType.Joystick)
		{
			return;
		}

		if(!m_bIsPointDown)
		{
			Debug.Log("m_bIsPointDown : " + m_bIsPointDown + "   " + name);
			return;
		}

		//LocalHandler.IsUsingOtherStick.Set(false);

		eventData.selectedObject = null;
		m_bIsPointDown = false;

		StateStart();
	}*/

	/*public void OnUpdateSelected(BaseEventData eventData)
	{
		Debug.LogError("OnUpdateSelected : " + name + "    " + eventData.selectedObject.name);

		return;
	}

	public void OnCancel(BaseEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnCancel : " + name + "    " + eventData.selectedObject.name);
		}
	}

	public void OnSubmit(BaseEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnSubmit : " + name + "    " + eventData.selectedObject.name);
		}
	}

	public void OnInitializePotentialDrag(PointerEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnInitializePotentialDrag : " + name + "    " + eventData.selectedObject.name);
		}
	}

	public void OnMove(AxisEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnMove : " + name + "    " + eventData.selectedObject.name);
		}
	}

	public void OnDeselect(BaseEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnDeselect : " + name + "    " + eventData.selectedObject.name);
		}
	}

	public void OnSelect(BaseEventData eventData)
	{		
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnSelect : " + name + "    " + eventData.used + "   " + eventData.selectedObject.name + "    " + eventData);
			//eventData.selectedObject = null;
		}
	}

	public void OnScroll(PointerEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnScroll : " + name + "    " + eventData.selectedObject.name);
		}
	}

	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnDrop : " + name + "    " + eventData.selectedObject.name);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnEndDrag : " + name + "    " + eventData.selectedObject.name);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnDrag : " + name + "    " + eventData.selectedObject.name);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnBeginDrag : " + name + "    " + eventData.selectedObject.name);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnPointerExit : " + name + "    " + eventData.selectedObject.name);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnPointerClick : " + name + "    " + eventData.selectedObject.name);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.selectedObject != null)
		{
			Debug.LogError("OnPointerEnter : " + name + "    " + eventData.selectedObject.name);
		}
	}*/
}
