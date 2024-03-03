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
		_Size ("Size", FLOAT) = .2
		_Thickness ("Thickness", FLOAT) = .2
		_Gap ("Gap", FLOAT) = .2
		_GapOffset ("Gap Offset", FLOAT) = .2
		_AnimSpeed ("Animation speed", FLOAT) = 3.0
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
				float _Size;
				float _Thickness;
				float _Gap;
				float _GapOffset;
				float _AnimSpeed;
				float _AnimT;
				float _AnimDir;

				float Box(fixed2 p, float s)
				{
					return length(max(p - s, 0));
				}

				fixed4 frag (v2f i) : SV_Target
				{
					fixed4 col = fixed4(1.0, 0.0, 0.0, 1.0);
					fixed2 uv = i.texcoord - fixed2(.5, .5);

					float time = (_Time.y - _AnimT) * _AnimSpeed;
					float t;
					if(_AnimDir > 0)
						t = min(time, 1.0);
					else
						t = max(1.0 - time, 0.0); 
					float gap = _Gap + _GapOffset * (1.0 - t);

					float mask1 = step(Box(abs(uv) + _Roundness - _Thickness - gap, _Size), _Roundness);
					float mask2 = step(Box(abs(uv) + _Roundness - gap, _Size), _Roundness);
					float mask = mask1 * (1.0 - mask2);

					col.rgb = _Color;
					col.a = mask * t;
					return col;
				}
			ENDCG
		}
	}
}