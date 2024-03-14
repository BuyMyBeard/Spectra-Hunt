using Cinemachine;
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
    CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
        waveFrequency = material.GetFloat("_WaveFrequency");
        waveSpeed = material.GetFloat("_WaveSpeed");
        waveAmplitude = material.GetFloat("_WaveAmplitude");
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
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
        if (Application.isPlaying && !withinBounds) return;
        if (!Application.isPlaying && Camera.current == null) return;
        if (underwaterMaterial != null)
        {
            Transform camTransform = Application.isPlaying ? virtualCamera.transform : Camera.current.transform;
            bool isUnderWater = IsUnderWater(camTransform.position);
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
    bool withinBounds = false;

    private void OnTriggerEnter(Collider other)
    {
        withinBounds = true;
    }
    private void OnTriggerExit(Collider other)
    {
        withinBounds = false;
    }
}
