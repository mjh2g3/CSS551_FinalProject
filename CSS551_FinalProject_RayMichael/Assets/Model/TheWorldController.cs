using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TheWorldController : MonoBehaviour
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
    private bool joystickMove = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(jointBaseNode != null);
        Debug.Assert(dropBtnNode != null);
        Debug.Assert(resetBtnNode != null);

        stickNormal = (jointEndNode.GetComponent<SceneNode>().PrimitiveList[0].GetLocalPosition() 
                    - jointBaseNode.GetComponent<SceneNode>().PrimitiveList[0].GetLocalPosition()).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPressDown)
        {
            ButtonLower();
        }
        if (buttonPressUp)
        {
            ButtonRise();
        }
    }

    public void UpdateJointRotation(float dx, float dy)
    {
        //Up vector
        Vector2 up2 = Vector2.up;
        Vector2 nxt = new Vector2(dx, dy);

        Debug.Log("Degrees for dx = " + dx);
        Debug.Log("Degrees for dy = " + dy);
        //Step 1: rotate the handle about the y 
        if (dx >= 0.0f)
        {
            Quaternion up = Quaternion.AngleAxis(nxt.magnitude, jointBaseNode.up);
            jointBaseNode.localRotation *= up;
            Quaternion side = Quaternion.AngleAxis(dy, jointBaseNode.right);
            jointBaseNode.localRotation *= side;
        }
        else if (dx < 0.0f)
        {
            Quaternion up = Quaternion.AngleAxis(-nxt.magnitude, jointBaseNode.up);
            jointBaseNode.localRotation *= up;
            Quaternion side = Quaternion.AngleAxis(dy, jointBaseNode.right);
            jointBaseNode.localRotation *= side;
        }

    }

    public void UpdateJointRotation2(Vector3 prevMousePos, Vector3 currentMousePos) {
        Vector3 jointNodePos = jointBaseNode.GetComponent<SceneNode>().PrimitiveList[0].GetLocalPosition();
        Vector3 to = prevMousePos - jointNodePos;
        Vector3 from = currentMousePos - jointNodePos;;

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
