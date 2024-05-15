using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private float cameraSpeed = 0.3f;

    private Transform target;

    private float offsetZ;
    private Vector3 currentVelocity;

    private bool followsPlayer;

    private void Awake() {
        BoxCollider2D myCol = GetComponent<BoxCollider2D>();
        float camHeight = 2f * Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;
        myCol.size = new Vector2(camWidth, camHeight);
    }

    private void Start() {
        target = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).transform;
        offsetZ = (transform.position - target.position).z;
        followsPlayer = true;
    }

    private void FixedUpdate() {
        if (followsPlayer) {
            Vector3 aheadTargetPos = target.position + Vector3.forward * offsetZ;
            Vector3 backTargetPos = target.position - Vector3.back * offsetZ;

            if (aheadTargetPos.x >= transform.position.x) {
                Vector3 newCameraPosition = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, cameraSpeed);
                transform.position = new Vector3(newCameraPosition.x, transform.position.y, newCameraPosition.z);
            } else if (backTargetPos.x <= transform.position.x) {
                Vector3 newCameraPosition = Vector3.SmoothDamp( transform.position, backTargetPos, ref currentVelocity, cameraSpeed);
                transform.position = new Vector3( newCameraPosition.x, transform.position.y, newCameraPosition.z);
            }
        }
    }
}
