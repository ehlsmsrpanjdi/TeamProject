using System;
using System.Collections.Generic;
using UnityEngine;

// 풀링 시스템 클래스 (MonoBehaviour로 자동 생성 및 관리)
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    // 타입 기반 풀
    private Dictionary<Type, Queue<Component>> typePools = new();   // T 타입 기반 큐
    private Dictionary<Type, Component> typePrefabs = new();        // T 타입별 프리팹

    // 키 기반 풀
    private Dictionary<string, Queue<Component>> keyPools = new(); // 문자열 키 기반 큐
    private Dictionary<string, Component> keyPrefabs = new();      // 키별 프리팹

    // 게임 시작 전에 자동 실행되어 ObjectPool 오브젝트 생성 및 초기화
    // Awake보다 먼저 실행되어 확실한 선처리 가능
    // 별도의 추가 스크립트없이 오브젝트 풀링까지 한번에 작성가능
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateInstance()
    {
        // 중복 방지
        if (Instance != null) return;

        // GameObject 생성 및 컴포넌트 부착
        GameObject go = new GameObject("[ObjectPool]");
        Instance = go.AddComponent<ObjectPool>();
        DontDestroyOnLoad(go);

        // Resources에서 프리팹 로드
        var zombie1 = Resources.Load<Zombie>("Zombie/Zombie");
        var zombie2 = Resources.Load<Zombie>("Zombie/Zombie2");
        var projectile = Resources.Load<ZombieProjectile>("Zombie/ZombieProjectile");

        // 프리팹 로드
        Instance.Register("Zombie1", zombie1, 50);
        Instance.Register("Zombie2", zombie2, 25);
        Instance.Register("Projectile", projectile, 20);

        Debug.Log("[ObjectPool] 자동 초기화 및 프리팹 등록 완료");
    }

    // ---------- 타입 기반 등록 ----------
    public void Register<T>(T prefab, int count = 10) where T : Component
    {
        var type = typeof(T);
        if (typePools.ContainsKey(type)) return;

        typePools[type] = new Queue<Component>();
        typePrefabs[type] = prefab;

        for (int i = 0; i < count; i++)
        {
            T obj = Instantiate(prefab, transform);
            obj.gameObject.SetActive(false);
            typePools[type].Enqueue(obj);
        }
    }

    // ---------- 키 기반 등록 ----------
    public void Register<T>(string key, T prefab, int count = 10) where T : Component
    {
        if (keyPools.ContainsKey(key)) return;

        keyPools[key] = new Queue<Component>();
        keyPrefabs[key] = prefab;

        for (int i = 0; i < count; i++)
        {
            T obj = Instantiate(prefab, transform);
            obj.gameObject.SetActive(false);
            keyPools[key].Enqueue(obj);
        }
    }

    // ---------- 타입 기반 Get ---------- 큐에서 꺼내서 반환하며, 없을 경우 새로 생성
    public T Get<T>() where T : Component
    {
        var type = typeof(T);

        if (typePools.TryGetValue(type, out var queue) && queue.Count > 0)
        {
            var obj = queue.Dequeue() as T;
            obj.gameObject.SetActive(true);
            return obj;
        }

        if (typePrefabs.TryGetValue(type, out var prefab))
        {
            var obj = Instantiate(prefab as T, transform);
            obj.gameObject.SetActive(true);
            return obj;
        }

        Debug.LogError($"[ObjectPool] 등록되지 않은 타입: {type}");
        return null;
    }

    // ---------- 타입 기반 Return ---------- 사용 완료된 오브젝트를 비활성화 후 다시 큐에 넣음
    public void Return<T>(T obj) where T : Component
    {
        var type = typeof(T);
        obj.gameObject.SetActive(false);

        if (!typePools.ContainsKey(type))
            typePools[type] = new Queue<Component>();

        typePools[type].Enqueue(obj);
    }

    // ---------- 키 기반 Get ----------
    public T Get<T>(string key) where T : Component
    {
        if (keyPools.TryGetValue(key, out var queue) && queue.Count > 0)
        {
            var obj = queue.Dequeue() as T;
            obj.gameObject.SetActive(true);
            return obj;
        }

        if (keyPrefabs.TryGetValue(key, out var prefab))
        {
            var obj = Instantiate(prefab as T, transform);
            obj.gameObject.SetActive(true);
            return obj;
        }

        Debug.LogError($"[ObjectPool] 등록되지 않은 키: {key}");
        return null;
    }

    // ---------- 키 기반 Return ----------
    public void Return<T>(string key, T obj) where T : Component
    {
        obj.gameObject.SetActive(false);

        if (!keyPools.ContainsKey(key))
            keyPools[key] = new Queue<Component>();

        keyPools[key].Enqueue(obj);
    }
}
