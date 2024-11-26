using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CaptureSpherePresenter : MonoBehaviour
{
    public bool HasHitMonster = false;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (HasHitMonster) return;
        if (collision.gameObject.GetComponent<MonsterPresenter>())
        {
            BattleEventBus.NotifyMonsterHit();
            HasHitMonster = true;
        }
    }
}
