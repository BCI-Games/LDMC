using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CaptureDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject monsterIconPrefab;

    public void AddMonsterIcon(Sprite monsterIcon)
    {
        //Add monster icon to display
        Instantiate(monsterIconPrefab, transform).GetComponent<Image>().sprite = monsterIcon;
    }

    public void ClearIcons()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void RemoveLastIcon()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }
    }


}
