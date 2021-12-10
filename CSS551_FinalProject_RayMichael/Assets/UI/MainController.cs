using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainController : MonoBehaviour
{
    public Camera mainCamera = null;
    public TheWorld mModel = null;
    //public TheWorld2 mModel2 = null;
    //public TheWorldController mModel3 = null;

    private GameObject mSelected;
    private Vector3 clawPos = new Vector3(0.0f, 4f, 0.0f);
    private float speed = 0.05f;

    public Transform LookAt = null;

    private float mousPosX = 0f;
    private float mousPosY = 0f;

    private const float kPixelToDegree = 0.9f;
    private const float kPixelToDistant = 0.05f;

    //CraneArea
    private float leftWall = -3.5f;
    private float rightWall = 3.5f;
    private float backWall = 3.5f;
    private float frontWall = -3.5f;

    //Controller Buttons
    public NodePrimitive handle;
    public NodePrimitive dropBtn;
    public NodePrimitive resetBtn;
    private bool Drop = false;
    private bool Lift = false;
    private float timer = 0.0f;

    // Controller Handle
    private bool handleSelected = false;
    private Vector3 prevMousePos;
    private Vector3 depthBuffer = new Vector3(0, 0, 20);

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(mainCamera != null);
        //Debug.Assert(mModel2 != null);
        Debug.Assert(LookAt != null);

        Debug.Assert(handle != null);
        Debug.Assert(dropBtn != null);
        Debug.Assert(resetBtn != null);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Turning off for vr
        /*
        CraneMovement();
        CamManipulation();
        LMB();
        if (Drop)
        {
            DropClaw();
        }
        if (Lift)
        {
            LiftClaw();
        }

        ClawMovement();
        */
    }

    private void LMB()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousPosX = Input.mousePosition.x;
            mousPosY = Input.mousePosition.y;

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity);
            if (hit)
            {
                Debug.Log(hitInfo.transform.gameObject.name);
                Debug.Log(hitInfo.point);
                if (ComputeHandleDetection(hitInfo.point))
                {
                    handleSelected = true;
                    prevMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition + depthBuffer);
                }
                else if (ComputeDropDetection(hitInfo.point))
                {
                    Drop = true;
                    mModel.clawActionFlag = "drop";
                    mModel.PushButton(0);
                }
                else if (ComputeResetDetection(hitInfo.point))
                {
                    Debug.Log("You hit the reset button!");
                    mModel.PushButton(1);
                    ResetClaw();
                }
            }
            else
            {
                //SelectObject(null, );
            }
        }
        else if ((handleSelected) && (Input.GetMouseButton(0))){
            //Step4: Compute the delta position change for the mouse using the initial and nextPosition
            /* 
            float dx = mousPosX - Input.mousePosition.x;
             mousPosX = Input.mousePosition.x;

             float dy = mousPosY - Input.mousePosition.y;
             mousPosY = Input.mousePosition.y;
            */
            Vector2 p0 = new Vector2(mousPosX, mousPosY);
            Vector2 p1 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            float dx = mousPosX - Input.mousePosition.x;
            float dy = mousPosY - Input.mousePosition.y;

            mousPosX = Input.mousePosition.x;
            mousPosY = Input.mousePosition.y;

            dx = dx * kPixelToDegree;
            dy = dy * kPixelToDegree;

            Ray r = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.Log("Ray point at 100 units away: " + r.GetPoint(100.0f));
            Debug.Log("Ray point origin: " + r.origin);
            Vector3 p2 = r.GetPoint(100.0f);

            Vector3 initPos = handle.GetLocalPosition();
            mModel.UpdateJointRotation(p2, initPos);

            //DragHandle();
        }
    }

    private void DragHandle() {         
        Vector3 currentMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition + depthBuffer);
        mModel.UpdateJointRotation2(prevMousePos, currentMousePos);
    }

    private bool ComputeHandleDetection(Vector3 pos2)
    {
        bool hit = false;
        //Step 1: Compute the vector between the camera position (pos1) and hit position (pos2)
        Vector3 pos1 = mainCamera.transform.localPosition;
        Vector3 V = pos2 - pos1;
        float len = V.magnitude;
        V = V / len;

        Vector3 X = handle.GetLocalPosition() - pos1;
        float h = Vector3.Dot(X, V);

        Vector3 scaleHandle = handle.GetLocalScale();
        float r = scaleHandle.x * 0.5f;

        float d;

        d = Mathf.Sqrt(X.sqrMagnitude - (h * h));
        if (d < r)
        {
            hit = true;
        }

        return hit;
    }

    private bool ComputeDropDetection(Vector3 pos2)
    {
        bool hit = false;
        //Step 1: Compute the vector between the camera position (pos1) and hit position (pos2)
        Vector3 pos1 = mainCamera.transform.localPosition;
        Vector3 V = pos2 - pos1;
        float len = V.magnitude;
        V = V / len;

        Vector3 X = dropBtn.GetLocalPosition() - pos1;
        float h = Vector3.Dot(X, V);

        Vector3 scaleHandle = dropBtn.GetLocalScale();
        float r = scaleHandle.x * 0.5f;

        float d;

        d = Mathf.Sqrt(X.sqrMagnitude - (h * h));
        if (d < r)
        {
            hit = true;
        }

        return hit;
    }

    private bool ComputeResetDetection(Vector3 pos2)
    {
        bool hit = false;
        //Step 1: Compute the vector between the camera position (pos1) and hit position (pos2)
        Vector3 pos1 = mainCamera.transform.localPosition;
        Vector3 V = pos2 - pos1;
        float len = V.magnitude;
        V = V / len;

        Vector3 X = resetBtn.GetLocalPosition() - pos1;
        float h = Vector3.Dot(X, V);

        Vector3 scaleHandle = resetBtn.GetLocalScale();
        float r = scaleHandle.x * 0.5f;

        float d;

        d = Mathf.Sqrt(X.sqrMagnitude - (h * h));
        if (d < r)
        {
            hit = true;
        }
        return hit;
    }
}
