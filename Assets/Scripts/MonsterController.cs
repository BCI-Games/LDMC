using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MonsterController : MonoBehaviour
{

    [SerializeField] private List<GameObject> monsterList;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private GameObject monsterPlatform;

    public GameObject currentMonster;
    

    public Sprite currentMonsterIcon;

    void Start()
    {

        //Instantiate offscreen, have them slide onscreen.
        InitializeMonster();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Destroy(currentMonster);
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            InitializeMonster();
        }
   
    }

    public void InitializeMonster()
    {
        //Randomly select a monster from the list
        int monsterIndex = Random.Range(0, monsterList.Count);
        monsterPrefab = monsterList[monsterIndex];

        //Instantiate monster
        currentMonster = Instantiate(monsterPrefab, transform);

        //Set monster icon
        currentMonsterIcon = monsterPrefab.GetComponent<MonsterProperties>().ReturnMonsterIcon();

        //Update platform based on rarity
        UpdatePlatformFromRarity();
    
    }

    private void UpdatePlatformFromRarity()
    {
        //Change platform color based on rarity
        
    }

    public void DestroyMonster()
    {
        Destroy(currentMonster);
    }






}
