using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BgmType
{
    Lobby,
    Battle,
    GameOver,
}

public enum SfxType
{
    Attack,
    Hit,
    Die,
    Item,
    Gacha,
    Skill
    // 필요한 효과음 타입 추가
}


public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("SoundManager");
                    instance = obj.AddComponent<SoundManager>();
                    DontDestroyOnLoad(instance.gameObject);
                }
                else
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private Dictionary<BgmType, List<AudioClip>> bgm = new Dictionary<BgmType, List<AudioClip>>();
    private Dictionary<SfxType, List<AudioClip>> sfx = new Dictionary<SfxType, List<AudioClip>>();

    public float MasterVolume { get; private set; }
    public float BgmVolume { get; private set; }
    public float SfxVolume { get; private set; }

    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string BGM_VOLUME_KEY = "BgmVolume";
    private const string SFX_VOLUME_KEY = "SfxVolume";



    private void Awake()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        Init(bgmSource, sfxSource);
    }

    public void Init(AudioSource bgmSource, AudioSource sfxSource)
    {
        this.bgmSource = bgmSource;
        this.sfxSource = sfxSource;
        this.bgmSource.loop = true;

        LoadSounds();
        LoadVolumeSettings();
        SetMasterVolume(MasterVolume);
        SetBgmVolume(BgmVolume);
        SetSfxVolume(SfxVolume);
    }

    private void LoadSounds()
    {
        foreach (BgmType bgmType in System.Enum.GetValues(typeof(BgmType)))
        {
            string path = $"Sounds/BGM/{bgmType.ToString()}";
            AudioClip[] clips = Resources.LoadAll<AudioClip>(path).OrderBy(c => c.name).ToArray();
            if (clips.Length > 0)
            {
                bgm.Add(bgmType, new List<AudioClip>(clips));
            }
        }

        foreach (SfxType sfxType in System.Enum.GetValues(typeof(SfxType)))
        {
            string path = $"Sounds/SFX/{sfxType.ToString()}";
            AudioClip[] clips = Resources.LoadAll<AudioClip>(path).OrderBy(c => c.name).ToArray();
            if (clips.Length > 0)
            {
                sfx.Add(sfxType, new List<AudioClip>(clips));
            }
        }
    }

    public void PlayBGM(BgmType bgmType, int index)
    {
        if (!bgm.ContainsKey(bgmType)) return;
        List<AudioClip> clips = bgm[bgmType];
        if (clips.Count == 0) return;

        AudioClip clip = (index < 0) ? clips[Random.Range(0, clips.Count)] : clips[index];

        bgmSource.clip = clip;
        bgmSource.volume = BgmVolume;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(SfxType sfxType, int index)
    {
        if (!sfx.ContainsKey(sfxType)) return;
        List<AudioClip> clips = sfx[sfxType];
        if (clips.Count == 0) return;

        AudioClip clip = (index < 0) ? clips[Random.Range(0, clips.Count)] : clips[index];

        sfxSource.clip = clip;
        sfxSource.PlayOneShot(clip, SfxVolume);
    }

    public void SetMasterVolume(float volume)
    {
        MasterVolume = Mathf.Clamp01(volume);
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, MasterVolume);
    }

    public void SetBgmVolume(float volume)
    {
        BgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BgmVolume);
    }

    public void SetSfxVolume(float volume)
    {
        SfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, SfxVolume);
    }

    private void LoadVolumeSettings()
    {
        MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1.0f);
        BgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1.0f);
        SfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1.0f);
    }
}
