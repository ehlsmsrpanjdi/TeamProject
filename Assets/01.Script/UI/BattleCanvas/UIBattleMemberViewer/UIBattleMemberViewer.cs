using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIBattleMemberViewer : UIBase
{
    [SerializeField] List<BattleMember> battleMembers;

    [SerializeField] GameObject memberPrefab;

    private void Reset()
    {
        memberPrefab = Resources.Load<GameObject>("UI/BattleMember");
    }

    public void OnEnable()
    {
        List<CharacterInstance> characterList = CharacterManager.Instance.GetParticipateCharacters();
        foreach (CharacterInstance character in characterList)
        {
            AddMember(character);
        }
    }

    public void OnDisable()
    {
        foreach(BattleMember member in battleMembers)
        {
            Destroy(member.gameObject);
        }
        battleMembers.Clear();
    }

    public void AddMember(CharacterInstance _instance)
    {
        GameObject battleMemberUI = Instantiate(memberPrefab, transform);
        BattleMember component = battleMemberUI.GetComponent<BattleMember>();
        component.OnMemberSet(_instance);
        battleMembers.Add(component);
    }

}
