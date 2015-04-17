Shader "Custom/FlatShader" {
	Properties {
		_Repeat( "Scale", float ) = 10
		_Texture( "Texture", 2D ) = "Default"
		
	}
	SubShader{
		Pass {
			CGPROGRAM
			
			// Pragmas
			#pragma vertex vert
			#pragma fragment frag
			
			// User Defined Variables
			float _Repeat;
			sampler2D _Texture;
			
			// Base Input Structs
			struct vertexInput {
				float4 vertex : POSITION;
				float4 texcoord: TEXCOORD0;
			};
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 tex: TEXCOORD0;
			};
			
			// Vertex Function
			vertexOutput vert(vertexInput v) {
				vertexOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex = v.texcoord;
				return o;			
			}
			
			// Fragment Function
			float4 frag(vertexOutput i) : COLOR {
				float2 texCoords = 0.1 * _Repeat * i.pos.xy;
				texCoords = texCoords - floor( texCoords );
				float4 tex = tex2D(_Texture, texCoords );
				return tex;
			}			
			
			ENDCG
		}
	}
}