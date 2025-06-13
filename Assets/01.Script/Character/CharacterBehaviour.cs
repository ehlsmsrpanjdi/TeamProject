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
    private void Start()
    {
        // 예시로 CharacterManager에서 생성된 캐릭터 할당
        charInstance = CharacterManager.instance.GetCharacter(1001); // key 1001번 캐릭터 가져오기
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

        foreach (var enemyCollider in hitEnemies)
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
            ZombieStatHandler enemy = closestEnemy.GetComponent<ZombieStatHandler>();
            if (enemy != null)
            {
                int damage = charInstance != null ? charInstance.GetCurrentAttack() : 10; // 기본 데미지 10f로 fallback
                enemy.TakeDamage(damage);
                Debug.Log($"{charInstance.charcterName}이(가) {enemy.name}을(를) 공격! 데미지: {damage}");
            }
        }

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
