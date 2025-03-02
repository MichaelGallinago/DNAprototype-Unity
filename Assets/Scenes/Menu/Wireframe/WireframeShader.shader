// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "DNA/WireframeShader"
{
    Properties
    {
        _Snap ("Snap", RANGE(0.0, 1000.0)) = 100.0
        _WireColor ("Wire Color", Color) = (1.0, 0.0, 1.0)
        _ModelColor ("Model Color", Color) = (0.0, 0.0, 0.0)
    }

    SubShader
    {
        // Each color represents a meter.

        Tags { "RenderType"="Opaque" }

        Pass
        {
            // Wireframe shader based on the the following
            // http://developer.download.nvidia.com/SDK/10/direct3d/Source/SolidWireframe/Doc/SolidWireframe.pdf

            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _Snap;
            float4 _WireColor;
            float4 _ModelColor;

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2g
            {
                float4 projectionSpaceVertex : SV_POSITION;
                float4 worldSpacePosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO_EYE_INDEX
            };

            struct g2f
            {
                float4 projectionSpaceVertex : SV_POSITION;
                float2 barycentricCoords : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float3 inverseLerp(float3 a, float3 b, float3 t)
            {
                return (t - a) / (b - a);
            }

            v2g vert(appdata v)
            {
                v2g o = (v2g)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT_STEREO_EYE_INDEX(o);

                v.vertex.xyz = round(v.vertex.xyz * _Snap) / _Snap;
                o.projectionSpaceVertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            [maxvertexcount(3)]
            void geom(triangle v2g i[3], inout TriangleStream<g2f> triangleStream)
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i[0]);
                
                g2f o;
                
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.projectionSpaceVertex = i[0].projectionSpaceVertex;
                o.barycentricCoords = float2(1.0, 0.0);
                triangleStream.Append(o);
                
                o.projectionSpaceVertex = i[1].projectionSpaceVertex;
                o.barycentricCoords = float2(0.0, 1.0);
                triangleStream.Append(o);
                
                o.projectionSpaceVertex = i[2].projectionSpaceVertex;
                o.barycentricCoords = float2(0.0, 0.0);
                triangleStream.Append(o);
            }

            fixed4 frag(g2f i) : SV_Target
            {
                float3 barycentric;
                barycentric.xy = i.barycentricCoords;
                barycentric.z = 1.0 - barycentric.x - barycentric.y;
                
                float3 delta = fwidth(barycentric);
                barycentric = saturate(inverseLerp(0.0, delta, barycentric));
                float minBarycentric = min(barycentric.x, min(barycentric.y, barycentric.z));
                
                fixed4 finalColor = lerp(_ModelColor, _WireColor, round(1.0 - minBarycentric));
                finalColor.a = delta;

                return finalColor;
            }
            ENDCG
        }
    }
}