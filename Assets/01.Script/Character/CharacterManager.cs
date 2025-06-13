using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 캐릭터의 생성, 삭제 등의 관리를 위한 클래스
/// </summary>
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    private Dictionary<int, CharacterInstance> characters = new();

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
        }
    }

    /// <summary>
    /// 캐릭터 데이터를 기반으로 인스턴스를 생성하고 등록
    /// </summary>
    public CharacterInstance CreateCharacter(int key)
    {
        CharacterDataSO data = CharacterData.instance.GetData(key);

        if (data == null)
        {
            return null;
        }

        if (characters.ContainsKey(key))
        {
            return characters[key];
        }

        CharacterInstance newCharacter = new CharacterInstance(data);
        characters.Add(key, newCharacter);

        return newCharacter;
    }

    /// <summary>
    /// 특정 키의 캐릭터 인스턴스를 반환
    /// </summary>
    public CharacterInstance GetCharacter(int key)
    {
        characters.TryGetValue(key, out var character);
        return character;
    }

    /// <summary>
    /// 특정 캐릭터 삭제
    /// </summary>
    public void RemoveCharacter(int key)
    {
        if (characters.ContainsKey(key))
            characters.Remove(key);
    }

    /// <summary>
    /// 전체 캐릭터 목록 반환
    /// </summary>
    public List<CharacterInstance> GetAllCharacters()
    {
        return characters.Values.ToList();
    }
}
