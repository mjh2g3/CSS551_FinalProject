using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld2 : MonoBehaviour
{
    public Transform clawPos = null;
    public Transform clawBase = null;
    private float grabThreshold = 1f;
    private Transform mGrabbed = null;

    public MyMeshNxM upperBox = null;
    public MyMeshNxM lowerBox = null;

    private GameObject sightLine;
    private float sightMagnitude = 3.0f;
    public Camera clawCam = null;
    public List<Transform> prizes;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(clawPos != null);
        Debug.Assert(upperBox != null);
        Debug.Assert(lowerBox != null);
        Debug.Assert(clawCam != null);

        upperBox.SetHeight(3.5f);

        //The following is to create a line of sight; future to remove and have light source
        sightLine = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Vector3 scale = new Vector3(0.05f, sightMagnitude / 2, 0.05f);
        sightLine.transform.localScale = scale;
        sightLine.transform.up = -(clawPos.transform.up);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLineOfSight();
        UpdateClawCam();
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
        Vector3 endPoint = clawPos.transform.localPosition + -(clawPos.transform.up) * sightMagnitude;

        //Find the vector v between end point of axis direction beam
        Vector3 v = endPoint - startPoint;

        //Set the beam upright to align to the correct axis direction and compute the position of the axis
        sightLine.transform.up = -(clawPos.transform.up);
        sightLine.transform.localPosition = clawPos.transform.localPosition + 0.5f * v;
    }

    public void UpdateClawCam()
    {
        clawCam.transform.forward = -(clawPos.transform.up);
        clawCam.transform.localPosition = clawPos.transform.localPosition;
    }
}
