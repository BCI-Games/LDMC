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

        Settings.AnimationSimplified.ConnectToggle(_simplifiedAnimationToggle);
        Settings.MonsterAnimationEnabled.ConnectToggle(_monsterAnimationToggle);
        Settings.SpriteDeformationEnabled.ConnectToggle(_spriteDeformationToggle);

        _simplifiedAnimationToggle.onValueChanged.AddListener(UpdateAdvancedOptions);
    }


    private void SetAdvancedOptionsVisibility(bool visible)
    {
        _monsterAnimationInputElement.SetActive(visible);
        _spriteDeformationInputElement.SetActive(visible);
        RefreshLayout();
    }

    private void UpdateAdvancedOptions(bool value)
    {
        SetAdvancedOptionsVisibility(!value);
        if (!value)
        {
            _monsterAnimationToggle.isOn = Settings.MonsterAnimationEnabled;
            _spriteDeformationToggle.isOn = Settings.SpriteDeformationEnabled;
        }
    }
}