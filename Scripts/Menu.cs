using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void GoToScene(string _level)
    {
        Debug.Log($"Going to scene: {_level}!");
        SceneManager.LoadScene(_level);
    }

    public void CloseGame()
    {
        Debug.Log("Closing Game!");
        Application.Quit();
    }
}