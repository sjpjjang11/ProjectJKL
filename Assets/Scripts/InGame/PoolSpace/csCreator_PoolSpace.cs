using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csCreator_PoolSpace : MonoBehaviour
{
    private static Dictionary<string, csPoolSpace> m_Dictionary_Path_PoolSpace = new Dictionary<string, csPoolSpace>();

    public static void CreatePool(string _path, Scene _scene, out csPoolSpace _poolSpace)
    {
        CreatePool(_path, _scene);

        _poolSpace = GetPoolSpace(_path);
    }

    public static void CreatePool(string _path, Scene _scene)
    {     
        // 해당 경로의 풀이 이미 존재하는지
        if (CheckContains(_path))
        {
            return;
        }

        csPoolSpace PoolSpace;

        // '/'가 없으면 단일 오브젝트
        if (!_path.Contains('/'))
        {
            PoolSpace = CreatePoolSpaceObject(_path);
            PoolSpace.Settings();

            MoveToScene(PoolSpace.gameObject, SceneManager.GetSceneByName(csProjectManager.Instance.SceneType_InGame_InUse.ToString()));

            //PoolSpace.Transform.SetAsFirstSibling();

            m_Dictionary_Path_PoolSpace.Add(_path, PoolSpace);

            return;
        }

        // '/'를 기준으로 분리
        string[] Array_Path = _path.Split('/');

        // 현재 생성하려는 오브젝트를 포함한 상위 경로를 저장
        // 이 값을 Dictionary에 키값으로 저장하여 해당 오브젝트를 색인할 때 사용
        // 예) 최종적으로 OwnerPool/Hero/KATRINA를 생성하려고 할 때, 현재 Hero오브젝트를 생성 중이라면
        // OwnerPool/Hero 값이 저장되고 후에 OwnerPool 하위에 있는 Hero 오브젝트를 색인 할 때 OwnerPool/Hero 값 사용
        System.Text.StringBuilder Path_Total_ToCurrent = new System.Text.StringBuilder();

        for (int i = 0; i < Array_Path.Length; i++)
        {
            // 경로의 마지막이 '/'로 끝났을 때 예외처리
            // 예) OwnerPool/Hero/
            // 정상적인 호출은 OwnerPool/Hero 형태로 마지막에 '/'가 없어야 됨
            if (Array_Path[i].Length == 0)
            {
                break;
            }

            // 현재 생성할 오브젝트의 이름
            string Name_ToCreate = Array_Path[i];

            // 부모 이름과 현재 생성하려는 오브젝트의 이름 사이에 '/' 추가
            // Root 오브젝트일 경우 무시
            if (i != 0 && i + 1 <= Array_Path.Length)
            {
                Path_Total_ToCurrent.Append('/');
            }

            Path_Total_ToCurrent.Append(Name_ToCreate);

            // 현재 생성하려는 경로 중복 검사
            if(CheckContains(Path_Total_ToCurrent.ToString()))
            {
                continue;
            }

            // Root 오브젝트일 때
            if(i == 0)
            {
                PoolSpace = CreatePoolSpaceObject(Name_ToCreate);
                PoolSpace.Settings();

                MoveToScene(PoolSpace.gameObject, _scene);

                PoolSpace.Transform.SetAsFirstSibling();

                m_Dictionary_Path_PoolSpace.Add(Name_ToCreate, PoolSpace);

                continue;
            }

            PoolSpace = CreatePoolSpaceObject(Name_ToCreate);
            PoolSpace.Settings();

            // 부모 오브젝트를 찾아 하위에 넣어줌
            Transform Transform_ParentPool = GetPoolSpace(GetParentPath(Path_Total_ToCurrent.ToString(), Name_ToCreate)).Transform;
            PoolSpace.Transform.SetParent(Transform_ParentPool);

            m_Dictionary_Path_PoolSpace.Add(Path_Total_ToCurrent.ToString(), PoolSpace);         
        }
    }

    public static csPoolSpace GetPoolSpace(string _path)
    {
        if (CheckContains(_path))
        {
            return m_Dictionary_Path_PoolSpace[_path];
        }

        return null;
    }

    public static bool CheckContains(string _path)
    {
        if (m_Dictionary_Path_PoolSpace.ContainsKey(_path))
        {
            return true;
        }

        return false;
    }
  
    private static csPoolSpace CreatePoolSpaceObject(string _name)
    {
        return new GameObject(_name).AddComponent<csPoolSpace>();
    }

    private static string GetParentPath(string _path_Parent, string _name_Child)
    {
        return _path_Parent.ToString().Substring(0, _path_Parent.ToString().Length - (_name_Child.Length + 1));
    }

    private static void MoveToScene(GameObject _gameObject, Scene _scene)
    {
        SceneManager.MoveGameObjectToScene(_gameObject, _scene);
    }

    public static void Test()
    {
        foreach (KeyValuePair<string, csPoolSpace> p in m_Dictionary_Path_PoolSpace)
        {
            Debug.Log("Created : " + p.Key + "   " + p.Value);
        }
    }
}
