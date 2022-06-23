using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
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
    public int GetBlocksCounter()
    {
        return blocksCounter;
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
                SceneManager.LoadScene("GameOver");
            }
            Debug.Log("Win");
            int currentLevel = PlayerPrefs.GetInt("currentLevel");
            int level = PlayerPrefs.GetInt("SelectedLevel");
            if(level == currentLevel)
            {
                level++;
                PlayerPrefs.SetInt("currentLevel", level);
                Debug.Log("Player unlock level:" + level);
            }

           // var pool = Pool.Instance;
          //  pool.ReturnToPoolAction();
          
            SceneManager.LoadScene("LevelMap");
        }
    }
    
}
