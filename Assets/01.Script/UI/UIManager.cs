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
                }
            }
            return instance;
        }
    }

    Dictionary<Type, UIBase> UIDictionary = new Dictionary<Type, UIBase>();
    [SerializeField] UIBase[] AllUI;

    [SerializeField] Canvas mainCanvas;
    const string MainCanvasName = "MainCanvas";

    private void Reset()
    {
        mainCanvas = GameObject.Find(MainCanvasName)?.GetComponent<Canvas>();
        AllUI = mainCanvas.GetComponentsInChildren<UIBase>();
    }

    private void Awake()
    {
        if (mainCanvas == null)
        {
            mainCanvas = GameObject.Find(MainCanvasName)?.GetComponent<Canvas>();
            AllUI = mainCanvas.GetComponentsInChildren<UIBase>();
            foreach (UIBase UI in AllUI)
            {
                UIDictionary[UI.GetType()] = UI;
            }
            return;
        }
        foreach (UIBase UI in AllUI)
        {
            UIDictionary[UI.GetType()] = UI;
        }
    }

    public void OpenUI<T>() where T : UIBase
    {
        GetUI<T>()?.Open();
    }

    public T Add<T>() where T : UIBase
    {
        if (mainCanvas.GetComponentInChildren<T>() is T ui)
        {
            UIDictionary[ui.GetType()] = ui;
            return ui;
        }
        DebugHelper.LogError($"{typeof(T).Name} NONO.", mainCanvas);
        return null;
    }

    public void CloseUI<T>() where T : UIBase
    {
        GetUI<T>()?.Close();
    }

    public T GetUI<T>() where T : UIBase
    {
        Type key = typeof(T);
        if (UIDictionary.ContainsKey(key))
        {
            if (UIDictionary[key] is T)
            {
                return UIDictionary[key] as T;
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
}
