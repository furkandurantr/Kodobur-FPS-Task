using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject inGameGUI;
    public GameObject talentGUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            GamePaused();
        }
    }

    void GamePaused()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            inGameGUI.SetActive(true);
            talentGUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
         }
        else
        {
            Time.timeScale = 0;
            inGameGUI.SetActive(false);
            talentGUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }   
    }
}