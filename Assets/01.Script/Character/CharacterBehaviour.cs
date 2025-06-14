using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    [SerializeField] private float attackRange; //공격 사거리
    [SerializeField] private float attackDelay; //공격 딜레이
    [SerializeField] private float duration; //피격 딜레이

    private float lastAttackTime;

    private CharacterInstance charInstance;

    public void Init(CharacterInstance data)
    {
        charInstance = data;
        var usableSkills = charInstance.GetActiveSkills(); // 현재 활성화 되어있는 스킬만 사용 가능한 스킬에 들어감.
    }
    void Update()
    {
        if (Time.time - lastAttackTime >= attackDelay)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }
    //공격
    void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Zombie")); //캐릭터 기준 콜라이더 확인.

        if (hitEnemies.Length == 0) return; // 공격범위 내에 몬스터 없으면 땡.

        Collider closestEnemy = null; // 콜라이더에 몬스터 없음.
        float closestDistance = Mathf.Infinity;

        foreach (var enemyCollider in hitEnemies) // 가장 가까운 적을 우선적으로 공격
        {
            float distance = Vector3.Distance(transform.position, enemyCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyCollider;
            }
        }

        if (closestEnemy != null)
        {
            IDamageable target = closestEnemy.GetComponent<IDamageable>();
            if (target != null)
            {
                float damage = charInstance != null ? charInstance.GetCurrentAttack() : 10; // 기본 데미지 10f로 fallback
                target.TakeDamage((int)damage, transform.position, knockbackForce:1);
                Debug.Log($"{charInstance.charcterName}이(가) 공격! 데미지: {damage}");
            }
        }
    }

    //공격 범위 표시 위한 기즈모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    //스킬사용 (액티브로 하기로 했음)
    public void UseSkill()
    {

    }

    //죽음. 바리게이트와 캐릭터의 체력을 연결 시킬 예정.
    void Die()
    {

    }

    //Interface 처리
    public void TakeDamage(float damage)
    {

    }

}
