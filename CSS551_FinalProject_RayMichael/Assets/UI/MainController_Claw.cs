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
            // Add claw dropping movement
            Debug.Log("Opening claw");
            mModel.OpenClawFlag = true;
        }
    }
}
