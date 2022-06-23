using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    // config
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI gameLevelText;
    [SerializeField] private TextMeshProUGUI playerLivesText;

    // state
    private static GameSession _instance;
    public static GameSession Instance => _instance;

    public int GameLevel { get; set; }
    public int PlayerScore { get; set; }
    public int PlayerLives { get; set; }
    public int PointsPerBlock { get; set; }
    public float GameSpeed { get; set; }
    private Scene scene;
    public Ball _ball;

  
    private LevelController _levelController;
  
    /**
     * Singleton implementation.
     */
    private void Awake() 
    {
        // this is not the first instance so destroy it!
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        //first instance should be kept and do NOT destroy it on load
        _instance = this;
        // DontDestroyOnLoad(_instance);
       
        scene = SceneManager.GetActiveScene();
      

    }
    public int GetSceneLevel(Scene scene)
    {
        string name = scene.name;
        name = name.Substring(5, name.Length - 5);
        return int.Parse(name);
    }
    private void SetValueGameSession()
    {
        //   GameLevel = GetSceneLevel(scene);
        GameLevel = PlayerPrefs.GetInt("SelectedLevel");
        PlayerLives = 4;
        PointsPerBlock = 200;
        GameSpeed = 0.8f;
    }
    /**
     * Before first frame.
     */
    void Start()
    {

        SetValueGameSession();
       
       
        playerScoreText.text = this.PlayerScore.ToString();
        gameLevelText.text = this.GameLevel.ToString();
        playerLivesText.text = this.PlayerLives.ToString();

      
          _levelController = FindObjectOfType<LevelController>();
       
    }
  
    /**
     * Update per-frame.
     */
    void Update()
    {
        Time.timeScale = this.GameSpeed;
        
        // UI updates
        playerScoreText.text = this.PlayerScore.ToString();
        gameLevelText.text = this.GameLevel.ToString();
        playerLivesText.text = this.PlayerLives.ToString();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(_levelController.GetBlocksCounter());
          


        }

    }

    /**
     * Updates player score with given points and also updates the UI score. The total points that are
     * calculated is based on the basis value (this.PointsPerBlock).
     */
    public void AddToPlayerScore(int blockMaxHits)
    {
        this.PlayerScore += blockMaxHits * this.PointsPerBlock;
        playerScoreText.text = this.PlayerScore.ToString();
    }
   
   
    
    //public void DisableAllBlock(int number)
    //{
    //    breakableBlockHit1 = blocks.transform.Find("BreakableBlockHit1_" + number).gameObject;
    //    breakableBlockHit1.SetActive(false);
    //    breakableBlockHit2 = blocks.transform.Find("BreakableBlockHit2_" + number).gameObject;
    //    breakableBlockHit2.SetActive(false);
    //    breakableBlockHealth2Hit = blocks.transform.Find("BreakableBlockHealth2Hit_" + number).gameObject;
    //    breakableBlockHealth2Hit.SetActive(false);
    //    unbreakableBlock = blocks.transform.Find("UnbreakableBlock_" + number).gameObject;
    //    unbreakableBlock.SetActive(false);
    //}
    //public void EnableBreakableBlockHit1(int number)
    //{
    //    breakableBlockHit2 = blocks.transform.Find("BreakableBlockHit2_" + number).gameObject;
    //    breakableBlockHit2.SetActive(false);
    //    breakableBlockHealth2Hit = blocks.transform.Find("BreakableBlockHealth2Hit_" + number).gameObject;
    //    breakableBlockHealth2Hit.SetActive(false);
    //    unbreakableBlock = blocks.transform.Find("UnbreakableBlock_" + number).gameObject;
    //    unbreakableBlock.SetActive(false);
    //}
    //public void EnableUnBreakableBlock(int number)
    //{
    //    breakableBlockHit1 = blocks.transform.Find("BreakableBlockHit1_" + number).gameObject;
    //    breakableBlockHit1.SetActive(false);
    //    breakableBlockHit2 = blocks.transform.Find("BreakableBlockHit2_" + number).gameObject;
    //    breakableBlockHit2.SetActive(false);
    //    breakableBlockHealth2Hit = blocks.transform.Find("BreakableBlockHealth2Hit_" + number).gameObject;
    //    breakableBlockHealth2Hit.SetActive(false);
    //}
    //public void EnableBreakableBlockHit2(int number)
    //{
    //    breakableBlockHit1 = blocks.transform.Find("BreakableBlockHit1_" + number).gameObject;
    //    breakableBlockHit1.SetActive(false);
    //    breakableBlockHealth2Hit = blocks.transform.Find("BreakableBlockHealth2Hit_" + number).gameObject;
    //    breakableBlockHealth2Hit.SetActive(false);
    //    unbreakableBlock = blocks.transform.Find("UnbreakableBlock_" + number).gameObject;
    //    unbreakableBlock.SetActive(false);
    //}
    //public void EnableBreakableBlockHealth2Hit(int number)
    //{
    //    breakableBlockHit1 = blocks.transform.Find("BreakableBlockHit1_" + number).gameObject;
    //    breakableBlockHit1.SetActive(false);
    //    breakableBlockHit2 = blocks.transform.Find("BreakableBlockHit2_" + number).gameObject;
    //    breakableBlockHit2.SetActive(false);
    //    unbreakableBlock = blocks.transform.Find("UnbreakableBlock_" + number).gameObject;
    //    unbreakableBlock.SetActive(false);
    //}
}
