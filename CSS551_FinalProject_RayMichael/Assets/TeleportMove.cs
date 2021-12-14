using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportMove : MonoBehaviour
{
    private InputDevice leftController;
    private InputDevice rightController;
    public bool teleportActive = false;
    public Transform LeftHand = null;
    private bool currentlySwitching = false;

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
            TeleportOnOff();

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

    private void TeleportOnOff()
    {
        leftController.TryGetFeatureValue(CommonUsages.grip, out float gripValue);
        if (gripValue > 0.0f)
        {
            if (!currentlySwitching)
            {
                if (!teleportActive)
                {
                    XRRayInteractor xri = LeftHand.GetComponent<XRRayInteractor>();
                    xri.raycastMask = 8;
                    teleportActive = true;
                }
                else if (teleportActive)
                {
                    XRRayInteractor xri = LeftHand.GetComponent<XRRayInteractor>();
                    xri.raycastMask = -1;
                    teleportActive = false;
                }
                currentlySwitching = true;

            }

        }
        else
        {
            currentlySwitching = false;
        }
    }

}
