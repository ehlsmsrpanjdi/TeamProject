using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public ZombiePool zombiePool;
    public Transform spawnAreaCenter;
    public Vector2 spawnAreaSize = new Vector2(10f, 10f);
    public float spawnInterval = 5f;
    public int zombiesPerWave = 5;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnWave();
            timer = 0;
        }
    }

    private void SpawnWave()
    {
        for (int i = 0; i < zombiesPerWave; i++)
        {
            Vector3 randomPos = GetRandomPositionInArea();
            GameObject zombie = zombiePool.GetZombie(randomPos);
            if (zombie == null)
            {
                break;
            }
        }
    }


    private Vector3 GetRandomPositionInArea()
    {
        Vector3 center = spawnAreaCenter.position;
        float halfX = spawnAreaSize.x / 2f;
        float halfZ = spawnAreaSize.y / 2f;

        float randX = Random.Range(-halfX, halfX);
        float randZ = Random.Range(-halfZ, halfZ);

        return new Vector3(center.x + randX, center.y, center.z + randZ);
    }

    private void OnDrawGizmos()
    {
        if (spawnAreaCenter == null) return;

        Gizmos.color = Color.green;
        Vector3 size = new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y);
        Gizmos.DrawWireCube(spawnAreaCenter.position, size);
    }
}
