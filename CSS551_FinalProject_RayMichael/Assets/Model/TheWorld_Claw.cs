using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TheWorld : MonoBehaviour
{
    /*
    public Transform clawPos = null;
    
    private float grabThreshold = 2f;
    private Transform mGrabbed = null;

    public MyMeshNxM upperBox = null;
    public MyMeshNxM lowerBox = null;

    private GameObject sightLine;
    private float sightMagnitude = 3.0f;
    public Camera clawCam = null;
    public List<Transform> prizes;
    */

    
    public Vector3 UpdateClawPosition(Vector3 pos)
    {
        Vector3 curPos = clawPos.localPosition;
        curPos.x = pos.x;
        curPos.z = pos.z;
        curPos.y = pos.y;
        clawPos.localPosition = curPos;
        return clawPos.localPosition;
    }

    public void GrabPrize() {
        // checks each prize to see if a prize is grabbed
        foreach (Transform p in prizes) {
            float distance = (p.position - clawPos.position).magnitude;
            if (distance <= grabThreshold) {
                mGrabbed = p;
                mGrabbed.parent = clawPos;
                
                float localY = -(1f);
                clawPos.localPosition = new Vector3(0, localY, 0);
                break;
            }
        }
    }

    public void UpdateLineOfSight()
    {
        //Define the start and end point of the axis beam
        Vector3 startPoint = clawPos.transform.localPosition;
        // Debug.Log(startPoint);
        Vector3 endPoint = clawPos.transform.localPosition + clawPos.transform.up * sightMagnitude;

        //Find the vector v between end point of axis direction beam
        Vector3 v = endPoint - startPoint;

        //Set the beam upright to align to the correct axis direction and compute the position of the axis
        sightLine.transform.up = -clawPos.transform.up;
        sightLine.transform.localPosition = clawPos.transform.localPosition + 0.75f * v;
    }

    public void UpdateClawCam()
    {
        clawCam.transform.forward = clawPos.transform.up;
        clawCam.transform.localPosition = clawPos.transform.localPosition;
    }
}
