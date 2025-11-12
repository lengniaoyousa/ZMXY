Shader "Custom/DoppelgangerEffect"
{
    Properties
    {
        _MainTex ("主纹理", 2D) = "white" {}
        _Distance ("分身间距", Range(0, 2)) = 0.5
        _Alpha ("分身透明度", Range(0, 1)) = 0.7
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

        // Pass 0: 渲染本身
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
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return col;
            }
            ENDCG
        }

        // Pass 1: 渲染左侧分身
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
            float _Distance;
            float _Alpha;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                
                // 向左偏移创建分身
                float4 offsetVertex = v.vertex;
                offsetVertex.x -= _Distance; // 向左移动
                
                o.vertex = UnityObjectToClipPos(offsetVertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                col.rgb *= _Color.rgb;    // 应用颜色 tint
                col.a *= _Alpha;          // 应用透明度
                return col;
            }
            ENDCG
        }

        // Pass 2: 渲染右侧分身
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
            float _Distance;
            float _Alpha;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                
                // 向右偏移创建分身
                float4 offsetVertex = v.vertex;
                offsetVertex.x += _Distance; // 向右移动
                
                o.vertex = UnityObjectToClipPos(offsetVertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                col.rgb *= _Color.rgb;    // 应用颜色 tint
                col.a *= _Alpha;          // 应用透明度
                return col;
            }
            ENDCG
        }
    }
}