using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHead : MonoBehaviour
{
    public Transform player;
    public Transform neck;
    public Transform[] eyes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        neck.LookAt(player.position / 2);
        foreach (Transform eye in eyes)
        {
            eye.LookAt(player.position);
        }
    }
}
