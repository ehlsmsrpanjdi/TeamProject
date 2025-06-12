using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterInventory : MonoBehaviour
{
    public static CharacterInventory Instance;

    public List<DrawResult> ownedCharacters = new List<DrawResult>(); // DrawResult 구조체로 구성된 새로운 리스트 선언, 뽑은 캐릭터가 저장됨.

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GachaManager.Instance.OnCharacterDraw += AddCharacter; // OnCharacterDraw 이벤트 구독
    }

    private void AddCharacter (DrawResult newCharacter)
    {
        ownedCharacters.Add(newCharacter); // DrawCharacter로 뽑힌 result를 이벤트를 통해 전달받고 newCharacter 매개변수를 통해 ownedCharacters 리스트에 추가
    }
    public void CheckInventory()
    {
        Debug.Log($"보유캐릭터 {ownedCharacters.Count}");
        foreach (var characters in ownedCharacters)
        {
            // 보유한 모든 캐릭터의 정보를 콘솔에 출력
            Debug.Log($"{characters.character.Name}, 등급: {characters.rank}");
        }
    }

    /*public DrawResult GetOwnedCharacter(int index)
    {
        if (index >= 0 && index < ownedCharacters.Count)
        {
            return ownedCharacters[index];
        }
        return default(DrawResult);
    }*/

    public List<DrawResult> GetOwnedCharacters()
    {
        return new List<DrawResult>(ownedCharacters);
    }

    private void OnDestroy()
    {
        GachaManager.Instance.OnCharacterDraw -= AddCharacter; // 오브젝트가 사라질 때 이벤트 구독 해지
    }
}
