using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Explosion(Vector3 direction, float range, float skillDamage)
    {
        //방향 받아서 날아가는거 처리
        //몬스터피격 처리
        float throwForce = 10f; //던지는 힘
        rb.AddForce(direction.normalized * throwForce, ForceMode.Impulse);

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Zombie"));

    }
}
