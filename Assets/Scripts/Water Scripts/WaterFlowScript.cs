using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlowScript : MonoBehaviour {
    private Rigidbody2D myBody;
    private BoxCollider2D myCollider;

    [SerializeField]
    private GameObject water;

    private float width;
    private bool readyToDisable = false;
    private float travelledDistance;

    [SerializeField]
    private LayerMask groundLayer;

    private void Awake() {
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();

        width = myCollider.bounds.size.x;
    }

    private void FixedUpdate() {
        Move();

        if (readyToDisable) {
            DisableWater();
        }
    }

    private void Move() {
        myBody.velocity = new Vector2(0.2f, myBody.velocity.y);
    }

    private void DisableWater() {
        travelledDistance += Mathf.Abs(myBody.velocity.x * Time.fixedDeltaTime);

        RaycastHit2D ray = Physics2D.Raycast(new Vector2(transform.position.x - width / 2 - 0.01f, transform.position.y), Vector2.right, 0f, groundLayer);

        if (travelledDistance >= width && ray) {
            gameObject.SetActive(false);
        }
    }

    private void AddWater() {
        if (!gameObject.scene.isLoaded) return;

        Vector3 spawnPosition = transform.position;
        spawnPosition.x -= myCollider.bounds.size.x - 0.01f;

        GameObject newWater = Instantiate(water, spawnPosition, Quaternion.identity);
        newWater.name = "Water (Clone)";
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 3) {
            readyToDisable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == 3 && travelledDistance <= width) {
            AddWater();
        }
    }
}
