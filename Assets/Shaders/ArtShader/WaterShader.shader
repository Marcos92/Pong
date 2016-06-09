Shader "Custom/WaterShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_Scale("Scale ", float) = 1
		_Speed("Speed ", float) = 1
		_Frequency("Frequency ", float) = 1
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Lambert vertex:vert
#pragma surface surf Lambert vertex:vert


		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		// Variaveis vão dar valores a fequencia com que as cenas vão acontecer.
		sampler2D _MainTex;
	float _Scale, _Speed, _Frequency;

	struct Input {
		float2 uv_MainTex;
	};

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;

	// Vai dar forma à superficie:
	void vert(inout appdata_full v)
	{   // move-se a como ondas
		half offsetvert = v.vertex.x + v.vertex.z;

		// move-se a partir do centro:
		//	half offsetvert = (v.vertex.x * v.vertex.x) + (v.vertex.z * v.vertex.z);
		half value = _Scale * sin(_Time.w * _Speed + offsetvert * _Frequency);

		v.vertex.y += value;
	}

	// cores e assim da superficie:

	void surf(Input IN, inout SurfaceOutput o) {
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}
