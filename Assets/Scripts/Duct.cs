using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duct : MonoBehaviour
{
    [Range(0, 1000)]
    [SerializeField] private float minDistanceToReveal;
    
    private MeshRenderer ductMeshRenderer;
    private MaterialPropertyBlock materialPropertyBlock;
    private bool isRevealed;

    public Transform PlayerTransform { get; set; }

    private void Awake()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        ductMeshRenderer = GetComponentInChildren<MeshRenderer>();        
    }

    private void Update()
    {
        if (!isRevealed && Vector3.Distance(transform.position, PlayerTransform.position) <= minDistanceToReveal)
        {
            ductMeshRenderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetVector("Player_Position", transform.position);
            ductMeshRenderer.SetPropertyBlock(materialPropertyBlock);

            isRevealed = true;
        }
    }
}
