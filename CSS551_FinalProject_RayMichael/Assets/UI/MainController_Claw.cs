using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController_Claw : MonoBehaviour
{
    public TheWorld mModel;

    private void Update() {
        ClawMovement();
    }
    private void ClawMovement() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (mModel.clawActionFlag == "standby") {
                // Add claw dropping movement
                Debug.Log("Dropping Claw");
                mModel.clawActionFlag = "drop";
            } else {
                Debug.Log("Releasing Claw");
                mModel.clawActionFlag = "open";
            }
        }
        // if (Input.GetKeyUp(KeyCode.Space)) {
        //     Debug.Log("Reset claw");
        //     mModel.clawActionFlag = 0;
        // }
    }
}
