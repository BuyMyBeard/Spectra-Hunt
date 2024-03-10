using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FoxSound : MonoBehaviour
{
    public float NoiseLevel { get; set; } = 1;
    AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPlayRandomSound(RandomSoundDef randomSound)
    {
        audioSource.PlayOneShot(randomSound.soundPool.PickRandom(), randomSound.volume * NoiseLevel);
    }
}
