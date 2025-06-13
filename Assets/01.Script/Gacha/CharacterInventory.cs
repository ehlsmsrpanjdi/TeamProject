using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterInventory : MonoBehaviour
{
    public static CharacterInventory Instance;

    public List<CharacterInstance> ownedCharacters = new List<CharacterInstance>();


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

    private void AddCharacter (DrawResult drawResult)
    {
        if (drawResult.character == null)
            return;

        CharacterInstance newInstance = new CharacterInstance(drawResult.character);

        ownedCharacters.Add(newInstance);
    }

    public void CheckInventory()
    {
        foreach (var character in ownedCharacters)
        {
            Debug.Log($"이름: {character.characterData.characterName}, 랭크: {character.currentRank}, 공격력: {character.CurrentAttack}");
        }
    }

    public List<CharacterInstance> GetOwnedCharacters()
    {
        return new List<CharacterInstance>(ownedCharacters);
    }

    private void OnDestroy()
    {
        GachaManager.Instance.OnCharacterDraw -= AddCharacter; // 오브젝트가 사라질 때 이벤트 구독 해지
    }
}
