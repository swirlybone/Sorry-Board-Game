/**
 * Simply highlights certain GameObjects when they are moused over.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLighter : MonoBehaviour
{
    [SerializeField] Material highlightMat;
    Material normalMat;

    MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
        normalMat = mesh.material;
    }

    public void HighLight() { mesh.material = highlightMat; }
    public void UnHighLight() { mesh.material = normalMat; }

    private void OnMouseEnter()
    {
        HighLight();
    }

    private void OnMouseExit()
    {
        UnHighLight();
    }
}
