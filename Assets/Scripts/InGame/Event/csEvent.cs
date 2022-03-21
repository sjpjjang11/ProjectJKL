using System;
using System.Reflection;
using System.Collections.Generic;

/*2018.06.12
* hkh
* 플레이어 이벤트 클래스의 최상위 클래스
* 상속받는 이벤트들의 공통적인 메소드 및 필드 정의
*/
public abstract class csEvent {

    #region Field

    protected FieldInfo[] m_Fields;             // delegate 타입으로 선언된 필드의 필드 정보 모음

    protected Type[] m_DelegateTypes;           // delegate 타입으로 선언된 필드의 타입 정보 모음
	
	public Dictionary<string, int> m_DicPrefixes;    // 이벤트에 등록될 외부 메소드를 구분하기 위한 접두어 모음

	protected Timer m_Timer = null;

	public string[] FieldNames;					// 이벤트 필드 이름 모음

	protected string m_EventName = null;        // csPlayerEventHandler에 선언된 필드 이름. 이벤트 식별에 사용됨
    public string EventName
    {
        get
        {
            return m_EventName;
        }
    }

    protected Type m_Type = null;               // 클래스의 타입
    public Type Type
    {
        get
        {
            if(m_Type == null)
            {
                m_Type = GetType();
            }

            return m_Type;
        }
    }

    /*protected Type m_ArgumentType = null;       // 클래스의 인수 타입
    public Type ArgumentType                    // 클래스의 Generic 인수 반환
    {
        get
        {
            // 최초 참조일 때
            if(m_ArgumentType == null)
            {
                // Generic 타입이 아니면 인수가 없으므로 void 반환
                if (!Type.IsGenericType)
                {
                    m_ArgumentType = typeof(void);

                    return m_ArgumentType;
                }

                // 인수가 있을시 첫 번째로 배치된다. 그러므로 배열의 0번째 요소 저장
                m_ArgumentType = Type.GetGenericArguments()[0];
            }

            return m_ArgumentType;
        }
    }*/

    /*protected Type m_ReturnType = null;         // 클래스의 반환 타입
    public Type ReturnType                      // 클래스에 반환값이 있을 경우 그 Type을, 없으면 void 반환
    {
        get
        {
            // 최초 참조일 때
            if(m_ReturnType == null)
            {
                // Generic 타입이 아니면 반환값이 없으므로 void 반환
                if (!Type.IsGenericType)
                {
                    m_ReturnType = typeof(void);

                    return m_ReturnType;
                }

                // 이벤트 클래스는 반환값이 있을 경우 매개변수와 반환값 한 쌍으로 이뤄진다 
                //인수가 2개가 아니면 반환값이 없으므로 void 반환
                if (Type.GetGenericArguments().Length != 2)
                {
                    m_ReturnType = typeof(void);

                    return m_ReturnType;
                }

                // 반환값이 있을 경우 두 번째로 배치된다. 그러므로 배열의 1번째 요소 저장
                m_ReturnType = Type.GetGenericArguments()[1];
            }
            
            return m_ReturnType;
        }
    }*/
   
    #endregion

    #region Abstract Method

    /*2018.06.12
    * hkh
    * 필드 초기화 작업
    */
    protected abstract void InitFields();

    /*2018.06.12
    * hkh
    * 이벤트에 외부 메소드 등록
    */
    public abstract void Register(object _obj, string _method, int _variant);

	/*2018.08.15
    * hkh
    * 이벤트에 등록된 메소드 해제
    */
	public virtual void Unregister(object _obj)
	{
		for (int i = 0; i < m_Fields.Length; i++)
		{
			RemoveExternalMethodFromField(_obj, m_Fields[i]);
		}
	}

	#endregion

	// 생성자 인스턴스화할 때 필드 이름을 얻어와 저장
	public csEvent(string _name = "")
    {
        m_EventName = _name;

		m_Timer = new Timer();
	}

	protected void StoreFieldNames()
	{
		FieldNames = new string[m_Fields.Length];

		for (int i = 0; i < m_Fields.Length; i++)
		{
			FieldNames[i] = m_Fields[i].Name;
		}
	}

	/*2018.06.12
    * hkh
    * 클래스 인수 개수에 따른 Generic Type 생성
    */
	protected Type MakeGenericType(Type _type)
    {
		Type MakeType = null;
		Type[] GenericArguments = Type.GetGenericArguments();

		switch(GenericArguments.Length)
		{
			case 1:

				MakeType = _type.MakeGenericType(new Type[] { GenericArguments[0], GenericArguments[0] });
				 
				break;

			case 2:

				MakeType = _type.MakeGenericType(new Type[] { GenericArguments[0], GenericArguments[1], GenericArguments[0], GenericArguments[1] });

				break;

			case 3:

				MakeType = _type.MakeGenericType(new Type[] { GenericArguments[0], GenericArguments[1], GenericArguments[2], GenericArguments[0], GenericArguments[1], GenericArguments[2] });

				break;
		}

		return MakeType;
		/*// 반환값이 없으면
		if (ReturnType == typeof(void))
        {
            // 전달하는 인수가 두 개인 이유는 클래스로 받는 인수와 클래스 내 델리게이트 선언에 필요한 인수 총 두 개가 필요하기 때문
            Debug.Log(EventName + "  " + ArgumentType);
            return _type.MakeGenericType(new Type[] { ArgumentType, ArgumentType });
        }           
        else // 반환값이 있으면
        {
			Debug.Log(EventName + "  " + ArgumentType);
			// 위와 마찬가지로 반환값이 있을 경우 인수와 반환값 한 쌍이 필요한데 델리게이트 선언에 필요한 한 쌍 더 전달
			return _type.MakeGenericType(new Type[] { ArgumentType, ReturnType, ArgumentType, ReturnType });
        }       */   
    }

    /*2018.06.12
    * hkh
    * 필드에 외부 메소드 등록
    */
    protected void RegisterExternalMethodToField(object _obj, FieldInfo _field, string _method, Type _type)
    {        
        // 메소드가 포함된 클래스, 메소드 이름, 등록하려는 클래스 타입에 맞춰서 생성한 타입으로 델리게이트 생성
        Delegate Assignment = Delegate.CreateDelegate(_type, _obj, _method);

        if (Assignment == null)
        {
            return;
        }

        // 필드(해당 이벤트 클래스의 델리게이트 변수)에 델리게이트 등록
        _field.SetValue(this, Assignment);
    }

	/*2018.07.25
    * hkh
    * 필드에 복수의 외부 메소드 등록
    */
	protected void AddExternalMethodToField(object _obj, FieldInfo _field, string _method, Type _type)
	{
		// 복수의 메소드 등록의 경우 델리게이트를 생성 후 Combine으로 연결함
		Delegate Assignment = Delegate.Combine((Delegate)_field.GetValue(this), Delegate.CreateDelegate(_type, _obj, _method));

		if(Assignment == null)
		{
			return;
		}

		_field.SetValue(this, Assignment);
	}

	/*2018.08.15
    * hkh
    * 필드에 등록된 메소드 제거. 여기서 필드란 이벤트 클래스 내에 델리게이트 변수를 의미
    */
	protected void RemoveExternalMethodFromField(object _obj, FieldInfo _field)
	{
		// 필드에 메소드가 등록됐는지 체크
		object Obj = _field.GetValue(this);

		// 필드에 등록된 메소드가 없을 경우
		if (Obj == null)
		{
			return;
		}
		
		// 필드에 등록된 메소드를 델리게이트 리스트에 저장
		List<Delegate> assignment = new List<Delegate>(((Delegate)_field.GetValue(this)).GetInvocationList());

		for (int i = 0; i < assignment.Count; i++)
		{
			// 델리게이트를 생성할 때 델리게이트 생성을 요청한 오브젝트를 Target으로 삼는다(예 : ERWHARITE(Clone))
			// 그 Target과 델리게이트 삭제 요청한 오브젝트가 같으면 삭제한다
			if (assignment[i].Target == _obj)
			{
				assignment.Remove(assignment[i]);
			}
				
		}

		if (assignment != null)
		{
			// 필드에 비워진 값을 적용. Combine을 사용하는 이유는 필드에 복수의 메소드가 등록됐을 경우를 대비함
			_field.SetValue(this, Delegate.Combine(assignment.ToArray()));
		}			
	}
}
