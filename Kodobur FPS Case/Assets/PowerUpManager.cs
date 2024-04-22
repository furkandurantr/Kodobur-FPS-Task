using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Güçlendirme verilerini saklayacak sınıf
[Serializable]
public class PowerUpData
{
    public string powerUpName;
    public int maxLevel;
    public int cost;
    public List<PowerUpEffect> effects = new List<PowerUpEffect>();
}

// Güçlendirme etkilerini saklayacak sınıf
[Serializable]
public class PowerUpEffect
{
    public string effectName;
    public float value;
}
public class PowerUpManager : MonoBehaviour
{
    public List<PowerUpData> powerUps = new List<PowerUpData>();
    // Start is called before the first frame update
    void Start()
    {
    }

    void CreateSamplePowerUps()
    {
        // Saldırı Gücü güçlendirme verisi oluştur
        PowerUpData attackPowerUp = new PowerUpData();
        attackPowerUp.powerUpName = "Attack Power";
        attackPowerUp.maxLevel = 5;
        attackPowerUp.effects.Add(new PowerUpEffect { effectName = "Damage Increase", value = 10 });
        attackPowerUp.effects.Add(new PowerUpEffect { effectName = "Attack Speed Increase", value = 0.5f });
        powerUps.Add(attackPowerUp);

        // Hareket Hızı güçlendirme verisi oluştur
        PowerUpData movementSpeedUp = new PowerUpData();
        movementSpeedUp.powerUpName = "Movement Speed";
        movementSpeedUp.maxLevel = 3;
        movementSpeedUp.effects.Add(new PowerUpEffect { effectName = "Speed Increase", value = 2 });
        powerUps.Add(movementSpeedUp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
