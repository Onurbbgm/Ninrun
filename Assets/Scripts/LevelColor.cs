using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColor : MonoBehaviour
{
    private Camera mainCamera;

    /*
      colorPairs[,0] = Sky color
      colorPairs[,1] = Level/Floor color
     */
    private Color[,] colorPairs = new Color[5,2] {
        { new Color(1f, 0.58f, 0.23f), new Color(1f, 0.94f, 0.23f) },
        { new Color(0.84f, 0.21f, 0.49f), new Color(0.85f, 0.24f, 0.22f) },
        { new Color(0.2f, 0.85f, 0.85f), new Color(0.21f, 0.55f, 0.85f) },
        { new Color(0.21f, 0.85f, 0.29f), new Color(0.22f, 0.87f, 0.6f) },
        { new Color(0.87f, 0.1f, 0.08f), new Color(0.87f, 0.48f, 0.09f) }
    };

    private int currentPair;

    public float transitionDuration = 200f;

    public Material levelMaterial;

    private Color lastBackgroundColor;

    private float transitionDelta = 0f;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        currentPair = Random.Range(0, colorPairs.GetUpperBound(0) + 1);

        mainCamera.backgroundColor = colorPairs[currentPair,0];
        levelMaterial.SetVector("_Level_Color", GetLevelColor());
    }

    public void ChangeColor()
    {
        int lastPair = currentPair;
        while (currentPair == lastPair)
        {
            currentPair = Random.Range(0, colorPairs.GetUpperBound(0) + 1);
        }
        lastBackgroundColor = mainCamera.backgroundColor;
        transitionDelta = 0f;
        levelMaterial.SetVector("_Level_Color", GetLevelColor());
    }

    // Returns current level/floor color based on Sky color
    public Color GetLevelColor()
    {
        return colorPairs[currentPair,1];
    }

    void Update()
    {
        if (!colorPairs[currentPair,0].Equals(mainCamera.backgroundColor))
        {
            transitionDelta = Mathf.Min(transitionDelta + (Time.deltaTime * transitionDuration) / 100, 1f);
            mainCamera.backgroundColor = Color.Lerp(lastBackgroundColor, colorPairs[currentPair,0], transitionDelta);
        }
    }
}
