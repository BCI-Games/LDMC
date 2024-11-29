using UnityEngine;

public abstract class Lerper: MonoBehaviour
{
    protected TValue Lerp<TValue>(TValue a, TValue b, float t) where TValue: struct
    {
        Debug.LogWarning("Lerp Method not defined for type " + typeof(TValue));
        return a;
    }
    protected float Lerp(float a, float b, float t) => Mathf.Lerp(a, b, t);
    protected Color Lerp(Color a, Color b, float t) => Color.Lerp(a, b, t);
    protected Vector2 Lerp(Vector2 a, Vector2 b, float t) => Vector2.Lerp(a, b, t);
    protected Vector3 Lerp(Vector3 a, Vector3 b, float t) => Vector3.Lerp(a, b, t);
    protected Quaternion Lerp(Quaternion a, Quaternion b, float t) => Quaternion.Lerp(a, b, t);
}