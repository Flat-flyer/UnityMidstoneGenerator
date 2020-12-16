using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRangedAttack : AIBehaviour
{

    public bool PlayerDetectedAttack = false;
    private Collider Player;
    [SerializeField]
    private GameObject RangedAttack;
    [SerializeField]
    private float AttackDelay = 2f;
    [SerializeField]
    private float BulletSpeed = 4f;
    private GameObject Enemy;
    private Rigidbody Bullet;

    public override float GetWeight()
    {
        if (PlayerDetectedAttack == false)
        {
            return 0;
        }

        if (TimePassed < AttackDelay)
        {
            return 0;
        }
        else
        {
            TimePassed = 0;
            return 1;
        }
    }

    public override void Execute()
    {
        Enemy = this.transform.parent.gameObject;
        CurrentlyAttacking = true;
        StartCoroutine(EnableRangedAttack(2f));
    }

    IEnumerator EnableRangedAttack (float time)
    {
        yield return new WaitForSeconds(time);

        var bullet = Instantiate(RangedAttack, Enemy.transform.position, Enemy.transform.rotation);
        Bullet = bullet.GetComponent<Rigidbody>();
        Bullet.velocity = Enemy.transform.forward * BulletSpeed;
        CurrentlyAttacking = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            PlayerDetectedAttack = true;
            Player = other;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            PlayerDetectedAttack = false;
        }
    }
}
