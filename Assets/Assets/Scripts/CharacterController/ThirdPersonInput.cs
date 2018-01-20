using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class ThirdPersonInput : MonoBehaviour
    {
        #region Variables

        #region Movement Variables
        [Header("|   Default Inputs   |")]
        public string horizontalInput = "Horizontal";
        public string verticalInput = "Vertical";

        [Header("|   Movement Inputs   |")]
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;
        public KeyCode evadeFrontInput = KeyCode.W;
        public KeyCode evadeBackInput = KeyCode.S;
        public KeyCode evadeLeftInput = KeyCode.A;
        public KeyCode evadeRightInput = KeyCode.D;
        #endregion

        #region Action Variables
        [Header("|   Action Inputs   |")]
        public KeyCode jumpInput = KeyCode.Space;
        public int aimMouseButton = 1;
        public KeyCode switchShoulderInput = KeyCode.Hash;
        #endregion

        #region Ability Variables
        [Header("|   Hotbar Inputs   |")]
        public KeyCode hotbar1Input = KeyCode.Alpha1;
        public KeyCode hotbar2Input = KeyCode.Alpha2;
        public KeyCode hotbar3Input = KeyCode.Alpha3;
        #endregion

        #region Camera Variables
        [Header("Camera Settings")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        protected ThirdPersonCamera tpCamera;       // access camera information
        #endregion

        protected ThirdPersonController cc;         // access to the ThirdPersonController component
        protected EntityInterface.PlayerManager pm; // access to the PlayerManager component
        
        #region Hide Variables
        [HideInInspector]
        public string customCameraState;            // generic string to change the CameraState
        [HideInInspector]
        public string customLookAtPoint;            // generic string to change the CameraPoint of the Fixed Point Mode
        [HideInInspector]
        public bool changeCameraState;              // generic bool to change the CameraState
        [HideInInspector]
        public bool smoothCameraState;              // generic bool to know if the state will change with or without lerp
        [HideInInspector]
        public bool keepDirection;                  // keep the current direction in case you change the CameraState
        #endregion
        
        public float dbltapThreshold = 0.5f;
        public float lastInputTime = 0f;
        public KeyCode previousInput;
        
        #endregion

        protected virtual void Start()
        {
            CharacterInit();
        }

        protected virtual void CharacterInit()
        {
            cc = GetComponent<ThirdPersonController>();
            if (cc != null)
                cc.Init();

            pm = GetComponent<EntityInterface.PlayerManager>();
            if (pm != null)
                pm.Init();                

            tpCamera = FindObjectOfType<ThirdPersonCamera>();
            if (tpCamera) tpCamera.SetMainTarget(this.transform);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            lastInputTime = 0f;
        }

        protected virtual void LateUpdate()
        {
            if (cc == null) return;                 // returns if we didn't find the controller
            InputHandle();                          // update input methods
            UpdateCameraStates();                   // update camera states
        }

        protected virtual void FixedUpdate()
        {
            cc.AirControl();
            CameraInput();
            CameraPivetControl();
        }

        protected virtual void Update()
        {
            cc.UpdateMotor();                       // call ThirdPersonMotor methods
            cc.UpdateAnimator();                    // call ThirdPersonMotor methods
        }

        protected virtual void InputHandle()
        {
            ExitGameInput();
            CameraInput();

            if (!cc.lockMovement)
            {
                MoveCharacter();
                EvadeInput();
                SprintInput();
                StrafeInput();
                JumpInput();
            }

            AimInput();                         // custom call to handle aiming
            AbilityInput();                     // custom call to handle abilities
        }

        #region Basic Locomotion Inputs

        protected virtual void MoveCharacter()
        {
            cc.input.x = Input.GetAxis(horizontalInput);
            cc.input.y = Input.GetAxis(verticalInput);
        }

        protected virtual void EvadeInput()
        {
            if (Input.GetKeyDown(evadeFrontInput) && DoubleTap(evadeFrontInput))
                cc.Evade("Unarmed-Roll-Forward");

            if (Input.GetKeyDown(evadeBackInput) && DoubleTap(evadeBackInput))
                cc.Evade("Unarmed-Roll-Backward");

            if (Input.GetKeyDown(evadeLeftInput) && DoubleTap(evadeLeftInput))
                cc.Evade("Unarmed-Roll-Left");

            if (Input.GetKeyDown(evadeRightInput) && DoubleTap(evadeRightInput))
                cc.Evade("Unarmed-Roll-Right");
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
                cc.Strafe();
        }

        protected virtual void SprintInput()
        { 
            if (Input.GetKeyDown(sprintInput))
                cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput))
                cc.Jump();
        }

        protected virtual void AimInput()
        {
            if (Input.GetMouseButtonDown(aimMouseButton))
                cc.Aim(true);
            else if (Input.GetMouseButtonUp(aimMouseButton))
                cc.Aim(false);
            else if (Input.GetKeyDown(switchShoulderInput))
                cc.ChangeShoulder();
        }

        protected virtual void ExitGameInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!Cursor.visible)
                    Cursor.visible = true;
                else
                    Application.Quit();
            }
        }

        #endregion

        #region Ability Inputs

        protected virtual void AbilityInput()
        {
            if (Input.GetMouseButtonDown(0))
                pm.FireAbility();
            if (Input.GetMouseButtonDown(1))
                pm.CancelAbility();

            if (Input.GetKeyDown(hotbar1Input))
                pm.SetAbility(1);
            if (Input.GetKeyDown(hotbar2Input))
                pm.SetAbility(2);
            if (Input.GetKeyDown(hotbar3Input))
                pm.SetAbility(3);
        }

        #endregion
        
        protected bool DoubleTap(KeyCode currentInput)
        {
            if ((previousInput == currentInput) && (Time.time - lastInputTime < dbltapThreshold))
                return true;

            previousInput = currentInput;
            lastInputTime = Time.time;
            return false;
        }

        #region Camera Methods

        protected virtual void CameraInput()
        {
            if (tpCamera == null)
                return;
            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);

            tpCamera.RotateCamera(X, Y);

            // transform Character direction from camera if not KeepDirection
            if (!keepDirection)
                cc.UpdateTargetDirection(tpCamera != null ? tpCamera.transform : null);

            // rotate the character with the camera while strafing
            RotateWithCamera(tpCamera != null ? tpCamera.transform : null);
        }

        protected virtual void UpdateCameraStates()
        {
            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<ThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }
        }

        protected virtual void RotateWithCamera(Transform cameraTransform)
        {
            if (cc.isStrafing && !cc.lockMovement)
            {
                cc.RotateWithAnotherTransform(cameraTransform);
            }
        }

        protected virtual void CameraPivetControl()
        {
            if (!cc.isAiming)
                tpCamera.ResetTargetOffsets();
            else
                tpCamera.SetTargetOffsets(cc.aimCameraOffset, cc.aimCameraOffset);
        }

        #endregion
    }
}
