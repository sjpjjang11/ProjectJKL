Shader "Custom/Rim" {
	Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _RimColor ("Rim Color", Color) = (0, 0, 0, 0)
      _RimPower ("Rim Power", float) = 0
      _RimFactor ("Rim Factor", float) = 0
      _RgbFactor ("RGB Factor", float) = 1
      _SkillColor ("Skill Color", Color) = (0, 0, 0, 0)
      _ConditionColor ("Condition Color", Color) = (0, 0, 0, 0)
      
	  //_OutlineColor ("Outline Color", Color) = (1, 0, 0, 1)
	  //_Outline ("Outline Width", float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
	      struct Input 
	      {
	          float2 uv_MainTex;
	          float2 uv_BumpMap;
	          float3 viewDir;
	      };
	      
	      sampler2D _MainTex;
	      float4 _RimColor;
	      float4 _SkillColor;
	      float4 _ConditionColor;
	      float _RimPower;
	      float _RimFactor;
	      float _RgbFactor;
	      
	      void surf (Input IN, inout SurfaceOutput o) 
	      {
			  half4 c = tex2D (_MainTex, IN.uv_MainTex);
			  half reverseRim = 1 - saturate(dot (normalize (IN.viewDir), o.Normal));
	          //half reverseRim = 1 - rim;
	          //o.Emission = c.rgb + _RimFactor * _RimColor * c.rgb * pow (reverseRim, _RimPower) + _SkillColor.rgb + _ConditionColor.rgb;
	          o.Emission = c.rgb * _RgbFactor + _RimFactor * _RimColor * c.rgb * pow (reverseRim, _RimPower) + _SkillColor.rgb + _ConditionColor.rgb;
	      }
		ENDCG
	} 
	FallBack "Diffuse"
}
