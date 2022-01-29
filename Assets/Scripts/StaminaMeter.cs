using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StaminaMeter : MonoBehaviour
{
    public RectTransform staminaMeterBar;
    public Color staminaMeterColor;
    public Color exhaustedColor;
    [Range(0f, 1f)]
    public float value = 1f;
    [HideInInspector]
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        color = staminaMeterColor;
    }

    private void Awake()
    {
        gameObject.GetComponent<Image>().color = color;
        staminaMeterBar.gameObject.GetComponent<Image>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        var scale = staminaMeterBar.localScale;
        scale.x = value;
        staminaMeterBar.localScale = scale;
        staminaMeterBar.gameObject.GetComponent<Image>().color = color;
        gameObject.GetComponent<Image>().color = color;
    }
}
