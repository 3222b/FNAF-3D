using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightPower : MonoBehaviour
{
    public Image flashlightImage;
    public Sprite[] flashlightSprites;
    public float flashlightPower = 100;
    private float flashlightPowerMax;

    // Start is called before the first frame update
    void Start()
    {
        flashlightPowerMax = flashlightPower;
    }

    // Update is called once per frame
    void Update()
    {
        float stretchedLength = (((flashlightSprites.Length-1) / flashlightPowerMax) * flashlightPower);
        Debug.Log(stretchedLength);
        if (stretchedLength > flashlightSprites.Length - 1)
            stretchedLength = flashlightSprites.Length - 1;
        else if (stretchedLength < 0)
            stretchedLength = 0;
        flashlightImage.sprite = flashlightSprites[(int)Mathf.Ceil(stretchedLength)];
    }
}