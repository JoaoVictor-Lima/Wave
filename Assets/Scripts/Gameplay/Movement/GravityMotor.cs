using UnityEngine;

namespace Entities.Base
{
    [RequireComponent(typeof(CharacterController))]
    public class GravityMotor : MonoBehaviour
    {
        [Header("Gravity Settings")]
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float groundStickForce = -2f;

        private CharacterController controller;
        private float verticalVelocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            ApplyGravity();
        }

        private void ApplyGravity()
        {
            if (controller.isGrounded)
            {
                if (verticalVelocity < 0f)
                    verticalVelocity = groundStickForce;
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            Vector3 gravityMove = Vector3.up * verticalVelocity;
            controller.Move(gravityMove * Time.deltaTime);
        }
    }
}
