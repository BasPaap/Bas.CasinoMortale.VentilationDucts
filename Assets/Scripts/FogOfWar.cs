using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    private MeshRenderer ductMeshRenderer;
    private MaterialPropertyBlock materialPropertyBlock;
    private Vector3 closestPosition = Vector3.positiveInfinity;
    private float closestDistance = float.MaxValue;

    public Transform PlayerTransform { get; set; }

    private void Awake()
    {        
        materialPropertyBlock = new MaterialPropertyBlock();
        ductMeshRenderer = GetComponentInChildren<MeshRenderer>();

        if (!Settings.Instance.IsFogOfWarEnabled)
        {
            ductMeshRenderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetVector("Closest_Player_Position", transform.position);
            ductMeshRenderer.SetPropertyBlock(materialPropertyBlock);

            enabled = false;
        }
    }

    private void Update()
    {
        var distanceToPosition = Vector3.Distance(transform.position, PlayerTransform.position);
        if (distanceToPosition < closestDistance)
        {
            closestDistance = distanceToPosition;
            closestPosition = PlayerTransform.position;
        }

        ductMeshRenderer.GetPropertyBlock(materialPropertyBlock);
        materialPropertyBlock.SetVector("Closest_Player_Position", closestPosition);
        ductMeshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}
