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
	     Blend SrcAlpha OneMinusSrcAlpha
	     Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		CGPROGRAM
		#pragma surface surf Lambert noforwardadd alpha

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
			float alpha = 1.0-(length(cross(q,n))/(width/4.0));
			if (alpha <= 0)
				discard;
			alpha *= min(min(distance(pt,start), distance(pt, end))*10.0-1.0,1.0);
			
			if (alpha <= 0)
				discard;
			float t = distance(pt, start)*32.0;
			
			float f1 = max(cos(-_Time.x*500.0+t),0.1);
			float f2 = max(sin(_Time.x*150.9+t),0.1);
			
			float f3 = min(distance(pt,start)/distance(start,end),1.0);
			float f4 = min(distance(end,start)/distance(start,end),1.0);
			
			f3 = (sin(_Time.x*3000.0+f3*9.0)+1.0)/2.0;
			f4 = (cos(-_Time.x*2010.5+f4*2.0)+1.0)/2.0;
			
			alpha *= min(sqrt(f1+f2),1.0);
		
			//alpha *= min(1.0, distance(start, pt)*3.0);
			fixed4 c = float4(alpha,pow(f3*alpha,2.0),pow(f4*alpha,2.0),alpha);//float4(IN.uv_MainTex, IN.uv_MainTex);//tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Emission = c.rgb;
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
