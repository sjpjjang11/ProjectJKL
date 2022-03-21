using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

/*2018.06.12
* hkh
* 매개변수, 반환 없는 간단한 외부 메소드 호출을 위한 이벤트 클래스
* 외부 메소드의 이름에는 Prefixes 변수에 포함 된 접두어가 필요
*/
public class csCommand : csEvent
{
    public delegate void Sender();
	// 외부 메소드가 등록 될 delegate 변수. 
	// 이 변수를 호출하여 외부 메소드를 호출함
	public Sender Send;  

    public csCommand(string _name) : base(_name)
    {
        InitFields();
    }

    /*2018.06.12
    * hkh
    * 타입 및 접두어 초기화
    */
    protected override void InitFields()
    {
        m_Fields = new FieldInfo[] 
        {
            GetType().GetField("Send")
        };

		StoreFieldNames();

		m_DelegateTypes = new Type[] 
        {
            typeof(Sender)
        };

		m_DicPrefixes = new Dictionary<string, int>()
        {
            { "OnCommand_", 0 }
        };
    }

    /*2018.06.12
    * hkh
    * 이벤트에 외부 메소드 등록
    */
    public override void Register(object _obj, string _method, int _variant)
    {
        if (_method == null)
        {
            return;
        }

        Send += (Sender)Delegate.CreateDelegate(m_DelegateTypes[_variant], _obj, _method);
    }
}

/*2018.06.12
* hkh
* 매개변수가 포함 된 외부 메소드 호출을 위한 이벤트 클래스
* 외부 메소드의 이름에는 Prefixes 변수에 포함 된 접두어가 필요
*/
public class csCommand<V> : csCommand
{
    public delegate void Sender<T>(T _value);
	public delegate T Sender_Return<T>(T _value);
	// 외부 메소드가 등록 될 delegate 변수.
	// 이 변수를 호출하여 외부 메소드를 호출함.
	// 한 개의 매개변수 전달 가능
	public new Sender<V> Send;
	public Sender_Return<V> Send_Return;

	public csCommand(string _name) : base(_name) { }

    /*2018.06.12
    * hkh
    * 타입 및 접두어 초기화
    */
    protected override void InitFields()
    {
        m_Fields = new FieldInfo[] 
        {
            GetType().GetField("Send"),
			GetType().GetField("Send_Return")
        };

		StoreFieldNames();

		m_DelegateTypes = new Type[]
		{
			typeof(csCommand<>.Sender<>),
			typeof(csCommand<>.Sender_Return<>)
        };

		m_DicPrefixes = new Dictionary<string, int>()
        {
            { "OnCommand_", 0 },
			{ "OnCommandReturn_", 1 }
        };
    }

    /*2018.06.12
    * hkh
    * 필드에 외부 메소드 등록
    */
    public override void Register(object _obj, string _method, int _variant)
    {
        if (_method == null)
        {
            return;
        }

		//RegisterExternalMethodToField(_obj, m_Fields[_variant], _method, MakeGenericType(m_DelegateTypes[_variant]));
		AddExternalMethodToField(_obj, m_Fields[_variant], _method, MakeGenericType(m_DelegateTypes[_variant]));
    }
}

/*2018.07.24
* hkh
* 매개 변수 두 개를 사용하여 어떤 델리게이트를 등록하고 
* 호출하느냐에 따라 두 개의 인수를 전달하여 외부 메소드를 호출 할 수 있고, 
* 한 개의 인수를 전달하여 반환값을 얻을 수 있다.
* 외부 메소드의 이름에는 Prefixes 변수에 포함 된 접두어가 필요
*/
public class csCommand<V1, V2> : csCommand
{
	public delegate void Sender<T1, T2>(T1 _value1, T2 _value2);
	public delegate T2 Sender_Return<T1, T2>(T1 _value);

	public new Sender<V1, V2> Send;  // 외부 메소드가 등록 될 delegate 변수. 이 변수를 호출하여 외부 메소드를 호출함
	public Sender_Return<V1, V2> Send_Return;

	public csCommand(string _name) : base(_name) { }

	/*2018.06.12
    * hkh
    * 타입 및 접두어 초기화
    */
	protected override void InitFields()
	{
		m_Fields = new FieldInfo[]
		{
			GetType().GetField("Send"),
			GetType().GetField("Send_Return")
		};

		StoreFieldNames();

		m_DelegateTypes = new Type[]
		{
			typeof(csCommand<,>.Sender<,>),
			typeof(csCommand<,>.Sender_Return<,>)
		};

		m_DicPrefixes = new Dictionary<string, int>()
		{
			{ "OnCommand_", 0 },
			{ "OnCommandReturn_", 1 }
		};
	}

	/*2018.06.12
    * hkh
    * 필드에 외부 메소드 등록
    */
	public override void Register(object _obj, string _method, int _variant)
	{
		if (_method == null)
		{
			return;
		}

		AddExternalMethodToField(_obj, m_Fields[_variant], _method, MakeGenericType(m_DelegateTypes[_variant]));
		//RegisterExternalMethodToField(_obj, m_Fields[_variant], _method, MakeGenericType(m_DelegateTypes[_variant]));
	}
}

/*2018.07.24
* hkh
* 매개 변수 세 개를 사용하여 어떤 델리게이트를 등록하고 
* 호출하느냐에 따라 세 개의 인수를 전달하여 외부 메소드를 호출 할 수 있고, 
* 두 개의 인수를 전달하여 반환값을 얻을 수 있다.
* 외부 메소드의 이름에는 Prefixes 변수에 포함 된 접두어가 필요
*/
public class csCommand<V1, V2, V3> : csCommand
{
	public delegate void Sender<T1, T2, T3>(T1 _value1, T2 _value2, T3 _value3);
	public delegate T3 Sender_Return<T1, T2, T3>(T1 _value1, T2 _value2);

	public new Sender<V1, V2, V3> Send;  
	public Sender_Return<V1, V2, V3> Send_Return;

	public csCommand(string _name) : base(_name)
	{
		InitFields();
	}

	/*2018.06.12
    * hkh
    * 타입 및 접두어 초기화
    */
	protected override void InitFields()
	{
		m_Fields = new FieldInfo[]
		{
			GetType().GetField("Send"),
			GetType().GetField("Send_Return")
		};

		StoreFieldNames();

		m_DelegateTypes = new Type[]
		{
			typeof(csCommand<,,>.Sender<,,>),
			typeof(csCommand<,,>.Sender_Return<,,>)
		};

		m_DicPrefixes = new Dictionary<string, int>()
		{
			{ "OnCommand_", 0 },
			{ "OnCommandReturn_", 1 }
		};
	}

	/*2018.06.12
    * hkh
    * 필드에 외부 메소드 등록
    */
	public override void Register(object _obj, string _method, int _variant)
	{
		if (_method == null)
		{
			return;
		}

		AddExternalMethodToField(_obj, m_Fields[_variant], _method, MakeGenericType(m_DelegateTypes[_variant]));
		//RegisterExternalMethodToField(_obj, m_Fields[_variant], _method, MakeGenericType(m_DelegateTypes[_variant]));
	}
}
