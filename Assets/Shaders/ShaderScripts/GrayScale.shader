Shader "Sprites/GrayScale"
{
	Properties
	{
		_MainTexture("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _Colour("Colour", Color) = (1, 1, 1, 1)
		_Saturation("Saturation", Range(-100, 100)) = 0.0
		_Contrast("Contrast", Range(1, 10)) = 1.0
		_Exposure("Exposure", Range(-5, 5)) = 0.0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex Vertex
			#pragma fragment Fragment
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 Vertex   : POSITION;
				float4 Colour    : COLOR;
				float2 TexCoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 Vertex   : POSITION;
				fixed4 Colour	: COLOR;
				half2 TexCoord  : TEXCOORD0;
			};


			fixed4 _Colour;

			v2f Vertex(appdata_t IN)
			{
				v2f OUT;
				OUT.Vertex = UnityObjectToClipPos(IN.Vertex);
				OUT.TexCoord = IN.TexCoord;
				OUT.Colour = IN.Colour * _Colour;

				return OUT;
			}

			sampler2D _MainTexture;
			uniform float _Saturation;
			uniform float _Contrast;
			uniform float _Exposure;

			fixed4 Fragment(v2f IN) : COLOR
			{
				half4 texcol = tex2D(_MainTexture, IN.TexCoord);
				texcol.rgb = lerp(texcol.rgb, dot(texcol.rgb, float3(0.3, 0.59, 0.11)), _Saturation);
				texcol.rgb = ((texcol.rgb - 0.5f) * max(_Contrast, 0)) + 0.5f;
				texcol.rgb += _Exposure;
				texcol = texcol * IN.Colour;

				return texcol;
			}

			ENDCG
		}


	}

	Fallback "Sprites/Default"
}
