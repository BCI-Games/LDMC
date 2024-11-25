using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowManager : MonoBehaviour
{
    //Target Information
    [SerializeField] private Transform targetSprite;
    [SerializeField] private Transform sphereSpawnLocation;
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private Vector2 sphereForce;

    public int numberOfSpheresLeft;

    
    // Start is called before the first frame update=

    public void SetSphereForce(float x, float y)
    {
        sphereForce = new Vector2(x, y);
    }

    public void SetNumberOfSpheres(int num)
    {
        numberOfSpheresLeft = num;
    }

    public void ResetNumberOfSpheres(int newNum)
    {
        numberOfSpheresLeft = newNum;
    }
    

    public void ThrowSphere()
    {
        if(numberOfSpheresLeft > 0)
        {
            numberOfSpheresLeft--;
            GameObject sphere = Instantiate(spherePrefab, sphereSpawnLocation);
            sphere.GetComponent<Rigidbody2D>().AddForce(sphereForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("No more spheres left");
        }
   
    }

    public void AutoThrow()
    {
        // Auto throw all the spheres in 1 second intervals until they run out
        StartCoroutine(AutoThrowCoroutine());
    }

    private IEnumerator AutoThrowCoroutine()
    {
        while(numberOfSpheresLeft > 0)
        {
            ThrowSphere();
            yield return new WaitForSeconds(1f);
        }
    }


}
