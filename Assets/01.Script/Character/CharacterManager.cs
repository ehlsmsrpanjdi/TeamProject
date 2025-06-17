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

            }
            return instance;
        }
        set { instance = value; }
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
    public void SpawnParticipateCharacters()
    {
        var deployed = GetParticipateCharacters();
        var spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint"); // 스폰 위치
        var setPoints = GameObject.FindGameObjectsWithTag("SetPoint").Select(go =>go.transform).ToList(); //Set 위치

        for (int i = 0; i < deployed.Count; i++)
        {
            CharacterInstance charInstance = deployed[i];

            Vector3 random = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Vector3 spawn = spawnPoint.transform.position + random;

            GameObject go = GameObject.Instantiate(charInstance.charPrefab, spawn , Quaternion.identity);
            //GameObject go = GameObject.Instantiate(charInstance.charPrefab, position, Quaternion.identity);

            CharacterBehaviour behaviour = go.GetComponent<CharacterBehaviour>();
            if (behaviour != null)
            {
                behaviour.Init(charInstance, setPoints[i]); // 캐릭터 데이터 전달
            }
        }
    }

    /// <summary>
    /// 선택 슬롯의 캐릭터 강화
    /// </summary>

    public bool EnhanceCharacter(int index)
    {
        if(index <0 || index >= characters.Count)
        {
            return false;
        }

        CharacterInstance character = characters[index];
        if(character == null)
        {
            return false;
        }

        character.Enhance();
        QuestManager.Instance.OnEnchantButtonPressed();
        return true;
    }

    /// <summary>
    /// 랭크 업 기능
    /// </summary>
    public bool RankUpCharacter(int index)
    {
        if(index < 0  || index >= characters.Count)
        {
            return false;
        }

        CharacterInstance character = characters[index];
        if (character == null)
        {
            return false;
        }

        // 필요한것
        // 현재 캐릭터의 랭크 정보
        // 현재 캐릭터의 랭크
        // 현재 캐릭터의 랭크업에 필요한 요구 수량
        // 리스트에 동일한 랭크의 캐릭터가 있는지
        // 랭크업이 됐으면 캐릭터 삭제

        var charRank = character.rankInfo.FirstOrDefault(r => r.rank == character.currentRank); //선택된 캐릭터의 랭크인포 전달 (리스트로 가지고 있어서 이런식으로 줌)

        int requiredCount = charRank.requiredOwnedCount; //랭크인포의 랭크업 요구 수량 전달
        var sameCharacter = characters.Where(sc => sc.key == character.key && sc.currentRank == character.currentRank).ToList(); //동일한 키, 동일한 등급을 가지고 있는 캐릭터만 선택하여 리스트화
        if(sameCharacter.Count < requiredCount)
        {
            Debug.Log("강화에 필요한 수량이 부족합니다.");
            return false;
        }

        int consumed = 0;
        for (int i = characters.Count -1; i >= 0 && consumed < requiredCount -1; i--) // 포문 뒤에서부터 돌리기. 리스트 제거를 뒤에서부터 하기 위해서.
        {
            if (characters[i] != character && characters[i].key == character.key && characters[i].currentRank ==character.currentRank) // 보유리스트 안의 캐릭터가 랭크업을 시도한 캐릭터가 아니거나, 키가 같으면
            {
                characters.RemoveAt(i); //제거
                consumed++; //소모값 1개 추가. 요구 수량까지 진행
            }

        }
        character.RankUp();
        Debug.Log("랭크업 성공");
        //GetAllCharacters(); 필요없음.

        return true;

    }

    /// <summary>
    /// 참전 캐릭터의 모든 체력 합산 >> 바리게이트에서 가지고 감
    /// </summary>
    public float GetTotalHealt()
    {
        return participated.Values.Sum(character => character.GetCurrentHealth());
    }

    #if UNITY_EDITOR
    public void EditorFunction()
    {
        CreateCharacter(1001);
        CreateCharacter(1002);
        CreateCharacter(1003);
        CreateCharacter(1004);
        SelectParticipate(0);
        SelectParticipate(1);
        SelectParticipate(2);
        SelectParticipate(3);

        SpawnParticipateCharacters(); // 스폰 호출
    }
    public void EditorFunctionEnhance()
    {
        EnhanceCharacter(0);
    }

    public void EditorFunctionCreat()
    {
        CreateCharacter(1001);
        CreateCharacter(1001);

        foreach (var character in characters)
        {
            Debug.Log($"생성된 캐릭터 이름: {character.charcterName}\n 생성된 캐릭터 랭크: {character.currentRank}");
            Debug.Log($"공격력: {character.GetCurrentAttack()}\n 체력 : {character.GetCurrentHealth()}");
            List<Skill> activeSkills = character.GetActiveSkills();

            if (activeSkills.Count > 0)
            {
                string skillNames = string.Join(", ", activeSkills.Select(skill => skill.skillName));
                Debug.Log($"활성화된 스킬: {skillNames}");
            }
            else
            {
                Debug.Log("활성화된 스킬이 없습니다.");
            }

        }
    }

    public void EditorFunctionRankUp()
    {
        RankUpCharacter(0);
        foreach (var character in characters)
        {
            Debug.Log($"캐릭터 이름: {character.charcterName}\n 캐릭터 랭크: {character.currentRank}");
            Debug.Log($"공격력: {character.GetCurrentAttack()}\n 체력 : {character.GetCurrentHealth()}");
            List<Skill> activeSkills = character.GetActiveSkills();

            if (activeSkills.Count > 0)
            {
                string skillNames = string.Join(", ", activeSkills.Select(skill => skill.skillName));
                Debug.Log($"활성화된 스킬: {skillNames}");
            }
            else
            {
                Debug.Log("활성화된 스킬이 없습니다.");
            }
        }
    }

#endif
}
