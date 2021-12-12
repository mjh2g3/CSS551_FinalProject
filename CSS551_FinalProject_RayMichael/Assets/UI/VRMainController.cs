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
        if (triggerValue > 0.1f)
        {
            Debug.Log("Left trigger pressed");
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(LeftHand.position, LeftHand.forward, out hitInfo, Mathf.Infinity);
            if (hit)
            {
                if (ComputeHandleDetection(LeftHand.position, hitInfo.point))
                {
                    handleSelected = true;
                    //prevMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition + depthBuffer);
                }
                else if (ComputeDropDetection(LeftHand.position, hitInfo.point))
                {
                    Drop = true;
                    mModel.clawActionFlag = "drop";
                    mModel.PushButton(0);
                }
                else if (ComputeResetDetection(LeftHand.position, hitInfo.point))
                {
                    Debug.Log("You hit the reset button!");
                    mModel.PushButton(1);
                    ResetClaw();
                }
            }
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
                if (ComputeHandleDetection(RightHand.position, hitInfo.point))
                {
                    //handleSelected = true;
                    //prevMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition + depthBuffer);
                }
                else if (ComputeDropDetection(RightHand.position, hitInfo.point))
                {
                    Drop = true;
                    mModel.clawActionFlag = "drop";
                    mModel.PushButton(0);
                }
                else if (ComputeResetDetection(RightHand.position, hitInfo.point))
                {
                    Debug.Log("You hit the reset button!");
                    mModel.PushButton(1);
                    ResetClaw();
                }
            }
        }
    }

}
