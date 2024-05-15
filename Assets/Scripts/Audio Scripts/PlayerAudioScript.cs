using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioScript : MonoBehaviour {
    [HideInInspector]
    public AudioSource audioSource;

    public AudioClip coinAudio;
    public AudioClip bonusBlockAudio;

    public AudioClip jumpAudio;
    public AudioClip footstepAudio;

    public AudioClip healthLossAudio;
    public AudioClip gameOverAudio;
    public AudioClip levelClearAudio;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
}
