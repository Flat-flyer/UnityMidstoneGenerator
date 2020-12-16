using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCheck : MonoBehaviour
{
    [SerializeField]
    private PlayerController Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack") == true)
        {
            Player.GuardedAttack = true;
        }
    }
}
