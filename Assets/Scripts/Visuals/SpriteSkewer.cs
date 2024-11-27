using System;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSkewer: MonoBehaviour
{
    static readonly Vector2[] CornerPoints =
    {
        new(-1, 1),
        new(1, 1),
        new(1, -1),
        new(-1, -1)
    };
    static readonly String[] CornerNames =
    {
        "UpperLeft",
        "UpperRight",
        "LowerRight",
        "LowerLeft"
    };
    
    public SpriteRenderer Renderer{ get {
        if (!_renderer)
            _renderer = GetComponent<SpriteRenderer>();
        return _renderer;
    }}
    private SpriteRenderer _renderer;
    private Material _skewMaterial;

    [SerializeField]
    private Vector2[] _cornerPositions = new Vector2[4];

    private Vector2 _rendererSize;
    private Vector2 _normalizedPivot;


    private void Start()
    {
        Renderer.drawMode = SpriteDrawMode.Sliced;
        _skewMaterial = new(Shader.Find("Shader Graphs/Sprite Skew"));
        Renderer.sharedMaterial = _skewMaterial;

        ResetCorners();
    }

    private void Update()
    {
        if (Renderer.size != _rendererSize)
            ResetCorners();
    }


    public void ResetCorners()
    {
        _rendererSize = Renderer.size;
        _normalizedPivot = GetNormalizedPivot();
        for (int i = 0; i < CornerPoints.Length; i++)
            SetCornerPosition(i, GetUnscaledCornerPosition(i));
    }

    public Vector2 GetCornerPosition(int index) => _cornerPositions[index];

    public void SetCornerPosition(int index, Vector2 position)
    {
        _cornerPositions[index] = position;
        Vector2 axisScale = position / GetUnscaledCornerPosition(index);
        _skewMaterial.SetVector("_" + CornerNames[index] + "AxisScale", axisScale);
    }

    private Vector2 GetUnscaledCornerPosition(int cornerIndex)
    {
        Vector2 cornerOffset = CornerPoints[cornerIndex] - _normalizedPivot;
        return cornerOffset * _rendererSize / 2;
    }

    private Vector2 GetNormalizedPivot()
    {
        if (Renderer.sprite)
        {
            Vector2 pivot = Renderer.sprite.pivot;
            Vector2 spriteSize = Renderer.sprite.rect.size;
            return 2 * (pivot / spriteSize) - Vector2.one;
        }
        return Vector2.one / 2;
    }
}