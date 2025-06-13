using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManagerTester : MonoBehaviour
{
    void Start()
    {
        // 예: 1001, 1002가 존재하는 캐릭터 키라고 가정
        CharacterManager manager = CharacterManager.instance;

        manager.CreateCharacter(1001);
        manager.CreateCharacter(1002);

        manager.SelectParticipate(1001);
        manager.SelectParticipate(1002);

        manager.SpawnParticipateCharacters(); // 스폰 호출
    }

}
