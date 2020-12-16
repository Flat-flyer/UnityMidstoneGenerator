using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICreateTargettedAttack : AIBehaviour
{
    public bool PlayerDetectedAttack = false;
    [SerializeField]
    private float AttackDelay = 20f;
    private Collider Player;
    [SerializeField]
    private GameObject TargetAttack;
    private float TargetAttackCount;
    [SerializeField]
    private float TargetAttackTotal = 1;
    [SerializeField]
    private float TargetAttackDelay = 1f;
    private bool TargetAttackInitialized;
    private bool TargetAttackActive;

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
        TargetAttackInitialized = true;
        TimePassed = 0;
    }

    private void Update()
    {
        //checks if the amount of target attacks set off is below the total it can do in a sequence, performs this until it hits the total.
        if (TargetAttackCount < TargetAttackTotal && TargetAttackInitialized == true && TargetAttackActive == false)
        {
            StartCoroutine(CreateTargetAttack(TargetAttackDelay));
            TargetAttackCount = TargetAttackCount + 1;
            TargetAttackActive = true;
        }
        if (TargetAttackCount >= TargetAttackTotal)
        {
            TargetAttackInitialized = false;
            TargetAttackCount = 0;
        }
    }

    IEnumerator CreateTargetAttack (float time)
    {
        yield return new WaitForSeconds(time);
        Instantiate(TargetAttack, this.transform.position, Quaternion.identity);
        TargetAttackActive = false;
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
