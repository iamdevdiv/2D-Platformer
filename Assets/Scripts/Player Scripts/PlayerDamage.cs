using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerDamage : MonoBehaviour {
    private PlayerAudioScript playerAudio;
    private BackgroundMusicScript backgroundMusic;

    private Text lifeText;
    private int lifeScoreCount;

    private bool canDamage;
    private bool canMoveToCheckpoint;

    private Renderer myRenderer;
    private string blinkCoroutine = "Blink";

    public bool spiked = false;

    private Vector3 originalPos;

    public Vector3 LastCheckpoint {
        get; set;
    }

    private void Awake() {
        playerAudio = GetComponent<PlayerAudioScript>();
        backgroundMusic = GameObject.Find("Background Music").GetComponent<BackgroundMusicScript>();

        lifeText = GameObject.Find("Life Text").GetComponent<Text>();
        lifeScoreCount = 3;
        lifeText.text = "x" + lifeScoreCount;

        canDamage = true;
        canMoveToCheckpoint = true;

        myRenderer = GetComponent<Renderer>();

        originalPos = transform.position;
        LastCheckpoint = transform.position;
    }

    private void Update() {
        if (spiked) {
            DealDamage();
        }
    }

    private void DecrementLifeScore() {
        lifeScoreCount--;

        if (lifeScoreCount >= 0) {
            lifeText.text = "x" + lifeScoreCount;
        }

        if (lifeScoreCount > 0) {
            playerAudio.audioSource.PlayOneShot(playerAudio.healthLossAudio);
        }

        if (lifeScoreCount == 0) {
            backgroundMusic.audioSource.Stop();
            playerAudio.audioSource.PlayOneShot(playerAudio.gameOverAudio);

            StartCoroutine(RestartGame());
        }
    }

    public void DealDamage() {
        if (!canDamage) {
            return; 
        }

        DecrementLifeScore();

        if (lifeScoreCount > 0) {
            StartCoroutine(WaitForDamage());  
        }
    }

    public void MoveToCheckpoint() {
        if (!canMoveToCheckpoint) {
            return;
        }

        DecrementLifeScore();

        if (lifeScoreCount > 0) {
            StartCoroutine(MoveToPrevCheckpoint());
        }
    }
  
    private IEnumerator WaitForDamage() {
        canDamage = false;

        StopCoroutine(blinkCoroutine);
        StartCoroutine(blinkCoroutine);

        yield return new WaitForSeconds(2f);
        canDamage = true;

        StopCoroutine(blinkCoroutine);
        myRenderer.enabled = true;
    }

    private IEnumerator Blink() {
        yield return new WaitForSeconds(0.1f);

        if (myRenderer.enabled) {
            myRenderer.enabled = false;
        } else {
            myRenderer.enabled = true;
        }

        StartCoroutine(blinkCoroutine);
    }

    private IEnumerator MoveToPrevCheckpoint() {
        canMoveToCheckpoint = false;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);

        transform.position = new Vector3(LastCheckpoint.x, originalPos.y, originalPos.z);

        Time.timeScale = 1f;
        StartCoroutine(WaitForDamage());
        canMoveToCheckpoint = true;
    }

    private IEnumerator RestartGame() {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(4f);

        SceneManager.LoadScene("Gameplay");
        Time.timeScale = 1f;
    }
}
