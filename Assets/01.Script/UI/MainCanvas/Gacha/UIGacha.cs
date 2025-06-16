using DG.Tweening;
using System.Collections;
using UnityEngine;

public class UIGacha : UIBase
{
    [SerializeField] GameObject GachaCameraObject;
    [SerializeField] OnClickImage OnClickBackGround;

    GameObject SpawnBox;

    bool IsOpening = false;
    bool IsOpen = false;

    Coroutine CloseCoroutineValue;

    const string GachaCamera = "GachaCamera";

    private void Reset()
    {
        GachaCameraObject = GameObject.Find(GachaCamera);
        OnClickBackGround = GetComponentInChildren<OnClickImage>();
    }

    public void Awake()
    {
        if (GachaCameraObject != null)
        {
            GachaCameraObject = GameObject.Find(GachaCamera);
        }
        OnClickBackGround.NoHoverColor();
        OnClickBackGround.OnClick = OnClickGachaUI;
    }

    public override void Open()
    {
        base.Open();
        IsOpening = false;
        IsOpen = false;
        gameObject.transform.FadeOutXY();
        GachaCamera GachaComponent = GachaCameraObject.GetComponent<GachaCamera>();
        SpawnBox = GachaComponent.SpawnNormalBox();
    }

    public override void Close()
    {
        base.Close();
        UIHireScroll HireScroll = UIManager.Instance.GetUI<UIHireScroll>(UIManager.Instance.GetMainCanvas());
        HireScroll.Open();
        IsOpening = false;
        IsOpen = false;
    }

    void OnClickGachaUI()
    {
        if (false == IsOpening)
        {
            SpawnBox.GetComponent<GachaBox>().Opening();
            IsOpening = true;
        }
        else if (false == IsOpen)
        {
            Tween tween = SpawnBox.transform.ScaleUp(1.5f);
            tween.OnComplete(() =>
        {
            if(true == gameObject.activeSelf)
            {
                Destroy(SpawnBox);
                CloseCoroutineValue = StartCoroutine(CloseCoroutine());
            }
        });

            IsOpen = true;
        }
        else
        {
            if(CloseCoroutineValue != null)
            {
                StopCoroutine(CloseCoroutineValue);
                CloseCoroutineValue = null;
            }
            Destroy(SpawnBox);
            Close();
        }
    }

    IEnumerator CloseCoroutine()
    {
        yield return new WaitForSeconds(3f);
        if (true == gameObject.activeSelf){
            Close();
        }
    }

}
