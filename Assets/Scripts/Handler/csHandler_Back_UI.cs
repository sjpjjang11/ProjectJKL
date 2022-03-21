using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csHandler_Back_UI : MonoBehaviour
{
#if UNITY_ANDROID

    private static Stack<IUIBack> m_Stack_UIBack = new Stack<IUIBack>();

    private void Awake()
    {
        //Debug.LogError("Handler Awake");
        SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Back();
        }
    }

    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        //Debug.LogError("OnSceneLoaded : " + _scene.name);
        m_Stack_UIBack.Clear();
    }

    private void ActiveSceneChanged(Scene sceneA, Scene sceneB)
    {
        //Debug.LogError("ActiveSceneChanged");
        m_Stack_UIBack.Clear();
    }

    public static void Add(IUIBack _element)
    {
        if(!m_Stack_UIBack.Contains(_element))
        {
            m_Stack_UIBack.Push(_element);

            //Debug.LogError("m_Stack_UIBack : " + m_Stack_UIBack.Count);
        }
    }

    private static void Back()
    {
        if (m_Stack_UIBack.Count == 1)
        {
            m_Stack_UIBack.Peek().Back();
        }
        else
        {
            m_Stack_UIBack.Pop().Back();
        }

        //Debug.LogError("m_Stack_UIBack : " + m_Stack_UIBack.Count);
    }

    public static void Remove()
    {
        if (m_Stack_UIBack.Count == 1)
        {
            m_Stack_UIBack.Peek();
        }
        else
        {
            m_Stack_UIBack.Pop();
        }

        //Debug.LogError("m_Stack_UIBack : " + m_Stack_UIBack.Count);
    }

#endif
}
