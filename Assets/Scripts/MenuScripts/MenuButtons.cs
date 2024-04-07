using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject startButton;
    public GameObject quitButton;

    public void StartGame()
    {
        SceneManager.LoadScene("SelectionScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
