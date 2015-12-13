Shader "Hidden/Editor/GL" 
{
	Properties 
	{
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("RGB(A)", 2D) = "white" {}
	}
	
	Category 
	{
		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		BindChannels 
		{
			Bind "Color", color
		}
		
		SubShader 
		{
			Tags {"Queue" = "Transparent" }
			Pass 
			{
				SetTexture [_MainTex] 
				{
					constantColor [_Color]
					Combine previous  * constant, previous * constant
				}
			}
		}
	}
	Fallback "Unlit/Texture"
}
