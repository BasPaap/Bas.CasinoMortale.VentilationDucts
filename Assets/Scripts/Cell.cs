using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cell : MonoBehaviour
{
    [SerializeField] private Texture2D highlightedTexture;
    private Texture normalTexture;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        normalTexture = meshRenderer.material.mainTexture;
    }

    private void OnMouseOver()
    {
        meshRenderer.material.mainTexture = highlightedTexture;
    }

    private void OnMouseExit()
    {        
        meshRenderer.material.mainTexture = normalTexture;
    }
}
