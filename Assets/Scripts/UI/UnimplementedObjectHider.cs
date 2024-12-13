using UnityEngine;

public class UnimplementedObjectHider: MonoBehaviour
{
    private void OnEnable() => gameObject.SetActive(false);
}