Shader "Custom/BlockStencilWrite"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		
		[IntRange] _StencilRef("Stencil Reference Value", Range(0, 255)) = 0
	}
	Subshader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"Queue" = "Geometry-1"
		}

		Stencil
		{
			Ref [_StencilRef]
			Comp Always
			Pass Replace
		}
		
		Pass
		{
			//The colour returned by this shader is ignored in preference of the previous shader's
			Blend Zero One
			//Do not occlude objects behind
			ZWrite Off
		
			CGPROGRAM
			#include "UnityCG.cginc"
			
			#pragma vertex vert
			#pragma fragment frag
			
			//Input structure
			struct Input 
			{
				float4 vertex : POSITION;
			};
			
			//Vertex structure
			struct v2f 
			{
				float4 position : SV_POSITION;
			};

			//Vertex shader
			v2f vert(Input IN)
			{
				v2f o;
				o.position = UnityObjectToClipPos(IN.vertex);
				return o;
			}
			
			//Fragment shader
			fixed4 frag(v2f vert) : SV_TARGET
			{
				return 0;
			}
			
			ENDCG
		}
	}
}
