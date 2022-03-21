using System;
using UnityEngine;

/*2018.06.09
 * hkh
 * 데이터 타입 별로 인스턴스화 하여 값의 저장 및 읽기
 */
public class csValue<V> : csEvent {

    public delegate T Getter<T>();
    public delegate void Setter<T>(T o);

    public Getter<V> Get;   // 값을 읽어오는 delegate 변수  
    public Setter<V> Set;   // 값을 저장하는 delegate 변수

    private V m_Value;   // 타입의 따른 값이 저장되는 변수

    // 생성자
    public csValue(string name) : base(name)
    {
        Register();
    }

    protected override void InitFields()
    {
        throw new NotImplementedException();
    }

    public override void Register(object _obj, string _method, int _variant)
    {
        throw new NotImplementedException();
    }

    /*2018.06.09
    * hkh
    * delegate 변수에 메소드 등록
    */
    private void Register()
    {
        Get = GetValue;
        Set = SetValue;
    }

    /*2018.06.09
    * hkh
    * 등록 해지
    */
    private void Unregister()
    {
        Get = null;
        Set = null;
    }

    /*2018.06.09
    * hkh
    * 현재 저장 된 값을 읽어옴
    */
    private V GetValue()
    {
        return m_Value;
    }

    /*2018.06.09
    * hkh
    * 값을 저장
    */
    private void SetValue(V _value)
    {       
        m_Value = _value;
    }
}
