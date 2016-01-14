using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    //AudioSources
    [SerializeField]
    private AudioSource _mainSource;
    [SerializeField]
    private AudioSource _mainMusic;

    // FX
    [SerializeField]
    private AudioSource _FXBlock;
    [SerializeField]
    private AudioSource _chargeBlock;

    //Audio Clips
    [SerializeField]
    private AudioClip _punchClip;
    [SerializeField]
    private AudioClip _MegapunchClip;
    [SerializeField]
    private AudioClip _successClip;
    [SerializeField]
    private AudioClip _DefeatBGClip;
    [SerializeField]
    private AudioClip _VictoryBGClip;
    [SerializeField]
    private AudioClip _FallingClip;


    private int Cap = 3;
    private int _timesPlayed;
    private float _coolDown = 1.0f;
    private static AudioController _instance = null;

    private bool _isEffectMuted, _isMusicMuted;

    private Dictionary<AudioSource, float> _sourceInitialVolumes;
    private List<AudioSource> _effectList, _musicList;

    public bool IsEffectMuted
    {
        get { return _isEffectMuted; }
    }

    public bool IsMusicMuted
    {
        get { return _isMusicMuted; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        _instance = this;

        _sourceInitialVolumes = new Dictionary<AudioSource, float>();
        AddIfNotNull(_sourceInitialVolumes, _mainMusic);

        AddIfNotNull(_sourceInitialVolumes, _FXBlock);
        AddIfNotNull(_sourceInitialVolumes, _chargeBlock);

        _effectList = new List<AudioSource>();
        AddIfNotNull(_effectList, _FXBlock);
        AddIfNotNull(_effectList, _chargeBlock);

        _musicList = new List<AudioSource>();
        AddIfNotNull(_musicList, _mainMusic);

       
    }

    private void AddIfNotNull(List<AudioSource> list, AudioSource newSource)
    {
        if (newSource != null)
        {
            list.Add(newSource);
        }
    }

    private void AddIfNotNull(Dictionary<AudioSource, float> dictionary, AudioSource newSource)
    {
        if (newSource != null)
        {
            dictionary.Add(newSource, newSource.volume);
        }
    }

    public static AudioController Instance
    {
        get
        {
            return _instance;
        }
    }

    public void PlayClipOneShot(AudioClip clip)
    {
        _mainSource.PlayOneShot(clip);
    }

    public void PlayBGMusic()
    {
        if (_mainMusic != null)
        {
            _mainMusic.Play();
        }
    }

    public void StopBGMusic()
    {
        if (_mainMusic != null)
        {
            _mainMusic.Stop();
        }
    }

    public void PlayVictoryOneShot()
    {
        if (_mainSource != null && _VictoryBGClip != null)
        {
            _mainSource.PlayOneShot(_VictoryBGClip);
        }

    }

    public void PlayDefeatOneShot()
    {
        if (_mainSource != null && _DefeatBGClip != null)
        {
            _mainSource.PlayOneShot(_DefeatBGClip);
        }

    }

	public void PlayPunchSound()
	{
		if (_FXBlock != null && _punchClip != null)
		{
			_FXBlock.PlayOneShot(_punchClip);
		}
	}

    public void PlayMegaPunchSound()
    {
        if (_FXBlock != null && _MegapunchClip != null)
        {
            _FXBlock.PlayOneShot(_MegapunchClip);
        }
    }

    public void PlayFallingSound()
    {
        if (_FXBlock != null && _FallingClip != null)
        {
            if (_timesPlayed < Cap)
            {
                _FXBlock.PlayOneShot(_FallingClip);
                _timesPlayed++;
            }
        }
    }

    public void PlayChargeSound()
    {
        if (_chargeBlock != null && !_chargeBlock.isPlaying)
        {
			_chargeBlock.loop = true;
            _chargeBlock.Play();
        }
    }

    public void StopChargeSound()
    {
        if (_chargeBlock != null)
        {
            _chargeBlock.Stop();
        }
    }

    public void SetSFXMuting(bool isMuted)
    {
        if (!isMuted)
        {
            // Unmute
            foreach (AudioSource effectSource in _effectList)
            {
                if (effectSource != null) effectSource.volume = _sourceInitialVolumes[effectSource];
            }
        }
        else
        {
            foreach (AudioSource effectSource in _effectList)
            {
                if (effectSource != null) effectSource.volume = 0;
            }
        }
        _isEffectMuted = isMuted;
    }

    public void SetMusicMuting(bool isMuted)
    {
        if (!isMuted)
        {
            // Unmute
            foreach (AudioSource musicSource in _musicList)
            {
                if (musicSource != null) musicSource.volume = _sourceInitialVolumes[musicSource];
            }
        }
        else
        {
            foreach (AudioSource musicSource in _musicList)
            {
                if (musicSource != null) musicSource.volume = 0;
            }
        }
        _isMusicMuted = isMuted;
    }

    public void PlaySuccessSound()
    {
		if (_FXBlock != null && _successClip != null)
        {
            _FXBlock.PlayOneShot(_successClip);
        }
    }

    void Update()
    {
        if (_timesPlayed >= Cap)
        {
            _coolDown -= Time.deltaTime;
            if (_coolDown <= 0)
            {
                _timesPlayed = 0;
                _coolDown = 1;
            }

        }

    }



}