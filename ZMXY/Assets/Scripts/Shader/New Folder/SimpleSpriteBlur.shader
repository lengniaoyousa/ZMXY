Shader "Custom/SimpleSpriteBlur"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        
        // 模糊参数
        _HorizontalBlur ("Horizontal Blur", Range(0, 10)) = 1.0
        _VerticalBlur ("Vertical Blur", Range(0, 10)) = 0.0
        _Samples ("Sample Count", Range(1, 10)) = 5
        
        [Enum(Repeat,0, Clamp,1, Mirror,2)] _WrapMode ("Wrap Mode", Float) = 0
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _Color;
            float _HorizontalBlur;
            float _VerticalBlur;
            int _Samples;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;
                #ifdef PIXELSNAP_ON
                o.vertex = UnityPixelSnap(o.vertex);
                #endif
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 如果没有模糊，直接返回原始颜色
                if (_HorizontalBlur <= 0 && _VerticalBlur <= 0)
                {
                    fixed4 c = tex2D(_MainTex, i.texcoord) * i.color;
                    c.rgb *= c.a; // 预乘alpha
                    return c;
                }

                // 计算模糊偏移
                float2 blurOffset = float2(_MainTex_TexelSize.x * _HorizontalBlur, 
                                          _MainTex_TexelSize.y * _VerticalBlur);
                
                fixed4 blurredColor = fixed4(0, 0, 0, 0);
                float weightSum = 0.0;
                
                // 高斯模糊权重
                const float weights[10] = {0.227027, 0.1945946, 0.1216216, 0.054054, 0.016216, 
                                          0.008108, 0.004054, 0.002027, 0.0010135, 0.00050675};
                
                // 中心采样
                fixed4 centerColor = tex2D(_MainTex, i.texcoord);
                blurredColor += centerColor * weights[0];
                weightSum += weights[0];
                
                // 四周采样
                for (int j = 1; j < _Samples; j++)
                {
                    // 水平方向
                    if (_HorizontalBlur > 0)
                    {
                        fixed4 rightColor = tex2D(_MainTex, i.texcoord + float2(blurOffset.x * j, 0));
                        fixed4 leftColor = tex2D(_MainTex, i.texcoord - float2(blurOffset.x * j, 0));
                        
                        blurredColor += rightColor * weights[j];
                        blurredColor += leftColor * weights[j];
                        weightSum += weights[j] * 2;
                    }
                    
                    // 垂直方向
                    if (_VerticalBlur > 0)
                    {
                        fixed4 upColor = tex2D(_MainTex, i.texcoord + float2(0, blurOffset.y * j));
                        fixed4 downColor = tex2D(_MainTex, i.texcoord - float2(0, blurOffset.y * j));
                        
                        blurredColor += upColor * weights[j];
                        blurredColor += downColor * weights[j];
                        weightSum += weights[j] * 2;
                    }
                }
                
                // 归一化并应用颜色
                blurredColor /= weightSum;
                blurredColor *= i.color;
                blurredColor.rgb *= blurredColor.a; // 预乘alpha
                
                return blurredColor;
            }
            ENDCG
        }
    }
}