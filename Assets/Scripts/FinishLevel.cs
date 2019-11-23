using System.Collections;
using System.Collections.Generic;
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
