using System;
using System.Linq;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

/*2018.06.20
* hkh
* 에디터 상에서 마우스로 터치 사용 테스트를 위한 구조체.
*/
public struct UserTouch
{
    public int m_iFingerID;         // Touch.fingerId에 해당. 마우스 사용이기 때문에 사실상 0만 사용
    public Vector2 m_Position;      // Touch.position에 해당
    public Vector2 m_DeltaPosition; // Touch.deltaPosition에 해당
}

// 터치 이벤트 식별을 위한 열거형
public enum TouchEvent
{
	TouchBegan,       // 터치 시작
	TouchBegan_Additional,
	TouchMoved,       // 터치 드래그
	TouchMoved_Additional,
	TouchStationary,  // 터치 스테이
	TouchStationary_Additional,
	TouchEnded,        // 터치 종료     
	TouchEnded_Additional
}

/*2018.06.09
* hkh
* Input 기능을 하는 클래스들의 최상위 클래스. 
* 버튼 및 터치 이벤트 등록.  
*/
public class csInput : MonoBehaviour {

    private delegate void TouchEventHandler(UserTouch _touch);

    // 터치 이벤트 종류와 그에 해당하는 메소드를 매칭
    private static Dictionary<TouchEvent, TouchEventHandler> m_DicTouchEventBindings =
        new Dictionary<TouchEvent, TouchEventHandler>(new TouchEventCompare())
        {
            { TouchEvent.TouchBegan, null },
            { TouchEvent.TouchMoved, null },
            { TouchEvent.TouchStationary, null },
            { TouchEvent.TouchEnded, null }
        };

	protected Vector3 m_Axis = Vector3.zero;

	protected UserTouch m_Touch;

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

	protected virtual void Awake()
	{
		
	}

	protected virtual void Start()
	{

	}

	protected virtual void Update()
    {
		if(BattleManager.m_GameState == csBattleManager.GameState.Waiting)
		{
			//return;
		}

        if(BattleManager.EventHandler_Game.Pause.Active)
        {
            return;
        }

        if(BattleManager.EventHandler_Game.GameOver.Active)
        {
            return;
        }

		// 각 플랫폼별로 메소드 사용유무 구별
#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)

        InputKeyMove();

        InputTouch_Mouse();

#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX

		InputKeyMove();

		InputTouch_Mouse();

#elif UNITY_ANDROID || UNITY_IOS

        InputTouch();

#endif
	}

    protected virtual void InputKeyMove()
    {
        if(EventHandler_Hero == null)
		{
			return;
		}
	
		// 방향키 입력이 이루어지면
		if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
		{
			// Vector3 형식으로 저장
			m_Axis = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

			EventHandler_Hero.IsKeyMove.Set(true);
		}
		else
		{
			m_Axis = Vector3.zero;
			EventHandler_Hero.IsKeyMove.Set(false);
		}

		// 플레이어 이벤트 변수에 저장된 Vector3 값을 전달
		EventHandler_Hero.MoveDirection.Set(m_Axis);
    }

    /*2018.06.29
    * hkh
    * 에디터상에서 마우스로 터치 기능을 위해 구현
    */
    protected virtual void InputTouch_Mouse()
    {
        // 마우스 버튼 누르기 시작했을 때 || 마우스 버튼 누르고 있을 때 || 마우스 버튼을 땠을 때
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
			// 마우스 터치 데이터 생성
			/*UserTouch Touch = new UserTouch
			{
				// 마우스 클릭으로 이루어지기 때문에 입력되는 값의 개수는 한 개 이므로 항상 0
				m_iFingerID = 0,
				// 마우스의 현재 위치
				m_Position = Input.mousePosition,
				// 전 프레임과 현재 프레임의 마우스 위치 차이
				m_DeltaPosition = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"))
			};*/

			m_Touch.m_iFingerID = 0;
			m_Touch.m_Position = Input.mousePosition;
			m_Touch.m_DeltaPosition = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

			EventHandler_Hero.Touch.Set(m_Touch);

			// 마우스 버튼 클릭 시작 && TouchEvent.TouchBegan에 해당하는 메소드가 존재할 때
			if (Input.GetMouseButtonDown(0) && m_DicTouchEventBindings[TouchEvent.TouchBegan] != null)
            {
				// 등록된 이벤트 실행(터치 시작)
				m_DicTouchEventBindings[TouchEvent.TouchBegan](m_Touch);
            }

            // 전 프레임이랑 현재 프레임의 위치 차이가 있을 때 && 마우스 버튼 누르고 있을 때 && TouchEvent.TouchMoved에 해당하는 메소드가 존재할 때
            if (m_Touch.m_DeltaPosition != Vector2.zero && Input.GetMouseButton(0) && m_DicTouchEventBindings[TouchEvent.TouchMoved] != null)
            {
				// 등록된 이벤트 실행(터치 드래그)
				m_DicTouchEventBindings[TouchEvent.TouchMoved](m_Touch);
            }

            // 전 프레임이랑 현재 프레임의 위치 차이가 없을 때 && 마우스 버튼 누르고 있을 때 && TouchEvent.TouchStationary에 해당하는 메소드가 존재할 때
            if (m_Touch.m_DeltaPosition == Vector2.zero && Input.GetMouseButton(0) && m_DicTouchEventBindings[TouchEvent.TouchStationary] != null)
            {
				// 등록된 이벤트 실행(터치 스테이)
				m_DicTouchEventBindings[TouchEvent.TouchStationary](m_Touch);
            }

            // 마우스 버튼 땠을 때 && TouchEvent.TouchEnded에 해당하는 메소드가 존재할 때
            if (Input.GetMouseButtonUp(0) && m_DicTouchEventBindings[TouchEvent.TouchEnded] != null)
            {
				// 등록된 이벤트 실행(터치 종료)
				m_DicTouchEventBindings[TouchEvent.TouchEnded](m_Touch);
            }
        }
    }

    /*2018.06.29
    * hkh
    * 모바일에서 터치 기능 구현
    */
    protected virtual void InputTouch()
    {
        // 한 개 이상의 터치가 이루어질 때
        for (int i = 0; i < Input.touches.Length; i++)
        {
            // 해당 터치값 저장
           /* Touch Touch = Input.touches[i];

            // 사실 모바일 터치 구현 중에는 UserTouch 구조체를 사용할 필요가 없는데 굳이 사용하는 이유는
            // 에디터에서든 모바일에서든 동일한 이벤트를 사용하기 위함임(일반화 개념?)
            UserTouch UserTouch = new UserTouch
            {
                // 해당 터치의 ID
                m_iFingerID = Touch.fingerId,
                // 터치의 현재 위치
                m_Position = Touch.position,
                // 전 프레임과 현재 프레임의 터치 위치 차이
                m_DeltaPosition = Touch.deltaPosition
            };*/

			Touch Touch = Input.touches[i];

			m_Touch.m_iFingerID = Touch.fingerId;
			m_Touch.m_Position = Touch.position;
			m_Touch.m_DeltaPosition = Touch.deltaPosition;

			EventHandler_Hero.Touch.Set(m_Touch);

			// 터치 시작 && TouchEvent.TouchBegan에 해당하는 메소드가 존재할 때 
			if (Touch.phase == TouchPhase.Began && m_DicTouchEventBindings[TouchEvent.TouchBegan] != null)
            {
                // 등록된 이벤트 실행(터치 시작)
                m_DicTouchEventBindings[TouchEvent.TouchBegan](m_Touch);
            }

            // 터치 드래그 && TouchEvent.TouchMoved에 해당하는 메소드가 존재할 때 
            if (Touch.phase == TouchPhase.Moved && m_DicTouchEventBindings[TouchEvent.TouchMoved] != null)
            {
                // 등록된 이벤트 실행(터치 드래그)
                m_DicTouchEventBindings[TouchEvent.TouchMoved](m_Touch);
            }

            // 터치 스테이 && TouchEvent.TouchStationary에 해당하는 메소드가 존재할 때 
            if (Touch.phase == TouchPhase.Stationary && m_DicTouchEventBindings[TouchEvent.TouchStationary] != null)
            {
                // 등록된 이벤트 실행(터치 스테이)
                m_DicTouchEventBindings[TouchEvent.TouchStationary](m_Touch);
            }

            // 터치 종료 && TouchEvent.TouchEnded에 해당하는 메소드가 존재할 때 
            if (Touch.phase == TouchPhase.Ended && m_DicTouchEventBindings[TouchEvent.TouchEnded] != null)
            {
                // 등록된 이벤트 실행(터치 종료)
                m_DicTouchEventBindings[TouchEvent.TouchEnded](m_Touch);
            }          
        }
    }

    /*2018.06.29
    * hkh
    * m_DicTouchEventBindings Key에 해당하는 메소드 등록
    */
    protected void Register()
    {
        // 메소드 얻어옴
        MethodInfo[] MethodInfos = GetType().GetMethods
                (
                    BindingFlags.NonPublic |
                    BindingFlags.Instance
                );

        // 얻어온 메소드에서 식별 작업을 통해 해당하는 메소드일 경우 저장
        for (int i = 0; i < MethodInfos.Length; i++)
        {
            // 얻어온 메소드를 열거형 TouchEvent로 캐스팅하기 위한 변수
            TouchEvent CastToTouchEvent;

            // 예외처리. 얻어온 모든 메소드가 열거형 TouchEvent에 해당한다는 보장이 없기 때문
            try
            {
                // 메소드가 열거형 TouchEvent에 해당하는 메소드이면 TouchEvent로 캐스팅하고 진행
                CastToTouchEvent = (TouchEvent)Enum.Parse(typeof(TouchEvent), MethodInfos[i].Name);
            }
            catch
            {
                // 메소드가 열거형 TouchEvent에 해당하지 않으면 캐스팅 실패. 진행하지 않고 건너뛴다
                continue;
            }

            //Debug.Log(MethodInfos[i].Name);

            // TouchEventHandler 타입과 메소드 이름으로 델리게이트 생성 
            TouchEventHandler GetDelegate = (TouchEventHandler)Delegate.CreateDelegate(typeof(TouchEventHandler), this, MethodInfos[i].Name);

            // Key에 해당하는 Velue에 델리게이트 연결
            m_DicTouchEventBindings[CastToTouchEvent] += GetDelegate;
        }
    }
}

