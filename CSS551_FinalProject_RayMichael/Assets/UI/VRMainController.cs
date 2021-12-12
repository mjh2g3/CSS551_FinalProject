using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRMainController : MonoBehaviour
{
    //MotionControllerVariables
    public Transform LeftHand;
    public Transform RightHand;
    private InputDevice leftController;
    private InputDevice rightController;

    private bool leftTrigger = false;
    private bool rightTrigger = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
            //
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
}
