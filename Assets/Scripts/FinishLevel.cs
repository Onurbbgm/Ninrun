using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] float levelSlowMode = 0.2f;

    public AudioClip finishSound;

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            //Debug.Log("Touch it");
            //other.gameObject.GetComponent<AudioSource>().Stop();
            other.gameObject.GetComponent<Player>().FinishLevel();
            AudioSource.PlayClipAtPoint(finishSound, transform.position, 1f);
            SaveGame(other.gameObject.GetComponent<Player>().points);
            StartCoroutine(LoadNextLevel());
        }        
    }

    private IEnumerator LoadNextLevel()
    {
        Time.timeScale = levelSlowMode;
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        Time.timeScale = 1f;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Destroy(FindObjectOfType<ScenePersist>());
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    private void SaveGame(int points)
    {
        SaveData save = CreateSaveGame(points);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
    }

    private SaveData CreateSaveGame(int points)
    {
        SaveData save = new SaveData();
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            save = (SaveData)bf.Deserialize(file);
            file.Close();            
        }
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        save.level = currentLevel + 1;
        if (save.scoreLevel.ContainsKey(currentLevel) && save.scoreLevel[currentLevel] < points)
        {
            save.scoreLevel.Add(currentLevel, points);
        }
        else if (!save.scoreLevel.ContainsKey(currentLevel))
        {
            save.scoreLevel.Add(currentLevel, points);
        }
        
        return save;
    }

}
