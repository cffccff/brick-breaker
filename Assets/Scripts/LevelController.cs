using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private readonly string GAME_OVER_SCENE_NAME = "Scenes/GameOver";
    private readonly int NUMBER_OF_GAME_LEVELS = 3;
    
    // UI elements
    [SerializeField] int blocksCounter;

    // state
    //  private SceneLoader _sceneLoader;
    private Scene scene;
    private void Start()
    {
        // _sceneLoader = FindObjectOfType<SceneLoader>();
        scene = SceneManager.GetActiveScene();
    }

    public void IncrementBlocksCounter()
    {
        blocksCounter++;
    }
    
    public void DecrementBlocksCounter()
    {
        blocksCounter--;

        if (blocksCounter <= 0)
        {
            var gameSession = GameSession.Instance;
            
            // check for game over
            if (gameSession.GameLevel >= NUMBER_OF_GAME_LEVELS)
            {
 //               _sceneLoader.LoadSceneByName(GAME_OVER_SCENE_NAME);
                SceneManager.LoadScene("GameOver");
            }

            // increases game level
            // gameSession.GameLevel++;
            int level = gameSession.GetSceneLevel(scene);
            level++;
            PlayerPrefs.SetInt("currentLevel", level);
            Debug.Log("Player unlock level:" + level);
            SceneManager.LoadScene("LevelMap");
        }
    }
    
}
