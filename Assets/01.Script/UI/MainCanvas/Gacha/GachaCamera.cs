using UnityEngine;

public class GachaCamera : MonoBehaviour
{
    Vector3 GachaBallPosition = new Vector3(0, 0, 7);
    Vector3 GachaBallRotation = new Vector3(0, 60, 0);

    GameObject NormalBox;
    GameObject HighBox;

    const string NormalGachaBall = "UI/NormalGachaBall";
    const string HighGachaBall = "UI/HighGachaBall";

    private void Start()
    {

    }

    public GameObject SpawnNormalBox()
    {
        if (NormalBox == null)
        {
            NormalBox = Resources.Load<GameObject>(NormalGachaBall);
            HighBox = Resources.Load<GameObject>(HighGachaBall);
        }
        Vector3 BoxPosition = gameObject.transform.position + GachaBallPosition;
        Vector3 BoxRotation = gameObject.transform.rotation.eulerAngles + GachaBallRotation;
        GameObject obj = Instantiate(NormalBox);
        obj.transform.position = BoxPosition;
        obj.transform.rotation = Quaternion.Euler(BoxRotation);
        return obj;
    }

    public GameObject SpawnHighBox()
    {
        Vector3 BoxPosition = gameObject.transform.position + GachaBallPosition;
        Vector3 BoxRotation = gameObject.transform.rotation.eulerAngles + GachaBallRotation;
        GameObject obj = Instantiate(HighBox);
        obj.transform.position = BoxPosition;
        obj.transform.rotation = Quaternion.Euler(BoxRotation);
        return obj;
    }

}
