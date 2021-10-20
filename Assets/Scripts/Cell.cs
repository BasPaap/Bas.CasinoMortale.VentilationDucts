using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cell : MonoBehaviour
{
    [SerializeField] private Texture2D highlightedTexture;
    private Texture normalTexture;
    private MeshRenderer meshRenderer;
    private bool isMouseOver;

    public event EventHandler MouseEnter;
    public event EventHandler MouseLeave;
    public event EventHandler Click;

    public int Column { get; set; }
    public int Row { get; set; }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        normalTexture = meshRenderer.material.mainTexture;
    }

    internal void TestHits(IEnumerable<GameObject> hitGameObjects)
    {
        // The event order has to be MouseLeft, MouseEntered, Click.
        if (hitGameObjects.Contains(gameObject))
        {
            // Mouse is currently over this gameObject.
            if (!isMouseOver)
            {
                OnMouseHasEntered();
            }

            isMouseOver = true;
        }
        else
        {
            // Mouse is currently not over this gameObject.
            if (isMouseOver)
            {
                OnMouseHasLeft();
            }

            isMouseOver = false;
        }

        if (isMouseOver && Input.GetMouseButtonUp(0))
        {
            OnClick();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1005:Delegate invocation can be simplified.", Justification = "Can't simplify delegate invocation in Unity.")]
    private void OnMouseHasEntered()
    {
        meshRenderer.material.mainTexture = highlightedTexture;

        if (MouseEnter != null)
        {
            MouseEnter(this, EventArgs.Empty);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1005:Delegate invocation can be simplified.", Justification = "Can't simplify delegate invocation in Unity.")]
    private void OnClick()
    {
        if (Click != null)
        {
            Click(this, EventArgs.Empty);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1005:Delegate invocation can be simplified.", Justification = "Can't simplify delegate invocation in Unity.")]
    private void OnMouseHasLeft()
    {        
        meshRenderer.material.mainTexture = normalTexture;

        if (MouseLeave != null)
        {
            MouseLeave(this, EventArgs.Empty);
        }
    }

    
}
