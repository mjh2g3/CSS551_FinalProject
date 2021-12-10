using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;

public class VR_Test1 : MonoBehaviour
{
    public ActionBasedController controllerLeft;
    public ActionBasedController controllerRight;

    // Start is called before the first frame update
    void Start()
    {
        bool enabled = controllerLeft.enableInputActions;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
