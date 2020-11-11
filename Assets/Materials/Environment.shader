Shader "Unlit/Environment"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _TColor("Top Color", Color) = (1,0,0,1)
        _TopAngle("Angle of y axis", Vector) = (0,1,0)
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }
        CGPROGRAM
        #pragma surface surf NoLighting

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
        };

        struct appdata
        {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
        };

        float4 _BaseColor;
        float4 _TColor;
        float4 _TopAngle;

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = dot(o.Normal, _TopAngle.xyz) >= 1 - 0.3 ? _TColor : _BaseColor;
        }
        ENDCG
    }
    Fallback "Diffuse"
}