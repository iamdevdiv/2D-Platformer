using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogScript : MonoBehaviour{
    private Animator anim;

    private bool animationStarted;
    private bool animationFinished;

    private int jumpedTimes;
    private bool jumpLeft = true;

    private bool isDead = false;

    private string frogJumpCoroutine = "FrogJump";

    [SerializeField]
    private LayerMask playerLayer;

    private GameObject player;


    private void Awake() {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG);
    }
    private void Start() {
        StartCoroutine(frogJumpCoroutine);
    }

    private void Update() {
        if (Physics2D.OverlapCircle(transform.position, 0.5f, playerLayer) && !isDead) {
            player.GetComponent<PlayerDamage>().DealDamage();
        }
    }

    private void LateUpdate() {
        if (animationStarted && animationFinished) {
            animationStarted = false;

            transform.parent.position = transform.position;
            transform.localPosition = Vector3.zero;
        }
    }

    IEnumerator FrogJump() {
        yield return new WaitForSeconds(Random.Range(1f, 4f));

        animationStarted = true;
        animationFinished = false;

        jumpedTimes++;

        if (jumpLeft) {
            anim.Play("FrogJumpLeft");
        } else {
            anim.Play("FrogJumpRight");
        }

        StartCoroutine(frogJumpCoroutine);
    }

    private void AnimationFinished() {
        animationFinished = true;

        if (jumpLeft) {
            anim.Play("FrogIdleLeft");
        } else {
            anim.Play("FrogIdleRight");
        }

        if (jumpedTimes == 3) {
            jumpedTimes = 0;

            Vector3 tempScale = transform.localScale;
            tempScale.x *= -1;
            transform.localScale = tempScale;

            jumpLeft = !jumpLeft;
        }
    }

    private IEnumerator Dead() {
        isDead = true;

        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == Tags.BULLET_TAG) {
            anim.Play("FrogDead");
            StopCoroutine(frogJumpCoroutine);

            StartCoroutine(Dead());
        }
    }
}
