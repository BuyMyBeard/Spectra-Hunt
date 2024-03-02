Shader "Unlit/SpecialSilhouette"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 0, 0)
        _Thickness ("Thickness", Range(0, 1)) = 0.1
        _Offset ("Screen Offset", Vector) = (0, 0, 0, 0)
        _StencilWrite ("Stencil Write", Integer) = 1
    }
    SubShader
    {
        Tags 
        {
            "Queue"="Overlay+1"
            "RenderType"="Transparent"
        }
        LOD 100

        Stencil
        {
            Ref [_StencilWrite]
            ReadMask 240
            WriteMask 240
            Comp NotEqual
            Pass Replace
            //ZFail Replace
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Traditional transparency

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL0;
            };


            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Thickness;
            float4 _Offset;

            v2f vert (appdata v)
            {
                v2f o;

                // view to model space
                float3 tranformedOffset = mul((float3x3)UNITY_MATRIX_T_MV, _Offset);

                o.vertex = UnityObjectToClipPos(v.vertex + normalize(v.normal) * _Thickness + tranformedOffset);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
