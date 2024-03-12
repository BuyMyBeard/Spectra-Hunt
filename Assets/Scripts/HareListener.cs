using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HareListener : MonoBehaviour
{
    public UnityEvent onHearNoise; 
    [SerializeField] float weakHearRange = 3;
    [SerializeField] float strongHearRange = 6;
    private void Awake()
    {
        foreach (var noiseSource in FindObjectsOfType<MonoBehaviour>().OfType<INoiseSource>().ToList())
        {
            noiseSource.EmitNoise.AddListener(ListenToNoise);
        }
    }
    public void ListenToNoise(Vector3 origin, NoiseLevel noiseLevel)
    {
        if (noiseLevel == NoiseLevel.Quiet) return;
        float distance = Vector3.Distance(origin, transform.position);
        if (noiseLevel == NoiseLevel.Weak && distance < weakHearRange 
        || noiseLevel == NoiseLevel.Strong && distance < strongHearRange) 
            onHearNoise.Invoke();
    }
}
