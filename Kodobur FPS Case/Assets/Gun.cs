using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    public LayerMask ignoreLayer;
    public float damage = 10f;
    public float range = 100f;
    public int piercing = 0;
    public int maxAmmo = 10;
    public int curAmmo;
    public Camera fpsCam;
    [SerializeField] TextMeshProUGUI ammoText;
    // Start is called before the first frame update
    void Start()
    {
        curAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && curAmmo > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        curAmmo -= 1;
        ammoText.text = curAmmo + "/" + maxAmmo;
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
