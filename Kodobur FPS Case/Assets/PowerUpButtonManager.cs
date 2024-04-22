using UnityEngine;
using UnityEngine.UI;

public class PowerUpButtonManager : MonoBehaviour
{
    public Button[] powerUpButtons; // Butonları tutacak dizi
    public PowerUpData[] powerUpDataArray; // Güçlendirme verilerini tutacak dizi

    void Start()
    {
        // Butonları ve güçlendirme verilerini eşleştir
        SetupButtons();
    }

    // Butonları ve güçlendirme verilerini eşleştiren fonksiyon
    void SetupButtons()
    {
        // Buton ve güçlendirme verisi sayısını kontrol et
        if (powerUpButtons.Length != powerUpDataArray.Length)
        {
            Debug.LogError("Buton ve güçlendirme verisi sayısı eşleşmiyor!");
            return;
        }

        // Her bir butonu ve güçlendirme verisini eşleştir
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