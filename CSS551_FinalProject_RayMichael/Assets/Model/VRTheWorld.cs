using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public partial class VRTheWorld : MonoBehaviour
{
    public SceneNode TheClawRoot;
    public SceneNode TheControllerRoot;
    public Transform player;
    private float currentAngle = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(TheClawRoot != null);
        Debug.Assert(TheControllerRoot != null);

        rotateSpeed = 1 / 3f;

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
        UpdateController();

        if (buttonPressDown)
        {
            ButtonLower();
        }
        if (buttonPressUp)
        {
            ButtonRise();
        }

        if (mGrabbed != null)
        {
            RespawnPrize();
            FallingPrize();
        }
    }

    public void MovePlayer(float x, float z) {
        player.position += player.right * x/20 + player.forward * z/20;
    }

    public void UpdateController() {
        Transform controllerTransform = TheControllerRoot.gameObject.transform;
        if (currentAngle != player.eulerAngles.y) 
        {
            controllerTransform.RotateAround(player.position, player.up, player.eulerAngles.y - currentAngle);
            currentAngle = player.eulerAngles.y;
        }

        controllerTransform.position = player.position + player.up * 0.6f + player.forward * 0.5f;
    }
}
