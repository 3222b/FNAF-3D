using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var fadeImageColor = GetComponent<Image>().color;
        if (fadeImageColor.a <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            fadeImageColor.a -= Time.deltaTime;
        }
        GetComponent<Image>().color = fadeImageColor;
    }
}
