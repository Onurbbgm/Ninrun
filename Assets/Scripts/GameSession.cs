using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameSession : MonoBehaviour
{
    [SerializeField] Text scoreText;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        scoreText.text = "Score: " + player.points.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + player.points.ToString();
    }



}
