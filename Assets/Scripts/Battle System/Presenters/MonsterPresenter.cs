using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D),typeof(PolygonCollider2D))]
public class MonsterPresenter : MonoBehaviour
{
    public bool Catchable => _health <= 0;
    private int _health;

    private SpriteRenderer _renderer;
    private PolygonCollider2D _collider;
    private MonsterData _currentMonsterData;


    void Start()
    {
        BattleEventBus.MonsterAppeared += ShowNewMonster;
        BattleEventBus.MonsterHit += TakeDamage;

        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<PolygonCollider2D>();
    }

    private void OnDestroy()
    {
        BattleEventBus.MonsterAppeared -= ShowNewMonster;
        BattleEventBus.MonsterHit -= TakeDamage;
    }


    public void ShowNewMonster(MonsterData monsterData)
    {
        transform.localPosition = Vector2.zero;
        _health = monsterData.BaseHP;
        _renderer.sprite = monsterData.FrontSprite;
        _currentMonsterData = monsterData;
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        CaptureSpherePresenter sphere;
        if (collisionObject.TryGetComponent<CaptureSpherePresenter>(out sphere))
        {
            if (sphere.HasHitMonster)
                return;
            BattleEventBus.NotifyMonsterHit();
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        int damage = 1;
        _health -= damage;
        if(Catchable)
            BattleEventBus.NotifyMonsterCaptured(_currentMonsterData);
    }
}
