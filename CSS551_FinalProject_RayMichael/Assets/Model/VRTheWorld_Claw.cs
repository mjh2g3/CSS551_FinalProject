using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VRTheWorld : MonoBehaviour
{
    // claw related variables
    public Transform clawBase;
    public List<Transform> clawNodes;
    public string clawActionFlag;
    private float rotateSpeed, moveSpeed;

    public Transform clawPos = null;

    private float grabThreshold = 2f;
    public Transform mGrabbed = null;

    public Camera clawCam = null;
    public List<Transform> prizes;
    public Transform DropZone = null;
    private float spawnTimer = 0.0f;

    public void SetClawAction(string s)
    {
        clawActionFlag = s;
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

    void RaisingClawAnimation()
    {
        if (clawBase.position.y <= 5)
        {
            clawBase.position += new Vector3(0, moveSpeed, 0);
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

    public Vector3 UpdateClawPosition(Vector3 pos)
    {
        Vector3 curPos = clawPos.localPosition;
        curPos.x = pos.x;
        curPos.z = pos.z;
        curPos.y = pos.y;
        clawPos.localPosition = curPos;
        return clawPos.localPosition;
    }

    public void GrabPrize()
    {
        // checks each prize to see if a prize is grabbed
        foreach (Transform p in prizes)
        {
            float distance = (p.position - clawPos.position).magnitude;
            if (distance <= grabThreshold)
            {
                mGrabbed = p;
                mGrabbed.parent = clawPos;

                float localY = -(1f);
                clawPos.localPosition = new Vector3(0, localY, 0);
                break;
            }
        }
    }

    private void RespawnPize()
    {
        Vector3 dropPos = mGrabbed.localPosition;
        Vector3 dropZone = DropZone.localPosition;
        float LRedge = 1.0f;
        float TBedge = 1.0f;
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
                    ComputeNewPos();
                }
                
            }
        }
    }

    private void ComputeNewPos()
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
