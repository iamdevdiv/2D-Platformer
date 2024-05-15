using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour {
    private LevelClear levelClearScript;

    private Animator anim;

    private int health = 5;
    private bool canDamage;

    public GameObject bossHealthImage;

    [SerializeField]
    private GameObject healthBar;

    private void Awake() {
        levelClearScript = GameObject.Find("Player").GetComponent<LevelClear>();

        anim = GetComponent<Animator>();
        canDamage = true;
    }

    private IEnumerator WaitForDamage() {
        yield return new WaitForSeconds(2f);
        canDamage = true;
    }

    public void PopulateHealthBars() {
        float pixelsPerUnit = bossHealthImage.GetComponentInParent<Canvas>().referencePixelsPerUnit;
        float imgWidth = (bossHealthImage.GetComponent<RectTransform>().rect.width / pixelsPerUnit) * bossHealthImage.transform.localScale.x;
        float barWidth = (healthBar.GetComponent<RectTransform>().rect.width / pixelsPerUnit) * healthBar.transform.localScale.x;

        for (int i = 0; i < health; i++) {
            Vector3 spawnPosition = new Vector3(
                (bossHealthImage.transform.position.x - imgWidth - (barWidth * i * 2f)),
                bossHealthImage.transform.position.y,
                bossHealthImage.transform.position.z
            );
            Instantiate(healthBar, spawnPosition, Quaternion.identity, bossHealthImage.transform);
        }
    }

    private void DecrementHealthBar() {
        int barIndex = bossHealthImage.transform.childCount - 1;
        Destroy(bossHealthImage.transform.GetChild(barIndex).gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == Tags.BULLET_TAG && canDamage) {
            health--;
            DecrementHealthBar();
            canDamage = false;

            if (health == 0) {
                GetComponent<BossScript>().DeactivateBossScript();
                anim.Play("BossDead");
            }

            StartCoroutine(WaitForDamage());
        }
    }

    private void RestartGame() {
        levelClearScript.PlayLevelClearAudio();
    }
}
