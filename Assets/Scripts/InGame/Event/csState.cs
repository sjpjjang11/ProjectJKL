using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*2018.07.07
* hkh
* 플레이어의 특정 상태를 관리하는 외부 메소드 호출을 위한 이벤트 클래스 
* 외부 메소드의 이름에는 Prefixes 변수에 포함 된 접두어가 필요
*/
public class csState : csEvent
{
    public delegate void Callback(Action _callback = null);

    // 외부 메소드가 등록 될 delegate 변수. 이 변수를 호출하여 외부 메소드를 호출함
    public Callback StartCallback;
    public Callback StopCallback;
	public Callback ForceStopCallback;

	public List<eBugsStateType> m_ListLockState = new List<eBugsStateType>();

	public csState m_ReservationState = null;
	public csState m_ReservationCancel = null;
	public Action m_ReservationCallback = null;

	protected eBugsStateType m_StateNameType;        // csPlayerEventHandler에 선언된 필드 이름. 이벤트 식별에 사용됨
	public eBugsStateType StateNameType
	{
		get
		{
			return m_StateNameType;
		}
	}

	protected bool m_bIsActive;   // 상태의 현재 활성화 상태
    public bool Active          // 활성화 여부를 얻어오거나 값을 설정함
    {
        get
        {
            return m_bIsActive;
        }
        private set
        {
            if ((value == true) && !m_bIsActive)
            {
				m_bIsActive = true;
            }
            else if ((value == false) && m_bIsActive)
            {
				m_bIsActive = false;
            }
        }
    }

	protected bool m_bIsSameReservation = false;
	public bool IsSameReservation
	{
		get
		{
			return m_bIsSameReservation;
		}
	}

	protected bool m_bIsReservation = false;
	public bool IsReservation
	{
		get
		{
			return m_bIsReservation;
		}
	}

	protected bool m_bIsLock;		// 상태의 현재 잠금 상태
	public bool Locked			// 잠김 여부를 얻어옴
	{
		get
		{
			return m_bIsLock;
		}
	}

    public csState(string _name) : base(_name)
    {
		m_StateNameType = (eBugsStateType)Enum.Parse(typeof(eBugsStateType), _name);
		InitFields();
    }

    /*2018.07.07
    * hkh
    * 타입 및 접두어 초기화
    */
    protected override void InitFields()
    {
		m_Fields = new FieldInfo[]
		{
			Type.GetField("StartCallback"),
			Type.GetField("StopCallback"),
			Type.GetField("ForceStopCallback")
        };

		StoreFieldNames();

		m_DelegateTypes = new Type[] 
        {
            typeof(Callback),
            typeof(Callback),
			typeof(Callback)
		};

		m_DicPrefixes = new Dictionary<string, int>()
        {
            { "OnStart_", 0 },
            { "OnStop_", 1 },
			{ "OnForceStop_", 2 }
        };
    }

    public override void Register(object _obj, string _method, int _variant)
    {
        //RegisterExternalMethodToField(_obj, m_Fields[_variant], _method, m_DelegateTypes[_variant]);
		AddExternalMethodToField(_obj, m_Fields[_variant], _method, m_DelegateTypes[_variant]);
    }	

	/*2018.07.07
    * hkh
    * 상태 시작 메소드 호출. 만약 인수로 콜백을 전달받으면 콜백을 포함한 델리게이트 호출
    */
	public virtual void Start(Action _callback = null)
	{
		if (Locked)
		{
			Debug.LogError("Locked : " + EventName);
			return;
		}
		//Debug.LogError("Locked : " + EventName + "   " + Locked);
		// 만약 이미 활성화 상태라면
		if (Active)
        {
            return;
        }

		// 상태 시작이므로 값 설정
		Active = true;
		//Debug.Log("Start!!!!!!!");
		// 등록된 외부 메소드 호출
		if (StartCallback != null)
		{
			if (_callback != null)
			{
				//Debug.Log(m_EventName + "   Start");
				StartCallback(_callback);
			}
			else
			{
				//Debug.Log(m_EventName + "   Start");
				StartCallback();
			}
		}
		//Debug.Log(EventName);
		//StartCallback(_callback);
	}

    /*2018.07.07
    * hkh
    * 상태 종료 메소드 호출. 만약 인수로 콜백을 전달받으면 콜백을 포함한 델리게이트 호출
    */
    public virtual void Stop(Action _callback = null)
    {
		//Debug.Log(EventName + " Stop : " + Active);
        // 만약 이미 종료 상태라면
        if (!Active)
        {
            return;
        }

		// 상태 종료이므로 값 설정
		Active = false;

		if (StopCallback != null)
		{
			// 등록된 외부 메소드 호출
			if (_callback != null)
			{
				//Debug.Log(m_EventName + "   Stop");
				StopCallback(_callback);
			}
			else
			{
				//Debug.Log(m_EventName + "   Stop");
				StopCallback();
			}
		}
	}

	public virtual void ForceStop(Action _callback = null)
	{
		if (!Active)
		{
			return;
		}

		Active = false;

		if (_callback != null)
		{
			ForceStopCallback(_callback);
		}
		else
		{
			ForceStopCallback();
		}
	}

	/*2018.08.03
    * hkh
    * 상태 잠금 설정
    */
	public virtual void StartLock()
	{
		// 잠김 상태가 아니면 잠금
		if(!Locked)
		{
			m_bIsLock = true;
		}
		//Debug.LogError("StartLock : " + EventName);
		// 잠금 시간을 설정 후 시간 초과시 StopLock 호출
		//m_Timer.OnDelay(_delay, StopLock);
	}

	/*2018.08.03
    * hkh
    * 상태 잠금 해제
    */
	public virtual void StopLock()
	{
		// 잠김 상태이면 해제
		if (Locked)
		{
			m_bIsLock = false;
		}
	}

	public void AddLockState(eBugsStateType _state)
	{
		m_ListLockState.Add(_state);
	}

	public void AllLockState()
	{
		AddLockState(eBugsStateType.Walk);
		//AddLockState(eBugsStateType.Run);
		AddLockState(eBugsStateType.Rotate);
		AddLockState(eBugsStateType.Attack_Ready);
		AddLockState(eBugsStateType.Attack_Action);
		AddLockState(eBugsStateType.Attack);
		AddLockState(eBugsStateType.Skill_1_Ready);
		AddLockState(eBugsStateType.Skill_1_Action);
		AddLockState(eBugsStateType.Skill_1);
		AddLockState(eBugsStateType.Skill_2_Ready);
		AddLockState(eBugsStateType.Skill_2_Action);
		AddLockState(eBugsStateType.Skill_2);
		AddLockState(eBugsStateType.Skill_3_Ready);
		AddLockState(eBugsStateType.Skill_3_Action);
		AddLockState(eBugsStateType.Skill_3);
		//AddLockState(eBugsStateType.Skill_Default_Ready);
		//AddLockState(eBugsStateType.Skill_Default_Action);
		//AddLockState(eBugsStateType.Skill_Default);
		AddLockState(eBugsStateType.Slow);
		AddLockState(eBugsStateType.Stun);
		AddLockState(eBugsStateType.KnockBack);
		AddLockState(eBugsStateType.Hero);
		AddLockState(eBugsStateType.Pet);
		AddLockState(eBugsStateType.Groggy);
		AddLockState(eBugsStateType.Escape_Ready);
		AddLockState(eBugsStateType.Escape_Action);
		AddLockState(eBugsStateType.Escape);
		AddLockState(eBugsStateType.Die);
	}

	public void ClearLockState()
	{
		m_ListLockState.Clear();
	}

	public bool CheckLockState(eBugsStateType _stateName)
	{
		bool Result = false;

		/*eBugsStateType Parse;

		try
		{
			Parse = (eBugsStateType)Enum.Parse(typeof(eBugsStateType), _stateName);
		}
		catch
		{
			return Result;
		}		*/

		if (m_ListLockState.Contains(_stateName))
		{
			Result = true;
		}

		return Result;
	}
}

public class Waiting : csState
{
	public Waiting(string _name) : base(_name)
	{
		//AddLockState(eBugsStateType.Walk);
		//AddLockState(eBugsStateType.Run);
		//AddLockState(eBugsStateType.Rotate);
		AddLockState(eBugsStateType.Attack_Ready);
		AddLockState(eBugsStateType.Attack_Action);
		AddLockState(eBugsStateType.Attack);
		AddLockState(eBugsStateType.Skill_1_Ready);
		AddLockState(eBugsStateType.Skill_1_Action);
		AddLockState(eBugsStateType.Skill_1);
		AddLockState(eBugsStateType.Skill_2_Ready);
		AddLockState(eBugsStateType.Skill_2_Action);
		AddLockState(eBugsStateType.Skill_2);
		AddLockState(eBugsStateType.Skill_3_Ready);
		AddLockState(eBugsStateType.Skill_3_Action);
		AddLockState(eBugsStateType.Skill_3);
		//AddLockState(eBugsStateType.Skill_Default_Ready);
		//AddLockState(eBugsStateType.Skill_Default_Action);
		//AddLockState(eBugsStateType.Skill_Default);
		AddLockState(eBugsStateType.Slow);
		AddLockState(eBugsStateType.Stun);
		AddLockState(eBugsStateType.KnockBack);
		AddLockState(eBugsStateType.Hero);
		AddLockState(eBugsStateType.Pet);
		AddLockState(eBugsStateType.Groggy);
		AddLockState(eBugsStateType.Escape_Ready);
		AddLockState(eBugsStateType.Escape_Action);
		AddLockState(eBugsStateType.Escape);
		AddLockState(eBugsStateType.Die);
	}
}

public class Pause : csState
{
	public Pause(string _name) : base(_name)
    {

    }
}

public class GameOver : csState
{
	public GameOver(string _name) : base(_name)
	{

	}
}

public class Walk : csState
{
	public Walk(string _name) : base(_name)
	{

	}
}

public class Run : csState
{
	public Run(string _name) : base(_name)
	{
		m_bIsReservation = true;
	}
}

public class Rotate : csState
{
	public Rotate(string _name) : base(_name)
	{

	}
}

/*public class Dash : csState
{
	public Dash(string _name) : base(_name)
	{
		AddLockState(eBugsStateType.Walk);
		AddLockState(eBugsStateType.Run);
		AddLockState(eBugsStateType.Attack_Ready);
		AddLockState(eBugsStateType.Attack);
		AddLockState(eBugsStateType.Skill_1_Action);
		AddLockState(eBugsStateType.Skill_1);
		AddLockState(eBugsStateType.Skill_2_Action);
		AddLockState(eBugsStateType.Skill_2);
		AddLockState(eBugsStateType.Skill_3_Action);
		AddLockState(eBugsStateType.Skill_3);
	}
}*/

public class Attack_Ready : csState
{
	public Attack_Ready(string _name) : base(_name)
	{
		m_bIsReservation = true;
	}
}

public class Attack_Action : csState
{
	public Attack_Action(string _name) : base(_name)
	{
		m_bIsReservation = true;
	}
}

public class Attack : csState
{
	public Attack(string _name) : base(_name)
	{
		m_bIsReservation = true;
	}
}

public class Skill_1_Ready : csState
{
	public Skill_1_Ready(string _name) : base(_name)
	{

	}
}

public class Skill_1_Action : csState
{
	public Skill_1_Action(string _name) : base(_name)
	{
		m_bIsReservation = true;
	}
}

public class Skill_1 : csState
{
	public Skill_1(string _name) : base(_name)
	{
		m_bIsReservation = true;
	}
}

public class Skill_2_Ready : csState
{
	public Skill_2_Ready(string _name) : base(_name)
	{

	}
}

public class Skill_2_Action : csState
{
	public Skill_2_Action(string _name) : base(_name)
	{
		m_bIsReservation = true;
	}
}

public class Skill_2 : csState
{
	public Skill_2(string _name) : base(_name)
	{
		m_bIsReservation = true;
	}
}

public class Skill_3_Ready : csState
{
	public Skill_3_Ready(string _name) : base(_name)
	{

	}
}

public class Skill_3_Action : csState
{
	public Skill_3_Action(string _name) : base(_name)
	{
		m_bIsReservation = true;
	}
}

public class Skill_3 : csState
{
	public Skill_3(string _name) : base(_name)
	{
		m_bIsReservation = true;
	}
}

public class Skill_Default_Ready : csState
{
	public Skill_Default_Ready(string _name) : base(_name)
	{

	}
}

public class Skill_Default_Action : csState
{
	public Skill_Default_Action(string _name) : base(_name)
	{
		m_bIsReservation = true;
		m_bIsSameReservation = true;
	}
}

public class Skill_Default : csState
{
	public Skill_Default(string _name) : base(_name)
	{
		m_bIsReservation = true;
		m_bIsSameReservation = true;
	}
}

public class Slow : csState
{
	public Slow(string _name) : base(_name)
	{

	}
}

public class Stun : csState
{
	public Stun(string _name) : base(_name)
	{
		AddLockState(eBugsStateType.Walk);
		AddLockState(eBugsStateType.Run);
		AddLockState(eBugsStateType.Rotate);
		AddLockState(eBugsStateType.Attack_Ready);
		AddLockState(eBugsStateType.Attack_Action);
		AddLockState(eBugsStateType.Attack);
		AddLockState(eBugsStateType.Skill_1_Ready);
		AddLockState(eBugsStateType.Skill_1_Action);
		AddLockState(eBugsStateType.Skill_1);
		AddLockState(eBugsStateType.Skill_2_Ready);
		AddLockState(eBugsStateType.Skill_2_Action);
		AddLockState(eBugsStateType.Skill_2);
		AddLockState(eBugsStateType.Skill_3_Ready);
		AddLockState(eBugsStateType.Skill_3_Action);
		AddLockState(eBugsStateType.Skill_3);
		AddLockState(eBugsStateType.Skill_Default_Ready);
		AddLockState(eBugsStateType.Skill_Default_Action);
		AddLockState(eBugsStateType.Skill_Default);
	}
}

public class KnockBack : csState
{
	public KnockBack(string _name) : base(_name)
	{
		AddLockState(eBugsStateType.Walk);
		AddLockState(eBugsStateType.Run);
		AddLockState(eBugsStateType.Rotate);
		AddLockState(eBugsStateType.Attack_Ready);
		AddLockState(eBugsStateType.Attack_Action);
		AddLockState(eBugsStateType.Attack);
		AddLockState(eBugsStateType.Skill_1_Ready);
		AddLockState(eBugsStateType.Skill_1_Action);
		AddLockState(eBugsStateType.Skill_1);
		AddLockState(eBugsStateType.Skill_2_Ready);
		AddLockState(eBugsStateType.Skill_2_Action);
		AddLockState(eBugsStateType.Skill_2);
		AddLockState(eBugsStateType.Skill_3_Ready);
		AddLockState(eBugsStateType.Skill_3_Action);
		AddLockState(eBugsStateType.Skill_3);
		AddLockState(eBugsStateType.Skill_Default_Ready);
		AddLockState(eBugsStateType.Skill_Default_Action);
		AddLockState(eBugsStateType.Skill_Default);
	}
}

public class Hero : csState
{
	public Hero(string _name) : base(_name)
	{

	}
}

public class Pet : csState
{
	public Pet(string _name) : base(_name)
	{
		
		AddLockState(eBugsStateType.Attack_Ready);
		AddLockState(eBugsStateType.Attack_Action);
		AddLockState(eBugsStateType.Attack);
		AddLockState(eBugsStateType.Skill_1_Ready);
		AddLockState(eBugsStateType.Skill_1_Action);
		AddLockState(eBugsStateType.Skill_1);
		AddLockState(eBugsStateType.Skill_2_Ready);
		AddLockState(eBugsStateType.Skill_2_Action);
		AddLockState(eBugsStateType.Skill_2);
		AddLockState(eBugsStateType.Skill_3_Ready);
		AddLockState(eBugsStateType.Skill_3_Action);
		AddLockState(eBugsStateType.Skill_3);
		AddLockState(eBugsStateType.Skill_Default_Ready);
		AddLockState(eBugsStateType.Skill_Default_Action);
		AddLockState(eBugsStateType.Skill_Default);
	}
}

public class Groggy : csState
{
	public Groggy(string _name) : base(_name)
	{
		AddLockState(eBugsStateType.Walk);
		AddLockState(eBugsStateType.Run);	
		AddLockState(eBugsStateType.Rotate);
		AddLockState(eBugsStateType.Attack_Ready);
		AddLockState(eBugsStateType.Attack_Action);
		AddLockState(eBugsStateType.Attack);
		AddLockState(eBugsStateType.Skill_1_Ready);
		AddLockState(eBugsStateType.Skill_1_Action);
		AddLockState(eBugsStateType.Skill_1);
		AddLockState(eBugsStateType.Skill_2_Ready);
		AddLockState(eBugsStateType.Skill_2_Action);
		AddLockState(eBugsStateType.Skill_2);
		AddLockState(eBugsStateType.Skill_3_Ready);
		AddLockState(eBugsStateType.Skill_3_Action);
		AddLockState(eBugsStateType.Skill_3);
		AddLockState(eBugsStateType.Skill_Default_Ready);
		AddLockState(eBugsStateType.Skill_Default_Action);
		AddLockState(eBugsStateType.Skill_Default);
	}
}

public class Escape_Ready : csState
{
	public Escape_Ready(string _name) : base(_name)
	{
		AddLockState(eBugsStateType.Walk);
		AddLockState(eBugsStateType.Run);		
		AddLockState(eBugsStateType.Rotate);
		AddLockState(eBugsStateType.Attack_Ready);
		AddLockState(eBugsStateType.Attack_Action);
		AddLockState(eBugsStateType.Attack);
		AddLockState(eBugsStateType.Skill_1_Ready);
		AddLockState(eBugsStateType.Skill_1_Action);
		AddLockState(eBugsStateType.Skill_1);
		AddLockState(eBugsStateType.Skill_2_Ready);
		AddLockState(eBugsStateType.Skill_2_Action);
		AddLockState(eBugsStateType.Skill_2);
		AddLockState(eBugsStateType.Skill_3_Ready);
		AddLockState(eBugsStateType.Skill_3_Action);
		AddLockState(eBugsStateType.Skill_3);
		AddLockState(eBugsStateType.Skill_Default_Ready);
		AddLockState(eBugsStateType.Skill_Default_Action);
		AddLockState(eBugsStateType.Skill_Default);
	}
}

public class Escape_Action : csState
{
	public Escape_Action(string _name) : base(_name)
	{
		AddLockState(eBugsStateType.Walk);
		AddLockState(eBugsStateType.Run);		
		AddLockState(eBugsStateType.Rotate);
		AddLockState(eBugsStateType.Attack_Ready);
		AddLockState(eBugsStateType.Attack_Action);
		AddLockState(eBugsStateType.Attack);
		AddLockState(eBugsStateType.Skill_1_Ready);
		AddLockState(eBugsStateType.Skill_1_Action);
		AddLockState(eBugsStateType.Skill_1);
		AddLockState(eBugsStateType.Skill_2_Ready);
		AddLockState(eBugsStateType.Skill_2_Action);
		AddLockState(eBugsStateType.Skill_2);
		AddLockState(eBugsStateType.Skill_3_Ready);
		AddLockState(eBugsStateType.Skill_3_Action);
		AddLockState(eBugsStateType.Skill_3);
		AddLockState(eBugsStateType.Skill_Default_Ready);
		AddLockState(eBugsStateType.Skill_Default_Action);
		AddLockState(eBugsStateType.Skill_Default);
	}
}

public class Escape : csState
{
	public Escape(string _name) : base(_name)
	{
		AddLockState(eBugsStateType.Walk);
		AddLockState(eBugsStateType.Run);		
		AddLockState(eBugsStateType.Rotate);
		AddLockState(eBugsStateType.Attack_Ready);
		AddLockState(eBugsStateType.Attack_Action);
		AddLockState(eBugsStateType.Attack);
		AddLockState(eBugsStateType.Skill_1_Ready);
		AddLockState(eBugsStateType.Skill_1_Action);
		AddLockState(eBugsStateType.Skill_1);
		AddLockState(eBugsStateType.Skill_2_Ready);
		AddLockState(eBugsStateType.Skill_2_Action);
		AddLockState(eBugsStateType.Skill_2);
		AddLockState(eBugsStateType.Skill_3_Ready);
		AddLockState(eBugsStateType.Skill_3_Action);
		AddLockState(eBugsStateType.Skill_3);
		AddLockState(eBugsStateType.Skill_Default_Ready);
		AddLockState(eBugsStateType.Skill_Default_Action);
		AddLockState(eBugsStateType.Skill_Default);
	}
}

public class Revival : csState
{
	public Revival(string _name) : base(_name)
	{
		AddLockState(eBugsStateType.Walk);
		AddLockState(eBugsStateType.Run);		
		AddLockState(eBugsStateType.Rotate);
		AddLockState(eBugsStateType.Attack_Ready);
		AddLockState(eBugsStateType.Attack_Action);
		AddLockState(eBugsStateType.Attack);
		AddLockState(eBugsStateType.Skill_1_Ready);
		AddLockState(eBugsStateType.Skill_1_Action);
		AddLockState(eBugsStateType.Skill_1);
		AddLockState(eBugsStateType.Skill_2_Ready);
		AddLockState(eBugsStateType.Skill_2_Action);
		AddLockState(eBugsStateType.Skill_2);
		AddLockState(eBugsStateType.Skill_3_Ready);
		AddLockState(eBugsStateType.Skill_3_Action);
		AddLockState(eBugsStateType.Skill_3);
		AddLockState(eBugsStateType.Skill_Default_Ready);
		AddLockState(eBugsStateType.Skill_Default_Action);
		AddLockState(eBugsStateType.Skill_Default);
	}
}

public class Die : csState
{
	public Die(string _name) : base(_name)
	{

	}
}



