using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public Text timeText;
    public int hourLengthInSeconds = 60;

    private int hour = 0;
    private float timeIncrease = 0;

    public float getTime() => hour + (timeIncrease / hourLengthInSeconds);

    void Update()
    {
        timeText.text = (hour > 0) ? hour.ToString() : "12";
        timeIncrease += Time.deltaTime;
        if (timeIncrease >= hourLengthInSeconds)
        {
            hour++;
            timeIncrease = 0;
        }
    }
}