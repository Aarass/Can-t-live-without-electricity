// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Transparent" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", COLOR) = (1,1,1,1)
		_Roundness ("Roundness", FLOAT) = .2
		[ShowAsVector2] _Dimensions("Dimensions", Vector) = (0, 0, 0, 0)
	}

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
	
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
	
		Pass {  
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
			
				#include "UnityCG.cginc"

				struct p2v {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
			
				v2f vert (p2v v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.color = v.color;
					return o;
				}
				float4 _Color;
				float _Roundness;
				fixed2 _Dimensions;

				fixed4 frag (v2f i) : SV_Target
				{
					fixed2 uv = i.texcoord;
					fixed4 col = tex2D(_MainTex, uv);

					uv = abs(uv - fixed2(.5, .5));

					float j = (1.0 - _Dimensions.y / _Dimensions.x) * .5;
					float t = max(0, (uv.x - j) / (.5 - j));

					uv.x = t * .5;

					uv += _Roundness;
					uv = max(0, uv);
					float mask = length(uv) - _Roundness;
					mask = step(mask, .5);


					col.a *= mask;


					return col;
				}
			ENDCG
		}
	}
}