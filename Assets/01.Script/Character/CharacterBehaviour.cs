using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    public CharacterInstance chrInstance;

    public void Start()
    {
        chrInstance = GetComponent<CharacterInstance>();
    }

    public void Update()
    {

    }

    void SpawnCharacter()
    {

    }

    //공격
    void Attack()
    {

    }

    //스킬사용 (액티브로 하기로 했음)
    public void UseSkill()
    {

    }

    //죽음 (실제로는 죽지 않지만)
    void Die()
    {

    }

    //Interface 처리
    public void TakeDamage(float damage)
    {

    }

}
