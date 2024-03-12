using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class FoxSound : MonoBehaviour, INoiseSource
{
    public UnityEvent<Vector3, NoiseLevel> EmitNoise { get; set; } = new();
    public NoiseLevel NoiseLevel { get; set; } = NoiseLevel.Quiet;
    AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    float SoundLevelFromNoiseLevel()
    {
        return NoiseLevel switch
        {
            NoiseLevel.Quiet => 0f,
            NoiseLevel.Weak => .5f,
            NoiseLevel.Strong => 1f,
            _ => throw new System.NotImplementedException(),
        };
    }
    private void Update()
    {
        EmitNoise.Invoke(transform.position, NoiseLevel);
    }

    public void OnPlayRandomSound(RandomSoundDef randomSound)
    {
        audioSource.PlayOneShot(randomSound.soundPool.PickRandom(), randomSound.volume * (randomSound.affectedByNoiseLevel ? SoundLevelFromNoiseLevel() : 1));
    }
    public void OnPlaySound(SoundDef sound)
    {
        audioSource.PlayOneShot(sound.clip, sound.volume * (sound.affectedByNoiseLevel ? SoundLevelFromNoiseLevel() : 1));
    }
}
