using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld2 : MonoBehaviour
{
    public Transform clawPos = null;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(clawPos != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 UpdateClawPosition(Vector3 pos)
    {
        Vector3 curPos = clawPos.localPosition;
        curPos.x = pos.x;
        curPos.z = pos.z;
        clawPos.localPosition = curPos;
        return clawPos.localPosition;
    }
}
