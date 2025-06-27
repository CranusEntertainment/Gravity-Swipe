Shader "Custom/ConstantSpeedLineBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurAmount ("Blur Amount", Float) = 0.5
        _Opacity ("Opacity", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _BlurAmount;
            float _Opacity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = v.uv + float2(0.02, 0); // Sabit UV kayması
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 blurUV = i.uv;
                fixed4 col = tex2D(_MainTex, blurUV);

                // Sabit bir smear efekti
                for (int j = 1; j < 5; j++)
                {
                    blurUV.x += _BlurAmount * 0.01;
                    col += tex2D(_MainTex, blurUV) * 0.2;
                }

                col /= 2.0;
                col.a *= _Opacity;
                return col;
            }
            ENDCG
        }
    }
}
