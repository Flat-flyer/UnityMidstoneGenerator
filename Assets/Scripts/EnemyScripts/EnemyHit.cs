using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{

    public GameObject AttackData;
    [SerializeField]
    private Collider HitCollider;
    [SerializeField]
    private EnemyHealthManager HealthManager;
    public bool BreakablePart;
    public float PartHealth = 1;
    public float PartDamageMulti = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack") == true && HealthManager.IsHit == false)
        {
            Debug.Log("Enemy Hit");
            AttackData = other.gameObject;
            HealthManager.AddAtkToList(AttackData);
            HealthManager.AddPartToList(this);
        }
    }
}
