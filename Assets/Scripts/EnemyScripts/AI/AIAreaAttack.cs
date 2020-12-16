using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAreaAttack : AIBehaviour
{
    public bool PlayerDetectedAttack = false;
    private Collider Player;
    [SerializeField]
    private Collider AreaAttackCol;
    [SerializeField]
    private GameObject AreaAttack;
    [SerializeField]
    private float AttackDelay = 20f;
    [SerializeField]
    private float AttackStartup = 3;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private string animationName;

    public override float GetWeight()
    {
        //checks if player is in range and enough time has passed for the attack
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
    //gets animator for the object
    private void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    public override void Execute()
    {
        CurrentlyAttacking = true;
        TimePassed = 0;
        StartCoroutine(MarkArea(0.5f));

    }

    //creates marker for area attack will hit in
    IEnumerator MarkArea (float time)
    {
        yield return new WaitForSeconds(time);
        AreaAttack.SetActive(true);
        animator.Play(animationName);
        StartCoroutine(EnableAttackCollider(AttackStartup));

    }
    //performs attack function
    IEnumerator EnableAttackCollider(float time)
    {
        yield return new WaitForSeconds(time);

        AreaAttackCol.enabled = true;
        AreaAttack.SetActive(false);
        StartCoroutine(DisableAttackCollider(0.2f));
    }

    IEnumerator DisableAttackCollider(float time)
    {
        yield return new WaitForSeconds(time);

        AreaAttackCol.enabled = false;
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
