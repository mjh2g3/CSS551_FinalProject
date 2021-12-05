using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TheWorld : MonoBehaviour
{
    public SceneNode TheRoot;
    public Transform mSelected;


    // claw related variables
    public List<Transform> clawNodes;

    [Range(27, 75)]
    public float angle = 45;
    private float rotateSpeed;
    public bool OpenClawFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(TheRoot != null);
        mSelected = TheRoot.transform;

        SceneNode cn1 = mSelected.GetComponent<SceneNode>();
        
        //cn1.CreateAxisFrame();

        rotateSpeed = (75-angle)/600f;
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 i = Matrix4x4.identity;
        TheRoot.CompositeXform(ref i);

        if (OpenClawFlag) {
            OpenClaw();
        }

        foreach (Transform claw in clawNodes)
        {
            Quaternion q = new Quaternion();
            q.eulerAngles = new Vector3(claw.localRotation.eulerAngles.x, claw.localRotation.eulerAngles.y, angle);
            claw.localRotation = q;
        }
    }

    public void SetSelected(Transform selected)
    {
        //Upon selection, check if different from current mSelected
        if (mSelected == selected)
        {
            //if same, do nothing
        }
        else
        {
            //else if different, set previous mSelected Deactive
            SceneNode cn1 = mSelected.GetComponent<SceneNode>();
            
            //cn1.DestroyAxisFrame();

            //set new selected to Active
            SceneNode cn2 = selected.GetComponent<SceneNode>();
            
            //cn2.CreateAxisFrame();
            mSelected = selected;
        }
    }

    public void OpenClaw() {
        if (angle <= 75) {
            angle += rotateSpeed;
        } else {
            OpenClawFlag = false;
        }
    }
}
