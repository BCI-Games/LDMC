using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D),typeof(PolygonCollider2D))]
public class MonsterPresenter : MonoBehaviour
{
    [SerializeField] private PhysicsMaterial2D _basePhysicsMaterial;
    [SerializeField] private bool _useMonsterHealth = false;

    public bool Catchable => _health <= 0;
    private int _health;

    private SpriteRenderer _renderer;
    private PolygonCollider2D _collider;
    private Animator _animator;
    private MonsterData _currentMonsterData;


    void Start()
    {
        BattleEventBus.MonsterAppeared += ShowNewMonster;
        BattleEventBus.MonsterHit += TakeDamage;

        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<PolygonCollider2D>();
        _animator = GetComponent<Animator>();

        _collider.sharedMaterial = Instantiate(_basePhysicsMaterial);
    }

    private void OnDestroy()
    {
        BattleEventBus.MonsterAppeared -= ShowNewMonster;
        BattleEventBus.MonsterHit -= TakeDamage;
    }


    public void ShowNewMonster(MonsterData monsterData)
    {
        _currentMonsterData = monsterData;
        transform.localPosition = Vector2.zero;
        _health = _useMonsterHealth? monsterData.BaseHP: Settings.OnBlockCycleCount;
        _renderer.sprite = monsterData.FrontSprite;
        _collider.sharedMaterial.bounciness = monsterData.Bounciness;
        UpdateCollisionShape(_renderer.sprite);
    }

    private void UpdateCollisionShape(Sprite sprite)
    {
        int pathCount = sprite.GetPhysicsShapeCount();
        _collider.pathCount = pathCount;

        List<Vector2> physicsShape = new();

        for (int index = 0; index < pathCount; index ++)
        {
            sprite.GetPhysicsShape(index, physicsShape);
            _collider.SetPath(index, physicsShape);
        }
    }


    private void TakeDamage()
    {
        int damage = 1;
        _health -= damage;
        if(Catchable)
        {
            _animator.SetTrigger("Capture");
            _health = 1000;
        }
        else
            _animator.SetTrigger("Recoil");
    }

    private void _OnCaptureAnimationFinished()
    {
        BattleEventBus.NotifyMonsterCaptured(_currentMonsterData);
    }
}
