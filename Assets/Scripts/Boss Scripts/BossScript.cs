using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossScript : MonoBehaviour {
    private AudioSource audioSource;

    [SerializeField]
    private GameObject stone;

    [SerializeField]
    private Transform attackInstantiate;

    private Animator anim;

    private string startAttackCoroutine = "StartAttack";

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    private void Start() {
        StartCoroutine(startAttackCoroutine);
    }

    private void Attack() {
        GameObject stoneClone = Instantiate(stone, attackInstantiate.position, Quaternion.identity);
        stoneClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-300f, -650f), 0f));
        audioSource.Play();
    }

    private void BackToIdle() {
        anim.Play("BossIdle");
    }

    public void DeactivateBossScript() {
        StopCoroutine(startAttackCoroutine);
        enabled = false;
    }

    private IEnumerator StartAttack() {
        yield return new WaitForSeconds(Random.Range(2f, 5f));

        anim.Play("BossAttack");
        StartCoroutine(startAttackCoroutine);
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == Tags.PLAYER_TAG && enabled) {
            collision.gameObject.GetComponent<PlayerDamage>().MoveToCheckpoint();
        }
    }
}
