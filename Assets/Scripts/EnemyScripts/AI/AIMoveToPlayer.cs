using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveToPlayer : AIBehaviour
{
    public bool PlayerDetected = false;
    private Collider Player;
    private GameObject Enemy;
    public float speed = 1f;
    private Vector3 rotation = Vector3.zero;
    private Vector3 TargetLocation;

    public override float GetWeight()
    {
        if (PlayerDetected == true)
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
        float step = speed * Time.deltaTime;
        Enemy = this.transform.parent.gameObject;
        TargetLocation = new Vector3(Player.transform.position.x, Enemy.transform.position.y, Player.transform.position.z );
        Enemy.transform.position = Vector3.MoveTowards(Enemy.transform.position, TargetLocation, step);
        rotation = new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
        Enemy.transform.LookAt(rotation);
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
