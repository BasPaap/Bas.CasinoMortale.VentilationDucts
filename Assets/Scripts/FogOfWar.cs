using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    private void Update()
    {
        foreach (var material in materials)
        {
            material.SetVector("Player_Position", transform.position);
        }
    }
}
