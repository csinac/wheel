// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "WOPF/SliceSolidUI"
{
    Properties
    {
        tint("Tint", Color) = (1,1,1,1)
        borderColor("Border Color", Color) = (0,0,0,1)
        border("BorderThickness", Range(0, 1)) = 0.35
        alpha("Alpha", Range(0, 1)) = 1
        _MainTex ("Texture", 2D) = "white" {}
        blur("Blur", Range(0, 0.25)) = 0.01
        arc("Arc", Range(0.001, 3.1415926)) = 1
        aspect("Aspect", Float) = 1

        /* Necessary for making the shader UI compatible */
        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
    }
    SubShader
    {
        /* Necessary for making the shader UI compatible */
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        /* Necessary for making the shader UI compatible */
        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest[unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask[_ColorMask]

        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work

            #include "UnityCG.cginc"
            #include "UnityUI.cginc" //Necessary for making the shader UI compatible

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT //Necessary for making the shader UI compatible
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP //Necessary for making the shader UI compatible

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR; //Necessary for making the shader UI compatible
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPosition : TEXCOORD1; //Necessary for making the shader UI compatible
                fixed4 color : COLOR0; //Necessary for making the shader UI compatible
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ClipRect;
            float border, blur;
            fixed4 tint, borderColor;
            float arc;
            float alpha;
            float aspect;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;

                float2 pos;
                pos.x = i.uv.y;
                pos.y = i.uv.x;
                float xRange = sin(arc / 2) * 2;
                pos.x = (pos.x - 0.5) * xRange;
                pos.y = -pos.y;

                float fade = 0;
                bool inSlice = false;

                float ang = atan(pos.y / pos.x) / (UNITY_PI);
                ang = (ang + 0.5) / 2;
                if(pos.x < 0) ang += 0.5;
                ang *= UNITY_PI * 2;
                ang += arc / 2;
                ang %= UNITY_PI * 2;

                if(ang < arc) {
                    float dist = pos.x * pos.x + pos.y * pos.y;

                    if (dist > (1 - border) && dist < 1) {
                        fade = 1;
                        inSlice = true;
                    }
                    else if(dist < (1 - border)) {
                        fade = 1;
                        inSlice = true;
                        fade = 1-(1 - border - dist) / blur;
                    }
                    else if(dist > 1) {
                        //fade = 0;
                        fade = 1-(dist - 1) / blur;
                    }

                    if(ang > arc)
                        fade *= 1-(ang - arc);

                    fade = clamp(fade, 0, 1);
                }

                col.rgb = borderColor.rgb * fade + tint.rgb * (1 - fade);
                float2 uvCorrected = i.uv;
                uvCorrected.y *= aspect;
                col.rgb *= tex2D(_MainTex, uvCorrected);

                col.a = inSlice ? borderColor.a * fade + tint.a * (1-fade) * alpha : 0;

                col *= i.color;

#ifdef UNITY_UI_CLIP_RECT
                col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
#endif

#ifdef UNITY_UI_ALPHACLIP
                clip(col.a - 0.001);
#endif
                return col;
            }
            ENDCG
        }
    }
}
