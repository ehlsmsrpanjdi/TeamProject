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
    }

    public GachaCamera GetGachaCamera()
    {
        if (null == GachaComponent)
        {
            GameObject GachaObj = Resources.Load<GameObject>(GachaCamera);
            GachaComponent = Instantiate(GachaObj).GetComponent<GachaCamera>();
        }
        return GachaComponent;
    }

    public override void Close()
    {
        base.Close();
        UIHireScroll HireScroll = UIManager.Instance.GetUI<UIHireScroll>(UIManager.Instance.GetMainCanvas());
        HireScroll.Open();
        IsOpening = false;
        IsOpen = false;
    }

    public void SetBox(GameObject _Box)
    {
        SpawnBox = _Box;
    }

    void OnClickGachaUI()
    {
        Transform GachaTransform = GachaComponent.transform;
        if (false == IsOpening)
        {
            SpawnBox.GetComponent<GachaBox>().Opening();
            GachaTransform.MoveZ(GachaTransform.position.z, GachaTransform.transform.position.z + 2f);
            IsOpening = true;
        }
        else if (false == IsOpen)
        {
            Tween tween = SpawnBox.transform.ScaleUp(1.5f);
            GachaTransform.MoveZ(GachaTransform.position.z, GachaTransform.transform.position.z + 1f);
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
        yield return CoroutineHelper.GetTime(3f);
        if (true == gameObject.activeSelf)
        {
            Close();
        }
    }

}
