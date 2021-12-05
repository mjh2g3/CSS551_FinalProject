using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public Camera mainCamera = null;
    public TheWorld2 mModel = null;

    private GameObject mSelected;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(mainCamera != null);
        Debug.Assert(mModel != null);
    }

    // Update is called once per frame
    void Update()
    {
        CraneMovement();
    }

    private void CraneMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Move the crane claw "up"
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {

        }
    }
}
