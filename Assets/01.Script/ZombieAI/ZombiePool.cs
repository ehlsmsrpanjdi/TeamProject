using UnityEngine;
using System.Collections.Generic;

public class ZombiePool : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int poolSize = 50;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(zombiePrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetZombie(Vector3 position)
    {
        if (pool.Count == 0)
        {
            return null;
        }

        GameObject obj = pool.Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnZombie(GameObject zombie)
    {
        zombie.SetActive(false);
        pool.Enqueue(zombie);
    }
}
