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
    public float attackCooldown = 0.5f;
    public float attackSpeed = 1;
    public int curAmmo;
    public float lifeSteal = 0f;
    public int ammoOnKill = 0;
    public Camera fpsCam;
    public GameObject particle;
    float attackCd = 0;
    [SerializeField] TextMeshProUGUI ammoText;
    // Start is called before the first frame update
    void Start()
    {
        curAmmo = maxAmmo;
        Reload();
    }

    // Update is called once per frame
    void Update()
    {

        if (attackCooldown > 0)
        {
            attackCd += Time.deltaTime * attackSpeed;
            if (Input.GetButton("Fire1") && curAmmo > 0 && Time.timeScale != 0 && attackCd >= attackCooldown)
            {
                attackCd = 0;
                Shoot();
            }
        }
        else if (Input.GetButtonDown("Fire1") && curAmmo > 0 && Time.timeScale != 0)
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
                    gameObject.GetComponent<PlayerHP>().Heal(damage/100*lifeSteal);
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
