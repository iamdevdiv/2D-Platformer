using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour {
    BackgroundMusicScript bgMusicScript;
    BossHealth bossHealth;

    private void Awake() {
        bgMusicScript = GameObject.Find("Background Music").GetComponent<BackgroundMusicScript>();
        bossHealth = GameObject.Find("Boss").GetComponent<BossHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag != Tags.PLAYER_TAG) {
            return;
        }

        collision.gameObject.GetComponent<PlayerDamage>().LastCheckpoint = transform.position;
        gameObject.SetActive(false);

        if (gameObject.name == "Boss Checkpoint" && !bossHealth.bossHealthImage.activeSelf) {
            bossHealth.bossHealthImage.SetActive(true);
            bossHealth.PopulateHealthBars();

            bgMusicScript.audioSource.Stop();
            bgMusicScript.audioSource.clip = bgMusicScript.bossMusic;
            bgMusicScript.audioSource.Play();
        }
    }
}