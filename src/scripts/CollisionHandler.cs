using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler {

    [HideInInspector]
    public Vector3[] desiredCameraClipPoints;
    public Vector3[] adjustedCameraClipPoints;
    public bool colliding;

    public void Initialize(Camera camera) {
        adjustedCameraClipPoints = new Vector3[5];
        desiredCameraClipPoints = new Vector3[5];
    }

    public void UpdateCameraClipPoints(Vector3 cameraPos, Quaternion atRotation, ref Vector3[] intoArray) {
        // //clear the contents
        // intoArray = new Vector3[5];

        // float z = camera.nearClipPlane;
        // float x = Mathf.Tan(camera.fieldOfView / 2f) * z;
        // float y = x / camera.aspect;

        // //top left clip point
        // intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPos;
        // //top right clip point
        // intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPos;
        // //bottom left clip point
        // intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPos;
        // //bottom right clip point
        // intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPos;
        // //camera's position
        // intoArray[4] = cameraPos - camera.transform.forward;
    }

    private bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition) {
        for(int i=0; i<clipPoints.Length; i++) {
            Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
            float distance = Vector3.Distance(clipPoints[i], fromPosition);
            
            if(Physics.Raycast(ray, distance)) {
                return true;
            }
        }

        return false;
    }

    public float GetAdjustedDistanceWithRayFrom(Vector3 from) {
        float distance = -1.0f;

        for(int i=0; i<desiredCameraClipPoints.Length; i++) {
            Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
            RaycastHit raycastHit;

            if(Physics.Raycast(ray, out raycastHit)) {
                if(distance == -1) {
                    distance = raycastHit.distance;
                } else if(raycastHit.distance < distance) {
                    distance = raycastHit.distance;
                }
            }
        }

        if(distance == -1) {
            return 0;
        } else {
            return 0;
        }
    }

    public void CheckColliding(Vector3 targerPosition) {
        colliding = CollisionDetectedAtClipPoints(desiredCameraClipPoints, targerPosition);
    }

}
