Shader "Unlit/451NoCullShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
	Cull off

        Pass
        {
            CGPROGRAM
            //vert == my vertex processor
            #pragma vertex vert
            //frag == my fragment processor
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
		float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
		float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;  //texture element, allows for addition of image
            float4x4 MyXformMat; // our own transform matrix
            fixed4 MyColor;  //linkage to Node Primitive script for primitive nodes within scene nodes

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(MyXformMat, v.vertex);  //use our own transform matrix
                o.vertex = mul(UNITY_MATRIX_VP, o.vertex);  //camera + gameobject transform TRS
		o.uv = v.uv;
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);

		o.normal = v.normal;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col += MyColor;

		//Week8 Example
		//col = 0.5 * col + 0.5 * fixed4(i.normal, 1.0);

                return col;
            }
            ENDCG
        }
    }
}
