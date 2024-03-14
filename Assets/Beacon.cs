using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    [SerializeField] GameObject beaconPrefab;
    [SerializeField] float clipRange = 130;
    [SerializeField] float maxIntensityRange = 200;
    [SerializeField] float maxIntensity = 3;
    [ColorUsage(false, true)]
    [SerializeField] Color emissionColor;
    [SerializeField] Material beaconMat;
    Material material;
    Transform fox;
    GameObject beacon;
    // Start is called before the first frame update
    void Awake()
    {
        beacon = Instantiate(beaconPrefab);
        material = Instantiate(beaconMat);
        beacon.GetComponent<MeshRenderer>().sharedMaterial = material;
        beacon.transform.SetParent(transform, false);
        fox = FindObjectOfType<FoxMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, fox.position);
        Color mainColor = new(1, 1, 1, distance < clipRange ? 0 : .01f);
        material.color = mainColor;

        float intensity = Mathf.Min(math.remap(clipRange, maxIntensityRange, 0, maxIntensity, distance), maxIntensity);
        beacon.transform.localScale = new Vector3(intensity / maxIntensity, beacon.transform.localScale.y, intensity / maxIntensity);
        float factor = Mathf.Pow(2, intensity);
        Color newEmission = emissionColor * factor;
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", newEmission);
    }
}
