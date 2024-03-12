using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BushSound : MonoBehaviour, INoiseSource
{
    [SerializeField] RandomSoundDef bushSounds;
    AudioSource audioSource;
    [SerializeField] NoiseLevel noiseLevel = NoiseLevel.Strong;

    public UnityEvent<Vector3, NoiseLevel> EmitNoise { get; set; } = new();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayRustleSound(other);
    }
    private void OnTriggerExit(Collider other)
    {
        PlayRustleSound(other);
    }

    [ContextMenu("PlaySound")]
    void PlayRustleSound(Collider sourceCollider)
    {
        audioSource.PlayOneShot(bushSounds.soundPool.PickRandom(), bushSounds.volume);
        if (sourceCollider.TryGetComponent<FoxMovement>(out _))
            EmitNoise.Invoke(transform.position, noiseLevel);

    }
}
