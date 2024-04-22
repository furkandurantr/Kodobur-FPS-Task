using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoLoot : MonoBehaviour
{
    public int ammoNumber = 3;
    public GameObject spawner;
    [SerializeField] TextMeshPro ammoText;
    public Transform camPos;
    // Start is called before the first frame update
    void Start()
    {
        ammoText.text = ammoNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = new Vector3(transform.position.x- camPos.position.x, 0, transform.position.z- camPos.position.z);
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDirection, 1000f, 0.0f);
        ammoText.transform.rotation = Quaternion.LookRotation(newDir);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var gun = other.transform.GetComponent<Gun>();
            float playerAmmoDiff = gun.maxAmmo - gun.curAmmo;
            float diffVal = MathF.Min(ammoNumber, playerAmmoDiff);
            if (diffVal > 0)
            {
                gun.curAmmo += (int)diffVal;
                ammoNumber -= (int)diffVal;
                ammoText.text = ammoNumber.ToString();
                if (ammoNumber <= 0)
                {
                   spawner.GetComponent<AmmoSpawner>().curAmmo -= 1;
                 Destroy(gameObject);
                }
                gun.Reload();
            }
        }
    }
}
