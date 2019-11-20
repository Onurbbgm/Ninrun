using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] float levelSlowMode = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            Debug.Log("Touch it");
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
}
