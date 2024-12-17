using Unity.Hierarchy;
using UnityEngine;
using UnityUtils;

namespace AdvancedController {
    public class TurnTowardController : MonoBehaviour {
        [SerializeField] PlayerController controller;
        [SerializeField] GameObject camera;
        public float turnSpeed = 50f;
        //public float actionTurnSpeed = 1000f;

        Transform tr;
        float currentYRotation;
        const float fallOffAngle = 90f;
        bool rotateToCamera = false;

        void Start() {
            tr = transform;
            
            currentYRotation = tr.localEulerAngles.y;
        }

        void LateUpdate() {
            Vector3 vector = controller.GetMovementVelocity();
            float speed = Time.deltaTime * turnSpeed;

            if (rotateToCamera)
            {
                vector = camera.transform.forward;
                speed = 100;
                rotateToCamera = false;
            }

            Vector3 velocity = Vector3.ProjectOnPlane(vector, tr.parent.up);
            if (velocity.magnitude < 0.001f) return;
            
            float angleDifference = VectorMath.GetAngle(tr.forward, velocity.normalized, tr.parent.up);
            
            float step = Mathf.Sign(angleDifference) *
                         Mathf.InverseLerp(0f, fallOffAngle, Mathf.Abs(angleDifference)) * speed;
            
            currentYRotation += Mathf.Abs(step) > Mathf.Abs(angleDifference) 
                                ? angleDifference 
                                : step;
            
            tr.localRotation = Quaternion.Euler(0f, currentYRotation, 0f);
        }

        public void RotateToCamera()
        {
            rotateToCamera = true;
        }
    }
}
