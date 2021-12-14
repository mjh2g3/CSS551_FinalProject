using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CraneCam : MonoBehaviour
{
    public Camera craneCam = null;
    private InputDevice leftController;
    private InputDevice rightController;
    private float curDegrees = 0.0f;
    private Vector3 curAxis = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(craneCam != null);

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
            if (craneCam.enabled)
            {
                CraneCamMove();
            }

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

    private void CraneCamMove()
    {
        leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joyV1);
        if (joyV1.y == 1.0f)
        {
            if (curDegrees < 45.0f)
            {
                curDegrees += 1.0f * Time.deltaTime;
                Debug.Log(curDegrees);
            }
            curAxis = craneCam.transform.right;
        }
        else if (joyV1.y == -1.0f)
        {
            //Move down
            curAxis = craneCam.transform.right;

        }
        else if (joyV1.x == 1.0f)
        {
            //Move right
        }
        else if (joyV1.x == -1.0f)
        {
            //move left
        }

        rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joyV2);
        if (joyV2.y == 1.0f)
        {
            //move up
            curAxis = craneCam.transform.right;

        }
        else if (joyV2.y == -1.0f)
        {
            //Move down
            curAxis = craneCam.transform.right;

        }
        else if (joyV2.x == 1.0f)
        {
            //Move right
        }
        else if (joyV2.x == -1.0f)
        {
            //move left
        }

        Quaternion q = Quaternion.AngleAxis(curDegrees, curAxis);
        craneCam.transform.localRotation *= q;
    }
}
