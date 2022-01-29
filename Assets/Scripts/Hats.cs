using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hats : MonoBehaviour
{
    public TaskManager taskManager;
    private bool inRange = false;
    private float progress = 0;

    [HideInInspector]
    private bool taskComplete = false;
    private List<GameObject> objects;

    // Start is called before the first frame update
    void Start()
    {
        taskManager.partyHatCount++;
        objects = new List<GameObject>();
        foreach (var child in gameObject.transform.GetComponentsInChildren<Transform>())
        {
            if (child != gameObject.transform)
            {
                objects.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] rayHits;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        rayHits = Physics.RaycastAll(ray, 1f);
        Debug.DrawRay(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        bool hitRay = false;
        foreach (var rayHit in rayHits)
        {
            if (rayHit.transform == transform)
            {
                hitRay = true;
                break;
            }
        }
        if (hitRay)
        {
            hitRay = taskManager.SetActiveTask(this.gameObject);
            inRange = true;
        }
        if (!hitRay)
        {
            progress = 0;
            taskManager.RemoveActiveTask(this.gameObject);
            inRange = false;
        }

        if (inRange && !taskComplete && hitRay)
        {
            taskManager.text = "Hold [E] to place hats.";
            if (Input.GetKey(KeyCode.E))
            {
                progress += Time.deltaTime;
                if (progress > 1)
                {
                    taskComplete = true;
                    taskManager.partyHatsPlaced++;
                    progress = 1;
                    foreach (var child in objects)
                    {
                        if (child != null)
                            child.gameObject.SetActive(true);
                    }
                }
            }
            else if (progress > 0)
            {
                progress -= Time.deltaTime;
            }
            taskManager.progress = progress;
        }
        else
        {
            taskManager.RemoveActiveTask(this.gameObject);
        }
    }
}
