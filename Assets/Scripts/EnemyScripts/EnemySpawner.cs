using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject[] Enemies;
    private GameObject SelectedEnemy;
    private bool PlayerIsInRange = false;
    private bool EnemySpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerIsInRange == true && EnemySpawned == false)
        {
            SelectedEnemy = Enemies[Random.Range(0, Enemies.Length)];
            Instantiate(SelectedEnemy, this.gameObject.transform.position, this.gameObject.transform.rotation);
            EnemySpawned = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            PlayerIsInRange = true;
        }
    }
}
