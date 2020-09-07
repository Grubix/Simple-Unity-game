using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour {

    public GameObject player;

    private Vector3 offset;

    public Vector3 forwardXZ;

    public Vector3 rightXZ;

    private void Start() {
        offset = new Vector3(0, 1.5f, -1.5f);
    }

    private void LateUpdate() {
        Vector3 tmp = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 3f, Vector3.up) * Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * - 3f, transform.right) * offset;

        if(!Physics.CheckSphere(player.transform.position + tmp, 0.3f)) {
            transform.position = player.transform.position + tmp;
            offset = tmp;
        } else {
            transform.position = player.transform.position + offset;
        }

        transform.LookAt(player.transform.position);

        // transform.position = Vector3.Lerp(transform.position, target, 0.1f);

        forwardXZ = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        rightXZ = new Vector3(transform.right.x, 0, transform.right.z).normalized;
    }

}
