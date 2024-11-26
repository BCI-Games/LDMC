using UnityEngine;

public class CaptureSpherePresenter : MonoBehaviour
{
    public bool HasHitMonster = false;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MonsterPresenter>())
            HasHitMonster = true;
    }
}
