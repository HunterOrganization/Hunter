Shader "Hunter/BgShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("MainTex", 2D) = "white" {}
		_Speed ("Speed", Range(-10,10)) = 0.1
		_Cutoff ("Cutoff", Float) = 0.1
		_Dict ("Dict", Float) = 0
		_TheTime ("TheTime", Range(0,1)) = 1
		_ShadowTex ("ShadowTex", 2D) = "white" {}
	}
	SubShader {
		Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
		Pass{
			Tags {"LightMode" = "ForwardBase"}
			ZWrite off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#include "UnityCG.cginc"   
			#pragma vertex vert
			#pragma fragment frag
			
			sampler2D _MainTex;
			fixed4 _Color;
			fixed _Speed;
			float _Cutoff;
			float _Dict;
			fixed _TheTime;
			sampler2D _ShadowTex;

			struct a2v {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
			};

			v2f vert(a2v v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

				//fixed inc = frac(_Time.y * _Speed);

				fixed4 scrolledUV  = v.texcoord;

				//scrolledUV.x += inc;
				scrolledUV.x += _Dict * _Speed; // add
				o.uv = scrolledUV;
				return o;
			}

			fixed4 frag(v2f i): SV_Target{
				fixed4 c = tex2D(_MainTex, i.uv);
				fixed4 s = tex2D(_ShadowTex, i.uv);
				clip(c.a - _Cutoff);
				return lerp(c, s, _TheTime);
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
