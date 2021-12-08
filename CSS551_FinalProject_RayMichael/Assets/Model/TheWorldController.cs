using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TheWorldController : MonoBehaviour
{
    public Transform jointNode = null;
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
        Debug.Assert(jointNode != null);
        Debug.Assert(dropBtnNode != null);
        Debug.Assert(resetBtnNode != null);
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
            Quaternion up = Quaternion.AngleAxis(nxt.magnitude, jointNode.up);
            jointNode.localRotation *= up;
            Quaternion side = Quaternion.AngleAxis(dy, jointNode.right);
            jointNode.localRotation *= side;
        }
        else if (dx < 0.0f)
        {
            Quaternion up = Quaternion.AngleAxis(-nxt.magnitude, jointNode.up);
            jointNode.localRotation *= up;
            Quaternion side = Quaternion.AngleAxis(dy, jointNode.right);
            jointNode.localRotation *= side;
        }

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
