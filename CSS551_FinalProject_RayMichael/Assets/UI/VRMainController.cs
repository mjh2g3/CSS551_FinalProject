using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public partial class VRMainController : MonoBehaviour
{
    //public TheWorld mModel = null;
    public VRTheWorld mModel = null;
    //MotionControllerVariables
    public Transform LeftHand;
    public Transform RightHand;
    private InputDevice leftController;
    private InputDevice rightController;

    //private bool leftTrigger = false;
    //private bool rightTrigger = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(mModel != null);
        
        Debug.Assert(handle != null);
        Debug.Assert(dropBtn != null);
        Debug.Assert(resetBtn != null);
    }

    // Update is called once per frame
    void Update()
    {
        if ((!leftController.isValid) | (!rightController.isValid))
        {
            InitDevices();
        }
        else
        {
            //CraneMovement();
            //CamManipulation();
            //LMB(); Replace with LeftHand and RightHand MotionControllerButtons (LMCB, RMCB)
            LMCB();
            RMCB();
            PlayerMovement();
            if (Drop)
            {
                DropClaw();
            }
            if (Lift)
            {
                LiftClaw();
            }

            //ClawMovement();
        }

    }

    private void InitDevices()
    {
        //LeftController
        var leftControlDevices = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerChars =
            InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerChars, leftControlDevices);

        if (leftControlDevices.Count == 0)
        {
            return;
        }

        leftController = leftControlDevices[0];

        //RightController
        var rightControlDevices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerChars =
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerChars, rightControlDevices);

        if (rightControlDevices.Count == 0)
        {
            return;
        }

        rightController = rightControlDevices[0];
    }

    private void LMCB()
    {
        leftController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        if (triggerValue > 0.7f) {
            if (!handleSelected && !buttonSelected)
            {
                Debug.Log("Left trigger pressed");
                
                bool HandleOperation, DropBtnOperation, ResetBtnOperation;
                HandleOperation = ComputeHandleDetection(LeftHand.position);
                DropBtnOperation = ComputeDropDetection(LeftHand.position);
                ResetBtnOperation = ComputeResetDetection(LeftHand.position);
                if (HandleOperation)
                {
                    handleSelected = true;
                    prevControllerPos = LeftHand.position;
                } 
                else if (DropBtnOperation && ResetBtnOperation) 
                {
                    Debug.Log("closest button pressed");
                    buttonSelected = true;
                    ClosestButton(LeftHand.position);
                }
            } 
            else if (handleSelected)
            {
                mModel.UpdateJointRotation(prevControllerPos, LeftHand.position);
            }
            else if (buttonSelected)
            {
                Debug.Log("Claw dropping/resetting");
            }
        }
        else
        {
            buttonSelected = false;
            handleSelected = false;
            Debug.Log("Left trigger released");
        }

    }  

    private void RMCB()
    {
        rightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        if (triggerValue > 0.1f)
        {
            Debug.Log("Right trigger pressed");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(RightHand.position, RightHand.forward, out hitInfo, Mathf.Infinity);
            if (hit)
            {
                if (ComputeHandleDetectionRayCast(RightHand.position, hitInfo.point))
                {
                    handleSelected = true;
                    //prevMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition + depthBuffer);
                }
                else if (ComputeDropDetectionRayCast(RightHand.position, hitInfo.point))
                {
                    Drop = true;
                    mModel.clawActionFlag = "drop";
                    mModel.PushButton(0);
                }
                else if (ComputeResetDetectionRayCast(RightHand.position, hitInfo.point))
                {
                    Debug.Log("You hit the reset button!");
                    mModel.PushButton(1);
                    ResetClaw();
                }
            }
        }
    }

    private void PlayerMovement() {
        bool touchpad = leftController.TryGetFeatureValue(CommonUsages.secondary2DAxis, out Vector2 touchpadValue);
        mModel.MovePlayer(touchpadValue.x, touchpadValue.y);
    }

}
