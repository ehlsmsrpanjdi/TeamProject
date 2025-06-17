Shader "Unlit/PerlinNoise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("Scale", Float) = 1.0
        _Speed ("Speed", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Scale;
            float _Speed;

            float2 hash22(float2 p)
            {
                p = frac(p * float2(26214.7, 44375.8));
                p += dot(p, p + 39.7);
                return frac(float2(p.x * p.y, p.y * p.y) * 45.2317);
            }


            // Basic Perlin Noise function
            float snoise(float2 uv)
            {
                uv *= _Scale;
                uv += _Time.y * _Speed;
                float2 i = floor(uv);
                float2 f = frac(uv);
                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(
                    lerp(dot(hash22(i + float2(0.0, 0.0)), f - float2(0.0, 0.0)),
                         dot(hash22(i + float2(1.0, 0.0)), f - float2(1.0, 0.0)), u.x),
                    lerp(dot(hash22(i + float2(0.0, 1.0)), f - float2(0.0, 1.0)),
                         dot(hash22(i + float2(1.0, 1.0)), f - float2(1.0, 1.0)), u.x), u.y);
            }

            // Hash function for Perlin noise

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the noise and color it
                fixed4 col = fixed4(snoise(i.uv),snoise(i.uv),snoise(i.uv), 1.0);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}