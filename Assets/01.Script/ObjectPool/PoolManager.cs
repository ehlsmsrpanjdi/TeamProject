using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static PoolManager Instance { get; private set; }

    // 타입별 풀을 저장하는 딕셔너리
    private Dictionary<Type, object> pools = new Dictionary<Type, object>();

    private void Awake()
    {
        // 인스턴스 초기화 (중복 방지)
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 지정된 타입 T의 오브젝트 풀 생성
    public void CreatePool<T>(T prefab, int count = 10, Transform parent = null) where T : Component
    {
        Type type = typeof(T);
        if (pools.ContainsKey(type)) return;

        var pool = new GenericObjectPool<T>(prefab, count, parent);
        pools.Add(type, pool);
    }

    // 오브젝트 풀에서 오브젝트 하나 꺼냄
    public T Get<T>() where T : Component
    {
        if (pools.TryGetValue(typeof(T), out var pool))
        {
            return ((GenericObjectPool<T>)pool).Get();
        }

        Debug.LogError($"[PoolManager] {typeof(T)} 풀을 찾을 수 없습니다.");
        return null;
    }

    // 오브젝트를 풀로 반환
    public void Return<T>(T obj) where T : Component
    {
        if (pools.TryGetValue(typeof(T), out var pool))
        {
            ((GenericObjectPool<T>)pool).ReturnToPool(obj);
        }
        else
        {
            Debug.LogWarning($"[PoolManager] {typeof(T)} 풀에 반환할 수 없습니다.");
        }
    }
}
