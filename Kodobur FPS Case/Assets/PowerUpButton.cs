using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpButton : MonoBehaviour
{
    public PowerUpData powerUpData; // Güçlendirme verilerini tutacak referans
    public TextMeshProUGUI myLabel;
    public TMP_Text buttonText; // Buton metni için referans
    private int currentLevel = 0; // Mevcut güçlendirme seviyesi

    public GameObject player;
    public GameObject ammoSpawner;

    // Buton oluşturulduğunda çalışacak fonksiyon
    void Start()
    {
        // Buton metnini güçlendirme adı ve seviye bilgisiyle güncelle
        player = GameObject.FindGameObjectWithTag("Player");
        ammoSpawner = GameObject.FindGameObjectWithTag("AmmoSpawner");
        UpdateButtonText();
    }

    // Butona tıklandığında çalışacak fonksiyon
    public void ButtonPressed()
    {
        if (currentLevel < powerUpData.maxLevel && powerUpData.cost <= player.GetComponent<PlayerLevel>().curPoint)
        {
            var playerLevel = player.GetComponent<PlayerLevel>();
            playerLevel.curPoint -= powerUpData.cost;
            playerLevel.pointText.text = playerLevel.curPoint.ToString();
            
            currentLevel += 1;

            for(int e = 0; e < powerUpData.effects.Count; e += 1)
            {
                BuyPowerUp(powerUpData.effects[e]);
            }
 
            UpdateButtonText();
        }
    }

    // Buton metnini güncelleyen fonksiyon
    public void UpdateButtonText()
    {
        buttonText.text = powerUpData.cost + "P\n" + powerUpData.powerUpName + "\n" + currentLevel + "/" + powerUpData.maxLevel;
    }

    // Güçlendirme satın alma işlemini gerçekleştiren fonksiyon
    void BuyPowerUp(PowerUpEffect effect)
    {
        // Güçlendirme satın alma işlemleri burada gerçekleştirilir
        switch (effect.effectName)
        {
            case "Damage":
            player.GetComponent<Gun>().damage += effect.value;
                break;
            case "Ammo":
            player.GetComponent<Gun>().maxAmmo += (int)effect.value;
            player.GetComponent<Gun>().curAmmo += (int)effect.value;
            player.GetComponent<Gun>().Reload();
                break;
            case "Pierce":
            player.GetComponent<Gun>().piercing += (int)effect.value;
                break;
            case "HP":
            var hpComp = player.GetComponent<PlayerHP>();
            hpComp.maxHP += effect.value;
            hpComp.Heal(effect.value);
                break;
            case "Speed":
            player.GetComponent<PlayerMovement>().speed += effect.value;
                break;
            case "Sprint":
            player.GetComponent<PlayerMovement>().sprintSpeed += effect.value;
                break;
            case "Jump":
            player.GetComponent<PlayerMovement>().jumpHeight += effect.value;      
                break;
            case "Ammo Kill":
            player.GetComponent<Gun>().ammoOnKill += (int)effect.value;      
                break;
            case "Ammo Spawn Timer":
            ammoSpawner.GetComponent<AmmoSpawner>().ammoSpawnTime -= effect.value;      
                break;
            case "Max Ammo Spawn":
            ammoSpawner.GetComponent<AmmoSpawner>().maxAmmoSpawn += (int)effect.value;      
                break;
            case "Exp":
            player.GetComponent<PlayerLevel>().bonusExp += effect.value;      
                break;
            default:
                break;
        }
    }
}