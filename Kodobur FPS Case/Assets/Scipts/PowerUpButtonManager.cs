using UnityEngine;
using UnityEngine.UI;

public class PowerUpButtonManager : MonoBehaviour
{
    public Button[] powerUpButtons;
    public PowerUpData[] powerUpDataArray;

    void Start()
    {
        SetupButtons();
    }

    void SetupButtons()
    {
        if (powerUpButtons.Length != powerUpDataArray.Length)
        {
            Debug.LogError("Buton ve güçlendirme verisi sayısı eşleşmiyor!");
            return;
        }

        for (int i = 0; i < powerUpButtons.Length; i++)
        {
            PowerUpButton buttonScript = powerUpButtons[i].GetComponent<PowerUpButton>();
            if (buttonScript != null)
            {
                // Buton scriptine güçlendirme verisini ata
                buttonScript.powerUpData = powerUpDataArray[i];
                // Buton metnini güncelle
                buttonScript.UpdateButtonText();
            }
            else
            {
                Debug.LogError("PowerUpButton scripti butona eklenmemiş!");
            }
        }
    }
}