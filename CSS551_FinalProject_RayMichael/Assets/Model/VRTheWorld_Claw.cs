using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VRTheWorld : MonoBehaviour
{
    // claw related variables
    public Transform clawBase;
    public List<Transform> clawNodes;
    public string clawActionFlag;
    private float rotateSpeed;

    public Transform clawPos = null;

    private float grabThreshold = 2f;
    public Transform mGrabbed = null;
    public bool clawFull = false;
    public Transform prizeListObj;

    public Camera clawCam = null;
    public List<Transform> prizes;
    public Transform DropZone = null;
    private float spawnTimer = 0.0f;

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
            clawPos.position += new Vector3(movement.x, 0, movement.z) / 2;
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

    void UpdateClawAnimations()
    {
        if (clawActionFlag == "drop")
        {
            DroppingClawAnimation();
        }
        else if (clawActionFlag == "close")
        {
            ClosingClawAnimation();
        }
        else if (clawActionFlag == "open")
        {
            OpeningClawAnimation();
        }
        else if (clawActionFlag == "reset")
        {
            ResettingClawAnimation();
        }

    }

    void DroppingClawAnimation()
    {
        if (clawNodes[0].localEulerAngles.z < 75)
        {
            foreach (Transform claw in clawNodes)
            {
                Quaternion q = new Quaternion();
                q.eulerAngles = claw.localRotation.eulerAngles + new Vector3(0, 0, rotateSpeed);
                claw.localRotation = q;
            }
        }
    }

    void ClosingClawAnimation()
    {
        if (clawNodes[0].localEulerAngles.z > 27)
        {
            foreach (Transform claw in clawNodes)
            {
                Quaternion q = new Quaternion();
                q.eulerAngles = claw.localRotation.eulerAngles - new Vector3(0, 0, rotateSpeed);
                claw.localRotation = q;
            }
        }
        else
        {
            clawActionFlag = "raise";
        }
    }

    void OpeningClawAnimation()
    {
        if (clawNodes[0].localEulerAngles.z < 75)
        {
            foreach (Transform claw in clawNodes)
            {
                Quaternion q = new Quaternion();
                q.eulerAngles = claw.localRotation.eulerAngles + new Vector3(0, 0, rotateSpeed);
                claw.localRotation = q;
            }
        }
        else
        {
            clawActionFlag = "reset";
        }
    }

    void ResettingClawAnimation()
    {
        if (clawNodes[0].localEulerAngles.z > 45)
        {
            foreach (Transform claw in clawNodes)
            {
                Quaternion q = new Quaternion();
                q.eulerAngles = claw.localRotation.eulerAngles - new Vector3(0, 0, rotateSpeed);
                claw.localRotation = q;
            }
        }
        else
        {
            clawActionFlag = "standby";
        }
    }

    public void UpdateClawPosition(Vector3 pos)
    {
        Vector3 curPos = clawPos.localPosition;
        curPos.y = pos.y;
        clawPos.localPosition = curPos;
    }

    public void ResetClawPosition() {
        if (clawActionFlag == "standby")
            clawPos.localPosition = new Vector3(0, 4, 0);
    }

    public void GrabPrize()
    {
        // checks each prize to see if a prize is grabbed
        foreach (Transform p in prizes)
        {
            if (!clawFull) {
                float distance = (p.position - clawPos.position).magnitude;
                if (distance <= grabThreshold)
                {
                    mGrabbed = p;
                    mGrabbed.parent = clawPos;
                    clawFull = true;

                    break;
                }
            }

        }
    }

    public void DropPrize() {
        clawActionFlag = "open";
        mGrabbed.parent = prizeListObj;
    }

    private void FallingPrize() {
        if (mGrabbed.parent == prizeListObj)
        {
            float y;
            if (mGrabbed.name.Contains("Cylinder"))
                y = mGrabbed.localScale.y;
            else 
                y = mGrabbed.localScale.y/2;

            if (mGrabbed.position.y > y) {
                mGrabbed.position -= new Vector3(0, 1/10f, 0);
            }
            else 
            {
                mGrabbed.position = new Vector3(0, y, 0);
                clawFull = false;
            }
        }
    }

    private void RespawnPrize()
    {
        Vector3 dropPos = mGrabbed.localPosition;
        Vector3 dropZone = DropZone.localPosition;
        float LRedge = 0.2f;
        float TBedge = 0.2f;
        if (dropPos.y < 0.3f) {
            //Check if between left and right ledges
            if ((dropPos.x < dropZone.x + LRedge) && (dropPos.x > dropZone.x - LRedge))
            {
                if ((dropPos.z < dropZone.z + TBedge) && (dropPos.z > dropZone.z - TBedge))
                {
                    spawnTimer += Time.deltaTime;
                    if (spawnTimer < 3.0f)
                    {
                        Debug.Log(mGrabbed.localPosition);
                    }
                    else
                    {
                        ComputeNewPrizePos();
                    }
                    
                }
            }
        }
        
    }

    private void ComputeNewPrizePos()
    {
        Debug.Log("Computing new position!");
        float y = 0.0f;
        if (mGrabbed.name.Contains("Cylinder"))
        {
            y = 0.25f;
        }

        Vector3 position = new Vector3(Random.Range(-5.0f, 5.0f), y, Random.Range(-5.0f, 5.0f));
        for (int i = 0; i < prizes.Count; i++)
        {
            Vector3 p = prizes[i].localPosition;
            float mag = (p - position).magnitude;
            if (mag < 1.0f)
            {
                if ((position.x + 1.0f) < 5.0f)
                {
                    position.x += 1.0f;
                }
                else if ((position.x - 1.0f) > -5.0f)
                {
                    position.x -= 1.0f;
                }
                else if ((position.z + 1.0f) < 5.0f)
                {
                    position.z -= 1.0f;
                }
                else if ((position.z - 1.0f) > -5.0f)
                {
                    position.z -= 1.0f;
                }
            }
        }

        mGrabbed.localPosition = position;
        mGrabbed = null;
        spawnTimer = 0.0f;
    }

    public void UpdateClawCam()
    {
        clawCam.transform.forward = clawPos.transform.up;
        clawCam.transform.localPosition = clawPos.transform.localPosition;
    }
}
