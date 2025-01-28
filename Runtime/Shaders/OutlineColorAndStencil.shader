Shader "Hidden/Outline Color And Stencil"
{
    Properties
    {
        [MainColor][HDR]_BaseColor("Base Color", Color) = (0, 1, 0, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        // To make the Unity shader SRP Batcher compatible, declare all
        // properties related to a Material in a a single CBUFFER block with 
        // the name UnityPerMaterial.
        CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
        CBUFFER_END

        ENDHLSL

        Pass
        {
            Name "Draw Solid Color"

            Stencil
            {
                Ref 15
                Comp Always
                Pass Replace
                Fail Keep
                ZFail Keep
            }
            
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag() : SV_Target
            {         
                return _BaseColor;
            }
            ENDHLSL
        }
    }
}