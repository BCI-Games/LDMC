#ifndef RADIAL_FILL_INCLUDED
#define RADIAL_FILL_INCLUDED

// cut alpha by radial angle
void FillRadially_float(float2 uv, float fillAmount, float angleOffset, out float Out)
{
    const float TAU = PI * 2;

    float2 atan2Coord = float2(lerp(-1, 1, uv.x), lerp(-1, 1, uv.y));
    float angle = atan2(atan2Coord.x, atan2Coord.y);

    angle += angleOffset * PI / 180;

    while (angle < 0) angle += TAU;
    while (angle > TAU) angle -= TAU;

    Out = step(0, angle) * step(angle, fillAmount * TAU);
}

#endif