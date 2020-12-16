using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossLegBreak : MonoBehaviour
{
    [SerializeField]
    private EnemyHit BreakablePart;
    [SerializeField]
    private GameObject BossPart;
    [SerializeField]
    private Collider BrokenLegCol;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BreakablePart.PartHealth <= 0)
        {
            BossPart.GetComponent<Renderer>().enabled = false;
            BossPart.GetComponent<Collider>().enabled = false;
            this.GetComponent<Collider>().enabled = false;
            BrokenLegCol.enabled = true;

        }
    }
}
