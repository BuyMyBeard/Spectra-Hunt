using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))]
public class Water : MonoBehaviour
{
    [SerializeField] Material underwaterMaterial;
    Material material;
    float waveFrequency;
    float waveSpeed;
    float waveAmplitude;
    private void Awake()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
        waveFrequency = material.GetFloat("_WaveFrequency");
        waveSpeed = material.GetFloat("_WaveSpeed");
        waveAmplitude = material.GetFloat("_WaveAmplitude");
    }
    public float GetHeightAtPosition(Vector3 position)
    {
         return (Mathf.Sin(position.x * waveFrequency + waveSpeed * Time.time) * waveAmplitude + transform.position.y);
    }
    public bool IsUnderWater(Vector3 position)
    {      
        float height = GetHeightAtPosition(position);
        return height > position.y;
    }

    private void Update()
    {
        if (underwaterMaterial != null)
        {
            Camera cam = Camera.current != null ? Camera.current : Camera.main;
            bool isUnderWater = IsUnderWater(cam.transform.position);
            float intensity = isUnderWater ? 1 : 0;
            RenderSettings.fog = isUnderWater;
            underwaterMaterial.SetFloat("_Intensity", intensity);
        }
    }
    private void OnValidate()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
        waveFrequency = material.GetFloat("_WaveFrequency");
        waveSpeed = material.GetFloat("_WaveSpeed");
        waveAmplitude = material.GetFloat("_WaveAmplitude");
    }
}
