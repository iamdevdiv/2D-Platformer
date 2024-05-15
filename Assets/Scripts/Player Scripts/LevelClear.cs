using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClear : MonoBehaviour {
    private BackgroundMusicScript backgroundMusic;
    private PlayerAudioScript playerAudio;

    private void Awake() {
        backgroundMusic = GameObject.Find("Background Music").GetComponent<BackgroundMusicScript>();
        playerAudio = GameObject.Find("Player").GetComponent<PlayerAudioScript>();
    }

    public void PlayLevelClearAudio() {
        Time.timeScale = 0f;
        backgroundMusic.audioSource.Stop();

        playerAudio.audioSource.PlayOneShot(playerAudio.levelClearAudio);
        StartCoroutine(MoveToMainMenu());
    }

    private IEnumerator MoveToMainMenu() {
        yield return new WaitForSecondsRealtime(6f);
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
