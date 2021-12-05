using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainController : MonoBehaviour
{
    public Camera mainCamera = null;
    public TheWorld2 mModel = null;

    private GameObject mSelected;
    private Vector3 clawPos = Vector3.zero;
    private float speed = 0.01f;

    public Transform LookAt = null;

    private float mousPosX = 0f;
    private float mousPosY = 0f;

    private const float kPixelToDegree = 0.1f;
    private const float kPixelToDistant = 0.05f;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(mainCamera != null);
        Debug.Assert(mModel != null);
        Debug.Assert(LookAt != null);
    }

    // Update is called once per frame
    void Update()
    {
        CraneMovement();
        CamManipulation();
    }

    private void CraneMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Move the crane claw "up" in positive Z direction
            if (clawPos.z < 3.0f)
            {
                Vector3 pos = new Vector3();
                pos.z = pos.z + speed * 1.0f;
                clawPos += pos;
                Debug.Log(clawPos);
                //clawPos = mModel.UpdateClawPosition(clawPos);
            }
            clawPos = mModel.UpdateClawPosition(clawPos);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Move the crane claw "left" in negative X direction
            if (clawPos.x > -3.0f)
            {
                Vector3 pos = new Vector3();
                pos.x = pos.x + speed * -1.0f;
                clawPos += pos;
                Debug.Log(clawPos);
                //clawPos = mModel.UpdateClawPosition(clawPos);
            }
            clawPos = mModel.UpdateClawPosition(clawPos);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //Move the crane claw "right" in positive X direction
            if (clawPos.x < 3.0f)
            {
                Vector3 pos = new Vector3();
                pos.x = pos.x + speed * 1.0f;
                clawPos += pos;
                Debug.Log(clawPos);
                //clawPos = mModel.UpdateClawPosition(clawPos);
            }
            clawPos = mModel.UpdateClawPosition(clawPos);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //Move the crane claw "down" in negative Z direction
            if (clawPos.z > -3.0f)
            {
                Vector3 pos = new Vector3();
                pos.z = pos.z + speed * -1.0f;
                clawPos += pos;
                Debug.Log(clawPos);
            }
            clawPos = mModel.UpdateClawPosition(clawPos);
        }
    }
}
