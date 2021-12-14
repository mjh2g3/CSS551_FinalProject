using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VRMainController : MonoBehaviour
{
    // Controller Handle
    private bool handleSelected = false;
    private Vector3 prevControllerPos;
    private Vector3 depthBuffer = new Vector3(0, 0, 20);

    //Controller Buttons
    private bool buttonSelected = false;
    public NodePrimitive handle;
    public NodePrimitive dropBtn;
    public NodePrimitive releaseBtn;
    public NodePrimitive resetBtn;
    private string pressedButton;
    private bool Drop = false;
    private bool Lift = false;
    private float timer = 0.0f;

    private bool ComputeHandleDetectionRayCast(Vector3 pos1, Vector3 pos2)
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

    private bool ComputeHandleDetection(Vector3 pos1)
    {
        bool hit = false;
        Vector3 X = handle.GetLocalPosition() - pos1;

        Vector3 scaleHandle = handle.GetLocalScale();

        float d;
        d = X.magnitude;

        if (d < 1)
        {
            hit = true;
        }

        return hit;
    }


    private bool ComputeDropDetectionRayCast(Vector3 pos1, Vector3 pos2)
    {
        bool hit = false;
        //Step 1: Compute the vector between the camera position (pos1) and hit position (pos2)
        
        Vector3 V = pos2 - pos1;
        float len = V.magnitude;
        V = V / len;

        Vector3 X = dropBtn.GetLocalPosition() - pos1;
        float h = Vector3.Dot(X, V);

        Vector3 scaleBtn = dropBtn.GetLocalScale();
        float r = scaleBtn.x * 0.5f;

        float d;

        d = Mathf.Sqrt(X.sqrMagnitude - (h * h));
        if (d < r)
        {
            hit = true;
        }

        return hit;
    }

    private bool ComputeDropDetection(Vector3 pos1)
    {
        bool hit = false;
        // Debug.Log("dropBtn: " + dropBtn.GetLocalPosition());
        // Debug.Log("pos1: " + pos1);

        Vector3 X = dropBtn.GetLocalPosition() - pos1;

        Vector3 scaleBtn = dropBtn.GetLocalScale();
        float r = scaleBtn.y * 0.5f;

        float d = X.magnitude;

        if (d < 1)
        {
            hit = true;
        }

        return hit;
    }

    private bool ComputeResetDetectionRayCast(Vector3 pos1, Vector3 pos2)
    {
        bool hit = false;
        //Step 1: Compute the vector between the camera position (pos1) and hit position (pos2)
        
        Vector3 V = pos2 - pos1;
        float len = V.magnitude;
        V = V / len;

        Vector3 X = resetBtn.GetLocalPosition() - pos1;
        float h = Vector3.Dot(X, V);

        Vector3 scaleBtn = resetBtn.GetLocalScale();
        float r = scaleBtn.x * 0.5f;

        float d;

        d = Mathf.Sqrt(X.sqrMagnitude - (h * h));
        if (d < r)
        {
            hit = true;
        }
        return hit;
    }

    private bool ComputeResetDetection(Vector3 pos1)
    {
        bool hit = false;
        
        Vector3 X = resetBtn.GetLocalPosition() - pos1;

        Vector3 scaleBtn = resetBtn.GetLocalScale();
        float r = scaleBtn.x * 0.5f;

        float d;
        d = X.magnitude;

        if (d < 1)
        {
            hit = true;
        }

        return hit;
    }

    private void PressClosestButton(Vector3 pos1) 
    {
        float dropBtnDist, resetBtnDist, releaseBtnDist;
        dropBtnDist = (dropBtn.GetLocalPosition() - pos1).sqrMagnitude;
        resetBtnDist = (resetBtn.GetLocalPosition() - pos1).sqrMagnitude;
        releaseBtnDist = (releaseBtn.GetLocalPosition() - pos1).sqrMagnitude;
        float[] buttons = {dropBtnDist, resetBtnDist, releaseBtnDist};
        if (Mathf.Min(buttons) == dropBtnDist) 
        {
            Debug.Log("You hit the drop button!");
            if (!mModel.clawFull)
            {
                Drop = true;
                mModel.clawActionFlag = "drop";
            }    
            mModel.PushButton(0);
        } 
        else if (Mathf.Min(buttons) == releaseBtnDist)
        {
            Debug.Log("You hit the release button!");
            mModel.PushButton(1);
            if (mModel.clawFull)
                mModel.DropPrize();
        }
        else 
        {
            Debug.Log("You hit the reset button!");
            mModel.PushButton(2);
            mModel.ResetClawPosition();
        }
    }
}
