using System;
using System.Collections.Generic;
using UnityEngine;
//==================== 사용 방법 ====================

// 투사체나 효과 등 단일 타입 오브젝트 (타입 기반 풀링)
// 초기화
//Pool.Instance.CreatePool<ZombieProjectile>(projectilePrefab, 20, transform);

// 꺼내기
//ZombieProjectile proj = Pool.Instance.Get<ZombieProjectile>();

// 반환하기
//Pool.Instance.Return(proj);

//=================================================

// 같은 타입(Zombie)인데 프리팹이 다른 경우 (키 기반 풀링)
// 초기화
//Pool.Instance.CreateZombiePool("Zombie1", zombie1Prefab, 50);
//Pool.Instance.CreateZombiePool("Zombie2", zombie2Prefab, 25);

// 꺼내기
//Zombie z1 = Pool.Instance.GetZombie("Zombie1");
//Zombie z2 = Pool.Instance.GetZombie("Zombie2");

// 반환하기
//Pool.Instance.ReturnZombie("Zombie1", z1);
//Pool.Instance.ReturnZombie("Zombie2", z2);

public class Pool : MonoBehaviour
{
    public static Pool Instance { get; private set; }
    private Dictionary<Type, object> pools = new Dictionary<Type, object>();
    private Dictionary<string, GenericObjectPool<Zombie>> zombiePools = new();

    [Header("좀비 프리팹")]
    [SerializeField] private Zombie zombie1Prefab;
    [SerializeField] private Zombie zombie2Prefab;
    [SerializeField] private int zombie1PoolSize = 50;
    [SerializeField] private int zombie2PoolSize = 25;

    [Header("투사체 프리팹")]
    [SerializeField] private ZombieProjectile zombieProjectilePrefab;
    [SerializeField] private int projectilePoolSize = 20;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        CreatePool(zombieProjectilePrefab, projectilePoolSize, transform);

        CreateZombiePool("Zombie1", zombie1Prefab, zombie1PoolSize);
        CreateZombiePool("Zombie2", zombie2Prefab, zombie2PoolSize);
    }

    // 제네릭 방식으로 풀 생성
    public void CreatePool<T>(T prefab, int count = 10, Transform parent = null) where T : Component
    {
        Type type = typeof(T);
        if (pools.ContainsKey(type)) return; // 중복 등록 방지

        // 풀 생성 및 등록
        var pool = new GenericObjectPool<T>(prefab, count, parent);
        pools.Add(type, pool);
    }

    // 타입 기반 풀에서 오브젝트 가져오기
    public T Get<T>() where T : Component
    {
        if (pools.TryGetValue(typeof(T), out var pool))
        {
            return ((GenericObjectPool<T>)pool).Get();
        }

        Debug.LogError($"[Pool] {typeof(T)} 풀 없음");
        return null;
    }

    // 타입 기반 풀에 오브젝트 반환
    public void Return<T>(T obj) where T : Component
    {
        if (pools.TryGetValue(typeof(T), out var pool))
        {
            ((GenericObjectPool<T>)pool).ReturnToPool(obj);
        }
        else
        {
            Debug.LogWarning($"[Pool] {typeof(T)} 풀 반환 실패");
        }
    }

    // 키 기반 좀비 풀 생성 ("Zombie1", "Zombie2" 등)
    private void CreateZombiePool(string key, Zombie prefab, int count)
    {
        if (zombiePools.ContainsKey(key)) return;

        // 이 Pool 오브젝트를 부모로 설정
        zombiePools[key] = new GenericObjectPool<Zombie>(prefab, count, transform);
    }

    // 키 기반 좀비 풀에서 오브젝트 가져오기
    public Zombie GetZombie(string key)
    {
        if (zombiePools.TryGetValue(key, out var pool))
            return pool.Get();

        Debug.LogError($"[Pool] 좀비 풀 '{key}' 없음");
        return null;
    }

    // 키 기반 좀비 풀에 오브젝트 반환
    public void ReturnZombie(string key, Zombie zombie)
    {
        if (zombiePools.TryGetValue(key, out var pool))
            pool.ReturnToPool(zombie);
        else
            Debug.LogWarning($"[Pool] 좀비 풀 '{key}' 반환 실패");
    }
}
