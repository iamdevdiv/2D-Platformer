using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireBullet : MonoBehaviour {
    private AudioSource audioSource;

    public AudioClip gunshotAudio;
    public AudioClip bulletHitAudio;

    private float speed = 10f;
    public float Speed { get { return speed; } set { speed = value; } }

    private Animator anim;

    private bool canMove;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    private void Start() {
        canMove = true;
        audioSource.PlayOneShot(gunshotAudio);
        StartCoroutine(DisableBullet(5f));
    }

    private void Update() {
        Move();
    }

    private void Move() {
        if (canMove) {
            Vector3 tempPos = transform.position;
            tempPos.x += speed * Time.deltaTime;
            transform.position = tempPos;
        }
    }

    IEnumerator DisableBullet(float timer) {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        string[] enemyTags = { Tags.BEETLE_TAG, Tags.SNAIL_TAG, Tags.SPIDER_TAG, Tags.BIRD_TAG, Tags.BOSS_TAG, Tags.FROG_TAG, Tags.BULLETPROOF_TAG };

        if (enemyTags.Contains(collision.gameObject.tag)) {
            audioSource.Stop();
            audioSource.PlayOneShot(bulletHitAudio);

            anim.Play("Explode");
            canMove = false;
            StartCoroutine(DisableBullet(0.2f));
        }
    }
}
