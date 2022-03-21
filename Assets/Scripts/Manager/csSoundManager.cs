using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class csSoundManager : Singleton<csSoundManager> {

	// temporary
	public static bool PLAY_BGM = false;
	public static bool PLAY_SFX = false;
	public static bool PLAY_AMB = false;

	protected bool m_bInitialized = false;
	protected int m_iPoolCount;
	protected int m_iAmbPoolCount = 5;
	protected int m_iAmbSoundPoolCount;
	protected AudioSource m_pBGMSource;
	protected AudioSource[] m_pAMBSources;
	protected AudioSource[] m_pAudioSources;
	protected Dictionary<GameObject, List<AudioSource>> m_DicObjectAudioSource;
	protected Dictionary <string, AudioSource> m_dicLoopingAudioSource;
	protected Dictionary <string, AudioSource> m_dicAmbientAudioSource;
	[SerializeField]
	protected List<AudioSource> m_ListAudioSource;

	protected int m_iCurrentUsingSourceIndex; 
	protected int m_iCurrentUsingAmbSourceIndex;

	protected float m_fVolumeBGM = 1.0f;

	public bool initialized { get { return m_bInitialized; } }

	protected override void Awake()
	{
		base.Awake();

		m_DicObjectAudioSource = new Dictionary<GameObject, List<AudioSource>>();
		m_ListAudioSource = new List<AudioSource>();
	}

	public void Initialize (int poolCount)
	{
		m_bInitialized = true;
		m_iPoolCount = poolCount;
		m_iAmbSoundPoolCount = 5;
		{
			m_pBGMSource = this.gameObject.AddComponent <AudioSource> ();
			m_pBGMSource.loop = true;

			m_dicAmbientAudioSource = new Dictionary <string, AudioSource> ();
			m_pAMBSources = new AudioSource[m_iAmbSoundPoolCount];


			for (int i = 0; i < m_iAmbPoolCount; i++) {
				m_pAMBSources[i] = this.gameObject.AddComponent<AudioSource>();
			}

			m_pAudioSources = new AudioSource[m_iPoolCount];

			for (int i = 0; i < m_iPoolCount; i++) {
				m_pAudioSources[i] = this.gameObject.AddComponent<AudioSource>();
			}

			m_iCurrentUsingSourceIndex = 0;

			m_dicLoopingAudioSource = new Dictionary<string, AudioSource> ();
		}
	}

	public void Register(GameObject _obj)
	{
		m_DicObjectAudioSource.Add(_obj, new List<AudioSource>());
	}

	public void Unregister(GameObject _obj)
	{
		if(!m_DicObjectAudioSource.ContainsKey(_obj))
		{
			return;
		}

		List<AudioSource> ListAudioSource = m_DicObjectAudioSource[_obj];

		for (int i = 0; i < ListAudioSource.Count; i++)
		{
			Destroy(ListAudioSource[i]);
		}

		ListAudioSource.Clear();
		ListAudioSource = null;
	}

	public void Release ()
	{
		Destroy (m_pBGMSource);

		m_dicLoopingAudioSource.Clear ();

		if (m_pAudioSources != null) {
			for (int i = 0; i < m_pAudioSources.Length; i++) {
				Destroy (m_pAudioSources[i]);	
			}
		}
	}

	protected AudioSource GetSource ()
	{
		return m_pAudioSources [m_iCurrentUsingSourceIndex] as AudioSource;
	}

	protected AudioSource GetAmbSource ()
	{
		//
		return m_pAMBSources [m_iCurrentUsingAmbSourceIndex] as AudioSource;
	}


	protected void IncreaseUsingIndex ()
	{
		if (++m_iCurrentUsingSourceIndex >= m_iPoolCount)
			m_iCurrentUsingSourceIndex = 0;
	}

	protected void IncreasUsingAmbSourceIndex ()
	{
		if (++m_iCurrentUsingAmbSourceIndex >= m_iAmbPoolCount)
			m_iCurrentUsingAmbSourceIndex = 0;
	}

	public void PlayAMB (string name, bool bLoop = false, float vol = 1.0f, float speed = 1.0f)
	{
		if (PLAY_AMB == false)
			return;

		if ( m_dicAmbientAudioSource.ContainsKey (name))
		{
			AudioSource pSource = m_dicAmbientAudioSource [name];
			if ( pSource != null )
			{
				if ( pSource.isPlaying ) 
				{
					return;
				}
				else 
				{
					m_dicAmbientAudioSource.Remove (name);
				}
			}

		}


		AudioClip clip = Resources.Load(name) as AudioClip;
		AudioSource source = GetAmbSource ();


		source.clip = clip;
        source.volume = vol;
        source.pitch = speed;
        source.loop = bLoop;
		source.Play ();

		m_dicAmbientAudioSource.Add ( name, source );



		IncreasUsingAmbSourceIndex ();
	
	}

	public void StopAMB (string name)
	{
		if ( m_dicAmbientAudioSource.ContainsKey (name))
		{
			AudioSource pSource = m_dicAmbientAudioSource [name];
			if ( pSource != null )
			{
				pSource.Stop();
			}
			m_dicAmbientAudioSource.Remove (name);
		}
		
		//AudioClip clip = ResourceManager.Instance.GetResource (eResourceType.eAudioClip, name) as AudioClip;
	}


	public void PlayBGM ( string name, float vol = 1.0f)
	{
		if (PLAY_BGM == false)
			return;
		
		AudioClip clip = Resources.Load(name) as AudioClip;
		if (m_pBGMSource != null && clip != null) {
			m_pBGMSource.clip = clip;
			m_pBGMSource.loop = true;
            m_pBGMSource.volume = vol;            
            m_pBGMSource.Play ();
		}
	}

	public void PlayBGM ( AudioClip clip, float vol = 1.0f)
	{
		if (m_pBGMSource != null && clip != null) {
			m_pBGMSource.clip = clip;
			m_pBGMSource.loop = true;
            m_pBGMSource.volume = vol;
            m_pBGMSource.Play ();
		}
	}

	protected IEnumerator FadeOutStopAMB (string name)
	{
		if ( m_dicAmbientAudioSource.ContainsKey (name))
		{
			AudioSource pSource = m_dicAmbientAudioSource [name];
			if ( pSource != null )
			{
				float v = pSource.volume;
				while (v > 0.0f)
				{
					v -= 0.025f;
					if (v < 0.0f)
						v = 0.0f;
					pSource.volume = v;
					yield return YieldCache.WaitForEndOfFrame;
				}
				pSource.Stop();
			}

			m_dicAmbientAudioSource.Remove (name);
		}
	}

	public void PauseBGM ()
	{
		StopCoroutine ("CoResumeBGM");
		StartCoroutine ("CoPauseBGM");
	}

	public void ResumeBGM ()
	{
		StopCoroutine ("CoPauseBGM");
		StartCoroutine ("CoResumeBGM");
	}


	public void StopBGM ()
	{
		StopCoroutine ("CoResumeBGM");
		StopCoroutine ("CoPauseBGM");
		m_pBGMSource.Stop ();
	}


	protected IEnumerator CoPauseBGM ()
	{
		if (m_pBGMSource == null)
			yield break;
		m_fVolumeBGM = m_pBGMSource.volume;
		float v = m_fVolumeBGM;
		while (v > 0.0f) {
			v -= 0.025f;
			if (v < 0.0f)
				v = 0.0f;
			m_pBGMSource.volume = v;
			yield return YieldCache.WaitForEndOfFrame;
		}

		m_pBGMSource.Pause ();
		yield return null;
	}


	protected IEnumerator CoResumeBGM ()
	{
		if (m_pBGMSource == null)
			yield break;
		m_pBGMSource.UnPause ();

		float v = m_pBGMSource.volume;
		while (v < m_fVolumeBGM) {
			v += 0.025f;
			if (v > m_fVolumeBGM)
				v = m_fVolumeBGM;
			m_pBGMSource.volume = v;
			yield return YieldCache.WaitForEndOfFrame;
		}


		yield return null;
	}


    public void PlayEffect(string name, float speed = 1.0f, float vol = 1.0f)
	{
		PlayEffect (name, false, speed, vol);
	}

	public void PlayEffect ( AudioClip clip, float speed = 1.0f, float vol = 1.0f)
	{
		if (PLAY_SFX == false)
			return;
		if (clip == null)
			return;
		AudioSource src = GetSource ();
		src.loop = false;
		src.clip = clip;
		src.pitch = speed;
		src.Play ();

		IncreaseUsingIndex ();
	}

    public void PlayEffect(string name, bool bLoop, float speed, float volume = 1.0f)
	{
		if (PLAY_SFX == false)
			return;
		
		AudioClip clip = Resources.Load(name) as AudioClip;
		if (clip == null)
			return;

		AudioSource src = GetSource ();

		//if (src.isPlaying )
		//return;
		src.loop = bLoop;
		src.clip = clip;
		src.pitch = speed;
        src.volume = volume;
		src.Play ();

		if (bLoop) {
			m_dicLoopingAudioSource.Add (name, src);
		}

		IncreaseUsingIndex ();
	}
	
	public void PlayEffect(GameObject _obj, AudioClip _clip, float _speed = 1.0f, float _vol = 1.0f)
	{
		AudioSource Source;
		Source = GetSource(_obj, m_DicObjectAudioSource[_obj]);
		//Source = _obj.AddComponent<AudioSource>();

		Source.loop = false;
		Source.clip = _clip;
		Source.pitch = _speed;
		Source.spatialBlend = 1.0f;
		Source.rolloffMode = AudioRolloffMode.Linear;
		Source.minDistance = 0.1f;
		Source.maxDistance = 30.0f;
		Source.Play();

		/*if(!m_ListAudioSource.Contains(Source))
		{
			m_ListAudioSource.Add(Source);
		}*/
	}

	public void StopEffect(GameObject _obj, AudioClip _clip)
	{
		//Debug.Log(_clip.name);
		for(int i = 0; i < m_DicObjectAudioSource[_obj].Count; i++)
		{
			if(m_DicObjectAudioSource[_obj][i].clip == _clip)
			{
				//Debug.Log("####STOP : " + m_DicObjectAudioSource[_obj].Count + "  " + m_DicObjectAudioSource[_obj][i].clip.name + "  " + _clip.name);
				m_DicObjectAudioSource[_obj][i].Stop();
			}
		}
		/*for(int i = 0; i < m_ListAudioSource.Count; i++)
		{
			if(m_ListAudioSource[i].clip == _clip)
			{
				Debug.Log("####STOP : " + m_ListAudioSource[i].clip.name + "  " + _clip.name);
				m_ListAudioSource[i].Stop();
			}
		}*/
	}

	public AudioSource GetSource(GameObject _obj, List<AudioSource> _objectAudioSource)
	{
		AudioSource Source;

		if (_objectAudioSource.Count == 0)
		{
			Source = _obj.AddComponent<AudioSource>();
			
			m_DicObjectAudioSource[_obj].Add(Source);
		}
		else
		{
			for(int i = 0; i < _objectAudioSource.Count; i++)
			{
				if(!_objectAudioSource[i].isPlaying)
				{
					return _objectAudioSource[i];
				}
			}

			Source = _obj.AddComponent<AudioSource>();

			m_DicObjectAudioSource[_obj].Add(Source);		
		}

		if(Source.playOnAwake)
		{
			Source.playOnAwake = false;
		}

		return Source;
	}
}
