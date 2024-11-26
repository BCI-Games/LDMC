using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UsedCaptureSphereDisposalZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        CaptureSpherePresenter sphere;
        if (collisionObject.TryGetComponent<CaptureSpherePresenter>(out sphere)
            && sphere.HasHitMonster)
        {
            Destroy(collisionObject);
        }
    }
}
