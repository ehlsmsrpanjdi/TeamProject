using System;
using System.Collections.Generic;
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

    Dictionary<Type, UIBase> UIDictionary = new Dictionary<Type, UIBase>();
    [SerializeField] UIBase[] MainUI;
    [SerializeField] UIBase[] BattleUI;

    [SerializeField] Canvas mainCanvas;
    const string MainCanvasName = "MainCanvas";
    [SerializeField] Canvas battleCanvas;
    const string BattleCanvasName = "BattleCanvas";

    private void Reset()
    {
        mainCanvas = GameObject.Find(MainCanvasName)?.GetComponent<Canvas>();
        MainUI = mainCanvas.GetComponentsInChildren<UIBase>(true);

        battleCanvas = GameObject.Find(BattleCanvasName)?.GetComponent<Canvas>();
        BattleUI = battleCanvas.GetComponentsInChildren<UIBase>(true);
    }

    private void Awake()
    {
        CheakCanvas();
        foreach (UIBase UI in BattleUI)
        {
            UIDictionary[UI.GetType()] = UI;
        }

        foreach (UIBase UI in MainUI)
        {
            UIDictionary[UI.GetType()] = UI;
        }
        MainUI = null;
        BattleUI = null;
    }

    public void CheakCanvas()
    {
        if (battleCanvas == null)
        {
            battleCanvas = GameObject.Find(BattleCanvasName)?.GetComponent<Canvas>();
            BattleUI = battleCanvas.GetComponentsInChildren<UIBase>(true);
            foreach (UIBase UI in BattleUI)
            {
                UIDictionary[UI.GetType()] = UI;
            }
        }

        if (mainCanvas == null)
        {
            mainCanvas = GameObject.Find(MainCanvasName)?.GetComponent<Canvas>();
            MainUI = mainCanvas.GetComponentsInChildren<UIBase>(true);
            foreach (UIBase UI in MainUI)
            {
                UIDictionary[UI.GetType()] = UI;
            }
        }
    }

    public T Add<T>() where T : UIBase
    {
        if (mainCanvas.GetComponentInChildren<T>() is T value)
        {
            UIDictionary[value.GetType()] = value;
            return value;
        }
        DebugHelper.LogError($"{typeof(T).Name} NONO.", mainCanvas);
        return null;
    }

    public T GetUI<T>() where T : UIBase
    {
        Type key = typeof(T);
        if (UIDictionary.ContainsKey(key))
        {
            if (UIDictionary[key] is T value)
            {
                return value;
            }
        }
        else
        {
            T temp = Add<T>();
            if (temp != null)
            {
                return temp;
            }
        }
        return null;
    }

    public void OpenUI<T>() where T : UIBase
    {
        GetUI<T>()?.Open();
    }
    public void CloseUI<T>() where T : UIBase
    {
        GetUI<T>()?.Close();
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
}
