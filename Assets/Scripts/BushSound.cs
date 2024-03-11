using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushSound : MonoBehaviour
{
    [SerializeField] RandomSoundDef bushSounds;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayRustleSound();
    }
    private void OnTriggerExit(Collider other)
    {
        PlayRustleSound();
    }

    [ContextMenu("PlaySound")]
    void PlayRustleSound()
    {
        audioSource.PlayOneShot(bushSounds.soundPool.PickRandom(), bushSounds.volume);
    }
}
