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

    public void AddMember()
    {
        GameObject battleMemberUI = Instantiate(memberPrefab, transform);
        BattleMember component = battleMemberUI.GetComponent<BattleMember>();

        battleMembers.Add(component);
    }

}
