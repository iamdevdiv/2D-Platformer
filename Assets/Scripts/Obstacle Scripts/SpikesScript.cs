using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SpikesScript : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == Tags.PLAYER_TAG) {
            collision.gameObject.GetComponent<PlayerDamage>().spiked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == Tags.PLAYER_TAG) {
            collision.gameObject.GetComponent<PlayerDamage>().spiked = false;
            ;
        }
    }
}
