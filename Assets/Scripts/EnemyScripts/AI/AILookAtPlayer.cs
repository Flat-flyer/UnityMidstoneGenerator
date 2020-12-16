using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is used for boss/stationary targets to just do a simple direction change to face the player
public class AILookAtPlayer : AIBehaviour
{
    public bool PlayerDetected = false;
    private Collider Player;
    private GameObject Enemy;
    private Vector3 rotation = Vector3.zero;
    private Vector3 TargetLocation;
    [SerializeField]
    private float AttackDelay = 5f;
    [SerializeField]
    private float TurnSpeed = 0.5f;
    private bool CanRotate = false;
    private Quaternion TargetRotation;

    public override float GetWeight()
    {
        if (PlayerDetected == true && TimePassed > AttackDelay)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public override void Execute()
    {
        Enemy = this.transform.parent.gameObject;
        rotation = new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
        //CanRotate = true;
        Enemy.transform.LookAt(rotation);
        TimePassed = 0;
    }

    private void FixedUpdate()
    {
        //if (CanRotate == true)
        //{
         //   Enemy.transform.rotation = Quaternion.Slerp(Enemy.transform.rotation, Quaternion.LookRotation(rotation), Time.deltaTime * TurnSpeed);
        //}
       // if (Enemy.transform.rotation == Quaternion.LookRotation(rotation))
        //{
        //    CanRotate = false;
       // }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            PlayerDetected = true;
            Player = other;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            PlayerDetected = false;
        }
    }
}
