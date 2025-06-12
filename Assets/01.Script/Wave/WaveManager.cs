using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public int currentWave = 1;
    public int zombiesPerWave = 5;
    public float waveInterval = 5f;

    private int aliveZombies = 0;
    private bool isWaveSpawning = false;

    public ZombieSpawner spawner;

    private void Update()
    {
        if (!isWaveSpawning && aliveZombies <= 0)
            StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        isWaveSpawning = true;

        Debug.Log($"[WaveManager] 다음 웨이브 시작 대기: {waveInterval}초");
        yield return new WaitForSeconds(waveInterval);

        int count = currentWave * zombiesPerWave;
        spawner.SpawnWave(count);

        aliveZombies = count;
        Debug.Log($"[WaveManager] 웨이브 {currentWave} 시작 - 좀비 수: {aliveZombies}");

        currentWave++;
        isWaveSpawning = false;
    }

    public void OnZombieDied()
    {
        aliveZombies--;
        Debug.Log($"[WaveManager] 좀비 사망 → 남은 수: {aliveZombies}");
    }
}
