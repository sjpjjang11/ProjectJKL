using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[Serializable]
public struct ResultUIInfo
{
	public GameObject m_GameObject;
	public TextMeshProUGUI m_TextMesh;
	public Image m_Image;
	public Color m_StartColor;
	public Color m_EndColor;
	public float m_FadeInSpeed;
}

public class csResultScore : MonoBehaviour {

	private IEnumerator m_CoFadeIn = null;
	private IEnumerator m_CoMoveUp = null;

	public List<Transform> m_MoveObject = null;

	private csBattleManager m_BattleManager = null;

	private Timer m_Timer = new Timer();

	public ResultUIInfo m_Background;
	public ResultUIInfo m_IsWin;
	public ResultUIInfo m_PlayTime;
	public ResultUIInfo m_Rank;
	public ResultUIInfo m_KillCount;
	public ResultUIInfo m_Score;
	public ResultUIInfo m_Lobby;
	public ResultUIInfo m_NickName;

	public float m_FadeInSpeed = 0.0f;
	public float m_MoveSpeed = 0.0f;

	private void Awake()
	{
		m_BattleManager = csBattleManager.Instance;

		Initialize();
	}

	public void Initialize()
	{
		GameObject[] Child = new GameObject[transform.childCount];

		for(int i = 0; i < transform.childCount; i++)
		{
			Child[i] = transform.GetChild(i).gameObject;

			if (Utility.IsActive(Child[i]))
			{
				Utility.Activate(Child[i], false);
			}
		}
	}

	private void OnEnable()
	{
		OpenTheResult();
	}

	public void OpenTheResult(int _isWin = 0, float _playTime = 1527.0f, int _remainder = 2, int _total = 100, int _killCount = 12, int _score = 325837320)
	{
		if(!Utility.IsActive(gameObject))
		{
			Utility.Activate(gameObject, true);
		}

		m_IsWin.m_TextMesh.text = _isWin == 1 ? "WIN" : "LOSE";
		string[] SetTimeText = m_Timer.SetTimerText(_playTime);
		m_PlayTime.m_TextMesh.text = SetTimeText[0] + " : " + SetTimeText[1];
		m_Rank.m_TextMesh.text = _remainder + " / " + _total;
		m_KillCount.m_TextMesh.text = "+ " + _killCount;
		m_Score.m_TextMesh.text = Utility.GetThousandCommaText(_score);

		FadeInImage(m_Background, 2.0f, () =>
		{
			Utility.Activate(m_IsWin.m_GameObject, true);
			Debug.Log("%%%%%%%%%%%");
			m_Timer.OnDelay(1.0f, () =>
			{
				Debug.Log("PlayTime");
				Utility.Activate(m_PlayTime.m_GameObject, true);

				m_Timer.OnDelay(1.0f, () =>
				{
					MoveToUp();
				});
			});				
		});
	}
	
	private void FadeInImage(ResultUIInfo _resultUIInfo, float _delay = 0.0f, Action _callback = null)
	{		
		m_CoFadeIn = CoFadeIn(_resultUIInfo, (color) =>
		{
			_resultUIInfo.m_Image.color = color;			
		}, () =>
		{
			m_Timer.OnDelay(_delay, () =>
			{
				_callback?.Invoke();
			});
		});

		StartCoroutine(m_CoFadeIn);
	}

	private void FadeInText(ResultUIInfo _resultUIInfo, float _delay = 0.0f, Action _callback = null)
	{
		m_CoFadeIn = CoFadeIn(_resultUIInfo, (color) =>
		{
			_resultUIInfo.m_TextMesh.color = color;
		}, () =>
		{
			m_Timer.OnDelay(_delay, () =>
			{
				_callback?.Invoke();
			});
		});

		StartCoroutine(m_CoFadeIn);
	}

	private void MoveToUp(int _index = 0)
	{
		if(_index == m_MoveObject.Count)
		{
			m_Timer.OnDelay(1.0f, () =>
			{
				Utility.Activate(m_Lobby.m_GameObject, true);
			});

			return;
		}

		if(!Utility.IsActive(m_MoveObject[_index].gameObject))
		{
			Utility.Activate(m_MoveObject[_index].gameObject, true);
		}

		m_CoMoveUp = CoMoveToUp(_index, m_MoveObject[_index], (index) =>
		{
			MoveToUp(index);
		});

		StartCoroutine(m_CoMoveUp);
	}

	public void OnClickGoToLobby()
	{
		SceneManager.LoadScene(csProjectManager.Instance.SceneType_Lobby_InUse.ToString());
	}

	private IEnumerator CoFadeIn(ResultUIInfo _resultUIInfo, Action<Color> _update, Action _complete)
	{
		Color ColorToApply;
		Color StartColor = _resultUIInfo.m_StartColor;
		Color EndColor = _resultUIInfo.m_EndColor;

		ColorToApply = StartColor;

		if (!Utility.IsActive(_resultUIInfo.m_GameObject))
		{
			Utility.Activate(_resultUIInfo.m_GameObject, true);
		}

		while (ColorToApply.a < EndColor.a)
		{
			yield return null;

			ColorToApply.a += _resultUIInfo.m_FadeInSpeed * Time.deltaTime;
			ColorToApply.a = Mathf.Clamp(ColorToApply.a, ColorToApply.a, EndColor.a);

			_update?.Invoke(ColorToApply);
		}

		_complete?.Invoke();
	}

	private IEnumerator CoMoveToUp(int _index, Transform _tr, Action<int> _callback = null)
	{
		float OriginY = _tr.localPosition.y;

		Vector2 Pos = _tr.localPosition;
		Pos.y -= 100.0f;
		_tr.localPosition = Pos;

		while (_tr.localPosition.y < OriginY)
		{
			yield return null;

			Pos.y += m_MoveSpeed * Time.deltaTime;
			Pos.y = Mathf.Clamp(Pos.y, Pos.y, OriginY);
			_tr.localPosition = Pos;
		}

		_index++;

		_callback?.Invoke(_index);
	}
}
