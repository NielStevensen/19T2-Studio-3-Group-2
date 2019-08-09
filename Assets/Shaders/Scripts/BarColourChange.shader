Shader "Custom/BarColourChange"
{
	Properties
	{
		 _Color("BarColour", Color) = (1,1,1,1)
		 _TexTure("BarTexture", 2D) = "white" {}
		 _Range("Brightness",Range(0,10)) = 2
	}

		SubShader
	{
		CGPROGRAM
			#pragma surface surf Lambert
		fixed4 _Color;
		sampler2D _TexTure;
		fixed _Range;

		struct Input
		{
			float2 uv_TexTure;
		};

        void surf (Input IN, inout SurfaceOutput o)
        {
			o.Albedo = (tex2D(_TexTure, IN.uv_TexTure) + _Color.rgb).rgb * _Range;
        }

        ENDCG
    }

    Fallback "Diffuse"
}
