using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public static Text textWidget;
    private static List<string> strings;

    void Start()
    {
        textWidget = GetComponent<Text>();
        strings = new List<string>();
    }

    void LateUpdate()
    {
        textWidget.text = "FPS: " + Mathf.Ceil(1.0f / Time.deltaTime) + "\n";
        foreach (string s in strings)
        {
            textWidget.text += s + "\n";
        }
        strings.Clear();
    }

    public static void PushInformation(string text)
    {
        strings.Add(text);
    }
}
