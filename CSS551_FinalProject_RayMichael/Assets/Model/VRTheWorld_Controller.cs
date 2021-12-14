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

    public void UpdateJointRotation(Vector3 prevMousePos, Vector3 currentMousePos)
    {
        Vector3 jointNodePos = jointBaseNode.GetComponent<SceneNode>().PrimitiveList[0].GetLocalPosition();
        Vector3 from = prevMousePos - jointNodePos;
        Vector3 to = currentMousePos - jointNodePos;

        Quaternion q = Quaternion.FromToRotation(from, to);

        jointBaseNode.up = stickNormal;
        jointBaseNode.localRotation *= q;
    }



    public void PushButton(int btn)
    {
        if (btn == 0)
        {
            btnSelected = dropBtnNode;
            Debug.Log(btnSelected.gameObject.name);
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
