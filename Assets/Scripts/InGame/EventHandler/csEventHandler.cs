using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/*2018.06.21
* hkh
* 이벤트 등록 및 사용
* 이벤트 필드 선언 후 기능에 사용될 메소드를 얻어와 필드에 등록 및 사용
*/
public class csEventHandler : MonoBehaviour {

    // 지원되는 접두어 모음 열거형
    public enum SupportedPrefixes
    {
		OnCommand_,
		OnCommandReturn_,
		OnStart_,
        OnStop_,
		OnForceStop_
    }

    // 이벤트 필드 모음
    protected List<FieldInfo> m_ListField = null;

	// 이벤트 필드에 등록할 외부 메소드 모음
	protected List<MethodInfo> m_ListMethod = null;

	// 이벤트 모음
	protected List<csEvent> m_Events = new List<csEvent>();

    // 이벤트 필드를 추가하여 필드에 등록할 외부 메소드 매칭
    protected Dictionary<string, csEvent> m_DicEventsToRegister = new Dictionary<string, csEvent>();
	// 이벤트 필드를 얻기 위한 용도
	protected Dictionary<string, csEvent> m_DicEvents = new Dictionary<string, csEvent>();
	// 상태 필드를 얻기 위한 용도
	protected Dictionary<eBugsStateType, csState> m_DicStates = new Dictionary<eBugsStateType, csState>(new BugsStateTypeCompare());
	public Dictionary<eBugsStateType, csState> DicStates
	{
		get
		{
			return m_DicStates;
		}
	}

    /*2018.06.13
    * hkh
    * 이벤트 필드 얻어오기
    */
    protected void GetFields(Type _type)
	{
		// 필드 저장할 List 인스턴스 생성
		m_ListField = new List<FieldInfo>();

		// 필드 얻어옴
		m_ListField.AddRange(_type.GetFields((BindingFlags.Public | BindingFlags.Instance)));

		for (int i = 0; i < m_ListField.Count; i++)
		{
			//Debug.Log("GetFields : " + m_ListField[i].FieldType + "   " + m_ListField[i].Name);
		}
	}

	/*2018.06.13
    * hkh
    * 이벤트 필드에 등록할 외부 메소드 얻어오기
    */
	protected void GetMethods(Type _type)
	{
		// 메소드 저장할 List 인스턴스 생성
		m_ListMethod = new List<MethodInfo>();

		// 메소드 얻어옴
		MethodInfo[] ArrayMethods = _type.GetMethods((BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

		// 얻어온 메소드에서 식별 작업을 통해 해당하는 메소드일 경우 저장
		for (int i = 0; i < ArrayMethods.Length; i++)
		{
			// 접두어는 접두어 끝에 '_'가 붙어있다 (예 : OnStart_) 
			// 메소드 이름에서 '_'를 기준으로 접두어 추출
			string GetPrefix = ArrayMethods[i].Name.Substring(0, ArrayMethods[i].Name.IndexOf('_') + 1);

			// 예외처리. 얻어온 모든 메소드가 해당하는 접두어를 가지고 있다는 보장이 없기 때문
			try
			{
				// 추출한 접두어(GetPrefix)가 열거형 SupportedPrefixes에 해당하는 접두어인지 확인
				Enum.Parse(typeof(SupportedPrefixes), GetPrefix);
			}
			catch
			{
				// 접두어가 열거형 SupportedPrefixes에 해당하지 않으면 진행하지 않고 건너뛴다
				continue;
			}

			// 여기까지 진행 되면 이벤트에 등록될 메소드라는 의미이므로 저장
			m_ListMethod.Add(ArrayMethods[i]);

			//Debug.Log("GetMethods : " + ArrayMethods[i].Name);
		}
	}

	/*2018.06.12
    * hkh
    * 필드의 인스턴스화 및 외부 메소드를 등록하기 위한 준비
    */
	protected void InstantiateField()
    {
        // GetFields(GetType())에서 얻어온 필드가 없으면 return
        if (m_ListField == null || m_ListField.Count == 0)
        {
            return;
        }

        // 이벤트 필드 처리
        for (int i = 0; i < m_ListField.Count; i++)
        {
            // 필드의 해당 타입과 이름으로 인스턴스 생성
            object Obj = Activator.CreateInstance(m_ListField[i].FieldType, m_ListField[i].Name);

            // 생성한 인스턴스를 필드에 값으로 설정
            m_ListField[i].SetValue(this, Obj);
			//Debug.Log("@@@ : " + m_ListField[i].Name);
            // 모든 이벤트 필드는 csEvent 클래스를 상속 받음. 상속받는 공통 클래스로 캐스팅하여 동일한 처리
            csEvent EventObj = (csEvent)Obj;

			AddStateList(Obj);
			
			// 접두어가 선언되지 않았다는 것은 외부 메소드의 기능 없이 단독으로 기능을 수행하는 클래스이므로 건너뜀
			if (EventObj.m_DicPrefixes == null)
            {
                continue;
            }

			if(!m_Events.Contains(EventObj))
			{
				m_Events.Add(EventObj);
			}

			if(!m_DicEvents.ContainsValue(EventObj))
			{
				//Debug.Log("$#@$#!$#@!$#@!$#@!$#@!%@!#%@#!$#@!$#@!$#@!$#@!$#@!     " + EventObj.EventName);
				m_DicEvents.Add(EventObj.EventName, EventObj);
			}

			// 이벤트 필드의 접두어를 추출, 필드 이름과 묶어서 Key로 삽입한다. (예 : OnStart_(접두어) + NormalAttack(필드 이름) = OnStart_NormalAttack)
			// 이 Key는 외부 메소드의 이름이 되며 외부 메소드 식별에 사용된다.
			foreach (string prefixes in EventObj.m_DicPrefixes.Keys)
            {
                m_DicEventsToRegister.Add(prefixes + m_ListField[i].Name, (csEvent)Obj);
				//Debug.Log(m_DicEventsToRegister.Count + "   " + prefix + m_ListField[i].Name);
                //Debug.Log("InstantiateField : " + prefix + m_ListField[i].Name + "   " + (csEvent)Obj);
            }
		}
    }

	/*2018.07.25
	* hkh
	* 상태 관련 클래스 따로 모음
	*/
	private void AddStateList(object _obj)
	{
		csState State = null;

		try
		{
			State = (csState)_obj;
		}
		catch
		{
			return;
		}

		m_DicStates.Add(State.StateNameType, State);
	}

    /*2018.06.12
    * hkh
    * 이벤트에 외부 메소드 등록
    */
    public void Register(object _obj)
    {
        // 매개변수로 전달받은 클래스의 메소드 추출. m_ListMethod에 저장한다.
        GetMethods(_obj.GetType());
		//Debug.Log("&&&&&&&&&&&& : " + _obj.GetType());
        // InstantiateField 메소드에서 저장한 m_DicEventsToRegister의 csEvent 클래스를 추출하여 저장하기 위한 용도
        csEvent Event = null;

        // 추출한 메소드를 이벤트 필드 내의 델리게이트 변수에 등록
        for(int i = 0; i < m_ListMethod.Count; i++)
        {
            // m_DicEventsToRegister Key에 해당하는 메소드 이름이면 Value를 추출한 후 계속 진행. 없으면 건너뜀
            if (!(m_DicEventsToRegister.TryGetValue(m_ListMethod[i].Name, out Event)))
            {
                /*Debug.Log("@@@@@ : " + m_DicEventsToRegister.Count + "   "  + _obj + "   " + m_ListMethod[i].Name);
				
				foreach(string key in m_DicEventsToRegister.Keys)
				{
					Debug.Log("$$$$ : " + key);
				}*/
                continue;
            }
			//Debug.Log("@@@@@ : " + _obj + "   " + m_ListMethod[i].Name);
			//Debug.Log("Register : " + m_ListMethod[i].Name + "   " + Event);

			// 이벤트 필드에서 접두어를 선언할 때 아래 예처럼 접두어 개수가 복수일 때를 감안하여 식별을 위해 int형 값을 쌍으로 등록한다
			// Prefixes = new Dictionary<string, int>() {  { "OnStart_", 0 },
			//                                      { "OnStop_", 1 }};
			// 그 쌍으로 등록된 값을 추출하여 저장하기 위한 용도
			int Index;

            // 접두어는 접두어 끝에 '_'가 붙어있다 (예 : OnStart_) 
            // 메소드 이름에서 '_'를 기준으로 접두어만 빼내어 그 접두어에 해당하는 값을 추출(예 : OnStart_NormalAttack(메소드 이름) => OnStart_)
            Event.m_DicPrefixes.TryGetValue(m_ListMethod[i].Name.Substring(0, m_ListMethod[i].Name.IndexOf('_') + 1), out Index);
			//Debug.Log(_obj + "  " + m_ListMethod[i].Name + "   " + Event);
            // 필드에 메소드 등록(정확히는 필드 내의 델리게이트 변수에 등록)
            // _obj : 외부 메소드가 선언된 클래스, m_ListMethod[i].Name : 외부 메소드 이름, Index : 접두어 식별값
            Event.Register(_obj, m_ListMethod[i].Name, Index);
        }
    }

	/*2018.08.15
    * hkh
    * 이벤트에 등록된 메소드 해제
    */
	public void Unregister(object _obj)
	{
		if (_obj == null)
		{
			return;
		}

		FieldInfo Field;
		object Obj;
		Delegate Delegate;

		for(int i = 0; i < m_Events.Count; i++)
		{
			if(m_Events[i] == null)
			{
				continue;
			}

			for(int j = 0; j < m_Events[i].FieldNames.Length; j++)
			{
				string FieldName = m_Events[i].FieldNames[j];

				// 이벤트에 해당 필드 존재 유무 체크
				Field = m_Events[i].Type.GetField(FieldName);

				if(Field == null)
				{
					continue;
				}

				// 필드(클래스 내에 델리게이트)에 등록된 메소드 존재 유무 체크
				Obj = Field.GetValue(m_Events[i]);

				if(Obj == null)
				{
					continue;
				}

				Delegate = (Delegate)Obj;

				if(Delegate == null)
				{
					continue;
				}

				for(int k = 0; k < Delegate.GetInvocationList().Length; k++)
				{
					if(Delegate.GetInvocationList()[k].Target != _obj)
					{
						continue;
					}

					m_Events[i].Unregister(_obj);
				}
			}
		}
	}	
}

