using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoLoot : MonoBehaviour
{
    public int ammoNumber = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
