using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Monsters", menuName = "Monster/Create New Monster")]
public class MonsterBase : ScriptableObject
{
    [SerializeField] private string monsterName;
    [TextArea]
    [SerializeField] private string description;

    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite iconSprite;

    [SerializeField] private MonsterType type1;
    [SerializeField] private MonsterType type2;

    //BaseStats
    [SerializeField] private int maxHP;
    [SerializeField] private int rarity;

    public string MonsterName => monsterName;
    public string Description => description;
    public Sprite FrontSprite => frontSprite;
    public Sprite IconSprite => iconSprite;
    public int MaxHP => maxHP;
    public int Rarity => rarity;

}

public enum MonsterType
{
    None,
    Fire,
    Water,
    Grass,
    Electric,
    Ice,
    Psychic,
    Dark,
    Dragon,
    Fairy,
    Normal,
    Bear,
    Crystal,
    Wolf,
    Bird,
    Bug,
    Ghost,
    Silly,
    Cute
}
