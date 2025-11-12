// ---------------------------【2D 外发光效果】---------------------------
// create by 长生但酒狂
// modified for outer glow
Shader "lcl/shader2D/OuterGlow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlowWidth("Glow Width",Range(0,10)) = 1
        _GlowColor("Glow Color",Color)=(1,1,1,1)
        _GlowIntensity("Glow Intensity",Range(0,5)) = 1
    }
    // ---------------------------【子着色器】---------------------------
    SubShader
    {
        // 渲染队列采用 透明
        Tags{
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            //顶点着色器输入结构体 
            struct VertexInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            //顶点着色器输出结构体 
            struct VertexOutput
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            // ---------------------------【顶点着色器】---------------------------
            VertexOutput vert (VertexInput v)
            {
                VertexOutput o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _GlowWidth;
            float4 _GlowColor;
            float _GlowIntensity;

            // ---------------------------【片元着色器】---------------------------
            fixed4 frag (VertexOutput i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                
                // 采样周围8个点进行外发光检测
                float2 up_uv = i.uv + float2(0,1) * _GlowWidth * _MainTex_TexelSize.xy;
                float2 down_uv = i.uv + float2(0,-1) * _GlowWidth * _MainTex_TexelSize.xy;
                float2 left_uv = i.uv + float2(-1,0) * _GlowWidth * _MainTex_TexelSize.xy;
                float2 right_uv = i.uv + float2(1,0) * _GlowWidth * _MainTex_TexelSize.xy;
                float2 upLeft_uv = i.uv + float2(-1,1) * _GlowWidth * _MainTex_TexelSize.xy;
                float2 upRight_uv = i.uv + float2(1,1) * _GlowWidth * _MainTex_TexelSize.xy;
                float2 downLeft_uv = i.uv + float2(-1,-1) * _GlowWidth * _MainTex_TexelSize.xy;
                float2 downRight_uv = i.uv + float2(1,-1) * _GlowWidth * _MainTex_TexelSize.xy;
                
                // 获取周围像素的alpha值
                float upAlpha = tex2D(_MainTex, up_uv).a;
                float downAlpha = tex2D(_MainTex, down_uv).a;
                float leftAlpha = tex2D(_MainTex, left_uv).a;
                float rightAlpha = tex2D(_MainTex, right_uv).a;
                float upLeftAlpha = tex2D(_MainTex, upLeft_uv).a;
                float upRightAlpha = tex2D(_MainTex, upRight_uv).a;
                float downLeftAlpha = tex2D(_MainTex, downLeft_uv).a;
                float downRightAlpha = tex2D(_MainTex, downRight_uv).a;
                
                // 计算周围像素的最大alpha值（用于外发光）
                float maxSurroundAlpha = max(
                    max(upAlpha, downAlpha),
                    max(leftAlpha, rightAlpha)
                );
                maxSurroundAlpha = max(maxSurroundAlpha, max(
                    max(upLeftAlpha, upRightAlpha),
                    max(downLeftAlpha, downRightAlpha)
                ));
                
                // 外发光逻辑：当前像素透明但周围有非透明像素
                float glowFactor = 0;
                if (col.a < 0.1 && maxSurroundAlpha > 0.1) {
                    glowFactor = maxSurroundAlpha * _GlowIntensity;
                }
                
                // 混合颜色：添加外发光
                fixed3 finalColor = col.rgb + _GlowColor.rgb * glowFactor;
                float finalAlpha = max(col.a, glowFactor * _GlowColor.a);
                
                return fixed4(finalColor, finalAlpha);
            }
            ENDCG
        }
    }
}