using DG.Tweening;
using System.Collections;
using UnityEngine;

public class UIGacha : UIBase
{
    [SerializeField] GameObject GachaCameraObject;
    [SerializeField] OnClickImage OnClickBackGround;
    [SerializeField] BackGroundHelper backGroundHelper;

    GameObject SpawnBox;

    bool IsOpening = false;
    bool IsOpen = false;

    Coroutine CloseCoroutineValue;

    const string GachaCamera = "GachaCamera";

    private void Reset()
    {
        GachaCameraObject = GameObject.Find(GachaCamera);
        OnClickBackGround = GetComponentInChildren<OnClickImage>();
        backGroundHelper = gameObject.transform.parent.GetComponent<BackGroundHelper>();
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
        backGroundHelper.gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        UIHireScroll HireScroll = UIManager.Instance.GetUI<UIHireScroll>();
        HireScroll.Open();
        backGroundHelper.gameObject?.SetActive(false);
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
            Destroy(SpawnBox);
            CloseCoroutineValue = StartCoroutine(CloseCoroutine());
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
