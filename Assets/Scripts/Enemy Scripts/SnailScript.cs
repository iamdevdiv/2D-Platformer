using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SnailScript : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;

    private Rigidbody2D myBody;
    private Animator anim;

    private bool moveLeft = true;
    private bool canMove = true;

    [SerializeField]
    private Transform topCollision, downCollision, leftCollision, rightCollision;

    private Vector3 leftCollisionPos, rightCollisionPos;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private LayerMask groundLayer;

    private void Awake() {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        leftCollisionPos = leftCollision.localPosition;
        rightCollisionPos = rightCollision.localPosition;
    }

    private void Update() {
        if (canMove) {
            if (moveLeft) {
                myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
            }
            else {
                myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
            }

            CheckCollision();
        }
    }

    private void CheckCollision() {
        if (!Physics2D.Raycast(downCollision.position, Vector2.down, 0.1f, groundLayer) || (Physics2D.Raycast(transform.position, Vector2.right, 1f, groundLayer) && !moveLeft)) {
            ChangeDirection();
        }

        RaycastHit2D leftHit = Physics2D.Raycast(leftCollision.position, Vector2.left, 0.1f, playerLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rightCollision.position, Vector2.right, 0.1f, playerLayer);
        Collider2D topHit = Physics2D.OverlapCircle(topCollision.position, 0.2f, playerLayer);

        if (topHit && topHit.gameObject.tag == Tags.PLAYER_TAG && canMove) {
            Rigidbody2D playerBody = topHit.gameObject.GetComponent<Rigidbody2D>();
            playerBody.velocity = new Vector2(playerBody.velocity.x, 7f);

            canMove = false;
            myBody.velocity = new Vector2(0, 0);

            anim.Play("Stunned");
        }

        if (leftHit && leftHit.collider.gameObject.tag == Tags.PLAYER_TAG) {
            if (canMove) {
                leftHit.collider.gameObject.GetComponent<PlayerDamage>().DealDamage();
            } else {
                if (tag != Tags.BEETLE_TAG) {
                    myBody.velocity = new Vector2(15f, myBody.velocity.y);
                    StartCoroutine(Dead(3f));
                }
            }
        } else if (rightHit && rightHit.collider.gameObject.tag == Tags.PLAYER_TAG) {
            if (canMove) {
                rightHit.collider.gameObject.GetComponent<PlayerDamage>().DealDamage();
            } else {
                if (tag != Tags.BEETLE_TAG) {
                    myBody.velocity = new Vector2(-15f, myBody.velocity.y);
                    StartCoroutine(Dead(3f));
                }
            }
        }
    }

    private void ChangeDirection() {
        moveLeft = !moveLeft;

        Vector3 tempScale = transform.localScale;

        if (moveLeft) {
            tempScale.x = Mathf.Abs(tempScale.x);

            leftCollision.localPosition = leftCollisionPos;
            rightCollision.localPosition = rightCollisionPos;
        } else {
            tempScale.x = -Mathf.Abs(tempScale.x);

            leftCollision.localPosition = rightCollisionPos;
            rightCollision.localPosition = leftCollisionPos;
        }

        transform.localScale = tempScale;
    }

    private IEnumerator Dead(float timer) {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == Tags.BULLET_TAG) {
            anim.Play("Stunned");
            myBody.velocity = new Vector2(0, 0);

            if (tag == Tags.BEETLE_TAG) {
                StartCoroutine(Dead(0.5f));
            } else if (!canMove && tag == Tags.SNAIL_TAG) {
                gameObject.gameObject.SetActive(false);
            }

            canMove = false;
        }
    }
}
