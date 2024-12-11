using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMethods: MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}