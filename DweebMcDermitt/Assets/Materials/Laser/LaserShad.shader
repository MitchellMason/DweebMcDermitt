Shader "Custom/LaserShad" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
		_Start ("Start", Vector) = (0,0,0)
		_End ("End", Vector) = (0,0,0)
		_Width ("Wdith", Float) = 2
	}
	SubShader { 
	
	Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		
		Lighting Off
		CGPROGRAM
		#pragma surface surf Lambert alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		float3 _Start;
		float3 _End;
		float _Width;
		
		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			float width = _Width;
			float3 start = _Start;
			float3 end = _End;
			float3 n = normalize(start-end);
			float3 pt = IN.worldPos;
			float3 q = pt-start;
			float alpha = 1.0-(length(cross(q,n))/(width));
			if (alpha <= 0)
				discard;
			float distend = distance(end, pt);
			alpha *= min(1.0, distend*2.0);
			alpha *= min(1.0, distance(start, pt)*3.0);
			fixed4 c = float4(1.0,0.1,0.1,1.0)*alpha;//float4(IN.uv_MainTex, IN.uv_MainTex);//tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
