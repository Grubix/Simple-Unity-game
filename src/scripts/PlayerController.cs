using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public CameraController camera;
    public Transform light;

    public float radius = 0.6f;
    public float speed = 1.0f;
    public float maxSpeed = 5.0f;
    public float jumpStrength = 1.5f;

    [HideInInspector]
    public Rigidbody rbody;

    // Start is called before the first frame update
    void Start() {
        if(GetComponent<Rigidbody>()) {
            rbody = GetComponent<Rigidbody>();
        } else {
            Debug.LogError("Player rigidbody is not set");
        }

        transform.localScale = new Vector3(radius, radius, radius);
    }

    private void Move() {
        bool isGrounded = Physics.Raycast(transform.position, -Vector3.up, radius);

        if(isGrounded) {
            if(rbody.velocity.magnitude < maxSpeed) {
                if(Input.GetKey("w")) {
                    rbody.velocity += camera.forwardXZ * speed;
                }
                if(Input.GetKey("a")) {
                    rbody.velocity -= camera.rightXZ * speed;
                }
                if(Input.GetKey("s")) {
                    rbody.velocity -= camera.forwardXZ * speed;
                }
                if(Input.GetKey("d")) {
                    rbody.velocity += camera.rightXZ * speed;
                }
            }

            if(Input.GetKey("space")) {
                rbody.velocity += Vector3.up * jumpStrength;
            }
        }

        light.position = new Vector3(transform.position.x, transform.position.y + 1.1f * radius, transform.position.z);
    }

    // private void Update() {
    //     GetInput();
    // }

    private void FixedUpdate() {
        Move();
    }


}
