using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VRMainController : MonoBehaviour
{
    private GameObject mSelected;
    private Vector3 clawPos = new Vector3(0.0f, 4f, 0.0f);
    private float speed = 0.05f;
    
    //CraneArea
    private float leftWall = -3.5f;
    private float rightWall = 3.5f;
    private float backWall = 3.5f;
    private float frontWall = -3.5f; 
    
    private void ClawMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (mModel.clawActionFlag == "standby")
            {
                // Add claw dropping movement
                Debug.Log("Dropping Claw");
                mModel.clawActionFlag = "drop";
            }
            else
            {
                Debug.Log("Releasing Claw");
                mModel.clawActionFlag = "open";
            }
        }
        // if (Input.GetKeyUp(KeyCode.Space)) {
        //     Debug.Log("Reset claw");
        //     mModel.clawActionFlag = 0;
        // }
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
                // Debug.Log(clawPos);
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
                // Debug.Log(clawPos);
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
                // Debug.Log(clawPos);
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
                // Debug.Log(clawPos);
            }
            clawPos = mModel.UpdateClawPosition(clawPos);
        }
    }

    private void DropClaw()
    {
        //Drop the crane claw in negative Y direction
        timer += Time.deltaTime;
        // Debug.Log(timer);


        //Vector3 pos = new Vector3();
        //pos.y = clawPos.y;
        if (clawPos.y > 1.5f)
        {
            Vector3 pos = new Vector3();
            pos.y = pos.y + (speed + 0.75f) * -1.0f * Time.deltaTime;
            clawPos.y += pos.y;
            // Debug.Log(clawPos);
            clawPos = mModel.UpdateClawPosition(clawPos);
            Debug.Log("Dropping the claw");
        }
        else if (mModel.clawActionFlag == "drop")
        {
            mModel.clawActionFlag = "close";
        }
        else if (mModel.clawActionFlag == "raise")
        {
            mModel.GrabPrize();
            Drop = false;
            Lift = true;
        }
    }

    private void LiftClaw()
    {
        if ((clawPos.y > 0.0f) && (clawPos.y < 4f))
        {
            Vector3 pos = new Vector3();
            pos.y = pos.y + (speed + 0.75f) * 1.0f * Time.deltaTime;
            clawPos.y += pos.y;
            // Debug.Log(clawPos);
            clawPos = mModel.UpdateClawPosition(clawPos);
            Debug.Log("Lifting the claw");
        }
        else
        {
            mModel.clawActionFlag = "standby";
            Lift = false;
        }
    }

    private void ResetClaw()
    {
        if (mModel.clawActionFlag == "standby") {
            //Reset the claw to original position
            Vector3 orig = new Vector3();
            orig.y = 4.0f;
            orig.x = 0.0f;
            orig.z = 0.0f;
            clawPos = mModel.UpdateClawPosition(orig);

            Drop = false;
        }
    }

}
