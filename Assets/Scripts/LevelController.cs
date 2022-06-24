using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private readonly int NUMBER_OF_GAME_LEVELS = 3;
    // UI elements
   private int blocksCounter;
    private static LevelController _instance;
    public static LevelController Instance => _instance;
private void Start()
    {
        
        //first instance should be kept and do NOT destroy it on load
        _instance = this;
        var pool = Pool.Instance;
        blocksCounter = pool.GetTotalBreakableBrick();
        Debug.Log("blocksCounter = " + blocksCounter);
    }

    public void IncrementBlocksCounter()
    {
        Debug.Log("increase block count");
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
                SceneManager.LoadScene("GameOver");
            }
            int currentLevel = PlayerPrefs.GetInt("currentLevel");
            int level = PlayerPrefs.GetInt("SelectedLevel");
            if(level == currentLevel)
            {
                level++;
                PlayerPrefs.SetInt("currentLevel", level);
                Debug.Log("Player unlock level:" + level);
            }
            var pool = Pool.Instance;
            pool.ReturnToPoolAction();
            SceneManager.LoadScene("LevelMap");         
        }     
    }
    
}
