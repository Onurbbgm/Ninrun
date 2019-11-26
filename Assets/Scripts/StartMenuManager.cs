using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenuManager : MonoBehaviour
{


    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "Credits")
        {
            GoBackMainMenu();
        }
    }

    public void LoadLevel(string name)
    {
        Debug.Log("Level load requested for: " + name);
        SceneManager.LoadScene(name);
    }

    public void QuitRequest()
    {
        Debug.Log("Quit the game!");
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoBackMainMenu()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(0);
        }
    }

}
