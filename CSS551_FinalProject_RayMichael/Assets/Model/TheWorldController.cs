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

    private Vector2 mDir2 = Vector2.up;

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
        jointBaseNode.rotation = Quaternion.FromToRotation(vHandle, vNext);



        return vNext;//
        
        
        
        
        /*
        
        float degrees = 0.0f;
        if (((p1 - p0) / (p1 - p0).magnitude) != mDir2)
        {
            Debug.Log("The direction is new");
            jointBaseNode.localRotation *= Quaternion.identity;

            //Step 1: Get current forward direction vector v0
            Vector2 p0a = p0;
            Vector2 p0b = p0 + mDir2 * 2.0f;
            Vector2 v0 = p0b - p0a;

            //Step2: Get next forward direction vector v1
            Vector2 v1 = p1 - p0;

            //Step3: Find the angle between the two vectors
            float dot = Vector2.Dot(v0, v1);
            float mag0 = v0.magnitude;
            float mag1 = (p1 - p0).magnitude;
            //angle is in radians
            double angle = Math.Acos((double)dot / (mag1 * mag0));
            //convert to degrees
            degrees = (float)((180 / Math.PI) * angle);
            Debug.Log(degrees);
            //Step4: Use angle, degrees, for rotation about the y axis to adjust forward z direction
            if (!Double.IsNaN(degrees))
            {
                Quaternion rotateOnY = Quaternion.AngleAxis(degrees, jointBaseNode.up);
                jointBaseNode.localRotation *= rotateOnY;

            }

            //Step5: Reset the mDir2 to the new direction
            mDir2 = (p1 - p0) / mag1;

            if (!Double.IsNaN(degrees))
            {
                Quaternion rotateOnX = Quaternion.AngleAxis(degrees, jointBaseNode.right);
                jointBaseNode.localRotation = rotateOnX;

            }
        }

        */

        /*
        //Step 1: Get current forward direction vector v0
        Vector2 p0a = p0;
        Vector2 p0b = p0 + mDir2 * 2.0f;
        Vector2 v0 = p0b - p0a;

        //Step2: Get next forward direction vector v1
        Vector2 v1 = p1 - p0;

        //Step3: Find the angle between the two vectors
        float dot = Vector2.Dot(v1, v0);
        float mag0 = v0.magnitude;
        float mag1 = (p1 - p0).magnitude;
        //angle is in radians
        double angle = Math.Acos((double)dot / (mag0 * mag1));
        //convert to degrees
        float degrees = (float) ( (180 / Math.PI) * angle);
        Debug.Log(degrees);
        //Step4: Use angle, degrees, for rotation about the y axis to adjust forward z direction
        if (!Double.IsNaN(degrees))
        {
            Quaternion rotateOnY = Quaternion.AngleAxis(degrees, jointBaseNode.up);
            jointBaseNode.localRotation *= rotateOnY;

        }

        //Step5: Reset the mDir2 to the new direction
        mDir2 = (p1 - p0) / mag1;
        */


        //Step6: Rotate the handle by the magnitude onto the x axis
        // Quaternion rotateOnX = Quaternion.AngleAxis(degrees, jointBaseNode.right);
        // jointBaseNode.localRotation *= rotateOnX;

        /*
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
        */
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
