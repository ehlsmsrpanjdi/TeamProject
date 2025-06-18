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
    public bool isMoving;

    List<Skill> GainSkill;

    //public float currentCooldown = 0f;


    public void Init(CharacterInstance data, Transform destination)
    {
        charInstance = data;
        charInstance.SetBehaviour(this);
        GainSkill = charInstance.GetActiveSkills(); // 현재 활성화 되어있는 스킬만 사용 가능한 스킬에 들어감.

        animator = GetComponent<Animator>();
        animController = new CharAnimController(animator);

        isMoving = true;
        StartCoroutine(MoveSetPosition(destination));

        charInstance.SetSkillCooltime();

        //animController.SetAttack(true);
    }

    private IEnumerator MoveSetPosition(Transform setPosition)
    {
        yield return CoroutineHelper.GetTime(1f);
        animController.Moving(true);
        float speed = 3f;
        while (Vector3.Distance(transform.position, setPosition.position) > 0.1f)
        {
            // 회전 처리
            Vector3 dir = (setPosition.position - transform.position).normalized;
            if (dir != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
            }

            // 이동
            transform.position = Vector3.MoveTowards(transform.position, setPosition.position, speed * Time.deltaTime);
            yield return null;
        }
        animController.Moving(false);
        animController.SetAttack(true);
    }


    void Update()
    {
        if (isMoving == false)
            return;

        if (Time.time - lastAttackTime >= attackDelay)
        {
            animController.Attacking(CheckEnemyInRange());

            lastAttackTime = Time.time;

        }

        ReduceCooltime();

    }

    /// <summary>
    /// 가장 가까운 적 반환 (없으면 null)
    /// </summary>
    private Collider GetClosestEnemy()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Zombie"));
        Collider closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in hitEnemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    /// <summary>
    /// 범위 내 적이 있는지 체크
    /// </summary>
    private bool CheckEnemyInRange()
    {
        return GetClosestEnemy() != null;
    }

    /// <summary>
    /// 가장 가까운 적에게 공격
    /// </summary>
    public void Attack()
    {
        Collider closestEnemy = GetClosestEnemy();
        if (closestEnemy == null)
        {
            isAttacking = false;
            return;
        }


        //if (State.Die == closestEnemy.GetComponent<ZombieAI>().currentState) // 타겟 몬스터의 상태 체크
        //{
        //    return;
        //}
        isAttacking = true;
        LookRotation(closestEnemy.transform);

        IDamageable target = closestEnemy.GetComponent<IDamageable>();
        if (target != null && isAttacking == true)
        {

            float damage = charInstance != null ? charInstance.GetCurrentAttack() : 10;
            target.TakeDamage((int)damage, transform.position, knockbackForce: 1);
            //Debug.Log($"{charInstance.charcterName} 공격 데미지: {damage}");
        }
        isAttacking = true;
    }

    /// <summary>
    /// 적 방향으로 회전
    /// </summary>
    private void LookRotation(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation; // 부드럽게 회전
        }
    }



    //스킬사용(액티브로 하기로 했음)
    public bool UseSkill(int skillIndex)
    {

        if (skillIndex < 0 || skillIndex >= charInstance.GetActiveSkills().Count)
        {
            return false;
        }

        Skill skill = GainSkill[skillIndex];

        if (skill.currentCooldown > 0) return false;

        if (!skill.isActive)
        {
            return false;
        }
        skill.UseSkill(skill.skillKey, transform.position);

        return true;
    }

    public void ReduceCooltime()
    {

        foreach(var time in GainSkill)
        {

            time.currentCooldown = Mathf.Max(0f, time.currentCooldown - Time.deltaTime);
        }

    }

    public float GetSkillCooltime(int index)
    {
        return GainSkill[index].currentCooldown;
    }




    public void Die()
    {
        isAttacking = false;
        isMoving = true;

        GameObject target = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (target != null)
        {
            StartCoroutine(MoveToSpawn(target.transform.position));
        }
    }

    private IEnumerator MoveToSpawn(Vector3 position)
    {
        animController.Moving(true);
        float speed = 3f;
        while (Vector3.Distance(transform.position, position) > 0.1f)
        {
            Vector3 dir = (position - transform.position).normalized;
            if (dir != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
            }

            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);

            yield return null;
        }

        animController.Moving(false);
        isMoving = false;
    }

    //Interface 처리
    public void TakeDamage(float damage)
    {

    }

}
