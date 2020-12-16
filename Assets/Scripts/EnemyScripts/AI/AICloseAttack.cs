using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICloseAttack : AIBehaviour
{
    public bool PlayerDetectedAttack = false;
    private Collider Player;
    [SerializeField]
    private Collider CloseAttack;
    [SerializeField]
    private float AttackDelay = 1f;

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
            return 1;
    }

    public override void Execute()
    {
        CurrentlyAttacking = true;
        StartCoroutine(EnableAttackCollider(3f));
        
    }

    IEnumerator EnableAttackCollider(float time)
    {
        yield return new WaitForSeconds(time);

        CloseAttack.enabled = true;
        StartCoroutine(DisableAttackCollider(1f));
    }

    IEnumerator DisableAttackCollider(float time)
    {
        yield return new WaitForSeconds(time);

        CloseAttack.enabled = false;
        CurrentlyAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            PlayerDetectedAttack = true;
            Player = other;
        }
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
