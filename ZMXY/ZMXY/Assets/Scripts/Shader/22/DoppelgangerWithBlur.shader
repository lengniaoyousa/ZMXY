Shader "Custom/DoppelgangerWithBlur"
{
    Properties
    {
        _MainTex ("主纹理", 2D) = "white" {}
        _Distance ("分身间距", Range(0, 2)) = 0.5
        _Alpha ("分身透明度", Range(0, 1)) = 0.7
        _Color ("分身颜色", Color) = (1, 1, 1, 1)
        
        // 模糊参数
        _BlurSize ("模糊强度", Range(0, 1)) = 0.02
        _BlurSamples ("模糊采样数", Range(1, 10)) = 5
        _MainBlur ("真身模糊", Range(0, 1)) = 0.01
        _DoppelgangerBlur ("分身模糊", Range(0, 1)) = 0.03
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

        // Pass 0: 渲染本身（带模糊）
        Pass
        {
            Name "MAIN"
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
            float _MainBlur;
            float _BlurSamples;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            // 高斯模糊函数
            float gaussian(float x, float sigma)
            {
                return exp(-(x * x) / (2.0 * sigma * sigma));
            }

            fixed4 blurSample(sampler2D tex, float2 uv, float blurStrength)
            {
                if (blurStrength < 0.001) return tex2D(tex, uv);
                
                fixed4 col = fixed4(0, 0, 0, 0);
                float kernelSum = 0.0;
                int samples = int(_BlurSamples);
                float2 texelSize = _MainTex_TexelSize.xy * blurStrength * 10;
                
                // 创建高斯核
                for (int i = -samples; i <= samples; i++)
                {
                    for (int j = -samples; j <= samples; j++)
                    {
                        float2 offset = float2(i, j) * texelSize;
                        float distance = length(float2(i, j));
                        float weight = gaussian(distance, samples * 0.5);
                        
                        fixed4 sampleCol = tex2D(tex, uv + offset);
                        col += sampleCol * weight;
                        kernelSum += weight;
                    }
                }
                
                return col / kernelSum;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = blurSample(_MainTex, i.uv, _MainBlur) * i.color;
                return col;
            }
            ENDCG
        }

        // Pass 1: 渲染左侧分身（带模糊）
        Pass
        {
            Name "LEFT_DOPPELGANGER"
            
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
            float _Distance;
            float _Alpha;
            float4 _Color;
            float _DoppelgangerBlur;
            float _BlurSamples;

            v2f vert (appdata v)
            {
                v2f o;
                
                // 向左偏移创建分身
                float4 offsetVertex = v.vertex;
                offsetVertex.x -= _Distance;
                
                o.vertex = UnityObjectToClipPos(offsetVertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                
                return o;
            }

            // 高斯模糊函数
            float gaussian(float x, float sigma)
            {
                return exp(-(x * x) / (2.0 * sigma * sigma));
            }

            fixed4 blurSample(sampler2D tex, float2 uv, float blurStrength)
            {
                if (blurStrength < 0.001) return tex2D(tex, uv);
                
                fixed4 col = fixed4(0, 0, 0, 0);
                float kernelSum = 0.0;
                int samples = int(_BlurSamples);
                float2 texelSize = _MainTex_TexelSize.xy * blurStrength * 10;
                
                for (int i = -samples; i <= samples; i++)
                {
                    for (int j = -samples; j <= samples; j++)
                    {
                        float2 offset = float2(i, j) * texelSize;
                        float distance = length(float2(i, j));
                        float weight = gaussian(distance, samples * 0.5);
                        
                        fixed4 sampleCol = tex2D(tex, uv + offset);
                        col += sampleCol * weight;
                        kernelSum += weight;
                    }
                }
                
                return col / kernelSum;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = blurSample(_MainTex, i.uv, _DoppelgangerBlur) * i.color;
                col.rgb *= _Color.rgb;
                col.a *= _Alpha;
                return col;
            }
            ENDCG
        }

        // Pass 2: 渲染右侧分身（带模糊）
        Pass
        {
            Name "RIGHT_DOPPELGANGER"
            
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
            float _Distance;
            float _Alpha;
            float4 _Color;
            float _DoppelgangerBlur;
            float _BlurSamples;

            v2f vert (appdata v)
            {
                v2f o;
                
                // 向右偏移创建分身
                float4 offsetVertex = v.vertex;
                offsetVertex.x += _Distance;
                
                o.vertex = UnityObjectToClipPos(offsetVertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                
                return o;
            }

            // 高斯模糊函数
            float gaussian(float x, float sigma)
            {
                return exp(-(x * x) / (2.0 * sigma * sigma));
            }

            fixed4 blurSample(sampler2D tex, float2 uv, float blurStrength)
            {
                if (blurStrength < 0.001) return tex2D(tex, uv);
                
                fixed4 col = fixed4(0, 0, 0, 0);
                float kernelSum = 0.0;
                int samples = int(_BlurSamples);
                float2 texelSize = _MainTex_TexelSize.xy * blurStrength * 10;
                
                for (int i = -samples; i <= samples; i++)
                {
                    for (int j = -samples; j <= samples; j++)
                    {
                        float2 offset = float2(i, j) * texelSize;
                        float distance = length(float2(i, j));
                        float weight = gaussian(distance, samples * 0.5);
                        
                        fixed4 sampleCol = tex2D(tex, uv + offset);
                        col += sampleCol * weight;
                        kernelSum += weight;
                    }
                }
                
                return col / kernelSum;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = blurSample(_MainTex, i.uv, _DoppelgangerBlur) * i.color;
                col.rgb *= _Color.rgb;
                col.a *= _Alpha;
                return col;
            }
            ENDCG
        }
    }
}