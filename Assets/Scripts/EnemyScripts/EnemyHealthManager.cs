using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{

    public float EnemyHealth = 100f;
    private BasicAttackData attackData;
    public bool IsHit = false;
    public bool HasDied = false;
    private List<GameObject> attackHits = new List<GameObject>();
    private List<EnemyHit> Parts = new List<EnemyHit>();
    public bool DisableColliders;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (attackHits.Count >= 1)
        {
            IsHit = true;
        }
        if (EnemyHealth <= 0f)
        {
            HasDied = true;
            Destroy(gameObject);
        }
        if (attackHits.Count >= 2)
        {
            attackHits.Remove(attackHits.Last());
            Parts.Remove(Parts.Last());
        }
        if (attackHits.Count == 1)
        {
            TakeDamage(attackHits[0], Parts[0]);
            attackHits.Remove(attackHits[0]);
            Parts.Remove(Parts[0]);
            IsHit = false;
        }
    }

    public void AddAtkToList (GameObject Attack)
    {
        attackHits.Add(Attack);
    }

    public void AddPartToList (EnemyHit Part)
    {
        Parts.Add(Part);
    }

    public void TakeDamage(GameObject HitData, EnemyHit PartHit)
    {
        attackData = HitData.GetComponent<BasicAttackData>();
        if (PartHit.BreakablePart == true)
        {
            PartHit.PartHealth = PartHit.PartHealth - (attackData.DamageValue * PartHit.PartDamageMulti);
            Debug.Log("Part Health: " + PartHit.PartHealth);
        }
        EnemyHealth = EnemyHealth - (attackData.DamageValue * PartHit.PartDamageMulti);
        Debug.Log("Enemy Health: " + EnemyHealth);
    }
}

