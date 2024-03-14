using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    [SerializeField] float hareCount = 6;
    [ContextMenu("Hare Death")]
    public void HareDeath()
    {
        hareCount--;
        audioSource.Play();
        if (hareCount == 0)
            GetComponent<PlayableDirector>().Play();
    }
}
