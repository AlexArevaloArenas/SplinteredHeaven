Shader "Hidden/FOWMemoryBlend"
{
    SubShader
    {
        Pass
        {
            ZTest Always ZWrite Off Cull Off
            Blend One OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_base v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 current = tex2D(_MainTex, i.uv);
                return max(current, fixed4(0,0,0,1)); // clamp visible areas
            }
            ENDCG
        }
    }
}
