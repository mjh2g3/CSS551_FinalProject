using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public partial class VRTheWorld : MonoBehaviour
{
    public SceneNode TheClawRoot;
    public SceneNode TheControllerRoot;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(TheClawRoot != null);
        Debug.Assert(TheControllerRoot != null);

        rotateSpeed = 1 / 30f;
        moveSpeed = 1 / 360f;

        clawActionFlag = "standby";

        Debug.Assert(clawPos != null);
        Debug.Assert(clawCam != null);

        Debug.Assert(jointBaseNode != null);
        Debug.Assert(dropBtnNode != null);
        Debug.Assert(resetBtnNode != null);

        stickNormal = (jointEndNode.GetComponent<SceneNode>().PrimitiveList[0].GetLocalPosition()
                    - jointBaseNode.GetComponent<SceneNode>().PrimitiveList[0].GetLocalPosition()).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 j = Matrix4x4.identity;
        TheControllerRoot.CompositeXform(ref j);

        Matrix4x4 i = Matrix4x4.identity;
        TheClawRoot.CompositeXform(ref i);

        UpdateClawAnimations();
        UpdateClawCam();

        if (buttonPressDown)
        {
            ButtonLower();
        }
        if (buttonPressUp)
        {
            ButtonRise();
        }
    }
}
