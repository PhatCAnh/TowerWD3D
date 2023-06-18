using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChooseTower : MonoBehaviour
{
    public MeshRenderer theMR;
    public Material defaultMaterial;
    public Material orginMaterial;

    public Node node;

    private void Start()
    {
        theMR = transform.GetChild(0).GetComponent<MeshRenderer>();
        orginMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        node = GetComponentInParent<Node>();
    }

    private void OnMouseEnter()
    {
        theMR.material = defaultMaterial;
    }

    private void OnMouseExit()
    {
        theMR.material = orginMaterial;
    }

    private void OnMouseDown()
    {
        node.AppearTower();
    }
}
