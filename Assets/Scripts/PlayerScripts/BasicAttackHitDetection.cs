using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackHitDetection : MonoBehaviour
{
    public bool BasicHitCollision;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target") == true)
        {
            BasicHitCollision = true;
        }
    }
}
