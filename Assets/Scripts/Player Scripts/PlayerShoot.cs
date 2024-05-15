using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {
    [SerializeField]
    private GameObject fileBullet;

    private void ShootBullet() {
        if (Input.GetKeyDown(KeyCode.J)) {
            GameObject bullet = Instantiate(fileBullet, transform.position, Quaternion.identity);
            bullet.GetComponent<FireBullet>().Speed *= transform.localScale.x;
        }
    }

    private void Update() {
        ShootBullet();
    }
}
