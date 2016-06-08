Shader "Custom/lava" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "yellow" {}
		_Lava ("LavaBlur (BW)", 2D) = "red" {}
		_Slide ("SliderTime", Range(0, 5)) = 0
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
			float2 uv_MainTex;
			float2 uv_Lava;
		};
		
		sampler2D _MainTex;
		sampler2D _Lava;
		uniform float _Slide;

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float time = _Time;
			half4 lavaflow = tex2D(_Lava, float2(IN.uv_Lava.x,IN.uv_Lava.y+(time/8)));
			half4 col1 = tex2D(_MainTex, float2(IN.uv_MainTex.x+lavaflow.r,IN.uv_MainTex.y+lavaflow.r))*1;
			o.Albedo = col1.rgb;
			o.Alpha = 0.2f;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}