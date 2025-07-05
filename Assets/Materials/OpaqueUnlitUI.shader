// Simple shader to draw a solid, opaque color for UI elements.
// It correctly writes to the depth buffer, fixing see-through issues.
Shader "UI/OpaqueUnlit"
{
    Properties
    {
        // This property allows you to pick a color in the Material's Inspector.
        [PerRendererData] _Color ("Main Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        Pass
        {
            // --- This is the important part ---
            // ZWrite On tells the GPU to write to the depth buffer.
            // ZTest LEqual tells it to only draw if it's in front of what's already there.
            ZWrite On
            ZTest LEqual
            Cull Off // UI quads are often single-sided, so we render both sides.

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Return the solid color set in the material.
                return _Color;
            }
            ENDCG
        }
    }
}
