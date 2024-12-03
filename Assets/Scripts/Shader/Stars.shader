Shader "Try/Stars"
{
    Properties
    {
        [Header(Sky Setting)]
        // _Color1 ("Top Color", Color) = (1, 1, 1, 0)
        // _Color2 ("Horizon Color", Color) = (1, 1, 1, 0)
        // _Color3 ("Bottom Color", Color) = (1, 1, 1, 0)
        _Color1 ("Top Color", Color) = (0, 0, 0, 1)
        _Color2 ("Horizon Color", Color) = (0, 0, 0, 1)
        _Color3 ("Bottom Color", Color) = (0, 0, 0, 1)
        _Exponent1 ("Exponent Factor for Top Half", Float) = 1.0
        _Exponent2 ("Exponent Factor for Bottom Half", Float) = 1.0
        _Intensity ("Intensity Amplifier", Float) = 1.0


        [Header(Star Setting)]
        [HDR]_StarColor ("Star Color", Color) = (1,1,1,0)
        _StarIntensity("Star Intensity", Range(0,1)) = 0.5
        _StarSpeed("Star Speed", Range(0,1)) = 0.5
    }

    SubShader
    {
        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 position : POSITION;
                float3 texcoord : TEXCOORD0;
                float3 normal : NORMAL;
            };
    
            struct v2f
            {
                float4 position : SV_POSITION;
                float3 texcoord : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            // »·¾³±³¾°ÑÕÉ«
            half4 _Color1;
            half4 _Color2;
            half4 _Color3;
            half _Intensity;
            half _Exponent1;
            half _Exponent2;


            //ÐÇÐÇ 
            half4 _StarColor;
            half _StarIntensity;
            half _StarSpeed;

            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos (v.position);
                o.texcoord = v.texcoord;
                o.normal = v.normal;
                return o;
            }
            
            // ÐÇ¿ÕÉ¢ÁÐ¹þÏ£
            float StarAuroraHash(float3 x) {
	            float3 p = float3(dot(x,float3(214.1 ,127.7,125.4)),
			                dot(x,float3(260.5,183.3,954.2)),
                            dot(x,float3(209.5,571.3,961.2)) );

	            return -0.001 + _StarIntensity*frac(sin(p)*43758.5453123);
            }

            // ÐÇ¿ÕÔëÉù
            float StarNoise(float3 st){
                // ¾í¶¯ÐÇ¿Õ
                st += float3(0,_Time.y*_StarSpeed,0);

                // fbm
                float3 i = floor(st);
                float3 f = frac(st);
    
	            float3 u = f*f*(3.0-1.0*f);

                return lerp(lerp(dot(StarAuroraHash( i + float3(0.0,0.0,0.0)), f - float3(0.0,0.0,0.0) ), 
                                 dot(StarAuroraHash( i + float3(1.0,0.0,0.0)), f - float3(1.0,0.0,0.0) ), u.x),
                            lerp(dot(StarAuroraHash( i + float3(0.0,1.0,0.0)), f - float3(0.0,1.0,0.0) ), 
                                 dot(StarAuroraHash( i + float3(1.0,1.0,0.0)), f - float3(1.0,1.0,0.0) ), u.y), u.z) ;
            }

            half4 frag(v2f i):COLOR
            {
                // return fixed4(SurAuroraNoise(i.texcoord.xy),SurAuroraNoise(i.texcoord.xy),SurAuroraNoise(i.texcoord.xy),1);
                // µ×É«
                float p = normalize(i.texcoord).y;
                float p1 = 1.0f - pow (min (1.0f, 1.0f - p), _Exponent1);
                float p3 = 1.0f - pow (min (1.0f, 1.0f + p), _Exponent2);
                float p2 = 1.0f - p1 - p3;
                int reflection = i.texcoord.y < 0 ? -1 : 1;


                // ÐÇÐÇ
                float star  =StarNoise(fixed3(i.texcoord.x,i.texcoord.y * reflection,i.texcoord.z) * 128);
                float4 starOriCol = float4(_StarColor.r + 3.25*sin(i.texcoord.x) + 2.45 * (sin(_Time.y * _StarSpeed) + 1)*0.5,
                                           _StarColor.g + 3.85*sin(i.texcoord.y) + 1.45 * (sin(_Time.y * _StarSpeed) + 1)*0.5,
                                           _StarColor.b + 3.45*sin(i.texcoord.z) + 4.45 * (sin(_Time.y * _StarSpeed) + 1)*0.5,
                                           _StarColor.a + 3.85*star);
                star = star > 0.75 ? star:smoothstep(0.81,0.98,star);

                float4 starCol = fixed4((starOriCol * star).rgb,star);

                //»ìºÏ
                float4 skyCol = (_Color1 * p1 + _Color2 * p2 + _Color3 * p3) * _Intensity;
                starCol = reflection==1?starCol:starCol*0.5;
                skyCol = skyCol*(1 - starCol.a) + starCol * starCol.a;


                return skyCol;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
