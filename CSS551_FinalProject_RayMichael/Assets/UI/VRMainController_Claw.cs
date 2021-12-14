using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VRMainController : MonoBehaviour
{
    private GameObject mSelected;
    private Vector3 clawPos = new Vector3(0.0f, 4f, 0.0f);
    private float speed = 0.05f;

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
            mModel.UpdateClawPosition(clawPos);
            // Debug.Log("Dropping the claw");
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
        if (clawPos.y < 4f)
        {
            Vector3 pos = new Vector3();
            pos.y = pos.y + (speed + 0.75f) * 1.0f * Time.deltaTime;
            clawPos.y += pos.y;
            // Debug.Log(clawPos);
            mModel.UpdateClawPosition(clawPos);
            // Debug.Log("Lifting the claw");
        }
        else
        {
            mModel.clawActionFlag = "standby";
            Lift = false;
        }
    }

}
