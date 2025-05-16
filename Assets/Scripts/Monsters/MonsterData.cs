using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "MonsterData")]
public class MonsterData : ScriptableObject
{
    [TextArea]
    public string Description;

    [Header("Sprites")]
    public Sprite FrontSprite;
    public Sprite IconSprite;

    [Header("Types")]
    public MonsterType PrimaryType;
    public MonsterType SecondaryType;

    [Header("Stats")]
    public int BaseHP;
    public int Rarity;

    [Header("Feedback Attributes")]
    [Range(0, 2)]
    public float Bounciness = 1;

    [Header("Sounds")]
    public AudioClip[] AppearSounds;
    public AudioClip[] HitSounds;
    public AudioClip[] CaptureSounds;

    
    public bool HasType(MonsterType type)
    {
        return ((type & PrimaryType) | (type & SecondaryType)) != 0;
    }
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
