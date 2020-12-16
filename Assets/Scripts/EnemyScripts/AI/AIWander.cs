using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWander : AIBehaviour
{

    public float wanderDistance = 10f;
    private GameObject Enemy;
    public float speed = 1f;
    [SerializeField]
    private Vector3 newPosition;

    public void Start()
    {
        Enemy = this.transform.parent.gameObject;
        newPosition = Enemy.transform.position;
    }


    public override float GetWeight()
    {
        return 1;
    }

    public void Update()
    {
        newPosition = new Vector3(newPosition.x, Enemy.transform.position.y, newPosition.z);
    }

    public override void Execute()
    {
        if (Enemy.transform.position == newPosition)
        {
            newPosition = new Vector3(Random.Range(Enemy.transform.position.x, Enemy.transform.position.x+wanderDistance), Enemy.transform.position.y, Random.Range(Enemy.transform.position.z, Enemy.transform.position.z + wanderDistance));
        }
        float step = speed * Time.deltaTime;
        Enemy.transform.LookAt(newPosition);
        Enemy.transform.position = Vector3.MoveTowards(Enemy.transform.position, newPosition, step);
    }
}
