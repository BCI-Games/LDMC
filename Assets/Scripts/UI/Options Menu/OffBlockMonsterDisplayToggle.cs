using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class OffBlockMonsterDisplayToggle: MonoBehaviour
{
    private void Start()
    {
        Toggle toggle = GetComponent<Toggle>();
        Settings.OffBockMonsterDisplayEnabled.ConnectToggle(toggle);
    }
}