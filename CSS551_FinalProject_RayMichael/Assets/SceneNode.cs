using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNode : MonoBehaviour
{
    //Matrix that combines all of its parents transforms
    protected Matrix4x4 mCombinedParentXform;

    public Vector3 NodeOrigin = Vector3.zero;
    public List<NodePrimitive> PrimitiveList;

    //added from Week 5, Example 3
    //for AxisFrame, reference the Axis frame in the hierarchy
    public Transform AxisFrame = null;
    public Camera SmallCamera = null;
    public Transform topPoint = null;

    //The default TreeTip; where the cube will be positioned on the 4 generations
    //public Vector3 kDefaultTreeTip = new Vector3(0.19f, 12.69f, 3.88f);
    public Vector3 kDefaultTreeTip = new Vector3(0.0f, 5.0f, 0.0f);

    //UseUnity boolean identifies if the user is applying unity or not
    public bool UseUnity = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSceneNode();
        //Debug.Log("PrimitiveList:" + PrimitiveList.Count);
        //AxisFrame.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeSceneNode()
    {
        mCombinedParentXform = Matrix4x4.identity;
    }

    // This must be called _BEFORE_ each draw!! 
    //This function allows parent to send me its transform (see parentXform as parameter)
    public void CompositeXform(ref Matrix4x4 parentXform)
    {
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
        
        //My current transform TRS
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

        //Mp * Mc1 == parentXform * orgT * trs
        mCombinedParentXform = parentXform * orgT * trs;

        // propagate to all children
        foreach (Transform child in transform)
        {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                cn.CompositeXform(ref mCombinedParentXform);
            }
        }

        // disenminate to primitives
        foreach (NodePrimitive p in PrimitiveList)
        {
            //Mp * Mc1 == mCombinedParentXform being sent to the primitives in primitive list
            p.LoadShaderMatrix(ref mCombinedParentXform);
        }


        //------------------------------------------------------------------------------------------------------new added from Week 5, Example 3
        // Compute AxisFrame 
        if (AxisFrame != null)
        {
            //ComputeAxisFrame(mCombinedParentXform);
            //AxisFrame current transform TRS
            Matrix4x4 afTRS = Matrix4x4.TRS(transform.localPosition, transform.localRotation, AxisFrame.localScale);//Return to figure out the scale situation
            Matrix4x4 mConcatMat = parentXform * orgT * afTRS;
            ComputeAxisFrame(mConcatMat);
        }

        if (SmallCamera != null)
        {
            ComputeCameraPosition(mCombinedParentXform);
        }
    }

    public void CreateAxisFrame()
    {
        GameObject temp = Instantiate(Resources.Load("AxisFrame")) as GameObject;
        AxisFrame = temp.transform;
    }

    public Transform GetAxisFrame()
    {
        return AxisFrame;
    }

    public void DestroyAxisFrame()
    {
        Destroy(AxisFrame.gameObject);
    }

    public void ComputeAxisFrame(Matrix4x4 concatMat)
    {
        //Step 1: Compute the current transform TRS of AxisFrame
        AxisFrame.localPosition = concatMat.GetColumn(3);

        //Step 2: Compute the Scale
        Vector3 x = concatMat.GetColumn(0);
        Vector3 y = concatMat.GetColumn(1);
        Vector3 z = concatMat.GetColumn(2);
        Vector3 size = new Vector3(x.magnitude, y.magnitude, z.magnitude);
        AxisFrame.localScale = size;

        //Step 3: Compute the Rotation
        // What is going on in the next two lines of code?
        //Column 1 == y
        Vector3 up = concatMat.GetColumn(1).normalized;
        //Column 2 == z
        Vector3 forward = concatMat.GetColumn(2).normalized;

        if (UseUnity)
        {
            AxisFrame.localRotation = Quaternion.LookRotation(forward, up);
        }
        else
        {
            // First align up direction, remember that the default AxisFrame.up is simply the y-axis
            float angle = Mathf.Acos(Vector3.Dot(Vector3.up, up)) * Mathf.Rad2Deg;
            Vector3 axis = Vector3.Cross(Vector3.up, up);
            AxisFrame.localRotation = Quaternion.AngleAxis(angle, axis);

            // Now, align the forward axis
            angle = Mathf.Acos(Vector3.Dot(AxisFrame.transform.forward, forward)) * Mathf.Rad2Deg;
            axis = Vector3.Cross(AxisFrame.transform.forward, forward);
            AxisFrame.localRotation = Quaternion.AngleAxis(angle, axis) * AxisFrame.localRotation;
        }
    }

    public void ComputeCameraPosition(Matrix4x4 concatMat)
    {
        //Step 1: Compute the current transform TRS of AxisFrame
        //Matrix4x4 axisTRS = Matrix4x4.TRS(AxisFrame.localPosition, AxisFrame.localRotation, AxisFrame.localScale);

        //AxisFrame.localPosition = mCombinedParentXform;
        //AxisFrame.localPosition = mCombinedParentXform.GetColumn(3);

        //This is for a single point (Camera)
        SmallCamera.transform.localPosition = concatMat.MultiplyPoint(kDefaultTreeTip);
        //topPoint.localPosition = concatMat.MultiplyPoint(kDefaultTreeTip);

        //No need to scale the camera
        /*
        //Step 2: Compute the Scale
        Vector3 x = mCombinedParentXform.GetColumn(0);
        Vector3 y = mCombinedParentXform.GetColumn(1);
        Vector3 z = mCombinedParentXform.GetColumn(2);
        Vector3 size = new Vector3(x.magnitude, y.magnitude, z.magnitude);
        AxisFrame.localScale = size;
        */

        //Step 3: Compute the Rotation
        // What is going on in the next two lines of code?
        //Column 1 == y
        Vector3 up = concatMat.GetColumn(1).normalized;
        //Column 2 == z
        Vector3 forward = concatMat.GetColumn(2).normalized;

        if (UseUnity)
        {
            AxisFrame.localRotation = Quaternion.LookRotation(forward, up);
        }
        else
        {
            
            // First align up direction, remember that the default AxisFrame.up is simply the y-axis
            float angle = Mathf.Acos(Vector3.Dot(Vector3.up, up)) * Mathf.Rad2Deg;
            Vector3 axis = Vector3.Cross(Vector3.up, up);
            SmallCamera.transform.localRotation = Quaternion.AngleAxis(angle, axis);

            // Now, align the forward axis
            angle = Mathf.Acos(Vector3.Dot(SmallCamera.transform.forward, forward)) * Mathf.Rad2Deg;
            axis = Vector3.Cross(SmallCamera.transform.forward, forward);
            SmallCamera.transform.localRotation = Quaternion.AngleAxis(angle, axis) * SmallCamera.transform.localRotation;
            

            /*
            // First align up direction, remember that the default AxisFrame.up is simply the y-axis
            float angle = Mathf.Acos(Vector3.Dot(Vector3.up, up)) * Mathf.Rad2Deg;
            Vector3 axis = Vector3.Cross(Vector3.up, up);
            topPoint.localRotation = Quaternion.AngleAxis(angle, axis);

            // Now, align the forward axis
            angle = Mathf.Acos(Vector3.Dot(topPoint.transform.forward, forward)) * Mathf.Rad2Deg;
            axis = Vector3.Cross(topPoint.transform.forward, forward);
            topPoint.localRotation = Quaternion.AngleAxis(angle, axis) * topPoint.localRotation;
            */
        }
    }
}
