using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float interval = 1.0f;
    [SerializeField] private Material[] scannableMaterials;

    private float currentRadius;
    private float timeSinceLastScan;
    private readonly List<Material> materialInstances = new List<Material>();

    private void Awake()
    {
        enabled = Settings.Instance.IsScannerPulseVisible;
    }

    private void Start()
    {        
        // Make sure we get the instanced versions of all scannable materials and apply our changes to them instead of the originals.
        foreach (var scannableMaterial in scannableMaterials)
        {
            if (MaterialInstanceFactory.Instance.TryGetMaterialInstance(scannableMaterial, out Material scannableMaterialInstance))
            {
                materialInstances.Add(scannableMaterialInstance);
            }
        }
    }

    private void Update()
    {
        timeSinceLastScan += Time.deltaTime;

        if (timeSinceLastScan > interval)
        {
            currentRadius = 0;
            timeSinceLastScan = 0;

            foreach (var materialInstance in materialInstances)
            {
                materialInstance.SetVector("Scan_Position", transform.position);
            }
        }
        else
        {
            currentRadius += speed * Time.deltaTime;
        }

        foreach (var materialInstance in materialInstances)
        {
            materialInstance.SetFloat("Max_Scan_Distance", speed * interval);
            materialInstance.SetFloat("Scan_Radius", currentRadius);
        }
    }    
}
