using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardInputProvider : MonoBehaviour, IInputProvider
{
    public float InputValue => _actionKeys.Any(
        key => Keyboard.current[key].isPressed
    ) ? 1 : 0;

    [SerializeField] private Key[] _actionKeys = new Key[] { Key.Space };
}