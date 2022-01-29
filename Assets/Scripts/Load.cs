using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public string sceneToLoad = "Pizzeria";
    public float progress;
    public GameObject loadingScreen;
    public RectTransform progressSlider;
    public Image fadeImage;
    private float timer = 0;
    private bool loading = false;

    void Update()
    {
        if (loading)
            return;
        timer += Time.deltaTime;
        if (timer >= 3)
        {
            var fadeImageColor = fadeImage.color;
            if (fadeImageColor.a >= 1)
            {
                StartCoroutine(LoadScene());
                loading = true;
            }
            else
            {
                fadeImageColor.a += Time.deltaTime;
            }
            fadeImage.color = fadeImageColor;
        }
    }

    IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        loadingScreen.SetActive(true);
        while (!asyncLoad.isDone)
        {
            Debug.Log(asyncLoad.progress);
            progress = asyncLoad.progress;
            var scale = progressSlider.localScale;
            scale.x = Mathf.Clamp01(progress / 0.9f);
            progressSlider.localScale = scale;
            yield return null;
        }
    }
}
