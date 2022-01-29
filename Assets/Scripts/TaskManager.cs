using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [HideInInspector]
    public float progress = 0;
    [SerializeField]
    private Text _text;
    [SerializeField]
    private RectTransform _progressBar;
    [HideInInspector]
    public string text;
    [HideInInspector]
    public GameObject _activeTask;

    public RectTransform taskInformationPanel;
    public Text taskInformationText;
    public CanvasGroup taskInformationCanvasGroup;

    [HideInInspector]
    public int partyHatCount = 0;
    [HideInInspector]
    public int partyHatsPlaced = 0;

    void Start()
    {
        
    }

    public float lastTaskNotificationTime = 10;
    // Update is called once per frame
    void Update()
    {
        taskInformationText.text = "Tonight's Tasks:\n";
        if (partyHatCount > 0)
            taskInformationText.text += "Place party hats in dining room (" + partyHatsPlaced + "/" + partyHatCount + ")";
        Vector2 panelSize = Vector2.zero;
        panelSize.x = taskInformationText.GetComponent<RectTransform>().sizeDelta.x + 20;
        panelSize.y = taskInformationText.GetComponent<RectTransform>().sizeDelta.y + 20;
        taskInformationPanel.sizeDelta = panelSize;

        bool showTaskInformationPanel = false;

        if (_activeTask != null)
        {
            showTaskInformationPanel = true;
            lastTaskNotificationTime = 0;
            if (GetComponent<CanvasGroup>().alpha < 0.6f)
            {
                GetComponent<CanvasGroup>().alpha = GetComponent<CanvasGroup>().alpha + (Time.deltaTime * 4.0f);
                if (GetComponent<CanvasGroup>().alpha > 0.6f)
                {
                    GetComponent<CanvasGroup>().alpha = 0.6f;
                }
            }
        }
        else
        {
            if (lastTaskNotificationTime > 10)
            {
                showTaskInformationPanel = false;
            }
            else
            {
                lastTaskNotificationTime += Time.deltaTime;
                showTaskInformationPanel = true;
            }
            if (GetComponent<CanvasGroup>().alpha > 0)
                GetComponent<CanvasGroup>().alpha = GetComponent<CanvasGroup>().alpha - (Time.deltaTime * 4.0f);
        }
        if (showTaskInformationPanel)
        {
            if (taskInformationCanvasGroup.alpha < 1)
                taskInformationCanvasGroup.alpha += (Time.deltaTime*2);
        }
        else
        {
            if (taskInformationCanvasGroup.alpha > 0.2)
            {
                taskInformationCanvasGroup.alpha -= (Time.deltaTime/2);
                if (taskInformationCanvasGroup.alpha <= 0.5)
                {
                    taskInformationCanvasGroup.alpha = 0.5f;
                }
            }
        }
        var scale = _progressBar.localScale;
        scale.x = progress;
        _progressBar.localScale = scale;
    }

    private void LateUpdate()
    {
        GetComponent<Text>().text = text;
    }

    internal void RemoveActiveTask(GameObject gameObject)
    {
        if (gameObject == _activeTask)
            _activeTask = null;
    }

    internal bool SetActiveTask(GameObject gameObject)
    {
        if (_activeTask == null)
        {
            _activeTask = gameObject;
            return true;
        }
        return (gameObject == _activeTask);
    }
}
