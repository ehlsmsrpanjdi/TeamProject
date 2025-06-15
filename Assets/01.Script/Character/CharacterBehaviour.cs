using System;
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

    private CharAnimController animController;
    public Animator animator;

    public bool isAttacking;

    public void Init(CharacterInstance data)
    {
        charInstance = data;
        var usableSkills = charInstance.GetActiveSkills(); // 현재 활성화 되어있는 스킬만 사용 가능한 스킬에 들어감.

        animator = GetComponent<Animator>();
        animController = new CharAnimController(animator);
        

        animController.SetAttack(true);
    }
    void Update()
    {
        if (Time.time - lastAttackTime >= attackDelay)
        {
            animController.Attacking(CheckEnemyInRange());
            lastAttackTime = Time.time;

        }
    }

    /// <summary>
    /// 범위 내에 적이 들어왔는지 확인해서 공격 애니메이션에 전달하기 위함.
    /// </summary>
    bool CheckEnemyInRange()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Zombie")); //캐릭터 기준 콜라이더 확인.
        if (hitEnemies.Length > 0)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
        return isAttacking;
    }
    public void Attack()
    {
        if (!CheckEnemyInRange()) return;
        
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
            Vector3 directionToEnemy = closestEnemy.transform.position - transform.position;
            directionToEnemy.x  = 0f;
            directionToEnemy.z = 0f;
            if(directionToEnemy != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(transform.position, directionToEnemy);
                transform.rotation = lookRotation;
            }

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
