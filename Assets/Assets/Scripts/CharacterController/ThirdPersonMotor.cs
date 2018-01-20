using UnityEngine;
using System.Collections;

namespace CharacterController
{
    public abstract class ThirdPersonMotor : MonoBehaviour
    {
        #region Variables

        #region Layers
        [Header("|  Layers  |")]
        [Tooltip("Layers that the character can walk on")]
        public LayerMask groundLayer = 1 << 0;
        [Tooltip("Distance to become not grounded")]
        [SerializeField]
        protected float groundMinDistance = 0.2f;
        [SerializeField]
        protected float groundMaxDistance = 0.5f;
        #endregion

        #region Character Variables

        public enum LocomotionType
        {
            FreeWithStrafe,
            OnlyStrafe,
            OnlyFree
        }        

        [Header("|   Locomotion Setup   |")]
        public LocomotionType locomotionType = LocomotionType.FreeWithStrafe;
        [Tooltip("Lock the player movement")]
        public bool lockMovement;
        [Tooltip("Speed of the rotation on free directional movement")]
        [SerializeField]
        public float freeRotationSpeed = 10f;
        [Tooltip("Speed of the rotation while strafe movement")]
        public float strafeRotationSpeed = 10f;

        [Header("|   Jump Options   |")]
        [Tooltip("Check to control the character while jumping")]
        public bool jumpAirControl = true;
        [Tooltip("How much time the character will be jumping")]
        public float jumpTimer = 0.3f;
        [HideInInspector]
        public float jumpCounter;
        [Tooltip("Add extra jump speed, based on your speed input the characters will move forward")]
        public float jumpForward = 3f;
        [Tooltip("Add extra jump height, if you want to jump only with root motion leave the value at 0")]
        public float jumpHeight = 4f;


        [Header("|   Movement Speed   |")]
        [Tooltip("Check to drive the character using RootMotion of the animation")]
        public bool useRootMotion = false;
        [Tooltip("Add extra speed for the locomotion movement, keep this value at 0 if you want to use only root motion speed.")]
        public float freeWalkSpeed = 2.5f;
        [Tooltip("Add extra speed for the locomotion movement, keep this value at 0 if you want to use only root motion speed.")]
        public float freeRunningSpeed = 3f;
        [Tooltip("Add extra speed for the locomotion movement, keep this value at 0 if you want to use only root motion speed.")]
        public float freeSprintSpeed = 4f;
        [Tooltip("Add extra speed for the strafe movement, keep this value at 0 if you want to use only root motion speed.")]
        public float strafeWalkSpeed = 2.5f;
        [Tooltip("Add extra speed for the locomotion movement, keep this value at 0 if you want to use only root motion speed.")]
        public float strafeRunningSpeed = 3f;
        [Tooltip("Add extra speed for the locomotion movement, keep this value at 0 if you want to use only root motion speed.")]
        public float strafeSprintSpeed = 4f;

        [Header("|   Grounded Setup   |")]
        [Tooltip("ADJUST IN PLAY MODE - Offset height limit for steps - GREY Raycast in front of the legs")]
        public float stepOffsetEnd = 0.45f;
        [Tooltip("ADJUST IN PLAY MODE - Offset height origin for steps, make sure to keep slight above the floor - GREY Raycast in front of the legs")]
        public float stepOffsetStart = 0.05f;
        [Tooltip("Higher value will result jittering on ramps, lower values will have difficulty on steps")]
        public float stepSmooth = 4f;
        [Tooltip("Max angle to walk")]
        [SerializeField]
        protected float slopeLimit = 45f;
        [Tooltip("Apply extra gravity when the character is not grounded")]
        [SerializeField]
        protected float extraGravity = -10f;
        protected float groundDistance;
        public RaycastHit groundHit;
        
        #endregion

        #region Actions
        [HideInInspector]
        public bool         // movement bools
            isMoving,
            isGrounded,
            isLashed,
            isEvading,
            isStrafing,
            isSprinting,
            isSliding;

        [HideInInspector]
        public bool         // action bools
            isJumping,
            isAiming;

        [HideInInspector]
        public bool         // combat bools
            isUnarmed = true;

        #endregion

        #region Direction Variables
        [HideInInspector]
        public Vector3 targetDirection;
        protected Vector3 gravityVector;
        protected Quaternion targetRotation;
        [HideInInspector]
        public Quaternion freeRotation;
        [HideInInspector]
        public bool keepDirection;
        [HideInInspector]
        public bool wasAiming;
        #endregion

        #region Aim Behaviour Variables
        public Texture2D crosshair;                                     // Crosshair Texture
        public float aimTurnSmoothing = 0.15f;                          // Speed of turn response when aiming to match camera facing.
        public Vector3 aimPivotOffset = new Vector3(0.5f, 1.2f, 0f);    // offset to repoint the camera when aiming.
        public Vector3 aimCameraOffset = new Vector3(0f, 1f, -1f);  // offset to relocate the camera when aiming.
        #endregion

        #region Components

        [HideInInspector]           
        public Animator animator;                   //  access the Animator component
        [HideInInspector]           
        public Rigidbody _rigidbody;                //  access the Rigidbody component
        [HideInInspector]               
        public PhysicMaterial                       //  create PhysicMaterial for the Rigidbody      
            maxFrictionPhysics,
            frictionPhysics, 
            slippyPhysics;   
        [HideInInspector]           
        public CapsuleCollider _capsuleCollider;    //  access CapsuleCollider information

        #endregion

        #region Hide Variables
        [HideInInspector]
        public float colliderHeight;                        // storage capsule collider extra information                
        [HideInInspector]
        public Vector2 input;                               // generate input for the controller     
        [HideInInspector]
        public KeyCode lastInput;                           // used to determine double tap inputs
        [HideInInspector]
        public float speed, direction, verticalVelocity;    // general variables to the locomotion
        [HideInInspector]
        public float velocity;                              // velocity to apply to rigidbody       


        #endregion

        #endregion

        /// <summary>
        /// this method is called on the Start of the ThirdPersonController
        /// </summary>
        public void Init()
        {
            // access components
            animator = GetComponent<Animator>();

            // slides the character through walls and edges
            frictionPhysics = new PhysicMaterial();
            frictionPhysics.name = "frictionPhysics";
            frictionPhysics.staticFriction = 0.25f;
            frictionPhysics.dynamicFriction = 0.25f;
            frictionPhysics.frictionCombine = PhysicMaterialCombine.Multiply;

            // prevents the collider from slipping on ramps
            maxFrictionPhysics = new PhysicMaterial();
            maxFrictionPhysics.name = "maxFrictionPhysics";
            maxFrictionPhysics.staticFriction = 1f;
            maxFrictionPhysics.dynamicFriction = 1f;
            maxFrictionPhysics.frictionCombine = PhysicMaterialCombine.Maximum;

            // air physics
            slippyPhysics = new PhysicMaterial();
            slippyPhysics.name = "slippyPhysics";
            slippyPhysics.staticFriction = 0f;
            slippyPhysics.dynamicFriction = 0f;
            slippyPhysics.frictionCombine = PhysicMaterialCombine.Minimum;

            // rigidbody information
            _rigidbody = GetComponent<Rigidbody>();

            // capsule collider information
            _capsuleCollider = GetComponent<CapsuleCollider>();
        }

        public virtual void UpdateMotor()
        {
            CheckGround();
            ControlJumpBehaviour();
            ControlAimBehaviour();
            ControlLocomotion();
        }

        #region Locomotion

        protected bool FreeLocomotionConditions
        {
            get
            {
                if (locomotionType.Equals(LocomotionType.OnlyStrafe)) isStrafing = true;
                return !isStrafing && !locomotionType.Equals(LocomotionType.OnlyStrafe) || locomotionType.Equals(LocomotionType.OnlyFree);
            }
        }

        void ControlLocomotion()
        {
            if (FreeLocomotionConditions)
                FreeMovement();     // free directional movement
            else
                StrafeMovement();   // move forward & backwards; strafe left & right            
        }

        void StrafeMovement()
        {
            var _speed = Mathf.Clamp(input.y, -1f, 1f);
            var _direction = Mathf.Clamp(input.x, -1f, 1f);
            speed = _speed;
            direction = _direction;
            if (isSprinting) speed += 0.5f;
            if (direction >= 0.7 || direction <= -0.7 || speed <= 0.01) isSprinting = false;

            if ((speed != 0 || direction != 0) && isGrounded)
                isMoving = true;
            else
                isMoving = false;
        }

        public virtual void FreeMovement()
        {
            // set speed to both vertical and horizontal inputs
            speed = Mathf.Abs(input.x) + Mathf.Abs(input.y);
            speed = Mathf.Clamp(speed, 0, 1f);

            // add 0.5f on sprint to change the animation on the animator
            if (isSprinting) speed += 0.5f;

            if (input != Vector2.zero && targetDirection.magnitude > 0.1f)
            {
                Vector3 lookDirection = targetDirection.normalized;
                freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
                var diferenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
                var eulerY = transform.eulerAngles.y;

                // apply free directional rotation while not turning180 animations
                if (isGrounded || (!isGrounded && jumpAirControl))
                {
                    if (diferenceRotation < 0 || diferenceRotation > 0)
                        eulerY = freeRotation.eulerAngles.y;
                    var euler = new Vector3(transform.eulerAngles.x, eulerY, transform.eulerAngles.z);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(euler), freeRotationSpeed * Time.deltaTime);
                }
            }

            if (speed != 0 && isGrounded)
                isMoving = true;
            else
                isMoving = false;
        }

        protected void ControlSpeed(float velocity)
        {
            if (Time.deltaTime == 0) return;

            if (useRootMotion)
            {
                Vector3 v = (animator.deltaPosition * (velocity > 0 ? velocity : 1f)) / Time.deltaTime;
                v.y = _rigidbody.velocity.y;
                _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, v, 20f * Time.deltaTime);
            }
            else
            {
                var velY = transform.forward * velocity * speed;
                velY.y = _rigidbody.velocity.y;
                var velX = transform.right * velocity * direction;
                velX.x = _rigidbody.velocity.x;

                if (isStrafing)
                {
                    Vector3 v = (transform.TransformDirection(new Vector3(input.x, 0, input.y)) * (velocity > 0 ? velocity : 1f));                    
                    v.y = _rigidbody.velocity.y;                    
                    _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, v, 20f * Time.deltaTime);
                }
                else 
                {
                    _rigidbody.velocity = velY;
                    _rigidbody.AddForce(transform.forward * (velocity * speed) * Time.deltaTime, ForceMode.VelocityChange);
                }
            }
        }

        #endregion

        #region Jump Methods

        protected void ControlJumpBehaviour()
        {
            if (!isJumping) return;

            jumpCounter -= Time.deltaTime;

            if (jumpCounter <= 0)
            {
                jumpCounter = 0;
                isJumping = false;
            }

            // apply extra force to the jump height
            var vel = _rigidbody.velocity;
            vel.y = jumpHeight;

            _rigidbody.velocity = vel;
        }
        
        public void AirControl()
        {
            if (isGrounded) return;
            if (!JumpFwdCondition) return;

            var velY = transform.forward * jumpForward * speed;
            velY.y = _rigidbody.velocity.y;
            var velX = transform.right * jumpForward * speed;
            velX.x = _rigidbody.velocity.x;

            if (jumpAirControl)
            {
                if (isStrafing)
                {
                    _rigidbody.velocity = new Vector3(velX.x, velY.y, _rigidbody.velocity.z);
                    var vel = transform.forward * (jumpForward * speed) + transform.right * (jumpForward * direction);
                    _rigidbody.velocity = new Vector3(vel.x, _rigidbody.velocity.y, vel.z);
                }
                else
                {
                    var vel = transform.forward * (jumpForward * speed);
                    _rigidbody.velocity = new Vector3(vel.x, _rigidbody.velocity.y, vel.z);
                }
            }
            else
            {
                var vel = transform.forward * (jumpForward);
                _rigidbody.velocity = new Vector3(vel.x, _rigidbody.velocity.y, vel.z);
            }
        }

        protected bool JumpFwdCondition
        {
            get
            {
                Vector3 p1 = transform.position + _capsuleCollider.center + Vector3.up * -_capsuleCollider.height * 0.5f;
                Vector3 p2 = p1 + Vector3.up * _capsuleCollider.height;
                return Physics.CapsuleCastAll(p1, p2, _capsuleCollider.radius * 0.5f, transform.forward, 0.6f, groundLayer).Length == 0;
            }
        }

        #endregion

        #region Aiming Methods
        protected void ControlAimBehaviour()
        {
            if (!isAiming && !wasAiming) return;

            if (!wasAiming)
                StartCoroutine(ToggleAimOn());
            else if (wasAiming)
                StartCoroutine(ToggleAimOff());
        }

        private IEnumerator ToggleAimOn()
        {
            yield return new WaitForSeconds(0.05f);     //TODO: Replace with public variable

            int signal = 1;
            aimCameraOffset.x = Mathf.Abs(aimCameraOffset.x) * signal;
            aimPivotOffset.x = Mathf.Abs(aimPivotOffset.x) * signal;
            
            yield return new WaitForSeconds(0.1f);            
        }
        private IEnumerator ToggleAimOff()
        {
            yield return new WaitForSeconds(0.3f);      //TODO: Replace with public variable

            int signal = 0;
            aimCameraOffset.x = Mathf.Abs(aimCameraOffset.x) * signal;
            aimPivotOffset.x = Mathf.Abs(aimPivotOffset.x) * signal;
            yield return new WaitForSeconds(0.05f);
        }
        #endregion

        #region Ground Check

        void CheckGround()
        {
            CheckGroundDistance();

            // change the physics material to very slip when not grounded or maxFriction when is
            if (isGrounded && input == Vector2.zero)
                _capsuleCollider.material = maxFrictionPhysics;
            else if (isGrounded && input != Vector2.zero)
                _capsuleCollider.material = frictionPhysics;
            else
                _capsuleCollider.material = slippyPhysics;

            var magVel = (float)System.Math.Round(new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z).magnitude, 2);
            magVel = Mathf.Clamp(magVel, 0, 1);

            var groundCheckDistance = groundMinDistance;
            if (magVel > 0.25f) groundCheckDistance = groundMaxDistance;

            // clear the checkground to free the character to attack on air
            var onStep = StepOffset();

            if (groundDistance <= 0.05f)
            {
                isGrounded = true;
                Sliding();
            }
            else
            {
                if (!isLashed)
                    gravityVector = transform.up;
                else
                    gravityVector = transform.up * -1;

                if (groundDistance >= groundCheckDistance)
                {
                    isGrounded = false;

                    // check vertical velocity
                    verticalVelocity = _rigidbody.velocity.y;

                    // apply extra gravity when falling
                    if (!onStep && !isJumping)
                        _rigidbody.AddForce(gravityVector * extraGravity * Time.deltaTime, ForceMode.VelocityChange);
                }
                else if (!onStep && !isJumping)
                {
                    _rigidbody.AddForce(gravityVector * (extraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);
                }
            }
        }

        void CheckGroundDistance()
        {
            if (_capsuleCollider != null)
            {
                // radius of the SphereCast
                float radius = _capsuleCollider.radius * 0.9f;
                var dist = 10f;
                
                // position of the SphereCast origin starting at the base of the capsule
                Vector3 pos = transform.position + Vector3.up * (_capsuleCollider.radius);
                
                // ray for Raycast
                Ray ray1 = new Ray(transform.position + new Vector3(0, colliderHeight / 2, 0), Vector3.down);
                
                // ray for Spherecast
                Ray ray2 = new Ray(pos, -Vector3.up);
                
                // raycast for ground check distance
                if (Physics.Raycast(ray1, out groundHit, colliderHeight / 2 + 2f, groundLayer))
                    dist = transform.position.y - groundHit.point.y;
                
                // sphere cast around the base of the capsule to check the ground distance
                if (Physics.SphereCast(ray2, radius, out groundHit, _capsuleCollider.radius + 2f, groundLayer))
                {
                    // check if sphereCast distance is smaller than the ray cast distance
                    if (dist > (groundHit.distance - _capsuleCollider.radius * 0.1f))
                        dist = (groundHit.distance - _capsuleCollider.radius * 0.1f);
                }

                groundDistance = (float)System.Math.Round(dist, 2);
            }
        }

        float GroundAngle()
        {
            var groundAngle = Vector3.Angle(groundHit.normal, Vector3.up);
            return groundAngle;
        }

        void Sliding()
        {
            var onStep = StepOffset();
            var groundAngle2 = 0f;
            RaycastHit hitInfo;
            Ray ray = new Ray(transform.position, -transform.up);

            if (Physics.Raycast(ray, out hitInfo, 1f, groundLayer))
            {
                groundAngle2 = Vector3.Angle(Vector3.up, hitInfo.normal);
            }

            if (GroundAngle() > slopeLimit + 1f && GroundAngle() <= 85 &&
                groundAngle2 > slopeLimit + 1f && groundAngle2 <= 85 &&
                groundDistance <= 0.05f && !onStep)
            {
                isSliding = true;
                isGrounded = false;
                var slideVelocity = (GroundAngle() - slopeLimit) * 2f;
                slideVelocity = Mathf.Clamp(slideVelocity, 1, 10);
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, -slideVelocity, _rigidbody.velocity.z);
            }
            else
            {
                isSliding = false;
                isGrounded = true;
            }
        }

        bool StepOffset()
        {
            if (input.sqrMagnitude < 0.1 || !isGrounded) return false;

            var _hit = new RaycastHit();
            var _movementDirection = isStrafing && input.magnitude > 0 ? (transform.right * input.x + transform.forward * input.y).normalized : transform.forward;
            Ray rayStep = new Ray((transform.position + new Vector3(0, stepOffsetEnd, 0) + _movementDirection * ((_capsuleCollider).radius + 0.05f)), Vector3.down);

            if (Physics.Raycast(rayStep, out _hit, stepOffsetEnd - stepOffsetStart, groundLayer) && !_hit.collider.isTrigger)
            {
                if (_hit.point.y >= (transform.position.y) && _hit.point.y <= (transform.position.y + stepOffsetEnd))
                {
                    var _speed = isStrafing ? Mathf.Clamp(input.magnitude, 0, 1) : speed;
                    var velocityDirection = isStrafing ? (_hit.point - transform.position): (_hit.point - transform.position).normalized;
                    _rigidbody.velocity = velocityDirection * stepSmooth * (_speed * (velocity > 1 ? velocity : 1));
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Camera Methods

        public virtual void RotateToTarget(Transform target)
        {
            if (target)
            {
                Quaternion rot = Quaternion.LookRotation(target.position - transform.position);
                var newPos = new Vector3(transform.eulerAngles.x, rot.eulerAngles.y, transform.eulerAngles.z);
                targetRotation = Quaternion.Euler(newPos);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newPos), strafeRotationSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Update the targetDirection variable using referenceTransform or just input.Rotate toword the referenceDirection
        /// </summary>
        /// <param name="referenceTransform"></param>
        public virtual void UpdateTargetDirection(Transform referenceTransform = null)
        {

            if (referenceTransform)
            {
                var forward = keepDirection ? referenceTransform.forward : referenceTransform.TransformDirection(Vector3.forward);
                forward.y = 0;

                forward = keepDirection ? forward : referenceTransform.TransformDirection(Vector3.forward);
                forward.y = 0;

                // get the right-facing direction of the referenceTransform
                var right = keepDirection ? referenceTransform.right : referenceTransform.TransformDirection(Vector3.right);

                // determine the direction the player will face based on input and the referenceTansform's right and forward directions
                targetDirection = input.x * right + input.y * forward;
            }
            else
                targetDirection = keepDirection ? targetDirection : new Vector3(input.x, 0, input.y);
        }
        #endregion

        #region GUI Methods
        // Draw the crosshair when aiming.
        void OnGUI()
        {
            if (crosshair && isAiming)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - (crosshair.width * 0.5f),
                                    Screen.height / 2 - (crosshair.height * 0.5f),
                                    crosshair.width, crosshair.height), crosshair);
            }
        }
        #endregion

    }
}