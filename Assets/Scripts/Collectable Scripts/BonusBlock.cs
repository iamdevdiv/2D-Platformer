using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBlock : MonoBehaviour {
    [SerializeField]
    private Transform bottomCollision;

    private Animator anim;
    private bool startAnim;
    private bool canAnimate = true;

    [SerializeField]
    private LayerMask playerLayer;
    private GameObject player;

    private Vector3 moveDirection = Vector3.up;
    private Vector3 originPosition;
    private Vector3 animPosition;

    private void Awake() {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start() {
        originPosition = transform.position;

        animPosition = transform.position;
        animPosition.y += 0.15f;
    }

    private void Update() {
        CheckForCollision();
        AnimateUpDown();
    }

    private void CheckForCollision() {
        RaycastHit2D hit = Physics2D.Raycast(bottomCollision.position, Vector2.down, 0.1f, playerLayer);
        if (hit && canAnimate) {
            player.GetComponent<ScoreScript>().IncreaseScore(3, "bonus");
            anim.Play("BlockIdle");
            startAnim = true;
            canAnimate = false;
        }
    }

    private void AnimateUpDown() {
        if (startAnim) {
            transform.Translate(moveDirection * Time.smoothDeltaTime);

            if (transform.position.y >= animPosition.y) {
                moveDirection = Vector3.down;
            } else if (transform.position.y <= originPosition.y) {
                startAnim = false;
            }
        }
    }
}
