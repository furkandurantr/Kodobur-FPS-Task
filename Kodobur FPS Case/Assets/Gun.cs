using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public LayerMask ignoreLayer;
    public float damage = 10f;
    public float range = 100f;
    public int piercing = 0;
    public int maxAmmo = 0;
    public int curAmmo = 0;
    public Camera fpsCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(fpsCam.transform.position, fpsCam.transform.forward, range, ~ignoreLayer);

        System.Array.Sort(hits, (a, b) => (a.distance.CompareTo(b.distance)));

        for(int i = 0; i < hits.Length; i += 1)
        {
            if (i < piercing + 1)
            {
                    Target target = hits[i].transform.GetComponent<Target>();
                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }
            }
            else
            {
                break;
            }
        }
    }
}
