using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainController : MonoBehaviour
{
    public Camera mainCamera = null;
    public TheWorld2 mModel = null;

    private GameObject mSelected;
    private Vector3 clawPos = new Vector3(0.0f, 3.5f, 0.0f);
    private float speed = 0.05f;

    public Transform LookAt = null;

    private float mousPosX = 0f;
    private float mousPosY = 0f;

    private const float kPixelToDegree = 0.1f;
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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(mainCamera != null);
        Debug.Assert(mModel != null);
        Debug.Assert(LookAt != null);

        Debug.Assert(handle != null);
        Debug.Assert(dropBtn != null);
        Debug.Assert(resetBtn != null);
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private void LMB()
    {
        
        if (Input.GetMouseButtonDown(0))
        {

            Debug.Log("Clicked the left mouse button");
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity);
            if (hit)
            {
                Debug.Log(hitInfo.transform.gameObject.name);
                Debug.Log(hitInfo.point);
                if (ComputeHandleDetection(hitInfo.point))
                {
                    Debug.Log("You hit the handle!");
                }
                else if (ComputeDropDetection(hitInfo.point))
                {
                    Debug.Log("You hit the drop button!");
                    Drop = true;
                }
                else if (ComputeResetDetection(hitInfo.point))
                {
                    Debug.Log("You hit the reset button!");
                    ResetClaw();
                }
            }
            else
            {
                //SelectObject(null, );
            }
        }
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

        Vector3 ph;
        float d, a;

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

        Vector3 ph;
        float d, a;

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

        Vector3 ph;
        float d, a;

        d = Mathf.Sqrt(X.sqrMagnitude - (h * h));
        if (d < r)
        {
            hit = true;
        }

        return hit;
    }


    private void CraneMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Move the crane claw "up" in positive Z direction
            if (clawPos.z < backWall)
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
            if (clawPos.x > leftWall)
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
            if (clawPos.x < rightWall)
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
            if (clawPos.z > frontWall)
            {
                Vector3 pos = new Vector3();
                pos.z = pos.z + speed * -1.0f;
                clawPos += pos;
                Debug.Log(clawPos);
            }
            clawPos = mModel.UpdateClawPosition(clawPos);
        }
    }

    private void DropClaw()
    {
        //Drop the crane claw in negative Y direction
        timer += Time.deltaTime;
        Debug.Log(timer);

        
        //Vector3 pos = new Vector3();
       //pos.y = clawPos.y;
        if (clawPos.y > 0.1f)
        {
            Vector3 pos = new Vector3();
            pos.y = pos.y + (speed + 0.75f) * -1.0f * Time.deltaTime;
            clawPos.y += pos.y;
            Debug.Log(clawPos);
            clawPos = mModel.UpdateClawPosition(clawPos);
            Debug.Log("Dropping the claw");
        }
        else
        {
            Drop = false;
            Lift = true;
        }        
    }

    private void LiftClaw()
    {
        if ((clawPos.y > 0.0f) && (clawPos.y < 3.5f))
        {
            Vector3 pos = new Vector3();
            pos.y = pos.y + (speed + 0.75f) * 1.0f * Time.deltaTime;
            clawPos.y += pos.y;
            Debug.Log(clawPos);
            clawPos = mModel.UpdateClawPosition(clawPos);
            Debug.Log("Lifting the claw");
        }
        else
        {
            Lift = false;
        }
    }

    private void ResetClaw()
    {
        //Reset the claw to original position
        Vector3 orig = new Vector3();
        orig.y = 3.5f;
        orig.x = 0.0f;
        orig.z = 0.0f;
        clawPos = mModel.UpdateClawPosition(orig);
    }
}
