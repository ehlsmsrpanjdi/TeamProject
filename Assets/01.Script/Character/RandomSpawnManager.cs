using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomSpawnManager : MonoBehaviour
{
    public CharacterInstance chrInstance;
    public CharacterDataSO[] chrData;
    public GameObject chrPrefab;

    private void Awake()
    {


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCharacter();
            InfoCharacter();
        }
    }

    void SpawnCharacter()
    {
        CharacterDataSO data = chrData[Random.Range(0, chrData.Length)];
        GameObject character = Instantiate(chrPrefab);

        CharacterBehaviour behaviour = character.GetComponent<CharacterBehaviour>();
        chrInstance = new CharacterInstance(data);
    }

    void InfoCharacter()
    {
        Debug.Log($"이름 : {chrInstance.characterData.characterName}");
        Debug.Log($"랭크 : {chrInstance.characterData.startRank}");
        Debug.Log($"보유 스킬 : {string.Join(", ", chrInstance.CurrentSkills.Select(skill => skill.skillName))}");
        Debug.Log($"공격력 : {chrInstance.CurrentAttack}");
        Debug.Log($"체력 : {chrInstance.CurrentHP}");

    }
}
