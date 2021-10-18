using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cell : MonoBehaviour
{
    [SerializeField] private Texture2D highlightedTexture;
    private Texture normalTexture;
    private MeshRenderer meshRenderer;

    public event EventHandler MouseEnter;
    public event EventHandler MouseExit;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        normalTexture = meshRenderer.material.mainTexture;
    }

    private void OnMouseEnter()
    {
        meshRenderer.material.mainTexture = highlightedTexture;

        if (MouseEnter != null)
        {
            MouseEnter(this, EventArgs.Empty);
        }
    }

    private void OnMouseExit()
    {        
        meshRenderer.material.mainTexture = normalTexture;

        if (MouseExit != null)
        {
            MouseExit(this, EventArgs.Empty);
        }
    }
}
