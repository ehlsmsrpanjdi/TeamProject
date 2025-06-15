using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

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
        this.sfxSource.loop = false;
        LoadVolumeSettings();
        SetMasterVolume(MasterVolume);
        SetBgmVolume(BgmVolume);
        SetSfxVolume(SfxVolume);
    }

    private AudioClip GetAudioClip(string path)
    {

        if (audioClips.ContainsKey(path))
        {
            return audioClips[path];
        }

        AudioClip clip = Resources.Load<AudioClip>(path);

        if (clip != null)
        {
            audioClips.Add(path, clip);
        }
        return clip;
    }

    public void PlayBGM(string path)
    {
        AudioClip clip = GetAudioClip(path);
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(string path)
    {
        AudioClip clip = GetAudioClip(path);
        sfxSource.PlayOneShot(clip);
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
