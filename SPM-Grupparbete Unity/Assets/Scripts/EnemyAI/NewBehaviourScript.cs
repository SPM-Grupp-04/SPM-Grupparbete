using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SkinnedMeshToMesh : MonoBehaviour
{

    public SkinnedMeshRenderer meshRenderer;
    public VisualEffect vfxGraph;
    public float refreshRate;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateVfxGraph());
    }

    IEnumerator UpdateVfxGraph()
    {
        while (gameObject.activeSelf)
        {
            Mesh m = new Mesh();
            meshRenderer.BakeMesh(m);

            Vector3[] vertices = m.vertices;
            Mesh m2 = new Mesh();
            m2.vertices = vertices;
            
            vfxGraph.SetMesh("EnemyMesh", m2);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
