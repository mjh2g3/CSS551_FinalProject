using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VRMainController : MonoBehaviour
{
    // Controller Handle
    private bool handleSelected = false;
    private Vector3 prevMousePos;
    private Vector3 depthBuffer = new Vector3(0, 0, 20);

    //Controller Buttons
    public NodePrimitive handle;
    public NodePrimitive dropBtn;
    public NodePrimitive resetBtn;
    private bool Drop = false;
    private bool Lift = false;
    private float timer = 0.0f;

    private bool ComputeHandleDetection(Vector3 pos1, Vector3 pos2)
    {
        bool hit = false;
        //Step 1: Compute the vector between the camera position (pos1) and hit position (pos2)
        
        Vector3 V = pos2 - pos1;
        float len = V.magnitude;
        V = V / len;

        Vector3 X = handle.GetLocalPosition() - pos1;
        float h = Vector3.Dot(X, V);

        Vector3 scaleHandle = handle.GetLocalScale();
        float r = scaleHandle.x * 0.5f;

        float d;

        d = Mathf.Sqrt(X.sqrMagnitude - (h * h));
        if (d < r)
        {
            hit = true;
        }

        return hit;
    }

    private bool ComputeDropDetection(Vector3 pos1, Vector3 pos2)
    {
        bool hit = false;
        //Step 1: Compute the vector between the camera position (pos1) and hit position (pos2)
        
        Vector3 V = pos2 - pos1;
        float len = V.magnitude;
        V = V / len;

        Vector3 X = dropBtn.GetLocalPosition() - pos1;
        float h = Vector3.Dot(X, V);

        Vector3 scaleHandle = dropBtn.GetLocalScale();
        float r = scaleHandle.x * 0.5f;

        float d;

        d = Mathf.Sqrt(X.sqrMagnitude - (h * h));
        if (d < r)
        {
            hit = true;
        }

        return hit;
    }

    private bool ComputeResetDetection(Vector3 pos1, Vector3 pos2)
    {
        bool hit = false;
        //Step 1: Compute the vector between the camera position (pos1) and hit position (pos2)
        
        Vector3 V = pos2 - pos1;
        float len = V.magnitude;
        V = V / len;

        Vector3 X = resetBtn.GetLocalPosition() - pos1;
        float h = Vector3.Dot(X, V);

        Vector3 scaleHandle = resetBtn.GetLocalScale();
        float r = scaleHandle.x * 0.5f;

        float d;

        d = Mathf.Sqrt(X.sqrMagnitude - (h * h));
        if (d < r)
        {
            hit = true;
        }
        return hit;
    }
}
