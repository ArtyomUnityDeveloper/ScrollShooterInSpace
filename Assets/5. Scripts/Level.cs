using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float delayInSeconds = 2f;

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game"); // загрузка сцены по её названию в редакторе юнити
        FindObjectOfType<GameSession>().ResetGame(); // нужно чтобы корректно обнулять очки и возобновлять их отсчёт после перезапуска игры
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad());
    }

    private IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("GameOver");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
