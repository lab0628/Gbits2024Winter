using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadRegion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = other.gameObject;
        if(go.layer == LayerMask.NameToLayer("Player"))
        {
            go.GetComponent<Player>().Dead();
        }
        
        Destroy(go);
    }
}
