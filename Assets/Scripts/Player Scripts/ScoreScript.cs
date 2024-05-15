using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {
    private Text coinTextScore;
    private int scoreCount;

    private PlayerAudioScript audioManager;

    private void Awake() {
        audioManager = GetComponent<PlayerAudioScript>();
    }

    private void Start () {
        coinTextScore = GameObject.Find("Coins Text").GetComponent<Text>();
    }

    public void IncreaseScore(int score, string audio) {
        scoreCount += score;
        coinTextScore.text = "x" + scoreCount;

        if (audio == "normal") {
            audioManager.audioSource.PlayOneShot(audioManager.coinAudio);
        } else if (audio == "bonus") {
            audioManager.audioSource.PlayOneShot(audioManager.bonusBlockAudio);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == Tags.COIN_TAG) {
            collision.gameObject.SetActive(false);
            IncreaseScore(1, "normal");
        }
    }
}
