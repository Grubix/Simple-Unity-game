using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public class CollisionHandler {

        [HideInInspector]
        public Vector3[] desiredCameraClipPoints;
        public Vector3[] adjustedCameraClipPoints;
        public bool colliding;
        public Camera camera;

        public void Initialize() {
            camera = GameObject.Find("Camera").GetComponent<Camera>();;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        public void UpdateCameraClipPoints(Vector3 cameraPos, Quaternion atRotation, ref Vector3[] intoArray) {
            //clear the contents
            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / 10f) * z;
            float y = x / camera.aspect;

            //top left clip point
            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPos;
            //top right clip point
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPos;
            //bottom left clip point
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPos;
            //bottom right clip point
            intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPos;
            //camera's position
            intoArray[4] = cameraPos + camera.transform.forward;
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
                return distance;
            }
        }

        public void CheckColliding(Vector3 targetPosition) {
            colliding = CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition);
        }

    }

    public class PositionSettings {
        public Vector3 targetPositionOffset = new Vector3(0, 0.5f, 0);
        public float distanceFromTarget = -2;
        public float zoomSmooth = 100;
        public float maxZoom = -2;
        public float minZoom = -10;
        public float adjustmentDistance = 0;
    }

    public class OrbitSettings {
        public float xRotation = -20;
        public float yRotation = -180;
        public float maxXRotation = 50;
        public float minXRotation = -85;
        public float vOrbitSmooth = 150;
        public float hOrbitSmooth = 150;
        public float sensitivity = 3;
    }

    public CollisionHandler collision = new CollisionHandler();

    public PositionSettings positionSettings = new PositionSettings();
    public OrbitSettings orbitSettings = new OrbitSettings();

    [HideInInspector]
    PlayerController playerController;

    public Transform target;

    [HideInInspector]
    public Vector3 forwardXZ = Vector3.up;
    [HideInInspector]
    public Vector3 rightXZ = Vector3.right;

    public Vector3 targetPosition, destination, adjustedDestination;

    float vOrbitInput, hOrbitInput, zoomInput;

    Vector3 camVel = Vector3.zero;

    // Start is called before the first frame update
    void Start() {
        setCameraTarget(target);
        MoveToTarget();
        
        collision.Initialize();
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        collision.CheckColliding(targetPosition);
        positionSettings.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(targetPosition);
    }

    // Update is called once per frame
    void LateUpdate() {
        GetInput();  
        MoveToTarget();
        OrbitTarget();
        ZoomInTarget();

        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);
        
        collision.CheckColliding(targetPosition);
        positionSettings.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(targetPosition);

        // for(int i=0; i<5; i++) {
        //     Debug.DrawLine(targetPosition, collision.desiredCameraClipPoints[i], Color.white);
        //     Debug.DrawLine(targetPosition, collision.adjustedCameraClipPoints[i], Color.green);
        // }
    }

    public void setCameraTarget(Transform target) {
        if(target != null) {
            this.target = target;
            
            if(target.GetComponent<PlayerController>()) {
                playerController = target.GetComponent<PlayerController>();
            } else {
                Debug.LogError("Target needs a player controller");
            }
        } else {
            Debug.LogError("Target is null");
        }
    }

    private void GetInput() {
        vOrbitInput = -Input.GetAxis("Mouse Y") * orbitSettings.sensitivity;
        hOrbitInput = -Input.GetAxis("Mouse X") * orbitSettings.sensitivity;
        zoomInput = Input.GetAxis("Mouse ScrollWheel");
    }

    private void OrbitTarget() {
        orbitSettings.xRotation += -vOrbitInput * orbitSettings.vOrbitSmooth * Time.deltaTime;
        orbitSettings.yRotation += -hOrbitInput * orbitSettings.hOrbitSmooth * Time.deltaTime;

        if(orbitSettings.xRotation > orbitSettings.maxXRotation) {
            orbitSettings.xRotation = orbitSettings.maxXRotation;
        } else if(orbitSettings.xRotation < orbitSettings.minXRotation) {
            orbitSettings.xRotation = orbitSettings.minXRotation;
        }
    }

    private void MoveToTarget() {
        // offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 3f, Vector3.up) * Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * - 3f, transform.right) * offset;
        // // transform.position = Vector3.Slerp(transform.position, target.transform.position + offset, smooth);
        // transform.position = target.transform.position + offset;
        // transform.LookAt(target.transform.position);
        targetPosition = target.position + positionSettings.targetPositionOffset;
        destination = targetPosition + Quaternion.Euler(orbitSettings.xRotation, orbitSettings.yRotation, 0) * -Vector3.forward * positionSettings.distanceFromTarget;

        if(collision.colliding && Mathf.Abs(positionSettings.distanceFromTarget) < 6) {
            adjustedDestination = targetPosition + Quaternion.Euler(orbitSettings.xRotation, orbitSettings.yRotation, 0) * Vector3.forward * positionSettings.adjustmentDistance;
            transform.position = adjustedDestination;
        } else {
            transform.position = destination;
        }

        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = targetRotation;

        forwardXZ = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        rightXZ = new Vector3(transform.right.x, 0, transform.right.z).normalized;
    }

    private void ZoomInTarget() {
        positionSettings.distanceFromTarget += zoomInput * positionSettings.zoomSmooth * Time.deltaTime;

        if(positionSettings.distanceFromTarget > positionSettings.maxZoom) {
            positionSettings.distanceFromTarget = positionSettings.maxZoom;
        } else if(positionSettings.distanceFromTarget < positionSettings.minZoom) {
            positionSettings.distanceFromTarget = positionSettings.minZoom;
        }
    }

}
