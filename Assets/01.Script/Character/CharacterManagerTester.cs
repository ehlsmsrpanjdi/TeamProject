using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManagerTester : MonoBehaviour
{
    void Start()
    {

        CharacterManager.instance.CreateCharacter(1001);
        CharacterManager.instance.CreateCharacter(1002);

        CharacterManager.instance.SelectParticipate(1001);
        CharacterManager.instance.SelectParticipate(1002);

        CharacterManager.instance.SpawnParticipateCharacters(); // 스폰 호출
    }

}
