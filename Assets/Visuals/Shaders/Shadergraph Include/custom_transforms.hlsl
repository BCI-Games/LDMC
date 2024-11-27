#ifndef CUSTOM_TRANSFORMS_INCLUDED
#define CUSTOM_TRANSFORMS_INCLUDED

// Skew vertex coords by axis scale
void SkewByQuadrant_float(float2 vertex, float2 upperLeftAxisScale, float2 upperRightAxisScale,
    float2 lowerRightAxisScale, float2 lowerLeftAxisScale, out float2 Out)
{
    if (vertex.x > 0)
    {
        if (vertex.y > 0)
            Out = vertex * upperRightAxisScale;
        else
            Out = vertex * lowerRightAxisScale;
    }
    else
    {
        if (vertex.y > 0)
            Out = vertex * upperLeftAxisScale;
        else
            Out = vertex * lowerLeftAxisScale;
    }
}

#endif