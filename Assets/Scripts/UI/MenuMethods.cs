using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuMethods : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && SceneManager.GetActiveScene().name != "Main Menu")
            LoadScene("Main Menu");
    }

    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);

    public void QuitGame() => Application.Quit();
}