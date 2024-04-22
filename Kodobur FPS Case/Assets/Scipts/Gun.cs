using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Gun : MonoBehaviour
{
    public LayerMask ignoreLayer;
    public float damage = 10f;
    public float range = 100f;
    public int piercing = 0;
    public int maxAmmo = 10;
    public int curAmmo;
    public int ammoOnKill = 0;
    public Camera fpsCam;
    public GameObject particle;
    [SerializeField] TextMeshProUGUI ammoText;
    // Start is called before the first frame update
    void Start()
    {
        curAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && curAmmo > 0 && Time.timeScale != 0)
        {
            Shoot();
        }
    }
    public void Reload()
    {
        ammoText.text = curAmmo + "/" + maxAmmo;
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
                    Instantiate(particle, hits[i].point, Quaternion.LookRotation(hits[i].normal));
            }
            else
            {
                break;
            }
        }
    }
    public void AmmoOnKill()
    {
        curAmmo += (int)MathF.Min(ammoOnKill, maxAmmo - curAmmo);
        Reload();
    }
}
