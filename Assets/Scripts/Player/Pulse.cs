using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Pulse : MonoBehaviour
{
    [SerializeField, Range(1, 100)] private float maxIntensity = 1;
    [SerializeField, Range(1, 100)] private float speed = 1;
    private MeshRenderer meshRenderer;
    private Vector4 color;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        color = meshRenderer.material.GetVector(Shader.PropertyToID("_EmissionColor"));
    }

    private void Update()
    {
        var ratio = Mathf.Abs(Mathf.Sin(Time.time * speed));
        meshRenderer.material.SetVector(Shader.PropertyToID("_EmissionColor"), Vector4.Lerp(color, color * maxIntensity, ratio));
    }
}
