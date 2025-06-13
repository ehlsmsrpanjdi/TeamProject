using System.Collections.Generic;
using UnityEngine;

public class ZombieProjectilePool : MonoBehaviour
{
    public static ZombieProjectilePool Instance { get; private set; }

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject proj = Instantiate(projectilePrefab);
            proj.SetActive(false);
            proj.transform.SetParent(transform);
            poolQueue.Enqueue(proj);
        }
    }

    public GameObject GetProjectile(Vector3 position, Quaternion rotation, int damage, GameObject shooter, Vector3 velocity)
    {
        GameObject proj;
        if (poolQueue.Count > 0)
        {
            proj = poolQueue.Dequeue();
            proj.SetActive(true);
        }
        else
        {
            proj = Instantiate(projectilePrefab);
            proj.transform.SetParent(transform);
        }

        proj.transform.position = position;
        proj.transform.rotation = rotation;

        ZombieProjectile projScript = proj.GetComponent<ZombieProjectile>();
        Rigidbody projRb = proj.GetComponent<Rigidbody>();

        if (projScript != null && projRb != null)
        {
            projScript.SetDamage(damage);
            projScript.SetShooter(shooter);
            projScript.Launch(velocity);
        }

        return proj;
    }

    public void Return(GameObject proj)
    {
        proj.SetActive(false);
        poolQueue.Enqueue(proj);
    }
}
