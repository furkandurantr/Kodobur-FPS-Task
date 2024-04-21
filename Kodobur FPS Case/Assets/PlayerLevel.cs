using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLevel : MonoBehaviour
{
    public float curExp = 0f;
    public float maxExp = 0f;
    public int curLevel = 0;
    public float expIncrease = 0f;
    public int curScore = 0;
    public Slider slider;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        maxExp = curLevel * expIncrease;
        slider.value = curExp / maxExp;
        levelText.text = curLevel.ToString();
        scoreText.text = curScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainExp(float exp)
    {
        float expDiff = maxExp - curExp;
        float overflow = exp - expDiff;

        curExp += exp;
        if (curExp >= maxExp)
        {
            curExp = 0;
            curExp += overflow;
            curLevel += 1;
            maxExp = curLevel * expIncrease;
            levelText.text = curLevel.ToString();
            // Gain Point
        }
        slider.value = curExp / maxExp;
    }

    public void GainScore(int score)
    {
        curScore += score;
        scoreText.text = curScore.ToString();
    }

    void SliderUpdate()
    {
        slider.value = curExp / maxExp;
    }
}
