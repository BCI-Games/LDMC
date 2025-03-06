using UnityEngine;
using System.Linq;

public class KeyboardInputProvider: MonoBehaviour, IInputProvider
{
    public float InputValue => _keyCodes.Any(Input.GetKey)? 1: 0;

    [SerializeField] private KeyCode[] _keyCodes = new KeyCode[]{KeyCode.Space};
}