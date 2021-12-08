using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Vector3 UpdateJointRotation(Vector3 angle)
    {
        Vector3 rV = Vector3.zero;
        return rV;
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
