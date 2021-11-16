using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class WaterManager : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Vector3[] verticeAnchors;
  //  private Vector3 pizzapos;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
       // pizzapos = this.GetComponentInParent<GameObject>().transform.position;
        verticeAnchors = meshFilter.mesh.vertices;
    }

    private void Update()
    {
        // TODO: Multithread this.
        Vector3[] vertices = meshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].x = verticeAnchors[i].x + WaveManager.instance.GetWaveHorizontal(transform.position.x + verticeAnchors[i].x);
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + verticeAnchors[i].x);
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
    }
}
