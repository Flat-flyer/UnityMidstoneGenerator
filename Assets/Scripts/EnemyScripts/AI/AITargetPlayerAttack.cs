using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITargetPlayerAttack : MonoBehaviour
{
    private Collider Player;
    [SerializeField]
    private Collider TargetAttackCol;
    [SerializeField]
    private GameObject TargetAttackMark;
    [SerializeField]
    private float AttackDelay = 20f;
    [SerializeField]
    private float AttackStartup = 3;
    private Vector3 TargetLocation;
    [SerializeField]
    private bool CurrentlyActive = false;


 
    void LateUpdate()
    {
        TargetLocation = new Vector3(Player.transform.position.x, Player.transform.position.y - 0.5f, Player.transform.position.z);
        if (CurrentlyActive == false)
        {
            StartCoroutine(MarkArea(0.5f));
            CurrentlyActive = true;
        }

    }

    IEnumerator MarkArea(float time)
    {
        yield return new WaitForSeconds(time);
        this.transform.position = TargetLocation;
        TargetAttackMark.SetActive(true);
        StartCoroutine(EnableAttackCollider(AttackStartup));

    }

    IEnumerator EnableAttackCollider(float time)
    {
        yield return new WaitForSeconds(time);

        TargetAttackCol.enabled = true;
        TargetAttackMark.SetActive(false);
        StartCoroutine(DisableAttackCollider(0.2f));
    }

    IEnumerator DisableAttackCollider(float time)
    {
        yield return new WaitForSeconds(time);

        TargetAttackCol.enabled = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            Player = other;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            Player = other;
        }
    }
}
