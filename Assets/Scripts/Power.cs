using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Power : MonoBehaviour
{
    public int powerConsumption = 0;
    public float powerLeft = 100;
    public Text powerText;
    public bool inOffice = false;
    public CanvasGroup canvasGroup;

    // Update is called once per frame
    void Update()
    {
        if (inOffice)
        {
            if (canvasGroup.alpha < 1)
                canvasGroup.alpha += (Time.deltaTime * 2f);
        }
        else
        {
            if (canvasGroup.alpha > 0)
                canvasGroup.alpha -= (Time.deltaTime * 2f);
        }

        powerLeft -= (Time.deltaTime * powerConsumption);
        powerText.text = Mathf.Ceil(powerLeft).ToString();

        for (int i = 0; i < transform.childCount; i ++)
        {
            transform.GetChild(i).gameObject.SetActive(powerConsumption > i);
        }
    }
}
