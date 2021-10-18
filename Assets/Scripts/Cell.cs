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
    public event EventHandler MouseUp;

    public int Column { get; set; }
    public int Row { get; set; }

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

    private void OnMouseUp()
    {
        if (MouseUp != null)
        {
            MouseUp(this, EventArgs.Empty);
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
