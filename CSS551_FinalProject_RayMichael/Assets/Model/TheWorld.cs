using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TheWorld : MonoBehaviour
{
    public SceneNode TheClawRoot;
    public SceneNode TheControllerRoot;
    public Transform mSelected;

    //Crane Controller Variables
    private bool TrackHandle = false;



    // claw related variables
    public Transform clawBase;
    public List<Transform> clawNodes;
    public string clawActionFlag;
    private float rotateSpeed, moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(TheClawRoot != null);
        Debug.Assert(TheControllerRoot != null);


        mSelected = TheClawRoot.transform;

        SceneNode cn1 = mSelected.GetComponent<SceneNode>();
        
        //cn1.CreateAxisFrame();

        rotateSpeed = 1/30f;
        moveSpeed = 1/360f;

        clawActionFlag = "standby";
    }

    // Update is called once per frame
    void Update()
    {

        Matrix4x4 j = Matrix4x4.identity;
        TheControllerRoot.CompositeXform(ref j);
        Debug.Log("Setthe ControllerRoot");

        Matrix4x4 i = Matrix4x4.identity;
        TheClawRoot.CompositeXform(ref i);
        Debug.Log("Setthe ClawRoot");

       
        UpdateClawAnimations();
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

    public void SetClawAction(string s) {
        clawActionFlag = s;
    }

    void UpdateClawAnimations() {
        if (clawActionFlag == "drop") {
            DroppingClawAnimation();
        } else if (clawActionFlag == "close") {
            ClosingClawAnimation();
        } else if (clawActionFlag == "open") {
            OpeningClawAnimation();
        } else if (clawActionFlag == "reset") {
            ResettingClawAnimation();
        }

    }

    void DroppingClawAnimation() {
        if (clawNodes[0].localEulerAngles.z < 75) {
            foreach (Transform claw in clawNodes)
            {
                Quaternion q = new Quaternion();
                q.eulerAngles = claw.localRotation.eulerAngles + new Vector3(0, 0, rotateSpeed);
                claw.localRotation = q;
            }
        }
    }

    void ClosingClawAnimation() {
        if (clawNodes[0].localEulerAngles.z > 27) {
            foreach (Transform claw in clawNodes)
            {
                Quaternion q = new Quaternion();
                q.eulerAngles = claw.localRotation.eulerAngles - new Vector3(0, 0, rotateSpeed);
                claw.localRotation = q;
            }
        } else {
            clawActionFlag = "raise";
        }
    }

    void RaisingClawAnimation() {
        if (clawBase.position.y <= 5) {
            clawBase.position += new Vector3(0, moveSpeed, 0);
        }
    }

    void OpeningClawAnimation() {
        if (clawNodes[0].localEulerAngles.z < 75) {
            foreach (Transform claw in clawNodes)
            {
                Quaternion q = new Quaternion();
                q.eulerAngles = claw.localRotation.eulerAngles + new Vector3(0, 0, rotateSpeed);
                claw.localRotation = q;
            }
        } else {
            clawActionFlag = "reset";
        }
    }

    void ResettingClawAnimation() {
        if (clawNodes[0].localEulerAngles.z > 45) {
            foreach (Transform claw in clawNodes)
            {
                Quaternion q = new Quaternion();
                q.eulerAngles = claw.localRotation.eulerAngles - new Vector3(0, 0, rotateSpeed);
                claw.localRotation = q;
            }
        } else {
            clawActionFlag = "standby";
        }
    }
    // void ResetClaw() {
    //     if (angle != 45) {
    //         if (angle > 45) 
    //         {
    //             angle -= rotateSpeed;
    //             if (angle <= 45)
    //                 angle = 45;
    //         }
            
    //         else if (angle < 45) 
    //         {
    //             angle += rotateSpeed;
    //             if (angle >= 45)
    //                 angle = 45;
    //         }
    //     }
    // }
    /*
    public void HandleTrackTarget()
    {
        if (TrackHandle)
        {
            // do joint
            Vector3 jointDir = FrontTip.localPosition - ChildOrg.localPosition;
            TheChild.RotateUpTowardsBy(childDir, ChildDelta);
            UpdateHierarchy();

            // do stick
            Vector3 stickDir = FrontTip.localPosition - FrontOrg.localPosition;
            TheFront.AlignUpWith(dir);
            UpdateHierarchy();

            // do handle
            Vector3 handleDir = FrontTip.localPosition - FrontOrg.localPosition;
            TheFront.AlignUpWith(dir);
            UpdateHierarchy();

            FrontTip.localPosition = FrontOrg.localPosition + FrontHeight * FrontOrg.up;
            FrontTip.localRotation = FrontOrg.localRotation;
        }
    }
    */
}
