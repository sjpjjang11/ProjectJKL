using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*2018.06.29
* hkh
* 인게임 내 움직이는 모든 오브젝트 클래스의(영웅, 몬스터)의 최상위 클래스
* 이 클래스를 상속받는 클래스들이 공통으로 사용하는 필드 및 메소드가 선언되며브
* 공통으로 사용되는 기능 또한 구현된다
*/
public abstract class csBugs : MonoBehaviour {

	protected Action m_AnimationEventCallback = null;

	protected Dictionary<int, csCrowdControl> m_DicCrowdControl = new Dictionary<int, csCrowdControl>();

	public Transform m_HpBarTr = null;
	public Transform m_UIDamageTr = null;
	public Transform m_TargetingTr = null;

	public GameObject[] m_Effect = null;
	public GameObject[] m_CrowdControlEffect = null;

	public AudioClip[] m_Sound = null;                 // 기본 공격 이펙트 사운드 모음	

	protected Transform m_Transform = null;
	public Transform Transform
	{
		get
		{
			if (m_Transform == null)
			{
				m_Transform = transform;
			}

			return m_Transform;
		}
	}

	protected Animator m_Animator = null;
	public Animator Animator
	{
		get
		{
			if (m_Animator == null)
			{
				m_Animator = GetComponent<Animator>();
			}

			return m_Animator;
		}
	}

	protected csBattleManager m_BattleManager = null;
	protected csSoundManager m_SoundManager = null;

	protected csOwner m_Owner = null;

	protected csRotationToTarget m_RotationToTarget = null;
	protected csActivity m_Activity = null;

	public int ObjectIndex
	{
		get;
		protected set;
	}

	public int m_iTempObjectIndex = 0;

	protected virtual void Awake()
	{
		for (int i = 0; i < m_Effect.Length; i++)
		{
			Utility.Activate(m_Effect[i], false);
		}
	}

	protected virtual void Start()
	{
		
	}

	public virtual void Settings(csOwner _owner)
	{
		m_BattleManager = csBattleManager.Instance;
		m_SoundManager = csSoundManager.Instance;
		m_SoundManager.Register(gameObject);

		m_RotationToTarget = GetComponent<csRotationToTarget>();
		m_Activity = GetComponent<csActivity>();

		m_Owner = _owner;	
	}

	protected virtual void GetBugsAction()
	{

	}

	protected virtual void SettingsCrowdControl(int _actionIndex)
	{

	}



	public virtual void Release()
	{
		m_SoundManager.Unregister(gameObject);
	}

	public virtual BugsAction GetCrowdControl(int _key)
	{
		return new BugsAction();
	}

	public virtual void RotateToTarget(Transform _originTr, Vector3 _target, float _speed = 0.0f, Action _callback = null)
	{
		if(_target != Vector3.zero)
		{
			m_RotationToTarget.Rotation(_originTr, _target, _speed, () =>
			{
				_callback?.Invoke();
			});
		}	
	}

	#region Animation Event Mothod

	/*2018.07.07
    * hkh
    * 해당 index 이펙트 활성화
    */
	protected virtual void EffectOn(int _index)
    {
        if(m_Effect.Length > _index)
        {
            Utility.Activate(m_Effect[_index], true);
        }        
    }

    /*2018.07.07
    * hkh
    * 해당 index 이펙트 비활성화
    */
    protected virtual void EffectOff(int _index)
    {
        if (m_Effect.Length > _index)
        {
            Utility.Activate(m_Effect[_index], false);
        }            
    }

    /*2018.07.07
    * hkh
    * 모든 이펙트 활성화
    */
    protected virtual void EffectOnAll()
    {
        for(int i = 0; i < m_Effect.Length; i++)
        {
            Utility.Activate(m_Effect[i], true);
        }
    }

    /*2018.07.07
    * hkh
    * 모든 이펙트 비활성화
    */
    protected virtual void EffectOffAll()
    {
        for (int i = 0; i < m_Effect.Length; i++)
        {
            Utility.Activate(m_Effect[i], false);
        }
    }

	protected virtual void SoundOn(int _index)
    {
		m_SoundManager.PlayEffect(gameObject, m_Sound[_index]);
    }

	protected virtual void SoundOff(int _index)
	{
		m_SoundManager.StopEffect(gameObject, m_Sound[_index]);
	}

	protected virtual void AnimationEventCallback()
	{
		m_AnimationEventCallback?.Invoke();
	}

	protected virtual void StateStop(eBugsStateType _state)
	{
		Debug.Log("EndState : " + _state);
		m_Owner.StateStop(_state);
	}

	#endregion

	#region BattleObject Action

	public virtual void Initialize()
	{
		//Debug.LogError("Initialize : " + name);
		Animator.SetTrigger("Initialize");
	}

	public virtual void WalkStart()
	{
		//Debug.Log("Walk");
		Animator.SetBool("Walk", true);
	}

	public virtual void WalkStop()
	{
		Animator.SetBool("Walk", false);
	}

	public virtual void RunStart()
	{
		//Debug.Log("RunStart");
		Animator.SetBool("Run", true);
		//Animator.SetTrigger("Run");
	}

	public virtual void RunStop()
	{
		//Debug.Log("RunStop");
		Animator.SetBool("Run", false);
		//Animator.SetTrigger("Stop");
	}

	public virtual void Attack_ActionStart()
	{
		//Debug.Log("Attack_ActionStart");
		Animator.SetTrigger("Attack");
	}

	public virtual void Attack_ActionStop()
	{
		//Animator.SetTrigger("Stop");
	}

	public virtual void Attack_Start()
	{
		Debug.Log("Attack_Start");
		Animator.SetTrigger("Attack");
	}

	public virtual void Attack_Stop()
	{
		//Animator.SetTrigger("Stop");
	}

	public virtual void Attack_ForceStop()
	{

	}

	public virtual void Attack_Start(Action _callback = null)
	{

	}

	public virtual void Slow_Start()
	{

	}

	public virtual void Slow_Stop()
	{

	}

	public virtual void KnockBack_Start()
	{
		CrowdControl_KnockBack KnockBack = (CrowdControl_KnockBack)m_Owner.m_EventHandler_Bugs.CrowdControl.Get();

		//m_RotationToTarget.Rotation(m_Owner.Transform, -KnockBack.KnockBacDirection);

		m_Activity.MoveStart(KnockBack.KnockBacDirection, KnockBack.KnockBackDistance, KnockBack.Time, false, null, () =>
		{
			m_Owner.StateStop(eBugsStateType.KnockBack);
		});
	}

	public virtual void KnockBack_Stop()
	{

	}	

	public virtual void Stun_Start()
	{
		Utility.Activate(m_CrowdControlEffect[0], true);
	}

	public virtual void Stun_Stop()
	{
		Utility.Activate(m_CrowdControlEffect[0], false);
	}

	public virtual void Die()
	{
		Animator.SetTrigger("Die");
	}

	#endregion
}
