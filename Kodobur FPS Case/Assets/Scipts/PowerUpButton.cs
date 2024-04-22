using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpButton : MonoBehaviour
{
    public PowerUpData powerUpData;
    public TextMeshProUGUI myLabel;
    public TMP_Text buttonText;
    private int currentLevel = 0;

    public GameObject player;
    public GameObject ammoSpawner;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ammoSpawner = GameObject.FindGameObjectWithTag("AmmoSpawner");
        UpdateButtonText();
    }

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

    public void UpdateButtonText()
    {
        buttonText.text = powerUpData.cost + "P\n" + powerUpData.powerUpName + "\n" + currentLevel + "/" + powerUpData.maxLevel;
    }

    void BuyPowerUp(PowerUpEffect effect)
    {
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
            case "Attack Speed":
            player.GetComponent<Gun>().attackSpeed += effect.value;      
                break;
            case "Lifesteal":
            player.GetComponent<Gun>().lifeSteal += effect.value;      
                break;
            default:
                break;
        }
    }
}