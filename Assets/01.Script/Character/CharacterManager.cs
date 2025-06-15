using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 캐릭터의 생성, 삭제 등의 관리를 위한 클래스
/// </summary>
public class CharacterManager
{
    private static CharacterManager instance;

    public static CharacterManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CharacterManager();
                instance.Init();
            }
            return instance;
        }
        set { instance = value; }
    }

    public void Init()
    {
        GachaManager.Instance.OnCharacterDraw += CreateCharacterOndraw;
    }

    private List<CharacterInstance> characters = new();
    private Dictionary<int, CharacterInstance> participated = new();

    /// <summary>
    /// 캐릭터 데이터를 기반으로 인스턴스를 생성하고 등록
    /// </summary>
    public CharacterInstance CreateCharacter(int key)
    {
        CharacterDataSO data = CharacterData.instance.GetData(key);

        if (data == null)
        {
            DebugHelper.Log("data is null", data);
            return null;
        }
        CharacterInstance newCharacter = new CharacterInstance(data);
        characters.Add(newCharacter);

        return newCharacter;
    }

    public void CreateCharacterOndraw(DrawResult result)
    {
        CharacterDataSO data = result.character;

        if (data == null)
        {
            DebugHelper.Log("data is null", data);
        }
        CharacterInstance newCharacter = new CharacterInstance(data);
        characters.Add(newCharacter);
    }

    /// <summary>
    /// 특정 슬롯의 캐릭터 인스턴스를 반환
    /// </summary>
    public CharacterInstance GetCharacter(int index)
    {
        //index 예외처리
        if (index < 0 || index >= characters.Count)
        {
            return null;
        }

        return characters[index];
    }

    /// <summary>
    /// 특정 캐릭터 삭제
    /// </summary>
    public void RemoveCharacter(int index)
    {
        //index 예외처리
        if (index < 0 || index >= characters.Count)
        {
            return;
        }
        characters.RemoveAt(index);
    }

    /// <summary>
    /// 전체 캐릭터 목록 반환
    /// </summary>
    public List<CharacterInstance> GetAllCharacters()
    {
        return characters;
    }

    /// <summary>
    /// 참전 리스트에 넣기. 최대인원 4명 제한.
    /// </summary>
     public bool SelectParticipate(int index)
    {
        if (participated.Count >= 4)
        {
            return false;
        }

        if (index < 0 || index >= characters.Count)
        {
            return false;
        }

        if (characters[index] != null)
        {
            if (!participated.ContainsKey(index))
            {
                participated.Add(index, characters[index]);
                return true; // 참전
            }
        }
        return false; // 캐릭터 없음
    }

    /// <summary>
    /// 참전 리스트에서 제거
    /// </summary>
    public bool RemoveParticipate(int index)
    {
        var character = participated[index];
        if (character != null)
        {
            participated.Remove(index);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 참전 리스트 불러오기 >> 실제로 전투에 참여할 캐릭터들
    /// </summary>
    public List<CharacterInstance> GetParticipateCharacters()
    {
        return participated.Values.ToList();
    }

    /// <summary>
    /// 딕셔너리로 참전 캐릭터 모두 볼 수 있게
    /// </summary>
    public Dictionary<int, CharacterInstance> GetParticipateCharactersAsDictionary()
    {
        return participated;
    }

    public bool IsParticipating(int index)
    {
        return participated.ContainsKey(index);
    }

    /// <summary>
    /// 실제로 전투에 참여할 캐릭터 스폰 함수
    /// </summary>
    public void SpawnParticipateCharacters(Vector3 position)
    {
        var deployed = GetParticipateCharacters();

        for (int i = 0; i < deployed.Count; i++)
        {
            CharacterInstance charInstance = deployed[i];
            //Transform spawnPoint = spawnPoints[i]; //스폰 포인트가 필요할 것으로 보여짐.

            GameObject go = GameObject.Instantiate(charInstance.charPrefab, position , Quaternion.identity); // 스폰포인트 안넣었음.

            CharacterBehaviour behaviour = go.GetComponent<CharacterBehaviour>();
            if (behaviour != null)
            {
                behaviour.Init(charInstance); // 캐릭터 데이터 전달
            }
        }
    }


    #if UNITY_EDITOR
    public void EditorFunction()
    {
        CreateCharacter(1005);
  
        SelectParticipate(0);
   


        SpawnParticipateCharacters(Vector3.zero); // 스폰 호출


    }
    #endif
}
