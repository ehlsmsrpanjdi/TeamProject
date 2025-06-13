using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// 전체 캐릭터의 데이터를 담는 클래스
/// </summary>
public class CharacterData : MonoBehaviour
{
    [SerializeField] private List<CharacterDataSO> characterList;
    private Dictionary<int, CharacterDataSO> Data = new Dictionary<int, CharacterDataSO>();
    public static CharacterData instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (CharacterDataSO characterData in characterList)
        {
            Data.Add(characterData.key, characterData);
        }
    }

    public CharacterDataSO GetData(int key)
    {
        Data.TryGetValue(key,out CharacterDataSO data);
        return data;
    }

}
