using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    [SerializeField]
    private PlayerController Player;

    private void Start()
    {
        Player = GetComponentInParent<PlayerController>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target") == true)
        {
            Player.ThrustCollided = true;
        }
    }
}
