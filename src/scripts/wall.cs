using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class wall : MonoBehaviour {

    void OnCollisionEnter(Collision c) {
        checkTopCollision(c);
    }

    private void checkTopCollision(Collision c) {
        if(c.gameObject.name != "Player") {
            return;
        }

        RaycastHit raycastHit;
        Vector3 direction = (this.transform.position - c.gameObject.transform.position).normalized;
        Ray MyRay = new Ray(c.gameObject.transform.position, direction);

        if(Physics.Raycast(MyRay, out raycastHit)) {

            if(raycastHit.collider != null){
                Vector3 normal = raycastHit.normal;
                normal = raycastHit.transform.TransformDirection(normal);
                if(normal == raycastHit.transform.up){
                    c.rigidbody.velocity += createVelocityVector();
                }
            }    
        }
    }

    private Vector3 createVelocityVector() {
        float x = Random.Range(-8.0f, 8.0f);
        if(Mathf.Abs(x) < 4) {
            x = Mathf.Sign(x) * 4;
        }

        float z = Random.Range(-8.0f, 8.0f);
        if(Mathf.Abs(z) < 4) {
            z = Mathf.Sign(z) * 4;
        }

        return new Vector3(x, 0, z);
    }

}
