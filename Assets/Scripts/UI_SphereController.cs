using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_SphereController : MonoBehaviour
{
    // [SerializeField] private Image[] images;
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private List<GameObject> sphereIcons;

    [SerializeField] private int currentSphere = 0;
    [SerializeField] private int remainingSpheres = 0;

    public int numSphereIcons;
    // public int NumberOfSphereIcons 

    void Start()
    {
        remainingSpheres = numSphereIcons;
        //Initialize the sphere icons        
        InitializeSphereIconsAsChild(numSphereIcons);

        //Now initialize the sphere icons
        InitializeSphereList();   
        
    }

    public void InitializeSphereIconsAsChild(int num)
    {
        if (num == 0)
        {
            Debug.LogError("Number of spheres cannot be 0, making it 1");
            num = 1;
        }
        else if (num > 5)
        {
            Debug.LogError("Number of spheres cannot be greater than 5, making it 5");
            num = 5;
        }
        for (int i = 0; i < num; i++)
        {
            GameObject newSphere = Instantiate(spherePrefab, this.transform);
            newSphere.name = "Sphere" + i;
        }
    }

    public void InitializeSphereList()
    {
        foreach (Transform child in transform)
        {
            sphereIcons.Add(child.gameObject);
        }
    }

    public void ClearList()
    {
        sphereIcons.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    //This is very resource intensive way to do this, make better later.
    public void ResetNumberOfSphereIcons(int num)
    {
        ClearList();
        InitializeSphereIconsAsChild(num);
        InitializeSphereList();
        ResetCounts();
    }

    public void ResetSphereIcons()
    {
        foreach (GameObject item in sphereIcons)
        {
            item.GetComponent<Image>().color = Color.white;
        }
        ResetCounts();
    }

    public void ResetCounts()
    {
        currentSphere = 0;
        remainingSpheres = numSphereIcons;
    }


    public void DecreaseSphereIconCountByOne()
    {
        if (remainingSpheres > 0)
        {
            remainingSpheres--;
            sphereIcons[currentSphere].GetComponent<Image>().color = Color.black;
            currentSphere++;
        }
        else
        {
            Debug.Log("No more spheres left");
        }
    }

}
