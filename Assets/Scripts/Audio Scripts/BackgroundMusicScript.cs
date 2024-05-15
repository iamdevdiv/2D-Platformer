using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicScript : MonoBehaviour {
    [HideInInspector]
    public AudioSource audioSource;

    public AudioClip bgMusic;
    public AudioClip bossMusic;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgMusic;
    }

    private void Start() {
        audioSource.Play();
    }
}
