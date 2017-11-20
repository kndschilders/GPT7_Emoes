// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:0,lgpr:1,limd:0,spmd:1,trmd:1,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:False,rpth:0,vtps:0,hqsc:False,nrmq:0,nrsp:0,vomd:1,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:6,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:1,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:1,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:33131,y:33267,varname:node_2865,prsc:2|emission-9883-OUT;n:type:ShaderForge.SFN_TexCoord,id:4219,x:31382,y:33620,cmnt:Default coordinates,varname:node_4219,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Relay,id:8397,x:31607,y:33620,cmnt:Refract here,varname:node_8397,prsc:2|IN-4219-UVOUT;n:type:ShaderForge.SFN_Tex2dAsset,id:4430,x:31382,y:33817,ptovrint:False,ptlb:MainTex,ptin:_MainTex,cmnt:MainTex contains the color of the scene,varname:node_9933,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7542,x:32258,y:33361,varname:node_1672,prsc:2,ntxv:0,isnm:False|UVIN-1516-UVOUT,TEX-4430-TEX;n:type:ShaderForge.SFN_Color,id:6271,x:31674,y:33103,ptovrint:False,ptlb:FogColor,ptin:_FogColor,varname:node_6271,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_DepthBlend,id:9516,x:31877,y:32546,varname:node_9516,prsc:2|DIST-6867-OUT;n:type:ShaderForge.SFN_Slider,id:6867,x:31365,y:32510,ptovrint:False,ptlb:depth,ptin:_depth,varname:node_6867,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.1,cur:11.67775,max:100;n:type:ShaderForge.SFN_Lerp,id:9883,x:32676,y:33368,varname:node_9883,prsc:2|A-7542-RGB,B-6019-OUT,T-7664-OUT;n:type:ShaderForge.SFN_Lerp,id:6019,x:32258,y:33213,cmnt:fog color,varname:node_6019,prsc:2|A-6271-RGB,B-918-OUT,T-4449-OUT;n:type:ShaderForge.SFN_Color,id:6082,x:31671,y:32615,ptovrint:False,ptlb:BG_bot,ptin:_BG_bot,varname:node_6082,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_TexCoord,id:5795,x:31671,y:32939,varname:node_5795,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Vector1,id:1002,x:32148,y:32667,varname:node_1002,prsc:2,v1:10;n:type:ShaderForge.SFN_Power,id:4449,x:32258,y:33053,varname:node_4449,prsc:2|VAL-9516-OUT,EXP-8618-OUT;n:type:ShaderForge.SFN_Power,id:1333,x:32593,y:33050,cmnt:fog mask,varname:node_1333,prsc:2|VAL-9516-OUT,EXP-9264-OUT;n:type:ShaderForge.SFN_Vector1,id:3876,x:32556,y:32582,varname:node_3876,prsc:2,v1:3;n:type:ShaderForge.SFN_Lerp,id:918,x:31877,y:32768,cmnt:Background gradient,varname:node_918,prsc:2|A-6082-RGB,B-8994-RGB,T-5795-V;n:type:ShaderForge.SFN_Color,id:8994,x:31671,y:32786,ptovrint:False,ptlb:BG_top,ptin:_BG_top,varname:_BG_top_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Slider,id:8733,x:31982,y:32361,ptovrint:False,ptlb:fog thickness,ptin:_fogthickness,varname:node_8733,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.01,cur:0.1,max:10;n:type:ShaderForge.SFN_Multiply,id:8618,x:32333,y:32667,varname:node_8618,prsc:2|A-8939-OUT,B-1002-OUT;n:type:ShaderForge.SFN_Multiply,id:9264,x:32556,y:32667,varname:node_9264,prsc:2|A-8939-OUT,B-3876-OUT;n:type:ShaderForge.SFN_Slider,id:7493,x:32436,y:33193,ptovrint:False,ptlb:maxFog,ptin:_maxFog,varname:node_7493,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:-1,max:1;n:type:ShaderForge.SFN_Add,id:7664,x:32778,y:33146,varname:node_7664,prsc:2|A-1333-OUT,B-7493-OUT;n:type:ShaderForge.SFN_Clamp01,id:310,x:32947,y:33146,varname:node_310,prsc:2|IN-7664-OUT;n:type:ShaderForge.SFN_Divide,id:8939,x:32333,y:32531,varname:node_8939,prsc:2|A-8733-OUT,B-2687-OUT;n:type:ShaderForge.SFN_Divide,id:2687,x:31877,y:32412,varname:node_2687,prsc:2|A-6867-OUT,B-6867-OUT;n:type:ShaderForge.SFN_TexCoord,id:1516,x:31141,y:33381,varname:node_1516,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_RemapRange,id:4379,x:31410,y:33378,varname:node_4379,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-1516-UVOUT;n:type:ShaderForge.SFN_RemapRange,id:329,x:31785,y:33378,varname:node_329,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-3684-OUT;n:type:ShaderForge.SFN_Add,id:3684,x:31588,y:33378,varname:node_3684,prsc:2|A-5855-OUT,B-4379-OUT;n:type:ShaderForge.SFN_Multiply,id:5855,x:31588,y:33261,varname:node_5855,prsc:2|A-9735-OUT,B-227-OUT,C-858-OUT;n:type:ShaderForge.SFN_Vector1,id:858,x:31350,y:32938,varname:node_858,prsc:2,v1:2;n:type:ShaderForge.SFN_RemapRange,id:227,x:31350,y:33180,varname:node_227,prsc:2,frmn:0,frmx:1,tomn:-0.5,tomx:0.5|IN-1516-V;n:type:ShaderForge.SFN_RemapRange,id:9735,x:31350,y:33005,varname:node_9735,prsc:2,frmn:0,frmx:1,tomn:-0.5,tomx:0.5|IN-1516-U;proporder:4430-6271-6867-6082-8994-8733-7493;pass:END;sub:END;*/

Shader "Shader Forge/FogShader" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _FogColor ("FogColor", Color) = (1,0,0,1)
        _depth ("depth", Range(0.1, 100)) = 11.67775
        _BG_bot ("BG_bot", Color) = (0,0,0,1)
        _BG_top ("BG_top", Color) = (0,0,0,1)
        _fogthickness ("fog thickness", Range(0.01, 10)) = 0.1
        _maxFog ("maxFog", Range(-1, 1)) = -1
    }
    SubShader {
        Tags {
            "Queue"="Geometry+1"
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZTest Always
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _FogColor;
            uniform float _depth;
            uniform float4 _BG_bot;
            uniform float4 _BG_top;
            uniform float _fogthickness;
            uniform float _maxFog;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 projPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
////// Lighting:
////// Emissive:
                float4 node_1672 = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_9516 = saturate((sceneZ-partZ)/_depth);
                float node_8939 = (_fogthickness/(_depth/_depth));
                float node_7664 = (pow(node_9516,(node_8939*3.0))+_maxFog);
                float3 emissive = lerp(node_1672.rgb,lerp(_FogColor.rgb,lerp(_BG_bot.rgb,_BG_top.rgb,i.uv0.g),pow(node_9516,(node_8939*10.0))),node_7664);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
