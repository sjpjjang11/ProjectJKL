using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csWaitingRoomUI : MonoBehaviour 
{
	protected int m_CharListCount = 0;

	public List<csWaitingIconPrefab> m_WaitingRoomList;
	protected Dictionary<int, csWaitingIconPrefab> m_dicWaitingRoomUser;

	public void OpenWaitingRoomUI()
	{
		if (!Utility.IsActive(gameObject))
		{
			Utility.Activate(gameObject, true);
		}
	}

	public void CloseWaitingRoomUI()
	{
		if (Utility.IsActive(gameObject))
		{
			Utility.Activate(gameObject, false);
		}
	}

	public void SetCharList(csCHARACTERINFO[] _charList, int _count)
	{
		Debug.LogError("SetCharList : " + _count);
		for(int i = 0; i < m_WaitingRoomList.Count; i++)
		{
			if(i < _count)
			{
				m_WaitingRoomList[i].gameObject.SetActive(true);
				m_WaitingRoomList[i].m_UserIndex = _charList[i].m_dwUserindex;
				m_WaitingRoomList[i].SetCharInfo(_charList[i].m_dwCharacterindex, _charList[i].m_strUsername);
				m_CharListCount++;
			}
			else
			{
				m_WaitingRoomList[i].gameObject.SetActive(false);
			}
		}
	}

	public void AddCharList(csCHARACTERINFO _charInfo, int _count)
	{
		Debug.LogError("AddCharList : " + _count);
		for (int i = 0; i < m_WaitingRoomList.Count; i++)
		{
			if(m_WaitingRoomList[i].m_UserIndex.Equals(-1))
			{
				m_WaitingRoomList[i].gameObject.SetActive(true);
				m_WaitingRoomList[i].m_UserIndex = _charInfo.m_dwUserindex;
				m_WaitingRoomList[i].SetCharInfo(_charInfo.m_dwCharacterindex, _charInfo.m_strUsername);
				m_CharListCount++;
				break;
			}
		}
	}

	public void DeleteCharList(int _charIndex)
	{
		for(int i = 0; i < m_WaitingRoomList.Count; i++)
		{
			if(m_WaitingRoomList[i].m_UserIndex.Equals(_charIndex))
	{
				m_WaitingRoomList[i].gameObject.SetActive(false);
				m_WaitingRoomList[i].m_UserIndex = -1;
				m_CharListCount--;
				break;
			}
		}

	}

	public void ChangeCharacter(csCHARACTERINFO _charInfo)
	{
		for(int i = 0; i < m_WaitingRoomList.Count; i++)
		{
			if(m_WaitingRoomList[i].m_UserIndex.Equals(_charInfo.m_dwUserindex))
			{
				m_WaitingRoomList[i].SetCharInfo(_charInfo.m_dwCharacterindex, _charInfo.m_strUsername);
				break;
			}
		}
	}
}
