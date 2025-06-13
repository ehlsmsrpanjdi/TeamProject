using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    [SerializeField] private float attackRange; //공격 사거리
    [SerializeField] private float attackDelay; //공격 딜레이
    [SerializeField] private float duration; //피격 딜레이

    //공격
    void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Zombie")); //캐릭터 기준 콜라이더 확인.

        if (hitEnemies.Length == 0) return; // 공격범위 내에 몬스터 없으면 땡.

        Collider closestEnemy = null; // 콜라이더에 몬스터 없음.
        float closestDistance = Mathf.Infinity;

        foreach (var enemyCollider in hitEnemies)
        {
            float distance = Vector3.Distance(transform.position, enemyCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyCollider;
            }
        }

    //     if (closestEnemy != null)
    //     {
    //         ZombieStatHandler enemy = closestEnemy.GetComponent<ZombieStatHandler>(); //EnemyBehaviour 는 임시 클래스.
    //         if (enemy != null)
    //         {
    //             enemy.TakeDamage(CharacterData.instance.); //피격 인터페이스 사용 예정
    //             Debug.Log($"{_charInstance.characterData.characterName}이(가) {enemy.name}을(를) 공격!");
    //         }
    //     }
    //
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
