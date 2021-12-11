using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VR_Test1 : MonoBehaviour
{
    private InputDevice targetDevice;
    private List<InputDevice> devices;
    // Start is called before the first frame update
    void Start()
    {
        InitDevices();
    }

    private void InitDevices()
    {
        devices = new List<InputDevice>();


        //InputDeviceCharacteristics rightControllerChars = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        //InputDevices.GetDevicesWithCharacteristics(rightControllerChars, devices);

        InputDevices.GetDevices(devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        /*
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (devices.Count < 5 )
        {
            InitDevices();
        }
        
        /*
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue)
        {
            Debug.Log("Pressing Primary Button");
        }

        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        if (triggerValue > 0.1f)
        {
            Debug.Log("Trigger Pressed + " + triggerValue);
        }
        */
    }

    /*
    public class HandPresence : MonoBehaviour
    {
        private InputDevice _targetDevice;

        void Start()
        {
            TryInitialize();
        }

        void TryInitialize()
        {
            var inputDevices = new List<InputDevice>();
            InputDeviceCharacteristics rightControllerCharacteristics =
                InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
            InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, inputDevices);

            if (inputDevices.Count == 0)
            {
                return;
            }

            _targetDevice = inputDevices[0];
        }

        void Update()
        {
            if (!_targetDevice.isValid)
            {
                TryInitialize();
            }
            else
            {
                // Do what you would like with _targetDevice here
            }
        }
    */
    }
