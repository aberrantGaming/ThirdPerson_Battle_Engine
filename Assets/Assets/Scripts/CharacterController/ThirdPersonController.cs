using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class ThirdPersonController : ThirdPersonAnimator
    {
        protected virtual void Start()
        {
#if !UNITY_EDITOR
            Cursor.visible = false;
#endif
        }

        public virtual void Sprint(bool value)
        {
            isSprinting = value;
        }

        public virtual void Aim(bool value)
        {
            wasAiming = isAiming;
            isAiming = value;
            
            if (isAiming)
            {
                Strafe(true);
            }
            else if (!isAiming && wasAiming)
                //Disable Strafeing
                Strafe(false);
        }

        public virtual void ChangeShoulder()
        {
            if (!isAiming) return;

            aimCameraOffset.x = aimCameraOffset.x * (-1);
            aimPivotOffset.x = aimPivotOffset.x * (-1);
        }

        public virtual void Strafe()
        {
            if (locomotionType == LocomotionType.OnlyFree) return;
            isStrafing = !isStrafing;
        }

        public virtual void Strafe(bool value)
        {
            isStrafing = value;
        }

        public virtual void Jump()
        {
            //conditions to do this action
            bool jumpConditions = isGrounded && !isJumping;

            //return if jumpConditions is false
            if (!jumpConditions) return;

            //trigger jump behavior
            jumpCounter = jumpTimer;
            isJumping = true;

            // trigger jump animations
            if (_rigidbody.velocity.magnitude < 1)
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                animator.CrossFadeInFixedTime("JumpMove", 0.2f);
        }

        public virtual void Evade(string trigger)
        {
            bool evadeConditions = isGrounded && isStrafing;

            if (!evadeConditions) return;

            //trigger animations
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(trigger))
                animator.SetTrigger("Trigger_" + trigger);
        }

        public virtual void RotateWithAnotherTransform(Transform referenceTransform)
        {
            var newRotation = new Vector3(transform.eulerAngles.x, referenceTransform.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), strafeRotationSpeed * Time.deltaTime);
            targetRotation = transform.rotation;
        }
    }
}

