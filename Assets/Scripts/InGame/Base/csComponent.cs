using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 2018.06.09
 * hkh
 * 유니티 오브젝트의 기본적인 Component 캐싱
 */
public abstract class csComponent : MonoBehaviour {

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

	protected RectTransform m_RectTransform = null;
	public RectTransform RectTransform
	{
		get
		{
			if(m_RectTransform == null)
			{
				m_RectTransform = GetComponent<RectTransform>();
			}

			return m_RectTransform;
		}
	}

    protected Transform m_Parent = null;
    public Transform Parent
    {
        get
        {
			m_Parent = transform.parent;

			return m_Parent;
        }
    }

    protected Transform m_Root = null;
    public Transform Root
    {
        get
        {
            if (m_Root == null)
            {
                m_Root = transform.root;
            }

            return m_Root;
        }
    }

    protected CharacterController m_CharacterController = null;
    public CharacterController CharacterController
    {
        get
        {
            if (m_CharacterController == null)
            {
                m_CharacterController = gameObject.GetComponent<CharacterController>();
            }

            return m_CharacterController;
        }
    }

	protected Animator m_Animator = null;
	public Animator Animator
	{
		get
		{
			if(m_Animator == null)
			{
				m_Animator = GetComponent<Animator>();
			}

			return m_Animator;
		}
	}

    protected AudioSource m_Audio = null;
    public AudioSource Audio
    {
        get
        {
            if (m_Audio == null)
            {
                m_Audio = GetComponent<AudioSource>();
            }

            return m_Audio;
        }
    }

    protected Collider m_Collider = null;
    public Collider Collider
    {
        get
        {
            if (m_Collider == null)
            {
                m_Collider = GetComponent<Collider>();
            }

            return m_Collider;
        }
    }

    protected BoxCollider m_BoxCollider = null;
    public BoxCollider BoxCollider
    {
        get
        {
            if (m_BoxCollider == null)
            {
                m_BoxCollider = GetComponent<BoxCollider>();
            }

            return m_BoxCollider;
        }
    }

    protected CapsuleCollider m_CapsuleCollider = null;
    public CapsuleCollider CapsuleCollider
    {
        get
        {
            if (m_CapsuleCollider == null)
            {
                m_CapsuleCollider = GetComponent<CapsuleCollider>();
            }

            return m_CapsuleCollider;
        }
    }

	protected Rigidbody m_Rigidbody = null;
	public Rigidbody Rigidbody
	{
		get
		{
			if(m_Rigidbody == null)
			{
				m_Rigidbody = GetComponent<Rigidbody>();
			}

			return m_Rigidbody;
		}
	}

    protected Camera m_Camera = null;
    public Camera Camera
    {
        get
        {
            if (m_Camera == null)
            {
                m_Camera = GetComponent<Camera>();
            }

            return m_Camera;
        }
    }

	protected csBattleManager m_BattleManager = null;
	public csBattleManager BattleManager
	{
		get
		{
			if (m_BattleManager == null)
			{
				m_BattleManager = csBattleManager.Instance;
			}

			return m_BattleManager;
		}		
	}

	protected csBattleUIManager m_BattleUIManager = null;
	public csBattleUIManager BattleUIManager
	{
		get
		{
			if(m_BattleUIManager == null)
			{
				m_BattleUIManager = BattleManager.m_BattleUIManager;
			}

			return m_BattleUIManager;
		}
	}

	protected csSoundManager m_SoundManager = null;
	public csSoundManager SoundManager
	{
		get
		{
			if(m_SoundManager == null)
			{
				m_SoundManager = csSoundManager.Instance;
			}

			return m_SoundManager;
		}
	}

	protected Camera m_UICamera = null;
	public Camera UICamera
	{
		get
		{
			if(m_UICamera == null)
			{
				m_UICamera = BattleUIManager.m_UICamera;
			}

			return m_UICamera;
		}
	}

    protected csEventHandler_Hero m_EventHandler_Hero = null;
    public csEventHandler_Hero EventHandler_Hero
	{
        get
        {
            if (m_EventHandler_Hero == null)
            {
				m_EventHandler_Hero = BattleManager.m_EventHandler_Hero;
            }

            return m_EventHandler_Hero;
        }
    }

	protected csBugsInfoManager m_BugsInfoManager = null;
	public csBugsInfoManager HeroInfoManager
	{
		get
		{
			if(m_BugsInfoManager == null)
			{
				m_BugsInfoManager = BattleManager.m_BugsInfoManager;
			}

			return m_BugsInfoManager;
		}
	}

    protected csPlayerCamera m_PlayerCamera = null;
    public csPlayerCamera PlayerCamera
    {
        get
        {
            if(m_PlayerCamera == null)
            {
                m_PlayerCamera = BattleManager.m_PlayerCamera;
            }

            return m_PlayerCamera;
        }
    }

	protected CountNumber m_CountNumber = null;
	public CountNumber CountNumber
	{
		get
		{
			if(m_CountNumber == null)
			{
				m_CountNumber = new CountNumber();
			}

			return m_CountNumber;
		}
	}

	// 공격 대상 레이어 마스크
	//[SerializeField]
	protected int m_iCollisionLayerMask = 0;

	/*protected void SetCollisionLayerMask(GameObject _obj)
	{
		string MyLayer = LayerMask.LayerToName(_obj.layer);

		switch (MyLayer)
		{
			case csBattleManager.m_strFriendlyLayer:

				m_iCollisionLayerMask = (1 << LayerMask.NameToLayer(csBattleManager.CollisionLayer.Enemy.ToString()) 
					| (1 << LayerMask.NameToLayer(csBattleManager.CollisionLayer.Map.ToString()))); 

				break;

			case csBattleManager.m_strEnemyLayer:

				m_iCollisionLayerMask = (1 << LayerMask.NameToLayer(csBattleManager.CollisionLayer.Friendly.ToString()) 
					| (1 << LayerMask.NameToLayer(csBattleManager.CollisionLayer.Map.ToString())));
				
				break;
		}
	}*/

	protected virtual bool TargetInMyFOV(Transform _from, Transform _target, float _fieldOfView)
	{
		bool TargetInMyFOV = false;

		Vector3 DirToTarget = (_target.position - _from.position).normalized;

		if (Vector3.Dot(_from.forward, DirToTarget) > Mathf.Cos((_fieldOfView / 2) * Mathf.Deg2Rad))
		{
			TargetInMyFOV = true;
		}

		return TargetInMyFOV;
	}
}
