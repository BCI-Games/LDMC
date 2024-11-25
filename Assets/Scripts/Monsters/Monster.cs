using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster 
{
    public MonsterBase MonsterBase {get; set;}
    public int Rarity {get ; set;}

    public int HP {get; set;} 

    public Monster(MonsterBase monsterBase, int rarity)
    {
        MonsterBase = monsterBase;
        Rarity = monsterBase.Rarity;
        HP = monsterBase.MaxHP;
    }

}
