using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GrabControllerObj : MonoBehaviour
{
    public Transform controller = null;
    private InputDevice leftController;
    private InputDevice rightController;

    public Transform LeftHand;
    public Transform RightHand;

    private bool ctrlSelected = false;
    private Vector3 initPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log(controller != null);

        
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
            GrabController();

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

    private void GrabController()
    {
        leftController.TryGetFeatureValue(CommonUsages.trigger, out float trigL);
        if (trigL > 0.1f)
        {
            Debug.Log("Left trigger pressed");
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(LeftHand.position, LeftHand.forward, out hitInfo, Mathf.Infinity);
            if (hit)
            {
                if ((ComputeContrlDetection(LeftHand.position, hitInfo.point)) && (!ctrlSelected))
                {
                    ctrlSelected = true;
                    initPos = hitInfo.point;
                }
                else if ((ComputeContrlDetection(LeftHand.position, hitInfo.point)) && (ctrlSelected))
                {
                    MoveController(initPos, hitInfo.point);
                }
            }
        }

        rightController.TryGetFeatureValue(CommonUsages.trigger, out float trigR);
        if (trigR > 0.1f)
        {
            Debug.Log("Right trigger pressed");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(RightHand.position, RightHand.forward, out hitInfo, Mathf.Infinity);
            if (hit)
            {
                if (ComputeContrlDetection(RightHand.position, hitInfo.point))
                {
                    //handleSelected = true;
                    //prevMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition + depthBuffer);
                }
                
            }
        }
    }

    private bool ComputeContrlDetection(Vector3 pos1, Vector3 pos2)
    {
        SceneNode cn = controller.GetComponent<SceneNode>();
        NodePrimitive cnChildNode = cn.PrimitiveList[0];
        Vector3 ctrlLocalPosition = cnChildNode.GetLocalPosition();
        Vector3 ctrlLocalScale = cnChildNode.GetLocalScale();
        bool hit = false;
        //Step 1: Compute the vector between the camera position (pos1) and hit position (pos2)

        Vector3 V = pos2 - pos1;
        float len = V.magnitude;
        V = V / len;

        Vector3 X = ctrlLocalPosition - pos1;
        float h = Vector3.Dot(X, V);

        Vector3 controllerBox = ctrlLocalScale;
        float r = controllerBox.x * 0.5f;

        float d;

        d = Mathf.Sqrt(X.sqrMagnitude - (h * h));
        if (d < r)
        {
            hit = true;
        }

        return hit;
    }

    private void MoveController(Vector3 pos1, Vector3 pos2)
    {
        SceneNode cn = controller.GetComponent<SceneNode>();
        NodePrimitive cnChildNode = cn.PrimitiveList[0];
        Vector3 ctrlLocalPosition = cnChildNode.GetLocalPosition();
        Vector3 ctrlLocalScale = cnChildNode.GetLocalScale();
        bool hit = false;
        //Step 1: Compute the vector between the camera position (pos1) and hit position (pos2)

        Vector3 V = pos2 - pos1;
        float len = V.magnitude;
        V = V / len;

        Vector3 X = ctrlLocalPosition - pos1;
        float h = Vector3.Dot(X, V);

        Vector3 controllerBox = ctrlLocalScale;
        float r = controllerBox.x * 0.5f;

        float d;

        d = Mathf.Sqrt(X.sqrMagnitude - (h * h));
        if (d < r)
        {
            hit = true;
        }
    }
}
