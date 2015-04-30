Shader "Custom/FadeoutEdge" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_Fade ("Fade", Range (0.0,0.25)) = 0.025
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="False" "RenderType"="Transparent"}
	LOD 300
	
CGPROGRAM
#pragma surface surf Lambert alpha

sampler2D _MainTex;
sampler2D _BumpMap;
fixed4 _Color;
float _Fade;

struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
};

void surf (Input IN, inout SurfaceOutput o) {
	float2 uv = IN.uv_MainTex;
	fixed4 c = tex2D(_MainTex, uv) * _Color;
	o.Albedo = c.rgb;
	float maxx = min(uv.x, 1.0-uv.x);
	float maxy = min(uv.y, 1.0-uv.y);
	float fade = _Fade;
	maxx = max(1.0-maxx/fade, 0.0);
	maxy = max(1.0-maxy/fade, 0.0);
	float maximum = 1.0;
	if (maxx && maxy > 0)
	{
		maximum -= sqrt(pow(maxx,2.0) + pow(maxy,2.0));
	}
	else
	{
		maximum -= maxx+maxy;
	}
	
	maximum = max(maximum, 0);
	if (maximum < 1.0)
		o.Alpha = maximum;
	else
		o.Alpha = 1.0;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
}
ENDCG
}

FallBack "Transparent/Diffuse"
}
