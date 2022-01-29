using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMeshCollider : MonoBehaviour
{
    void Awake()
    {
        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        {
            if (g.gameObject.GetComponent<MeshCollider>() != null
                || g.gameObject.GetComponent<NoCollider>() != null
                || g.gameObject.GetComponent<MeshRenderer>() == null)
            {
                continue;
            }
            MeshFilter meshFilter = g.gameObject.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                MeshCollider meshCollider = g.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = meshFilter.mesh;
            }
        }
    }
}