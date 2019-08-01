Shader "Custom/BlockStencilRead"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Colour ("Colour", Color) = (1, 1, 1, 1)
		_Brightness ("Brightness", Float) = 5
		
		[IntRange] _StencilRef("Stencil Reference Value", Range(0, 255)) = 0
	}
	Subshader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"Queue" = "Geometry"
		}

		Stencil
		{
			Ref[_StencilRef]
			Comp Equal
		}

		CGPROGRAM
		#include "UnityCG.cginc"

		#pragma surface surf Lambert
		#pragma target 3.0

		//Variables
		sampler2D _MainTex;
		fixed4 _Colour;
		float _Brightness;

		//Input structure
		struct Input 
		{
			float2 uv_MainTex;
		};
		
		//Draw
		void surf(Input IN, inout SurfaceOutput o) 
		{
			fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
			
			clip(col.a - 0.5);
			
			col *= _Colour * _Brightness;
			
			o.Albedo = col.rgba;
		}
		
		ENDCG
	}
	
	FallBack "Sprites/Default"
}
