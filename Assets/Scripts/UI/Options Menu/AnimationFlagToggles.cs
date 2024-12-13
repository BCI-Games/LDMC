using UnityEngine;
using UnityEngine.UI;

public class AnimationFlagToggles: DynamicLayoutBlock
{
    [SerializeField] private Toggle _simplifiedAnimationToggle;
    [SerializeField] private GameObject _monsterAnimationInputElement;
    [SerializeField] private GameObject _spriteDeformationInputElement;

    private Toggle _monsterAnimationToggle;
    private Toggle _spriteDeformationToggle;

    
    private void Start()
    {
        _monsterAnimationToggle = _monsterAnimationInputElement.GetComponentInChildren<Toggle>();
        _spriteDeformationToggle = _spriteDeformationInputElement.GetComponentInChildren<Toggle>();
        SetAdvancedOptionsVisibility(!Settings.AnimationSimplified);

        _simplifiedAnimationToggle.isOn = Settings.AnimationSimplified;
        _monsterAnimationToggle.isOn = Settings.MonsterAnimationEnabled;
        _spriteDeformationToggle.isOn = Settings.SpriteDeformationEnabled;

        _simplifiedAnimationToggle.onValueChanged.AddListener(SetAnimationSimplified);
        _monsterAnimationToggle.onValueChanged.AddListener(SetMonsterAnimationEnabled);
        _spriteDeformationToggle.onValueChanged.AddListener(SetSpriteDeformationEnabled);
    }


    private void SetAdvancedOptionsVisibility(bool visible)
    {
        _monsterAnimationInputElement.SetActive(visible);
        _spriteDeformationInputElement.SetActive(visible);
        RefreshLayout();
    }

    private void SetAnimationSimplified(bool value)
    {
        Settings.AnimationSimplified = value;
        SetAdvancedOptionsVisibility(!value);
        if (!value)
        {
            _monsterAnimationToggle.isOn = Settings.MonsterAnimationEnabled;
            _spriteDeformationToggle.isOn = Settings.SpriteDeformationEnabled;
        }
    }
    private void SetMonsterAnimationEnabled(bool value)
        => Settings.MonsterAnimationEnabled = value;
    private void SetSpriteDeformationEnabled(bool value)
        => Settings.SpriteDeformationEnabled = value;
}