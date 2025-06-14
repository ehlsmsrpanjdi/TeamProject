using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHireScroll : UIBase
{
    List<HireLog> HireList = new List<HireLog>();
    [SerializeField] GameObject hireLogPrefab;

    [SerializeField] GameObject contentObject;

    [SerializeField] OnClickImage ReturnButton;

    const string HireLog = "UI/HireLog";

    private void Reset()
    {
        hireLogPrefab = Resources.Load<GameObject>(HireLog);
        contentObject = gameObject.GetComponentInChildren<HorizontalLayoutGroup>(true).gameObject;

        ReturnButton = GetComponentInChildren<OnClickImage>();
    }

    private void Awake()
    {
        ReturnButton.OnClick = UIManager.Instance.CloseUI<UIHireScroll>;
    }

    private void OnDisable()
    {
        foreach(HireLog log in HireList)
        {
            Destroy(log.gameObject);
        }
        HireList.Clear();
    }

    public void AddHire(DrawResult _Result)
    {
        GameObject logObject = Instantiate(hireLogPrefab);
        logObject.transform.SetParent(contentObject.transform);
        HireLog log = logObject.GetComponent<HireLog>();
        log.SetHireImage(_Result.character.characterImage);
        HireList.Add(log);
    }
}
