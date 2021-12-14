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
    private float speed = 0.25f;
    private bool buttonPressDown = false;
    private bool buttonPressUp = false;
    //private bool joystickMove = false;

    private Vector2 mDir2 = Vector2.up;

    public void UpdateJointRotation(Vector3 prevPos, Vector3 currentPos)
    {
        Vector3 jointNodePos = jointBaseNode.GetComponent<SceneNode>().PrimitiveList[0].GetLocalPosition();
        Vector3 from = prevPos - jointNodePos;
        Vector3 to = currentPos - jointNodePos;

        Quaternion q = Quaternion.FromToRotation(from, to);

        jointBaseNode.up = stickNormal;
        jointBaseNode.localRotation *= q;

        UpdateCranePosition();

    }

    private void UpdateCranePosition() {
        Vector3 movement = jointEndNode.GetComponent<SceneNode>().PrimitiveList[0].GetLocalPosition()
                            -jointBaseNode.GetComponent<SceneNode>().PrimitiveList[0].GetLocalPosition();
        bool inLeftWall, inRightWall, inBackWall, inFrontWall;
        inLeftWall = clawPos.position.x >= -6;
        inRightWall = clawPos.position.x <= 6;
        inBackWall = clawPos.position.z <= 6;
        inFrontWall = clawPos.position.z >= -6;

        if (inLeftWall && inRightWall && inBackWall && inFrontWall) 
        {  
            clawPos.position += new Vector3(movement.x, 0, movement.z) * 2;
        }
        else
        {
            if (!inLeftWall) 
            {
                clawPos.position = new Vector3(-6, clawPos.position.y, clawPos.position.z);
            }
            if (!inRightWall) 
            {
                clawPos.position = new Vector3(6, clawPos.position.y, clawPos.position.z);
            }
            if (!inBackWall) 
            {
                clawPos.position = new Vector3(clawPos.position.x, clawPos.position.y, 6);
            }
            if (!inFrontWall) 
            {
                clawPos.position = new Vector3(clawPos.position.x, clawPos.position.y, -6);
            }
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
        Debug.Log(btnSelected.gameObject.name);
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
        if ((btnPos.y > -0.12f) && (btnPos.y < 0.0f))
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
