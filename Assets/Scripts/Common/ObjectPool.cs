using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool
{
	public List<GameObject> m_ListObject = new List<GameObject>();

	protected GameObject m_PoolObject;

	protected Transform m_ParentTr;

	protected int m_PoolCount;

	public virtual void CreatePool(GameObject _obj, Transform _parentTr, int _poolCount, bool _active = false)
	{
		m_PoolObject = _obj;

		m_ParentTr = _parentTr;

		m_PoolCount = _poolCount;

		for (int i = 0; i < _poolCount; i++)
		{
			GameObject Obj = Object.Instantiate(_obj as GameObject, _parentTr);
			Obj.SetActive(_active);

			m_ListObject.Add(Obj);
		}
	}

	public virtual void CreatePool(string _name, GameObject _obj, Transform _parentTr, int _poolCount, bool _active = false)
	{
		m_PoolObject = _obj;

		m_ParentTr = _parentTr;

		m_PoolCount = _poolCount;

		for (int i = 0; i < _poolCount; i++)
		{
			GameObject Obj = Object.Instantiate(_obj as GameObject, _parentTr);
			Obj.name = Obj.name + "_" + _name;
			Obj.SetActive(_active);

			m_ListObject.Add(Obj);
		}
	}

	public virtual int UseObject(Vector3 _position, bool _isLocal)
	{
		for (int i = 0; i < m_ListObject.Count; i++)
		{
			if (!m_ListObject[i].activeSelf)
			{
				if (_isLocal)
				{
					m_ListObject[i].transform.localPosition = _position;
				}
				else
				{
					m_ListObject[i].transform.position = _position;
				}

				m_ListObject[i].SetActive(true);

				return i;
			}
		}

		int Index = 0;

		for (int i = 0; i < m_PoolCount; i++)
		{
			Index = m_ListObject.Count;
			GameObject Obj = Object.Instantiate(m_PoolObject as GameObject, m_ParentTr);
			Obj.name = Obj.name + "_" + (Index) + "_emg";

			m_ListObject.Add(Obj);
		}

		Index = m_ListObject.Count - m_PoolCount;

		if (_isLocal)
		{
			m_ListObject[Index].transform.localPosition = _position;
		}
		else
		{
			m_ListObject[Index].transform.position = _position;
		}

		m_ListObject[Index].SetActive(true);

		return Index;
	}

	public virtual void AllClearPool()
	{
		for (int i = 0; i < m_ListObject.Count; i++)
		{
			Object.Destroy(m_ListObject[i]);
		}

		m_ListObject.Clear();

		m_ListObject = null;
	}
}

public class ObjectPool<V> : ObjectPool
{	
    public List<V> m_ListComponent = new List<V>();

	public override void CreatePool(GameObject _obj, Transform _parentTr, int _poolCount, bool _active = false)
	{
		m_PoolObject = _obj;

		m_ParentTr = _parentTr;

		m_PoolCount = _poolCount;

		for (int i = 0; i < _poolCount; i++)
		{
			GameObject Obj = Object.Instantiate(_obj as GameObject, _parentTr);

			Obj.name = Obj.name + "_" + i;
			Obj.SetActive(_active);

			m_ListObject.Add(Obj);

			V Component = Obj.GetComponent<V>();

			m_ListComponent.Add(Component);
		}
	}

	public override void CreatePool(string _name, GameObject _obj, Transform _parentTr, int _poolCount, bool _active = false)
	{
		m_PoolObject = _obj;

		m_ParentTr = _parentTr;

		m_PoolCount = _poolCount;

		for (int i = 0; i < _poolCount; i++)
		{
			GameObject Obj = Object.Instantiate(_obj as GameObject, _parentTr);

			Obj.name = Obj.name + "_" + _name;
			Obj.SetActive(_active);

			m_ListObject.Add(Obj);

			V Component = Obj.GetComponent<V>();

			m_ListComponent.Add(Component);			
		}
	}

	public override int UseObject(Vector3 _position, bool _isLocal)
	{
		for (int i = 0; i < m_ListObject.Count; i++)
		{
			if (!m_ListObject[i].activeSelf)
			{
                if(_isLocal)
                {
					m_ListObject[i].transform.localPosition = _position;
                }
                else
                {
					m_ListObject[i].transform.position = _position;
                }

				m_ListObject[i].SetActive(true);

				return i;
			}
		}

		int Index = m_ListObject.Count;
		GameObject Obj = Object.Instantiate(m_PoolObject as GameObject, m_ParentTr);
		Obj.name = Obj.name + "_" + (Index) + "_emg";

		m_ListObject.Add(Obj);
		m_ListComponent.Add(Obj.GetComponent<V>());

        if(_isLocal)
        {
			m_ListObject[Index].transform.localPosition = _position;
        }
        else
        {
			m_ListObject[Index].transform.position = _position;
        }

		m_ListObject[Index].SetActive(true);

		return Index;
	}

	public override void AllClearPool()
    {
        for (int i = 0; i < m_ListObject.Count; i++)
        {
			Object.Destroy(m_ListObject[i]);
        }

		m_ListObject.Clear();
		m_ListComponent.Clear();

		m_ListObject = null;
		m_ListComponent = null;
	}
}

