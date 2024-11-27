using System;
using UnityEditor;
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

    [SerializeField] private bool updateEveryFrame = true;

    [Header("Corner Positions")]
    [SerializeField] private Vector2 upperLeftCornerPosition;
    [SerializeField] private Vector2 upperRightCornerPosition;
    [SerializeField] private Vector2 lowerRightCornerPosition;
    [SerializeField] private Vector2 lowerLeftCornerPosition;

    public SpriteRenderer Renderer{ get {
        if (!_renderer)
            _renderer = GetComponent<SpriteRenderer>();
        return _renderer;
    }}
    private SpriteRenderer _renderer;
    private Material _skewMaterial;

    private Vector2 _rendererSize;
    private Vector2 _normalizedPivot;


    private void Start()
    {
        Undo.undoRedoEvent += OnUndoRedo;
        
        Renderer.drawMode = SpriteDrawMode.Sliced;
        _skewMaterial = new(Shader.Find("Shader Graphs/Sprite Skew"));
        Renderer.sharedMaterial = _skewMaterial;

        ResetCorners();
    }

    private void Update()
    {
        if (Renderer.size != _rendererSize)
            ResetCorners();
        else if (updateEveryFrame)
            ApplyAllCornerPositions();
    }

    private void OnUndoRedo(in UndoRedoInfo info) => ApplyAllCornerPositions();
    public void ApplyAllCornerPositions()
    {
        for (int i = 0; i < 4; i++)
            ApplyCornerPosition(i);
    }

    public void ResetCorners()
    {
        _rendererSize = Renderer.size;
        _normalizedPivot = GetNormalizedPivot();
        for (int i = 0; i < 4; i++)
            SetCornerPosition(i, GetUnscaledCornerPosition(i));
    }

    public Vector2 GetCornerPosition(int index)
    {
        return new Vector2[]
        {
            upperLeftCornerPosition,
            upperRightCornerPosition,
            lowerRightCornerPosition,
            lowerLeftCornerPosition
        }
        [index];
    }

    public void SetCornerPosition(int index, Vector2 position)
    {
        switch(index)
        {
            case 0: upperLeftCornerPosition = position; break;
            case 1: upperRightCornerPosition = position; break;
            case 2: lowerRightCornerPosition = position; break;
            case 3: lowerLeftCornerPosition = position; break;
        }
        ApplyCornerPosition(index);
    }

    private void ApplyCornerPosition(int index)
    {
        Vector2 axisScale = GetCornerPosition(index) / GetUnscaledCornerPosition(index);
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