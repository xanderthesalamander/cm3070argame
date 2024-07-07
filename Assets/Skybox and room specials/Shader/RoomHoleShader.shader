Shader "Custom/RoomHoleShader"
{
    Properties
    {
        [IntRange] _RoomHoleShaderID ("Room Hole Shader ID", Range(0, 255)) = 0
    }
    SubShader
    {
        Tags {
            "RenderType"="Opaque"
            "Queue"="Geometry"
            "RendererPipeline"="UniversalPipeline"
        }

        Pass
        {
            Blend Zero One
            ZWrite Off

            Stencil {
                Ref[_RoomHoleShaderID]
                Comp Always
                Pass Replace
            }
        }
    }
}
