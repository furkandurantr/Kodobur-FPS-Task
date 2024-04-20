using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public float health = 50f;
    float maxHealth;
    public GameObject myObj;
    public Transform camPos;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        HPRatio();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = new Vector3(camPos.position.x - transform.position.x, 0, camPos.position.z - transform.position.z);
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDirection, 1000f, 0.0f);
        slider.transform.rotation = Quaternion.LookRotation(newDir);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Navigation myNav = myObj.transform.GetComponent<Navigation>();
        myNav.Damaged();
        slider.gameObject.SetActive(true);
        HPRatio();
        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

    void HPRatio()
    {
        slider.value = health / maxHealth;
    }
}
