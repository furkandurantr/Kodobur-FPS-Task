using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI myLabel;
    public String levelName;
    // Start is called before the first frame update
    public void ButtonPressed()
    {
        SceneManager.LoadScene(levelName);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
