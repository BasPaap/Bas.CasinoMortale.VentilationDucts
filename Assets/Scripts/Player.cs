using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Material[] proximityMaterials;

    private void Update()
    {
        foreach (var material in proximityMaterials)
        {
            material.SetVector("Player_Position", transform.position);
        }
    }
}
