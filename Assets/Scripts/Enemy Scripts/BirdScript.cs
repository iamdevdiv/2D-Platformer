using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour {
    private Rigidbody2D myBody;
    private Animator anim;

    private Vector3 moveDirection = Vector3.left;
    private Vector3 originPosition;
    private Vector3 movePosition;

    [SerializeField]
    private GameObject birdEgg;

    [SerializeField]
    private LayerMask playerLayer;

    private bool attacked;

    private bool canMove;
    private float speed = 2.5f;

    private void Awake() {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start() {
        originPosition = transform.position;
        originPosition.x += 6f;

        movePosition = transform.position;
        movePosition.x -= 6f;

        canMove = true;
    }

    private void Update() {
        MoveBird();
        DropTheEgg();
    }

    private void MoveBird() {
        if (canMove) {
            transform.Translate(moveDirection * speed * Time.smoothDeltaTime);

            if (transform.position.x >= originPosition.x) {
                moveDirection = Vector3.left;
                ChangeDirection(0.5f);
            } else if (transform.position.x <= movePosition.x) {
                moveDirection = Vector3.right;
                ChangeDirection(-0.5f);
            }
        }
    }

    private void ChangeDirection(float direction) {
        Vector3 tempScale = transform.localScale;
        tempScale.x = direction;
        transform.localScale = tempScale;
    }

    private void DropTheEgg() {
        if (!attacked && Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, playerLayer)) {
            attacked = true;

            Instantiate(birdEgg, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), Quaternion.identity);
            anim.Play("BirdFly");
        }
    }

    private IEnumerator BirdDead() {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == Tags.BULLET_TAG) {
            anim.Play("BirdDead");

            GetComponent<BoxCollider2D>().isTrigger = true;
            myBody.bodyType = RigidbodyType2D.Dynamic;

            canMove = false;
            StartCoroutine(BirdDead());
        }
    }
}
