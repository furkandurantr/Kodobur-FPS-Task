using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthLoot : MonoBehaviour
{
    public int heal = 3;
    public GameObject spawner;
    [SerializeField] TextMeshPro healText;
    public Transform camPos;
    // Start is called before the first frame update
    void Start()
    {
        healText.text = heal.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = new Vector3(transform.position.x- camPos.position.x, 0, transform.position.z- camPos.position.z);
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDirection, 1000f, 0.0f);
        healText.transform.rotation = Quaternion.LookRotation(newDir);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var playerHP = other.transform.GetComponent<PlayerHP>();

            spawner.GetComponent<HealthSpawner>().curHealth -= 1;
            Destroy(gameObject);
            playerHP.Heal(heal);
        }
    }
}
