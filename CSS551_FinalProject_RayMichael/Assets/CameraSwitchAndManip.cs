using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraSwitchAndManip : MonoBehaviour
{
    public Camera mainCam = null;
    public Camera craneCam = null;
    private bool currentlySwitching = false;
    private InputDevice leftController;
    private InputDevice rightController;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(mainCam != null);
        Debug.Log(craneCam != null);

        mainCam.enabled = true;
        craneCam.enabled = false;
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
            CameraSwitch();
            
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

    private void CameraSwitch()
    {
        leftController.TryGetFeatureValue(CommonUsages.grip, out float gripValue);
        if (gripValue > 0.1f)
        {
            Debug.Log("Pressing grip");
            Debug.Log("The grip value = " + gripValue);

            //Perform camera switch
            if (mainCam.enabled)
            {
                mainCam.enabled = false;
                craneCam.enabled = true;
            }
            else if (craneCam.enabled)
            {
                craneCam.enabled = false;
                mainCam.enabled = true;
            }
        }

        rightController.TryGetFeatureValue(CommonUsages.grip, out float gripValue1);
        if (gripValue1 > 0.1f)
        {
            Debug.Log("Pressing grip");
            Debug.Log("The grip value = " + gripValue1);

            //Perform camera switch
            if (mainCam.enabled)
            {
                mainCam.enabled = false;
                craneCam.enabled = true;
            }
            else if (craneCam.enabled)
            {
                craneCam.enabled = false;
                mainCam.enabled = true;
            }
        }
    }
}
