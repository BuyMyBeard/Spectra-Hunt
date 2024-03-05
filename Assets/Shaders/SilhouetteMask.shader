﻿Shader "/Unlit/SilhouetteMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StencilWrite ("Stencil Write", Integer) = 30
        _Offset ("Screen Offset", Vector) = (-.1, 0, 0, 0)
        _Thickness ("Thickness", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags 
        {
            "RenderType"="Transparent"
            "Queue"="Overlay+1"
            "RenderPipeline"="UniversalRenderPipeline"
        }

        Stencil
        {
            Ref [_StencilWrite]
            WriteMask 240
            Comp Always
            Pass Replace
        }
        ZWrite On

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

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
            float4 _Offset;
            float _Thickness;


            v2f vert (appdata v)
            {
                float3 tranformedOffset = mul((float3x3)UNITY_MATRIX_T_MV, _Offset);

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex + normalize(v.normal) * _Thickness + tranformedOffset);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return half4(0,0,0,0);
            }
            ENDCG
        }
    }
}