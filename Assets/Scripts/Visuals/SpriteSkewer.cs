#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSkewer: MonoBehaviour
{
    static readonly Vector2[] DefaultCornerPoints =
    {
        new(-1, 1),
        new(1, 1),
        new(1, -1),
        new(-1, -1)
    };
    static readonly string[] CornerNames =
    {
        "UpperLeft",
        "UpperRight",
        "LowerRight",
        "LowerLeft"
    };

    [SerializeField] private bool updateEveryFrame = true;

    [Header("Normalized Corner Positions")]
    [SerializeField] private Vector2 upperLeftCornerPosition;
    [SerializeField] private Vector2 upperRightCornerPosition;
    [SerializeField] private Vector2 lowerRightCornerPosition;
    [SerializeField] private Vector2 lowerLeftCornerPosition;

    private SpriteRenderer Renderer{ get {
        if (!_renderer)
            _renderer = GetComponent<SpriteRenderer>();
        return _renderer;
    }}
    private SpriteRenderer _renderer;
    private Material SkewMaterial { get {
        if (!_skewMaterial)
            CreateAndAssignSkewMaterial();
        return _skewMaterial;
    }}
    private Material _skewMaterial;
    private Vector2 _rendererSize;


    private void Start()
    {
#if UNITY_EDITOR
        Undo.undoRedoEvent += OnUndoRedo;
#endif
        if (Application.IsPlaying(this))
            Settings.AddAndInvokeModificationCallback(OnSettingsModified);
        
        Renderer.drawMode = SpriteDrawMode.Sliced;
        CreateAndAssignSkewMaterial();

        ResetCorners();
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        Undo.undoRedoEvent -= OnUndoRedo;
#endif
        Settings.Modified -= OnSettingsModified;
    }

    private void Update()
    {
        if (Renderer.size != _rendererSize)
            ResetCorners();
        else if (updateEveryFrame)
            ApplyAllCornerPositions();
    }

    private void OnEnable() => ApplyAllCornerPositions();
    private void OnDisable() => ApplyDefaultCornerPositions();

    private void OnSettingsModified() => enabled = Settings.MeshAnimationEnabled;

#if UNITY_EDITOR
    private void OnUndoRedo(in UndoRedoInfo info) => ApplyAllCornerPositions();
#endif
    public void ApplyAllCornerPositions()
    {
        for (int i = 0; i < 4; i++)
            ApplyCornerPosition(i);
    }

    public void ResetCorners()
    {
        _rendererSize = Renderer.size;
        for (int i = 0; i < 4; i++)
            SetCornerPosition(i, DefaultCornerPoints[i]);
    }

    private void ApplyDefaultCornerPositions()
    {
        for (int i = 0; i < 4; i++)
            SkewMaterial.SetVector("_" + CornerNames[i] + "AxisScale", Vector2.one);
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


    public Vector2 GetHandlePosition(int index)
    {
        Vector2 normalizedCornerPosition = GetCornerPosition(index);
        Vector2 scaledPosition = normalizedCornerPosition * Renderer.size / 2;
        Vector2 worldPosition = transform.TransformPoint(scaledPosition);
        return worldPosition;
    }

    public void SetCornerPositionFromHandle(int index, Vector2 handlePosition)
    {
        Vector2 localPosition = transform.InverseTransformPoint(handlePosition);
        Vector2 normalizedPosition = 2 * localPosition / Renderer.size;
        SetCornerPosition(index, normalizedPosition);
    }


    private void ApplyCornerPosition(int index)
    {
        Vector2 axisScale = GetCornerPosition(index) / DefaultCornerPoints[index];
        SkewMaterial.SetVector("_" + CornerNames[index] + "AxisScale", axisScale);
    }

    private void CreateAndAssignSkewMaterial()
    {
        _skewMaterial = new(Shader.Find("Shader Graphs/Sprite Skew"));
        Renderer.sharedMaterial = _skewMaterial;
    }
}