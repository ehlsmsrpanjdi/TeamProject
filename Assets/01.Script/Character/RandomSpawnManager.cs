using System.Collections;
using System.Collections.Generic;
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
        Debug.Log($"이름 : {chrInstance.characterData.name}");
        Debug.Log($"랭크 : {chrInstance.currentRank}");
        Debug.Log($"보유 스킬 : {chrInstance.CurrentSkills}");
        Debug.Log($"공격력 : {chrInstance.CurrentAttack}");
        Debug.Log($"체력 : {chrInstance.CurrentHP}");

    }
}
