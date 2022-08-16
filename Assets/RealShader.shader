 Shader "Custom/ClipBoxReal" {
     Properties {
         _Color ("Color", Color) = (1,1,1,1)
         _MainTex ("Albedo (RGB)", 2D) = "white" {}
         _Glossiness ("Smoothness", Range(0,1)) = 0.5
         _Metallic ("Metallic", Range(0,1)) = 0.0
     }
 
     SubShader {
         Tags { "RenderType"="Opaque" }
         LOD 200
 
         CGPROGRAM
         #pragma surface surf Standard fullforwardshadows
         #pragma target 3.0
 
         sampler2D _MainTex;
         half _Glossiness;
         half _Metallic;
         float4x4 _WorldToBoxReal;
         float4x4 _WorldToBoxReal2;
 
         struct Input {
             float2 uv_MainTex;
             float3 worldPos;
         };
 
         void surf (Input IN, inout SurfaceOutputStandard o) {
             float3 boxPosition = mul(_WorldToBoxReal, float4(IN.worldPos, 1));
             float3 boxPosition2 = mul(_WorldToBoxReal2, float4(IN.worldPos, 1));
             clip(
                max(
                    min(
                        min(boxPosition.x + 0.5, boxPosition.y + 0.5),
                        boxPosition.z + 0.5
                    ),
                    min(
                        min(boxPosition2.x + 0.5, boxPosition2.y + 0.5),
                        boxPosition2.z + 0.5
                    )
                )
             );
             clip(
                max(
                    min(
                        min(0.5 - boxPosition.x, 0.5 - boxPosition.y),
                        0.5 - boxPosition.z
                    ),
                    min(
                        min(0.5 - boxPosition2.x, 0.5 - boxPosition2.y),
                        0.5 - boxPosition2.z
                    )
                )
             );
 
 
            //  fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            //  fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            //  o.Albedo = c.rgb;
             o.Albedo = fixed3(0.8, 0.6, 0.4);
            //  o.Albedo = fixed3(1, 1, 1);
             o.Metallic = _Metallic;
             o.Smoothness = _Glossiness;
            //  o.Alpha = c.a;
         }
         ENDCG
     }
     FallBack "Diffuse"
 }
