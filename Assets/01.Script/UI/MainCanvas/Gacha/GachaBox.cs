using UnityEngine;

public class GachaBox : MonoBehaviour
{
    [SerializeField] GameObject Particle;

    const string UIParticle = "UI/UIParticle";

    private void Reset()
    {
        Particle = Resources.Load<GameObject>(UIParticle);
    }

    private void Awake()
    {
        Particle = Resources.Load<GameObject>(UIParticle);
    }

    public void Opening()
    {
        transform.RightRotationSpeedLoop(4f);
    }

    private void OnDestroy()
    {
        transform.KillDoTween();
        Instantiate(Particle).transform.position = gameObject.transform.position;
    }
}
