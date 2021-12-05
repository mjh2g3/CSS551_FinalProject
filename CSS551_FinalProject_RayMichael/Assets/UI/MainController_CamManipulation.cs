using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainController : MonoBehaviour
{
    public void CamManipulation()
    {
        //Step1: Change the rotationg and direction of the camera
        mainCamera.transform.up = Vector3.up;
        mainCamera.transform.forward = (LookAt.transform.localPosition - mainCamera.transform.localPosition).normalized;

        //Step2: Obtain an initial mouse position at mouse click + Alt-Left click
        if (Input.GetKey(KeyCode.LeftAlt) && (Input.GetMouseButtonDown(0) || (Input.GetMouseButtonDown(1))))
        {
            mousPosX = Input.mousePosition.x;
            mousPosY = Input.mousePosition.y;
            Debug.Log("Cam Manip left alt");
        }

        //Step3: Obtain the nextPosition for the mouse position at mouse button hold + Alt-Left
        else if (Input.GetKey(KeyCode.LeftAlt) && (Input.GetMouseButton(0) || (Input.GetMouseButton(1))))
        {
            //Step4: Compute the delta position change for the mouse using the initial and nextPosition
            float dx = mousPosX - Input.mousePosition.x;
            float dy = mousPosY - Input.mousePosition.y;

            mousPosX = Input.mousePosition.x;
            mousPosY = Input.mousePosition.y;

            if (Input.GetMouseButton(0)) // Camera Rotation
            {
                RotateCameraAboutUp(-dx * kPixelToDegree);
                RotateCameraAboutSide(dy * kPixelToDegree);
            }
            else if (Input.GetMouseButton(1)) // Camera tracking
            {
                Vector3 delta = dx * kPixelToDistant * mainCamera.transform.right + dy * kPixelToDistant * mainCamera.transform.up;
                mainCamera.transform.localPosition += delta;
                LookAt.localPosition += delta;
            }
        }

        if (Input.GetKey(KeyCode.LeftAlt))  // dolly or zooming
        {
            Vector2 d = Input.mouseScrollDelta;
            // move camera position towards LookAt
            Vector3 v = mainCamera.transform.localPosition - LookAt.localPosition;
            float dist = v.magnitude;
            v /= dist;
            float m = dist - d.y;
            mainCamera.transform.localPosition = LookAt.localPosition + m * v;
        }
    }

    private void RotateCameraAboutUp(float degree)
    {
        Quaternion up = Quaternion.AngleAxis(degree, mainCamera.transform.up);
        RotateCameraPosition(ref up);
    }

    private void RotateCameraAboutSide(float degree)
    {
        Quaternion side = Quaternion.AngleAxis(degree, mainCamera.transform.right);
        RotateCameraPosition(ref side);
    }

    private void RotateCameraPosition(ref Quaternion q)
    {
        Matrix4x4 r = Matrix4x4.TRS(Vector3.zero, q, Vector3.one);
        Matrix4x4 invP = Matrix4x4.TRS(-LookAt.localPosition, Quaternion.identity, Vector3.one);
        Matrix4x4 m = invP.inverse * r * invP;

        Vector3 newCameraPos = m.MultiplyPoint(mainCamera.transform.localPosition);
        if (Mathf.Abs(Vector3.Dot(newCameraPos.normalized, Vector3.up)) < 0.985)
        {
            mainCamera.transform.localPosition = newCameraPos;

            // First way:
            // transform.LookAt(LookAt);
            // Second way:
            // Vector3 v = (LookAt.localPosition - transform.localPosition).normalized;
            // transform.localRotation = Quaternion.LookRotation(v, Vector3.up);
            // Third way: do everything ourselve!
            Vector3 v = (LookAt.localPosition - mainCamera.transform.localPosition).normalized;
            Vector3 w = Vector3.Cross(v, mainCamera.transform.up).normalized;
            Vector3 u = Vector3.Cross(w, v).normalized;
            // INTERESTING: 
            //    chaning the following directions must be done in specific sequence!
            //    E.g., NONE of the following order works: 
            //          Forward, Up, Right 
            //          Forward, Right, Up 
            //          Right, Forward, Up 
            //          Up, Forward, Right 
            //
            //   Forward-Vector MUST BE set LAST!!: both of the following works!
            //          Right, Up, Forward
            //          Up, Right, Forward
            mainCamera.transform.up = u;
            mainCamera.transform.right = w;
            mainCamera.transform.forward = v;
        }
    }

    public void SetLookAtPos(Vector3 p)
    {
        LookAt.localPosition = p;
    }
}
