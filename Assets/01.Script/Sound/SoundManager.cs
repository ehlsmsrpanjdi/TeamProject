using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BgmType // BGM 타입
{
    Lobby,
    Battle,
    GameOver,
}

public enum SfxType // SFX 타입
{
    Attack,
    Hit,
    Die,
    Item,
    Gacha,
    Skill,
    UI,
    ZombieAttack,
    ZombieHit,
    ZombieDie,
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

    private Dictionary<BgmType, List<AudioClip>> bgm = new Dictionary<BgmType, List<AudioClip>>(); // BGM 오디오 클립들을 타입별로 저장하는 딕셔너리
    private Dictionary<SfxType, List<AudioClip>> sfx = new Dictionary<SfxType, List<AudioClip>>(); // SFX 오디오 클립들을 타입별로 저장하는 딕셔너리

    public float MasterVolume { get; private set; }
    public float BgmVolume { get; private set; }
    public float SfxVolume { get; private set; }

    // 볼륨 설정은 PlayerPrefs를 통해 저장
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string BGM_VOLUME_KEY = "BgmVolume";
    private const string SFX_VOLUME_KEY = "SfxVolume";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        Init(bgmSource, sfxSource);
    }

    private void Start()
    {
        LoadSounds();
        LoadVolumeSettings();
        SetMasterVolume(MasterVolume);
        SetBgmVolume(BgmVolume);
        SetSfxVolume(SfxVolume);
    }

    public void Init(AudioSource bgmSource, AudioSource sfxSource)
    {
        this.bgmSource = bgmSource;
        this.sfxSource = sfxSource;
        this.bgmSource.loop = true;
    }

    private void LoadSounds()
    {
        // Resources 폴더에서 모든 BGM 및 SFX 오디오 클립을 로드
        foreach (BgmType bgmType in System.Enum.GetValues(typeof(BgmType))) // 모든 BgmType 열거형을 순회
        {
            string path = $"Sounds/BGM/{bgmType.ToString()}"; // Bgm 클립이 저장된 경로
            AudioClip[] clips = Resources.LoadAll<AudioClip>(path).OrderBy(c => c.name).ToArray(); // 해당 경로의 모든 오디오 클립을 로드하고 이름 순으로 정렬
            if (clips.Length > 0)
            {
                bgm.Add(bgmType, new List<AudioClip>(clips)); // 딕셔너리에 추가
            }
        }

        foreach (SfxType sfxType in System.Enum.GetValues(typeof(SfxType)))
        {
            string path = $"Sounds/SFX/{sfxType.ToString()}";
            AudioClip[] clips = Resources.LoadAll<AudioClip>(path).OrderBy(c => c.name).ToArray();
            if (clips.Length > 0)
            {
                sfx.Add(sfxType, new List<AudioClip>(clips)); // Bgm과 동일하게 딕셔너리에 추가한다.
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
