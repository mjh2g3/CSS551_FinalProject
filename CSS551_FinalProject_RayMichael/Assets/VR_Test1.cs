using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VR_Test1 : MonoBehaviour
{
    private InputDevice targetDevice;
    private List<InputDevice> devices;

    private InputDevice headSet;
    private InputDevice leftController;
    private InputDevice rightController;

    private bool leftTrigger = false;
    private bool rightTrigger = false;

    public Transform LeftHand;
    public Transform RightHand;

    // Start is called before the first frame update
    void Start()
    {
        InitDevices();
    }

    private void InitDevices()
    {
        //Headset
        /*
        var headSetDevices = new List<InputDevice>();
        InputDeviceCharacteristics headSetChars =
            InputDeviceCharacteristics.Camera | InputDeviceCharacteristics.HeadMounted;
        InputDevices.GetDevicesWithCharacteristics(headSetChars, headSetDevices);
        
        if (headSetDevices.Count == 0)
        {
            return;
        }

        headSet = headSetDevices[0];
        */

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

    // Update is called once per frame
    void Update()
    {
        if ((!leftController.isValid) | (!rightController.isValid))
        {
            InitDevices();
        }
        else
        {
            TestButtons();
            if ((leftTrigger) | (rightTrigger))
            {
                TestRayCast();
            }
        }
    }

    private void TestButtons()
    {
        
        leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue);
        if (primary2DAxisValue != Vector2.zero)
        {
            Debug.Log("Pressing Primary 2D Axis");
            Debug.Log("The vector2 = " + primary2DAxisValue);
        }

        leftController.TryGetFeatureValue(CommonUsages.secondary2DAxis, out Vector2 secondary2DAxisValue);
        if (secondary2DAxisValue != Vector2.zero)
        {
            Debug.Log("Pressing Secondary 2D Axis");
            Debug.Log("The vector2 = " + secondary2DAxisValue);
        }

        leftController.TryGetFeatureValue(CommonUsages.grip, out float gripValue);
        if (gripValue > 0.0f)
        {
            Debug.Log("Pressing grip");
            Debug.Log("The grip value = " + gripValue);
        }

        leftController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        if (triggerValue > 0.1f)
        {
            Debug.Log("Trigger Pressed + " + triggerValue);
            leftTrigger = true;
        }
        //RightHand
        rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue1);
        if (primary2DAxisValue1 != Vector2.zero)
        {
            Debug.Log("Pressing Primary 2D Axis");
            Debug.Log("The vector2 = " + primary2DAxisValue1);
        }

        rightController.TryGetFeatureValue(CommonUsages.secondary2DAxis, out Vector2 secondary2DAxisValue1);
        if (secondary2DAxisValue1 != Vector2.zero)
        {
            Debug.Log("Pressing Secondary 2D Axis");
            Debug.Log("The vector2 = " + secondary2DAxisValue1);
        }

        rightController.TryGetFeatureValue(CommonUsages.grip, out float gripValue1);
        if (gripValue1 > 0.0f)
        {
            Debug.Log("Pressing grip");
            Debug.Log("The grip value = " + gripValue1);
        }

        rightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue1);
        if (triggerValue1 > 0.1f)
        {
            Debug.Log("Trigger Pressed + " + triggerValue1);
            rightTrigger = true;
        }

    }

    private void TestRayCast()
    {
        RaycastHit hitInfo = new RaycastHit();

        if (leftTrigger)
        {
            //Ray ray = new Ray(LeftHand.localPosition, LeftHand.forward);
            bool hit = Physics.Raycast(LeftHand.position, LeftHand.forward, out hitInfo, Mathf.Infinity);
            if (hit)
            {
                Debug.Log("Left controller ray cast hitinfo: " + hitInfo.point);
                Debug.Log("Left controller ray cast hitinfo: " + hitInfo.transform.name);
                leftTrigger = false;
            }
        }
        
        if (rightTrigger)
        {
            //Ray ray = new Ray(RightHand.localPosition, RightHand.forward);
            bool hit = Physics.Raycast(RightHand.position, RightHand.forward, out hitInfo, Mathf.Infinity);
            if (hit)
            {
                Debug.Log("Right controller ray cast hitinfo: " + hitInfo.point);
                Debug.Log("Right controller ray cast hitinfo: " + hitInfo.transform.name);
                rightTrigger = false;
            }

        }

    }
}
