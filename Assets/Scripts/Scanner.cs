using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float interval = 1.0f;
    [SerializeField] private Material[] scannableMaterials;

    private float currentRadius;
    private float timeSinceLastScan;

    private void Update()
    {
        timeSinceLastScan += Time.deltaTime;

        if (timeSinceLastScan > interval)
        {
            currentRadius = 0;
            timeSinceLastScan = 0;

            foreach (var scannableMaterial in scannableMaterials)
            {
                scannableMaterial.SetVector("Scan_Position", transform.position);
            }
        }
        else
        {
            currentRadius += speed * Time.deltaTime;
        }

        foreach (var scannableMaterial in scannableMaterials)
        {
            scannableMaterial.SetFloat("Max_Scan_Distance", speed * interval);
            scannableMaterial.SetFloat("Scan_Radius", currentRadius);
        }
    }
}
