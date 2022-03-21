using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csStateManager : MonoBehaviour {

    public Dictionary<string, csState> m_DicCurrentActiveState = new Dictionary<string, csState>();

	private csEventHandler_Bugs m_EventHandler = null;

	public List<string> m_TestCurrentActiveState = null;
	public List<string> m_TestCurrentLockState = null;
	public List<string> m_TestCurrentReservationState = null;

	/*private void Update()
	{
		foreach(string Event in m_DicCurrentActiveState.Keys)
		{
			Debug.Log("###### : " + Event);
		}
	}*/

	private void Awake()
	{
		m_EventHandler = GetComponent<csEventHandler_Bugs>();
	}

	public void StateStart(csState _state, Action _callback = null)
    {
		//Debug.LogError(_state);
		foreach (csState CurrentState in m_DicCurrentActiveState.Values)
		{
			//Debug.Log("StateStart : " + _state + " | CurrentState :  " + State + " | " + State.CheckLockState(_state.StateName) + " | " + _state.StateStopReservation);
			if(CurrentState.CheckLockState(_state.StateNameType))
			{
				if(_state.IsReservation)
				{
					RegisterReservationState(CurrentState, _state, _callback);
				}
				
				return;
			}

			if(CurrentState == _state)
			{
				if (_state.IsSameReservation)
				{
					RegisterReservationState(CurrentState, _state, _callback);
				}

				return;
			}
		}
		
		//StateStart(_state, _callback);

		//Debug.Log("StateStart : " + _state.EventName);
		if (_state != null)
		{
			//Debug.Log("StateStart : " + _state);
			//Debug.LogError("FDSDASF  : " + _state.EventName);
			//Debug.Log("!m_DicCurrentActiveState.ContainsValue(_state) : " + m_DicCurrentActiveState.ContainsValue(_state));
			//Debug.Log("_state != null : " + _state.EventName);
			//Debug.Log(_state.EventName + "   " + _state.Active);
			if (!_state.Active)
			{
				LockState_StartLock(_state);

				if (!m_DicCurrentActiveState.ContainsValue(_state))
				{
					m_DicCurrentActiveState.Add(_state.EventName, _state);
					m_TestCurrentActiveState.Add(_state.EventName);
				}

				//Debug.Log("!_state.Active : " + _state.EventName);

				_state.Start(() =>
				{
					_callback?.Invoke();
				});

				/*if (_callback != null)
				{
					_state.Start(() =>
					{
						_callback();
					});
				}
				else
				{
					_state.Start();
				}*/
			}
		}
	}

	public void StateStop(string _name, Action _callback = null)
	{
		csState State = GetCurrentActiveState(_name);

		StateStop(State, _callback);
	}

	public void StateStop(csState _state, Action _callback = null)
	{
		if(_state != null && _state.Active)
		{
			_state.Stop(() =>
			{
				LockState_StopLock(_state);

				_callback?.Invoke();
			});

			RemoveCurrentState(_state.EventName);

			ReservationStateStart(_state);
		}		
	}

	public void StateForceStop(csState _state, Action _callback = null)
	{
		if (_state != null && _state.Active)
		{
			_state.ForceStop(() =>
			{
				LockState_StopLock(_state);

				_callback?.Invoke();
			});

			RemoveCurrentState(_state.EventName);

			ReservationStateStart(_state);
		}
	}

	private List<csState> GetListLockState(csState _state)
	{
		List<csState> ListLockState = new List<csState>();

		for (int i = 0; i < _state.m_ListLockState.Count; i++)
		{
			eBugsStateType LockStateNemeType = _state.m_ListLockState[i];

			m_EventHandler.DicStates.TryGetValue(LockStateNemeType, out csState LockState);

			if (LockState != null)
			{
				ListLockState.Add(LockState);
			}
		}

		return ListLockState;
	}

	private void LockState_StartLock(csState _state)
	{
		List<csState> ListLockState = GetListLockState(_state);

		for (int i = 0; i < ListLockState.Count; i++)
		{
			StateStop(ListLockState[i]);

			ListLockState[i].StartLock();

			m_TestCurrentLockState.Add(ListLockState[i].EventName);
		}
	}

	private void LockState_StopLock(csState _state)
	{
		List<csState> ListLockState = GetListLockState(_state);

		for (int i = 0; i < ListLockState.Count; i++)
		{			
			ListLockState[i].StopLock();
			m_TestCurrentLockState.Remove(ListLockState[i].EventName);
		}
	}

	private void ReservationStateStart(csState _state)
	{
		csState ReservationState = null;
		Action ReservationCallback = null;

		if (_state.m_ReservationState != null)
		{
			ReservationState = _state.m_ReservationState;
			ReservationCallback = _state.m_ReservationCallback;
			_state.m_ReservationState = null;
			_state.m_ReservationCallback = null;

			m_TestCurrentReservationState.Remove(_state.EventName);
			Debug.Log("Reservation Start : " + ReservationState);
			//StateStart(ReservationState, ReservationCallback);

			Debug.Log("ReservationState.m_StateStopReservationCancel : " + ReservationState.m_ReservationCancel);
			if (ReservationState.m_ReservationCancel != null)
			{
				Debug.Log("ReservationState.m_ReservationCancel != null : " + ReservationState.m_ReservationCancel + "    " + ReservationState.m_ReservationCancel.Active);
				if (!ReservationState.m_ReservationCancel.Active)
				{
					Debug.Log("_state : " + ReservationState);
					StateStart(ReservationState, ReservationCallback);
				}
			}
			else
			{
				Debug.Log("_state : " + ReservationState);
				StateStart(ReservationState, ReservationCallback);
			}
		}
	}	

	public void AllStateStop()
	{
		string[] StrArray = new string[m_DicCurrentActiveState.Count];

		m_DicCurrentActiveState.Keys.CopyTo(StrArray, 0);

		for(int i = 0; i < StrArray.Length; i++)
		{
			//Debug.Log(StrArray[i]);
			if(m_DicCurrentActiveState.ContainsKey(StrArray[i]))
			{
				csState State = m_DicCurrentActiveState[StrArray[i]];
				//Debug.Log("AllStateStop : " + State.EventName);
				if (State.Active)
				{
					StateStop(State);
				}
			}					
		}

		m_DicCurrentActiveState.Clear();
		//m_DicCurrentActiveState = null;
	}

	private void RegisterReservationState(csState _current, csState _reservation, Action _callback)
	{
		Debug.Log("Reservation Parent : " + _current + " Reservation Children :  " + _reservation);
		_current.m_ReservationState = _reservation;
		_current.m_ReservationCallback = _callback;

		if (!m_TestCurrentReservationState.Contains(_reservation.EventName))
		{
			m_TestCurrentReservationState.Add(_reservation.EventName);
		}
	}

	private csState GetCurrentActiveState(string _name)
    {
        csState State = null;

		if(m_DicCurrentActiveState != null && m_DicCurrentActiveState.ContainsKey(_name))
		{
			m_DicCurrentActiveState.TryGetValue(_name, out State);
		}
		
		return State;
    }

	private void RemoveCurrentState(string _name)
	{
		if (m_DicCurrentActiveState != null && m_DicCurrentActiveState.ContainsKey(_name))
		{
			//Debug.Log("Stop State : " + _name);
			m_DicCurrentActiveState.Remove(_name);
			m_TestCurrentActiveState.Remove(_name);
		}
	}
}
