// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Transparent" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_ForeColor ("Foreground Color", COLOR) = (1,1,1,1)
		_BackColor ("Background Color", COLOR) = (1,1,1,1)
		_Roundness ("Roundness", FLOAT) = .2
		_Size ("Size", FLOAT) = .2
		_Zoom ("Zoom", FLOAT) = 1.2
		_VignetteStrength ("Vignete strength", FLOAT) = 1.2
		_VignetteSize ("Vignete size", FLOAT) = 1.2
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
				float4 _ForeColor;
				float4 _BackColor;
				float _Roundness;
				float _Size;
				float _Zoom;
				float _VignetteStrength;
				float _VignetteSize;

				fixed4 frag (v2f i) : SV_Target
				{
					fixed4 col = fixed4(1.0, 0.0, 0.0, 1.0);
					fixed2 uv = i.texcoord;
					fixed2 centeredUv = uv - fixed2(.5, .5);

					fixed2 maskUv = centeredUv;
					maskUv = abs(maskUv);
					maskUv -= _Roundness;
					maskUv = max(fixed2(0.0, 0.0), maskUv);

					float mask = length(maskUv);
					mask = step(mask, _Size);
					
					fixed2 texUv = uv;
					texUv -= fixed2(.5, .5);
					texUv *= _Zoom;
					texUv += fixed2(.5, .5);
					
					float vignette = length(centeredUv);
					vignette -= _VignetteStrength;
					vignette = smoothstep(0.0, _VignetteSize, vignette);

					fixed4 texCol = tex2D(_MainTex, texUv);

					float background = 1.0 - texCol.a;

					col.rgb = background * _BackColor * vignette + texCol.a * texCol.rgb;

					col.a *= mask;

					return col;
				}
			ENDCG
		}
	}
}