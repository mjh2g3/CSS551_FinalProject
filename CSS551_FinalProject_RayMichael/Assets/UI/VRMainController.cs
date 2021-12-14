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
            if (!handleSelected)
            {
                Debug.Log("Left trigger pressed");
                
                bool HandleOperation;
                HandleOperation = ComputeHandleDetection(LeftHand.position);
                if (HandleOperation)
                {
                    handleSelected = true;
                    prevControllerPos = LeftHand.position;
                }
            } 
            else if (handleSelected)
            {
                mModel.UpdateJointRotation(prevControllerPos, LeftHand.position);
            }
        }
        else
        {
            handleSelected = false;
            // Debug.Log("Left trigger released");
        }
    }  

    private void RMCB()
    {
        rightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        if (triggerValue > 0.7f) {
            if (!buttonSelected)
            {
                Debug.Log("Right trigger pressed");
                
                bool DropBtnOperation, ResetBtnOperation;
                DropBtnOperation = ComputeDropDetection(RightHand.position);
                ResetBtnOperation = ComputeResetDetection(RightHand.position);
                if (DropBtnOperation || ResetBtnOperation) 
                {
                    buttonSelected = true;
                    PressClosestButton(RightHand.position);
                }
            }
        }
        else
        {

            buttonSelected = false;
            // Debug.Log("Right trigger released");
        }
    }

    private void PlayerMovement() {
        bool touchpad = leftController.TryGetFeatureValue(CommonUsages.secondary2DAxis, out Vector2 touchpadValue);
        mModel.MovePlayer(touchpadValue.x, touchpadValue.y);
    }

}
