using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csStateHandler_ByState : csStateHandler {

    //private IEnumerator m_CoStateCancel;              
	private IEnumerator m_CoStateActivation;
	private IEnumerator m_CoStateCoolDown = null;

	public csState m_StateReady;
	public csState m_StateAction;
	public csState m_StateUpdate;

	[HideInInspector]
	public csStateHandler_ByState_UI m_StateHandler_ByState_UI = null;

	public float m_fStateActivationTime = 0.0f;
	public float m_fStateCoolTime = 0.0f;

	public bool m_bIsStateActivation = false;
	public bool m_bIsImmediatelyCoolDown = false;

	public override void Settings(csInfo_Hero _info, int _actionIndex)
	{
		base.Settings(_info, _actionIndex);

		BugsAction BugsAction = _info.m_BugsAction[_actionIndex];

		m_fStateActivationTime = BugsAction.ActivationTime;
		m_fStateCoolTime = BugsAction.CoolTime;

		m_bIsStateActivation = BugsAction.IsActivation;
		m_bIsImmediatelyCoolDown = BugsAction.IsImmediatelyCoolDown;

		StateHandlerSettings_ByState StateHandlerSettings_ByState = _info.m_StateHandlerSettings_ByState[_actionIndex];

		m_StateReady = EventHandler_Hero.GetState(StateHandlerSettings_ByState.StateReadyType);
		m_StateAction = EventHandler_Hero.GetState(StateHandlerSettings_ByState.StateActionType);
		m_StateUpdate = EventHandler_Hero.GetState(StateHandlerSettings_ByState.StateUpdateType);
	}

	protected virtual void StateReady()
	{
		Debug.Log("StateReady : " + m_StateReady);

		if (m_StateReady == null)
		{
			Debug.Log("m_StateReady is Null");

			return;
		}

		Owner.StateStart(m_StateReady);
	}

	protected virtual void StateAction()
	{
		Debug.Log("StateAction : " + m_StateAction);

		if (m_StateAction == null)
		{
			Debug.Log("m_StateStart is Null");

			return;
		}

		if(m_StateReady != null)
		{
			Owner.StateStop(m_StateReady);
		}
		
		if (EventHandler_Hero.ActionDirection.Get() == Vector3.zero && EventHandler_Hero.IsUsedStick.Get())
		{		
			return;
		}

		/*if (m_StateAction.StateName == BugsState.Attack_Action)
		{
			bool test = false;

			if (!LocalHandler.IsThirdPerson.Get() && !m_StateAction.Active && !LocalHandler.IsUsableBullet.Send_Return(test))
			{
				//Debug.Log("!LocalHandler.IsUsableBullet.Send_Return(test)");

				//LocalHandler.StopRender.Send();
				//StateManager.StateStop(LocalHandler.Attack_Ready);

				return;
			}
		}*/
		//Debug.Log("!!!!!!!!!!!!!! : " + m_StateStart.EventName);
		//Debug.LogError("!!!!!!!!!!!!!!!POWER : " + m_Owner.ObjectInfo.Power);
		Owner.StateStart(m_StateAction, () =>
		{
			if (m_bIsStateActivation)
			{
				StateActivation(m_StateAction, () =>
				{
					if (m_fStateCoolTime != 0.0f)
					{
						StartCoolDown();
					}
				});
			}
		});

		/*if (m_CoStateCancel != null)
		{
			StopCoroutine(m_CoStateCancel);
			m_CoStateCancel = null;
		}*/
	}

	protected virtual void StateActivationStop(csState _state)
	{
		Owner.StateStop(_state);
	}

	protected virtual void StateUpdate()
	{
		if(m_StateUpdate == null)
		{
			Debug.Log("m_StateUpdate is Null");

			return;
		}

		Debug.Log("m_StateUpdate : " + m_StateUpdate.EventName);

		if(!Owner.IsActiveState(m_StateUpdate))
		{
			Owner.StateStart(m_StateUpdate, () =>
			{
				if (m_bIsStateActivation)
				{
					StateActivation(m_StateAction, () =>
					{
						if (m_fStateCoolTime != 0.0f)
						{
							StartCoolDown();
						}
					});
				}
			});
		}		
	}

	protected virtual void StateEnded()
	{
		Debug.Log("StateEnded : " + m_StateAction);
		if (m_fStateCoolTime != 0.0f)
		{
			if (Owner.IsActiveState(m_StateAction))
			{
				StartCoolDown();

				return;
			}

			if(!m_bIsImmediatelyCoolDown && !Owner.IsActiveState(m_StateAction))
			{
				StartCoolDown();
			}
		}
	}

	protected virtual void StateStop()
	{
		if (Owner.IsActiveState(m_StateAction))
		{
			Debug.LogError("@@@@@");
			Owner.StateStop(m_StateAction);
		}
	}

	/*protected virtual void StateCancel()
	{
		//csState State = (csState)(LocalHandler.GetState(m_StateCancel.ToString()));

		if(m_StateCancel == null)
		{
			Debug.Log("m_StateCancel is Null");

			return;
		}

		Debug.Log("StateCancel");
		StateManager.StateStop(m_StateCancel);

		if (m_CoStateCancel != null)
		{
			StopCoroutine(m_CoStateCancel);
			m_CoStateCancel = null;
		}
	}*/

	protected virtual void StateActivation(csState _state, Action _callback = null)
	{
		Utility.Activate(m_StateHandler_ByState_UI.m_ActivationImage.gameObject, true);

		m_CoStateActivation = CoStateActivation(_state, () =>
		{
			_callback?.Invoke();

			m_CoStateActivation = null;

			StateActivationStop(_state);

			Utility.Activate(m_StateHandler_ByState_UI.m_ActivationImage.gameObject, false);
		});

		StartCoroutine(m_CoStateActivation);
	}

	protected virtual void StartCoolDown(Action _callback = null)
	{
		Utility.Activate(m_StateHandler_ByState_UI.m_CoolDownText.gameObject, true);

		m_CoStateCoolDown = CoStateCoolDown(() =>
		{
			_callback?.Invoke();

			m_CoStateCoolDown = null;

			Utility.Activate(m_StateHandler_ByState_UI.m_CoolDownText.gameObject, false);
		});

		StartCoroutine(m_CoStateCoolDown);
	}

	#region Command Method

	#endregion

	#region Coroutine

	protected virtual IEnumerator CoStateActivation(csState _state, Action _callback = null)
	{
		float StateActivationTime = m_fStateActivationTime;

		while (true)
		{
			yield return null;

			StateActivationTime -= Time.deltaTime;

			if (!_state.Active || StateActivationTime <= 0.0f)
			{
				_callback?.Invoke();

				break;
			}
		}
	}

	protected virtual IEnumerator CoStateCoolDown(Action _callback = null)
	{
		float StateCoolTime = m_fStateCoolTime;

		while (true)
		{
			yield return null;

			StateCoolTime -= Time.deltaTime;

			if (StateCoolTime <= 0.0f)
			{
				_callback?.Invoke();

				break;
			}
		}
	}

	#endregion
}
