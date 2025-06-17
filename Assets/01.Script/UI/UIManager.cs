using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("UIManager");
                    instance = obj.AddComponent<UIManager>();
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

    //[SerializeField] UIBase[] MainUI;
    //[SerializeField] UIBase[] BattleUI;

    [SerializeField] Canvas mainCanvas;
    const string MainCanvasName = "MainCanvas";
    [SerializeField] Canvas battleCanvas;
    const string BattleCanvasName = "BattleCanvas";

    const string BattleMemberViewer = "UI/Canvas/BattleMemberViewer";
    const string HireAgent = "UI/Canvas/HireAgent";
    const string HireScroll = "UI/Canvas/HireScroll";
    const string Management = "UI/Canvas/Management";
    const string QuestScroll = "UI/Canvas/QuestScroll";
    const string SkillViewer = "UI/Canvas/SkillViewer";
    const string SoundOption = "UI/Canvas/SoundOption";
    const string Stage_Viewer = "UI/Canvas/Stage_Viewer";
    const string Status = "UI/Canvas/Status";
    const string UIGacha = "UI/Canvas/UIGacha";
    const string UILobby = "UI/Canvas/UILobby";
    const string UIOption = "UI/Canvas/UIOption";
    const string UIPopup = "UI/Canvas/UIPopup";
    const string UIDoor = "UI/Canvas/UIDoor";
    const string BattleCanvas = "UI/Canvas/BattleCanvas";
    const string MainCanvas = "UI/Canvas/MainCanvas";

    //[SerializeField] GameObject BattleMemberViewerPrefab;
    //[SerializeField] GameObject HireAgentPrefab;
    //[SerializeField] GameObject HireScrollPrefab;
    //[SerializeField] GameObject ManagementPrefab;
    //[SerializeField] GameObject QuestScrollPrefab;
    //[SerializeField] GameObject SkillViewerPrefab;
    //[SerializeField] GameObject SoundOptionPrefab;
    //[SerializeField] GameObject Stage_ViewerPrefab;
    //[SerializeField] GameObject StatusPrefab;
    //[SerializeField] GameObject UIGachaPrefab;
    //[SerializeField] GameObject UILobbyPrefab;
    //[SerializeField] GameObject UIOptionPrefab;

    Dictionary<Type, UIBase> UIDictionary = new Dictionary<Type, UIBase>();
    //Dictionary<Type, GameObject> PrefabDictionary = new Dictionary<Type, GameObject>();
    Dictionary<Type, string> ResourcesPathDictionary = new Dictionary<Type, string>();
    Dictionary<GameObject, float> UIReleaser = new Dictionary<GameObject, float>();

    private void Reset()
    {
        //mainCanvas = GameObject.Find(MainCanvasName)?.GetComponent<Canvas>();
        //MainUI = mainCanvas.GetComponentsInChildren<UIBase>(true);

        //battleCanvas = GameObject.Find(BattleCanvasName)?.GetComponent<Canvas>();
        //BattleUI = battleCanvas.GetComponentsInChildren<UIBase>(true);
    }

    void DictionaryAddFunction()
    {
        ResourcesPathDictionary.Add(typeof(UIBattleMemberViewer), BattleMemberViewer);
        ResourcesPathDictionary.Add(typeof(UIAgentHire), HireAgent);
        ResourcesPathDictionary.Add(typeof(UIHireScroll), HireScroll);
        ResourcesPathDictionary.Add(typeof(UIManagement), Management);
        ResourcesPathDictionary.Add(typeof(UIQuestScroll), QuestScroll);
        ResourcesPathDictionary.Add(typeof(UISkillViewer), SkillViewer);
        ResourcesPathDictionary.Add(typeof(UISoundOption), SoundOption);
        ResourcesPathDictionary.Add(typeof(UIStage), Stage_Viewer);
        ResourcesPathDictionary.Add(typeof(UIStatus), Status);
        ResourcesPathDictionary.Add(typeof(UIGacha), UIGacha);
        ResourcesPathDictionary.Add(typeof(UILobby), UILobby);
        ResourcesPathDictionary.Add(typeof(UIOption), UIOption);
        ResourcesPathDictionary.Add(typeof(UIPopup), UIPopup);
        ResourcesPathDictionary.Add(typeof(UIDoor), UIDoor);
    }

    private void Awake()
    {
        DictionaryAddFunction();
        //CheakCanvas();
        //foreach (UIBase UI in BattleUI)
        //{
        //    UIDictionary[UI.GetType()] = UI;
        //}

        //foreach (UIBase UI in MainUI)
        //{
        //    UIDictionary[UI.GetType()] = UI;
        //}
        //MainUI = null;
        //BattleUI = null;
    }

    public void Start()
    {
        GameObject mainobj = Resources.Load<GameObject>(MainCanvas);
        mainCanvas = Instantiate(mainobj).GetComponent<Canvas>();

        GameObject battleobj = Resources.Load<GameObject>(BattleCanvas);
        battleCanvas = Instantiate(battleobj).GetComponent<Canvas>();

        DontDestroyOnLoad(mainCanvas.gameObject);
        DontDestroyOnLoad(battleCanvas.gameObject);


        OpenUI<UILobby>(mainCanvas.gameObject.transform);
        OpenUI<UIStatus>(mainCanvas.gameObject.transform);
    }

    float ReleaseTime = 10f;
    float CurrentRelaseTime = 0f;

    public void Update()
    {
        CurrentRelaseTime += Time.deltaTime;
        if (CurrentRelaseTime > ReleaseTime)
        {
            CurrentRelaseTime = 0f;

            List<GameObject> keys = UIReleaser.Keys.ToList(); // 키 목록 복사

            foreach (var key in keys)
            {
                if (true == key.activeSelf)
                {
                    continue;
                }
                UIReleaser[key] -= ReleaseTime;
                if (UIReleaser[key] < 0f)
                {
                    ReleaseUIReleaser(key.GetComponent<UIBase>());
                }
            }
        }
    }

    //public void CheakCanvas()
    //{
    //    if (battleCanvas == null)
    //    {
    //        battleCanvas = GameObject.Find(BattleCanvasName)?.GetComponent<Canvas>();
    //        BattleUI = battleCanvas.GetComponentsInChildren<UIBase>(true);
    //        foreach (UIBase UI in BattleUI)
    //        {
    //            UIDictionary[UI.GetType()] = UI;
    //        }
    //    }

    //    if (mainCanvas == null)
    //    {
    //        mainCanvas = GameObject.Find(MainCanvasName)?.GetComponent<Canvas>();
    //        MainUI = mainCanvas.GetComponentsInChildren<UIBase>(true);
    //        foreach (UIBase UI in MainUI)
    //        {
    //            UIDictionary[UI.GetType()] = UI;
    //        }
    //    }
    //}

    const float UILifeTime = 10f;

    void AddUIRelease(UIBase _Base)
    {
        UIReleaser.Add(_Base.gameObject, UILifeTime);
    }

    void ResetUIRelease(UIBase _Base)
    {
        if (UIReleaser.ContainsKey(_Base.gameObject))
        {
            UIReleaser[_Base.gameObject] = UILifeTime;
        }
    }

    void ReleaseUIReleaser(UIBase _Base)
    {
        if (UIReleaser.ContainsKey(_Base.gameObject))
        {
            UIReleaser.Remove(_Base.gameObject);
            UIDictionary.Remove(_Base.GetType());
            Destroy(_Base.gameObject);
        }
    }

    T Add<T>(Transform _transform) where T : UIBase
    {
        if (true == ResourcesPathDictionary.TryGetValue(typeof(T), out var _Path))
        {
            GameObject obj = Resources.Load<GameObject>(_Path);
            GameObject inst = Instantiate(obj);
            T type = inst.GetComponent<T>();
            AddUIRelease(type);
            UIDictionary.Add(type.GetType(), type);
            inst.gameObject.transform.SetParent(_transform, false);
            return type;
        }
        DebugHelper.LogError($"{typeof(T).Name} NOExist.", this);
        return null;
    }

    public T GetUI<T>(Transform _transform) where T : UIBase
    {
        Type key = typeof(T);
        if (UIDictionary.ContainsKey(key))
        {
            if (UIDictionary[key] is T value)
            {
                ResetUIRelease(value);
                return value;
            }
        }
        else
        {
            T temp = Add<T>(_transform);
            if (temp != null)
            {
                return temp;
            }
        }
        return null;
    }

    public void OpenUI<T>(Transform _transform) where T : UIBase
    {
        T getUI = GetUI<T>(_transform);
        if (getUI != null)
        {
            getUI.Open();
        }
        else
        {
            T addUI = Add<T>(_transform);
            addUI.gameObject.transform.SetParent(_transform, false);
            addUI.Open();
        }
    }
    public void CloseUI<T>(Transform _transform) where T : UIBase
    {
        T getUI = GetUI<T>(_transform);
        getUI.Close();
    }

    public void OpenMainCanvas()
    {
        mainCanvas.gameObject.SetActive(true);
    }

    public void CloseMainCanvas()
    {
        mainCanvas.gameObject.SetActive(false);
    }

    public void OpenBattleCanvas()
    {
        battleCanvas.gameObject.SetActive(true);
    }

    public void CloseBattleCanvas()
    {
        battleCanvas.gameObject.SetActive(false);
    }

    public Transform GetMainCanvas()
    {
        return mainCanvas.gameObject.transform;
    }

    public Transform GetBattleCanvas()
    {
        return battleCanvas.gameObject.transform;
    }
}
