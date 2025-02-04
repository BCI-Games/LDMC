using UnityEngine;
using System.Linq;

public class KeyboardInputProvider: MonoBehaviour, IBooleanInputProvider
{
    public bool InputValue => _keyCodes.Any(Input.GetKey);

    [SerializeField] private KeyCode[] _keyCodes = new KeyCode[]{KeyCode.Space};
}