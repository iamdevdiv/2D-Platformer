using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private PlayerAudioScript audioManager;

    [SerializeField]
    private float speed = 5f;
    private float jumpPower = 12f;

    private Rigidbody2D myBody;
    private Animator anim;

    [SerializeField]
    private Transform groundCheckPosition;

    [SerializeField]
    private Transform playerFallPosition;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private LayerMask enemyLayer;

    private bool isGrounded;
    private bool jumped;

    private void Awake() {
        audioManager = GetComponent<PlayerAudioScript>();

        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        PlayerJump();
        PlayerFell();
    }

    private void FixedUpdate() {
        CheckIfGrounded();
        PlayerWalk();
    }

    private void PlayerWalk() {
        float h = Input.GetAxisRaw("Horizontal");

        if (h > 0) {
            myBody.velocity = new Vector2(speed, myBody.velocity.y);
            ChangeDirection(1);
            PlayFootstepAudio();
        } else if (h < 0) {
            myBody.velocity = new Vector2(-speed, myBody.velocity.y);
            ChangeDirection(-1);
            PlayFootstepAudio();
        } else {
            myBody.velocity = new Vector2(0f, myBody.velocity.y);
        }
        
        anim.SetInteger("Speed", Mathf.Abs((int)myBody.velocity.x));
    }

    private void ChangeDirection(int direction) {
        Vector3 tempScale = transform.localScale;
        tempScale.x = direction;
        transform.localScale = tempScale;
    }

    private void CheckIfGrounded() {
        isGrounded = Physics2D.Raycast(groundCheckPosition.position, Vector2.down, 0.1f, groundLayer) || Physics2D.Raycast(groundCheckPosition.position, Vector2.down, 0.1f, enemyLayer);
        if (isGrounded && jumped) {
            jumped = false;
            anim.SetBool("Jump", false);
            PlayFootstepAudio();
        }
    }

    private void PlayerJump() {
        if (isGrounded && Input.GetKey(KeyCode.Space)) {
            audioManager.audioSource.PlayOneShot(audioManager.jumpAudio);

            jumped = true;
            myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);

            anim.SetBool("Jump", true);
        }
    }

    private void PlayerFell() {
        if (transform.position.y < playerFallPosition.position.y) {
            GetComponent<PlayerDamage>().MoveToCheckpoint();
        }
    }

    private void PlayFootstepAudio() {
        if (!audioManager.audioSource.isPlaying && isGrounded) {
            audioManager.audioSource.PlayOneShot(audioManager.footstepAudio);
        }
    }
}
