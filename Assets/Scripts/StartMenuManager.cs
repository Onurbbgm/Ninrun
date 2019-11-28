using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartMenuManager : MonoBehaviour
{
    private GameObject continueButton = null;

    private void Start()
    {
        continueButton = GameObject.Find("Continue Button");
        if (continueButton)
        {
            
            if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
            {
                continueButton.GetComponent<Button>().interactable = true;
                continueButton.GetComponent<Button>().enabled = true;
            }
            else
            {
                continueButton.GetComponent<Button>().enabled = false;

                Destroy(continueButton);
            }
        }
    }

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

    public void QuitLevel()
    {
        SceneManager.LoadScene("Menu");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadNextLevel()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            SaveData save = (SaveData)bf.Deserialize(file);
            file.Close();
            SceneManager.LoadScene(save.level);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }        
    }

    public void GoBackMainMenu()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(0);
        }
    }

}
