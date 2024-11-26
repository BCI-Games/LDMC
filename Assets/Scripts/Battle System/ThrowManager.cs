using System.Collections;
using UnityEngine;

public class ThrowManager : MonoBehaviour
{
    [Range(1, 5)]
    [SerializeField] private int _sphereCount = 5;
    [SerializeField] private Vector2 _throwForce = new(15, 20);
    [SerializeField] private float _autoThrowDelay = 1;

    [Header("References")]
    [SerializeField] private SphereInventoryDisplay _inventoryDisplay;
    [SerializeField] private Transform _spawnLocation;
    [SerializeField] private GameObject _spherePrefab;

    private int _numberOfSpheresRemaining;


    private void Start()
    {
        _inventoryDisplay.BuildSphereIcons(_sphereCount);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            ThrowSphere();

        if(Input.GetKeyDown(KeyCode.R))
            Reset();

        if(Input.GetKeyDown(KeyCode.A))
            AutoThrow();
    }
    

    public void ThrowSphere()
    {
        if(_numberOfSpheresRemaining > 0)
        {
            _numberOfSpheresRemaining--;
            GameObject sphere = Instantiate(_spherePrefab, _spawnLocation);
            sphere.GetComponent<Rigidbody2D>().AddForce(_throwForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("No more spheres left");
        }
    }


    public void Reset()
    {
        _inventoryDisplay.ResetIcons();
        _numberOfSpheresRemaining = _sphereCount;
    }

    public void AutoThrow()
    {
        StartCoroutine(AutoThrowCoroutine());
    }

    private IEnumerator AutoThrowCoroutine()
    {
        while(_numberOfSpheresRemaining > 0)
        {
            ThrowSphere();
            yield return new WaitForSeconds(_autoThrowDelay);
        }
    }
}
