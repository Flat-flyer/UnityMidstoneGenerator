using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItemPull : MonoBehaviour
{

    [SerializeField]
    private float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, other.gameObject.transform.position, step);
        }
    }
}
