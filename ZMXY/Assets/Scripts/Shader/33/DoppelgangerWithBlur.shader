Shader "Custom/DoppelgangerWithBlur"
{
    Properties
    {
        _MainTex ("主纹理", 2D) = "white" {}
        _Distance ("分身间距", Range(0, 2)) = 0.5
        _Direction ("分身方向", Vector) = (1, 0, 0, 0) // 控制分身方向
        
        // 模糊参数
        _BlurHorizontal ("水平模糊", Range(0, 1)) = 0.02
        _BlurVertical ("垂直模糊", Range(0, 1)) = 0.02
        _BlurSamples ("模糊采样数", Range(1, 10)) = 5
        
        // 效果参数
        _Alpha ("透明度", Range(0, 1)) = 0.7
        _Color ("分身颜色", Color) = (1, 1, 1, 1)
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent"
            "Queue" = "Transparent+100"
            "IgnoreProjector" = "True"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        CGINCLUDE
        #include "UnityCG.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            float4 color : COLOR;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
            float4 color : COLOR;
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;
        float4 _MainTex_TexelSize;
        float _BlurSamples;
        float _BlurHorizontal;
        float _BlurVertical;
        float _Alpha;
        float4 _Color;
        float _Distance;
        float4 _Direction; // 分身方向控制

        // 高斯模糊函数
        float gaussian(float x, float sigma)
        {
            return exp(-(x * x) / (2.0 * sigma * sigma));
        }

        fixed4 directionalBlurSample(sampler2D tex, float2 uv, float2 blurStrength)
        {
            if (blurStrength.x < 0.001 && blurStrength.y < 0.001) 
                return tex2D(tex, uv);
            
            fixed4 col = fixed4(0, 0, 0, 0);
            float kernelSum = 0.0;
            int samples = int(_BlurSamples);
            float2 texelSize = _MainTex_TexelSize.xy * blurStrength * 10;
            
            for (int i = -samples; i <= samples; i++)
            {
                for (int j = -samples; j <= samples; j++)
                {
                    float2 offset = float2(i * texelSize.x, j * texelSize.y);
                    float distance = length(float2(i, j));
                    float weight = gaussian(distance, samples * 0.5);
                    
                    fixed4 sampleCol = tex2D(tex, uv + offset);
                    col += sampleCol * weight;
                    kernelSum += weight;
                }
            }
            
            return col / kernelSum;
        }
        
        // 通用顶点着色器
        v2f vertBase(appdata v, float2 offsetDirection)
        {
            v2f o;
            float4 offsetVertex = v.vertex;
            offsetVertex.xy += offsetDirection * _Distance;
            o.vertex = UnityObjectToClipPos(offsetVertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            o.color = v.color;
            return o;
        }
        ENDCG

        // Pass 0: 渲染第一个分身
        Pass
        {
            Name "DOPPELGANGER_1"
            
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            v2f vert (appdata v)
            {
                return vertBase(v, _Direction.xy * -1); // 第一个分身反向
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 blurStrength = float2(_BlurHorizontal, _BlurVertical);
                fixed4 col = directionalBlurSample(_MainTex, i.uv, blurStrength) * i.color;
                col.rgb *= _Color.rgb;
                col.a *= _Alpha;
                return col;
            }
            ENDCG
        }

        // Pass 1: 渲染第二个分身
        Pass
        {
            Name "DOPPELGANGER_2"
            
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            v2f vert (appdata v)
            {
                return vertBase(v, _Direction.xy); // 第二个分身正向
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 blurStrength = float2(_BlurHorizontal, _BlurVertical);
                fixed4 col = directionalBlurSample(_MainTex, i.uv, blurStrength) * i.color;
                col.rgb *= _Color.rgb;
                col.a *= _Alpha;
                return col;
            }
            ENDCG
        }

        // Pass 2: 渲染真身（最后渲染，显示在最上层）
        Pass
        {
            Name "MAIN"
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 blurStrength = float2(_BlurHorizontal, _BlurVertical);
                fixed4 col = directionalBlurSample(_MainTex, i.uv, blurStrength) * i.color;
                col.a *= _Alpha;
                return col;
            }
            ENDCG
        }
    }
}