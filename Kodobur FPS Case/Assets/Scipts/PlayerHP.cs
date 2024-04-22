using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    public float HP = 100;
    public float maxHP = 100;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        HPRatio();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        HPRatio();
        if (HP <= 0)
        {
            Cursor.lockState = CursorLockMode.Confined;
            SceneManager.LoadScene("Restart");
        }
    }

    void HPRatio()
    {
        slider.value = HP / maxHP;
    }

    public void Heal(float heal)
    {
        HP += MathF.Min(heal, maxHP - HP);
        HPRatio();
    }
}
