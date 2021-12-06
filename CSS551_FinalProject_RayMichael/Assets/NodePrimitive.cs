using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePrimitive : MonoBehaviour
{
    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Vector3 Pivot;
    private Matrix4x4 m;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //nodeMatrix == Mp * Mc1
    public void LoadShaderMatrix(ref Matrix4x4 nodeMatrix)
    {
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

        //nodeMatrix == Mp * Mc1 (matrices for parent and child scene nodes ; p * trs * invp == Md (matrix for primitive)
        //Matrix4x4
        m = nodeMatrix * p * trs * invp;

        GetComponent<Renderer>().material.SetMatrix("MyXformMat", m); //Send to Shader/Material
        GetComponent<Renderer>().material.SetColor("MyColor", MyColor); //Send to Shader/Material
    }

    public Vector3 GetLocalPosition()
    {
        Vector3 pos = m.GetColumn(3);
        return pos;
    }

    public Vector3 GetLocalScale()
    {
        Vector3 x = m.GetColumn(0);
        Vector3 y = m.GetColumn(1);
        Vector3 z = m.GetColumn(2);
        Vector3 size = new Vector3(x.magnitude, y.magnitude, z.magnitude);
        return size;
    }
}
