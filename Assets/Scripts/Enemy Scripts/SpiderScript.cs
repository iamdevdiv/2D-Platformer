using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderScript : MonoBehaviour {
    private Animator anim;
    private Rigidbody2D myBody;
    private Vector3 moveDirection = Vector3.down;

    private string changeMovementCoroutine = "ChangeMovement";

    private void Awake() {
        anim = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        StartCoroutine(changeMovementCoroutine);
    }

    private void Update() {
        MoveSpider();
    }

    private void MoveSpider() {
        transform.Translate(moveDirection * Time.smoothDeltaTime);
    }

    private IEnumerator ChangeMovement() {
        yield return new WaitForSeconds(Random.Range(2f, 5f));

        if (moveDirection == Vector3.down) {
            moveDirection = Vector3.up;
        } else {
            moveDirection = Vector3.down;
        }

        StartCoroutine(changeMovementCoroutine);
    }

    private IEnumerator SpiderDead() {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == Tags.BULLET_TAG) {
            anim.Play("SpiderDead");
            myBody.bodyType = RigidbodyType2D.Dynamic;

            StartCoroutine(SpiderDead());
            StopCoroutine(changeMovementCoroutine);
        } else if (collision.tag == Tags.PLAYER_TAG) {
            collision.GetComponent<PlayerDamage>().DealDamage();
        }
    }
}
