using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Power power;
    public Material material;
    public Animator animator;
    public AudioSource doorSound;
    public bool doorEnabled = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OpenDoor()
    {
        doorSound.Play();
        material.EnableKeyword("_EMISSION");
        power.powerConsumption++;
    }

    void CloseDoor()
    {
        doorSound.Play();
        material.DisableKeyword("_EMISSION");
        power.powerConsumption--;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Closed", doorEnabled);
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "DoorOpen" && animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "DoorClose")
        {
            RaycastHit rayHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (power.powerLeft > 0)
            {
                if (Physics.Raycast(ray, out rayHit, 2.0f))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (rayHit.transform == transform)
                        {
                            doorEnabled = !doorEnabled;
                            if (doorEnabled)
                                OpenDoor();
                            else
                                CloseDoor();
                        }
                    }
                    else if (!doorEnabled)
                    {
                        material.SetColor("_EmissionColor", Color.red);
                    }
                }
            }
            else if (doorEnabled)
            {
                doorEnabled = false;
                CloseDoor();
            }
        }
    }
}
