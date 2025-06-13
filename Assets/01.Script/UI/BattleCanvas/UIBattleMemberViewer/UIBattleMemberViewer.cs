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
        Instantiate(memberPrefab, transform);
    }

}
