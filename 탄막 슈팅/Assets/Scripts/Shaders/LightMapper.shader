Shader "Custom/LightMapper"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float2 _LightCoord;
			float _SquaredLightRange;

			float Square(float x) {
				return x * x;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				float squaredDst = Square(_LightCoord.x - i.uv.x * _MainTex_TexelSize.z) + Square(_LightCoord.y - i.uv.y * _MainTex_TexelSize.w);
				if (squaredDst <= _SquaredLightRange)
				{
					if (squaredDst <= _SquaredLightRange / 8 * 3)
						return fixed4(1, 1, 1, 1);

					fixed4 col = tex2D(_MainTex, i.uv);
					if (col.a >= 1)
						return col;
					return fixed4(1, 1, 1, .5);
				}

				fixed4 col = tex2D(_MainTex, i.uv);
				return fixed4(1, 1, 1, col.r);
			}
			ENDCG
		}
	}
}
