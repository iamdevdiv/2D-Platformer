using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : MonoBehaviour {
    private void Start() {
        Invoke("DeactivateStone", 4f);
    }

    private void DeactivateStone() {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == Tags.PLAYER_TAG) {
            collision.GetComponent<PlayerDamage>().DealDamage();
            gameObject.SetActive(false);
        }
    }
}
