#ifndef S_OUTLINEVARS_01_INCLUDED
#define S_OUTLINEVARS_01_INCLUDED

uniform float2 meshOffsetPan = float2(0, 0);

float2 GetMeshOffsetPan_float(out float2 Out) {
    Out = meshOffsetPan;
    return meshOffsetPan;
}

#endif