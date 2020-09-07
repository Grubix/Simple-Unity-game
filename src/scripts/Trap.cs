using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    public float plateSpeed;

    public float maxHeight;

    public float strength;

    private Vector3 minPos;

    private Vector3 maxPos;

    private bool active;

    private float timeElapsed;

    private float waitTime;

    private void Start() {
        minPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        maxPos = minPos + new Vector3(0, maxHeight, 0);
        plateSpeed = 0.09f;
        maxHeight = 2f;
        strength = 10.0f;
    }

    void OnCollisionEnter(Collision c) {
        waitTime = Random.Range(0.5f, 1f);
    }

    void OnCollisionStay(Collision c) {
        if(c.gameObject.name == "Player") {
            timeElapsed += Time.deltaTime;

            if(timeElapsed >= waitTime) {
                c.rigidbody.velocity += new Vector3(Random.Range(-3.0f, 3.0f), strength, Random.Range(-3.0f, 3.0f));
                timeElapsed = 0;
                active = true;
            }
        }
    }

    void OnCollisionExit(Collision c) {
        if(c.gameObject.name == "Player") {
            timeElapsed = 0;
            active = false;
        }
    }

    private void FixedUpdate() {
        if(active) {
            transform.position = Vector3.Slerp(transform.position, maxPos, plateSpeed);
        } else {
            transform.position = Vector3.Slerp(transform.position, minPos, plateSpeed);
        }
    }
}
