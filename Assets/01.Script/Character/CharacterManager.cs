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
    private List<CharacterInstance> participated = new();

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

    /// <summary>
    /// 참전 리스트에 넣기
    /// </summary>
     public bool SelectParticipate(int key)
    {
        if (characters.TryGetValue(key, out var character))
        {
            if (!participated.Contains(character))
            {
                participated.Add(character);
                return true; // 참전
            }
            else
            {
                return false; // 이미 참전 중
            }
        }
        return false; // 캐릭터 없음
    }

    /// <summary>
    /// 참전 제거
    /// </summary>
    public bool RemoveDeployedCharacter(int key)
    {
        var character = participated.FirstOrDefault(c => c.key == key);
        if (character != null)
        {
            participated.Remove(character);
            return true;
        }
        return false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public List<CharacterInstance> GetParticipateCharacters()
    {
        return participated;
    }
    public void SpawnParticipateCharacters()
    {
        var deployed = GetParticipateCharacters();

        for (int i = 0; i < deployed.Count; i++)
        {
            CharacterInstance charInstance = deployed[i];
            //Transform spawnPoint = spawnPoints[i]; //스폰 포인트

            GameObject go = Instantiate(charInstance.charPrefab, transform.position , Quaternion.identity); // 스폰포인트 안넣었음.

            CharacterBehaviour behaviour = go.GetComponent<CharacterBehaviour>();
            if (behaviour != null)
            {
                behaviour.Init(charInstance); // 캐릭터 데이터 전달
            }
        }
    }
}
