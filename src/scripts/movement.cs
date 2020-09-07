using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{

    public GameObject cameraObject;

    public GameObject LightObject;

    public Rigidbody playerRigidbody;

    public float groundY;

    public float speed;

    private bool inAir;

    // Start is called before the first frame update
    void Start()
    {
        groundY = 0.5f;
        speed = 25;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 forwardXZ = cameraObject.GetComponent<camera>().forwardXZ;
        Vector3 rightXZ = cameraObject.GetComponent<camera>().rightXZ;
        bool isGrounded = IsGrounded();

        if(Input.GetKey("w") && isGrounded && playerRigidbody.velocity.magnitude < 15) {
            playerRigidbody.velocity += forwardXZ * speed * Time.deltaTime;
        }

        if(Input.GetKey("a") && isGrounded && playerRigidbody.velocity.magnitude < 15) {
            playerRigidbody.velocity -= rightXZ * speed * Time.deltaTime;
        }
        
        if(Input.GetKey("s") && isGrounded && playerRigidbody.velocity.magnitude < 15) {
            playerRigidbody.velocity -= forwardXZ * speed * Time.deltaTime;
        }

        if(Input.GetKey("d") && isGrounded && playerRigidbody.velocity.magnitude < 15) {
            playerRigidbody.velocity += rightXZ * speed * Time.deltaTime;
        }

        if(Input.GetKey("space") && isGrounded) {
            playerRigidbody.velocity += new Vector3(0, 2, 0);
        }

        LightObject.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, groundY);
    }
}
