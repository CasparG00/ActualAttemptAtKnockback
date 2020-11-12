Shader "Unlit/Environment"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _TColor("Top Color", Color) = (1,0,0,1)
        _TopAngle("Angle of y axis", Vector) = (0,1,0)

        _FogColor("Color of Fog", Color) = (1, 1, 1, 1)
        _FogStart("Fog Start", float) = 0
        _FogEnd("Fog End", float) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }
        CGPROGRAM
        #pragma surface surf NoLighting finalcolor:mycolor vertex:myvert

        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            fixed4 c;
            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }

        struct Input
        {
            float3 worldPos;
            float3 viewDir;
            half fog;
        };

        struct appdata
        {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
        };

        float4 _BaseColor;
        float4 _TColor;
        float4 _TopAngle;

        float4 _FogColor;
        half _FogStart;
        half _FogEnd;

        void myvert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);
            float4 pos = mul(unity_ObjectToWorld, v.vertex).xyzw;
            data.fog = saturate((_FogStart - pos.y) / (_FogStart - _FogEnd));
        }

        void mycolor(Input IN, SurfaceOutput o, inout fixed4 color)
        {
            fixed3 fogColor = _FogColor.rgb;
            //#ifdef UNITY_PASS_FORWARDADD
            //fogColor = 0;
            //#endif
            color.rgb = lerp(color.rgb, fogColor, IN.fog);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            //Change Colour of face when it's facing up (With slight margin)
            o.Albedo = dot(o.Normal, _TopAngle.xyz) >= 1 - 0.3 ? _TColor : _BaseColor;
        }
        ENDCG
    }
    Fallback "Diffuse"
}