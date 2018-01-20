using UnityEngine;
using System.Collections;

namespace CharacterController
{
    public class ThirdPersonAnimator : ThirdPersonMotor
    {
        public virtual void UpdateAnimator()
        {
            if (animator == null || !animator.enabled) return;
            
            animator.SetBool("IsMoving", isMoving);
            animator.SetBool("IsStrafing", isStrafing);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetBool("IsAiming", isAiming);
            animator.SetBool("IsEvading", isEvading);
            animator.SetBool("IsUnarmed", isUnarmed);
            animator.SetFloat("GroundDistance", groundDistance);
            
            if (!isGrounded)
                animator.SetFloat("VerticalVelocity", verticalVelocity);

            if (isStrafing)
            {
                // strafe movement. get the input as 1 or -1
                animator.SetFloat("InputHorizontal", direction, 0.1f, Time.deltaTime);
            }

            // free movement. get the input 0 to 1
            animator.SetFloat("InputVertical", speed, 0.1f, Time.deltaTime);
        }

        /// <summary>
        /// We implement this function to override the default root motion.
        /// This allows us to modify the positional speed before it's applied.
        /// </summary>
        public void OnAnimatorMove()
        {
            if (isGrounded)
            {
                transform.rotation = animator.rootRotation;

                var speedDir = Mathf.Abs(direction) + Mathf.Abs(speed);
                speedDir = Mathf.Clamp(speedDir, 0, 1);
                var strafeSpeed = (isStrafing ? 0.5f : 1f) * Mathf.Clamp(speedDir, 0f, 1f);

                // apply extra speed for strafing/free movement
                if (isStrafing)
                {
                    if (strafeSpeed <= 0.5f)
                        ControlSpeed(strafeWalkSpeed);
                    else if (strafeSpeed > 0.5f && strafeSpeed <= 1f)
                        ControlSpeed(strafeRunningSpeed);
                    else
                        ControlSpeed(strafeSprintSpeed);
                }
                else if (!isStrafing)
                {
                    if (speed <= 0.05f)
                        ControlSpeed(freeWalkSpeed);
                    else if (speed > 0.5 && speed <= 1f)
                        ControlSpeed(freeRunningSpeed);
                    else
                        ControlSpeed(freeSprintSpeed);
                }
            }
        }
    }
}

