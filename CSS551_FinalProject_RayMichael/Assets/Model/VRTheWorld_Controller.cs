using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VRTheWorld : MonoBehaviour
{
    public Transform jointBaseNode = null;
    public Transform jointEndNode = null;
    private Vector3 stickNormal;
    public Transform dropBtnNode = null;
    public Transform resetBtnNode = null;

    private Transform btnSelected;
    private float speed = 0.05f;
    private bool buttonPressDown = false;
    private bool buttonPressUp = false;
    //private bool joystickMove = false;

    private Vector2 mDir2 = Vector2.up;

    //public void UpdateJointRotation(float dx, float dy)
    //public void UpdateJointRotation(Vector2 p0, Vector2 p1, float kPixelToDegree)
    public Vector3 UpdateJointRotation(Vector3 p2, Vector3 pHandle)
    {
        Vector3 v = p2 - jointBaseNode.localPosition;
        float vMagnitude = v.magnitude;
        v /= vMagnitude;

        Vector3 vHandle = pHandle - jointBaseNode.localPosition;
        float d = Vector3.Dot(vHandle, v);

        Vector3 vNextEnd = jointBaseNode.localPosition + v * d;
        Debug.Log("vNextEnd point = " + vNextEnd);
        Vector3 vNext = vNextEnd - jointBaseNode.localPosition;
        Debug.Log("joint node pos  " + jointBaseNode.localPosition);


        //jointBaseNode.rotation = Quaternion.FromToRotation(vHandle, vNextEnd);
        jointBaseNode.rotation = Quaternion.FromToRotation(vNextEnd, vHandle);

        return vNext;//

    }

    public void UpdateJointRotation2(Vector3 prevMousePos, Vector3 currentMousePos)
    {
        Vector3 jointNodePos = jointBaseNode.GetComponent<SceneNode>().PrimitiveList[0].GetLocalPosition();
        Vector3 to = prevMousePos - jointNodePos;
        Vector3 from = currentMousePos - jointNodePos; ;

        Debug.Log("joinNodePos: " + jointNodePos);
        Debug.Log("from: " + from);
        Debug.Log("to: " + to);

        Quaternion q = Quaternion.FromToRotation(from, to);

        jointBaseNode.up = stickNormal;
        jointBaseNode.localRotation *= q;
    }



    public void PushButton(int btn)
    {
        if (btn == 0)
        {
            btnSelected = dropBtnNode;
        }
        else if (btn == 1)
        {
            btnSelected = resetBtnNode;
        }
        buttonPressDown = true;
    }

    private Vector3 ButtonLower()
    {
        Vector3 btnPos = btnSelected.localPosition;
        if (btnPos.y > -0.09f)
        {
            Vector3 pos = new Vector3();
            pos.y = pos.y + (speed) * -1.0f * Time.deltaTime;
            btnPos.y += pos.y;
            btnSelected.localPosition = btnPos;
        }
        else
        {
            buttonPressDown = false;
            buttonPressUp = true;
        }
        return btnPos;
    }

    private Vector3 ButtonRise()
    {
        Vector3 btnPos = btnSelected.localPosition;
        if ((btnPos.y > -0.11f) && (btnPos.y < 0.0f))
        {
            Vector3 pos = new Vector3();
            pos.y = pos.y + (speed) * 1.0f * Time.deltaTime;
            btnPos.y += pos.y;
            btnSelected.localPosition = btnPos;
        }
        else
        {
            buttonPressUp = false;
        }
        return btnPos;
    }
}
