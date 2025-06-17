using DG.Tweening;
using System.Collections;
using UnityEngine;

public class UIGacha : UIBase
{
    [SerializeField] OnClickImage OnClickBackGround;
    GachaCamera GachaComponent;
    GameObject SpawnBox;

    bool IsOpening = false;
    bool IsOpen = false;

    Coroutine CloseCoroutineValue;

    const string GachaCamera = "UI/GachaCamera";

    private void Reset()
    {
        OnClickBackGround = GetComponentInChildren<OnClickImage>();
    }

    public void Awake()
    {
        OnClickBackGround.NoHoverColor();
        OnClickBackGround.OnClick = OnClickGachaUI;
    }

    public override void Open()
    {
        base.Open();
        IsOpening = false;
        IsOpen = false;
        gameObject.transform.FadeOutXY();
        if (null == GachaComponent)
        {
            GameObject GachaObj = Resources.Load<GameObject>(GachaCamera);
            GachaComponent = Instantiate(GachaObj).GetComponent<GachaCamera>();
        }
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
            if (true == gameObject.activeSelf)
            {
                Destroy(SpawnBox);
                CloseCoroutineValue = StartCoroutine(CloseCoroutine());
            }
        });

            IsOpen = true;
        }
        else
        {
            if (CloseCoroutineValue != null)
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
        if (true == gameObject.activeSelf)
        {
            Close();
        }
    }

}
