using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer),typeof(HitBySphere),typeof(Rigidbody2D))]
public class MonsterProperties : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int rarity;
    public bool canCapture;

    [SerializeField] private MonsterBase monsterBase;
    [SerializeField] private Sprite monsterIcon;

    void Start()
    {
        GameEvents.current.onMonsterHit += TakeDamage;
        GameEvents.current.onCanCaptureMonster += CanCaptureMonster;

        //Initialize monster
        health = monsterBase.MaxHP;
        rarity = monsterBase.Rarity;
        canCapture = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = monsterBase.FrontSprite;
        monsterIcon = monsterBase.IconSprite;
                
        //Instantiate offscreen, have them slide onscreen.

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TakeDamage()
    {
        int damage = 1;
        health -= damage;
        if(health <= 1 && canCapture == false)
        {
            CanCaptureMonster();
        }
        if(health <= 0 && canCapture == true)
        {
            GameEvents.current.CaptureMonster();
        }
    }

    private void CanCaptureMonster()
    {
        Debug.Log("Can Capture Monster");
        canCapture = true;
    }  

    public Sprite ReturnMonsterIcon()
    {
        return monsterBase.IconSprite;
    }

    private void OnDestroy()
    {
        GameEvents.current.onMonsterHit -= TakeDamage;
        GameEvents.current.onCanCaptureMonster -= CanCaptureMonster;
    }
}
