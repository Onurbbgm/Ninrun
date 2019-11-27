using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GetBestScores : MonoBehaviour
{
    private Text bestScoresText;

    // Start is called before the first frame update
    void Start()
    {
        bestScoresText = GetComponent<Text>();
        bestScoresText.text = "";
        GetScores();
    }

    private void GetScores()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            SaveData save = (SaveData)bf.Deserialize(file);
            file.Close();            
            foreach(var levelPoin in save.scoreLevel)
            {
                bestScoresText.text += "Level " + levelPoin.Key+": "+levelPoin.Value+"\n";
            }
        }
        else
        {
            bestScoresText.text = "No scores yet!";
        }
    }
   
}
