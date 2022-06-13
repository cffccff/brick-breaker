using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    public float minRelativePosX = 1f;  // assumes paddle size of 1 relative unit
    
    [SerializeField]
    public float maxRelativePosX = 15f;  // assumes paddle size of 1 relative unit
    
    [SerializeField]
    public float fixedRelativePosY = .64f;  // paddle does not move on the Y directiob
    
    // Unity units of the WIDTH of the screen (e.g. 16)
    [SerializeField]
    public float screenWidthUnits = 16;

    //to handle time and function of GearPotion
    private float timeGear = 10;
    private bool isInGearPotion = false;
    //to handle time and function of BluePotion
    private float timeBlue = 10;
    private bool isInBluePotion = false;

   [SerializeField] float paddleSpeed = 7;
   

    List<float> listOfChangeSpeed;
    private void Awake()
    {
        listOfChangeSpeed = new List<float>();
    }
    // Start is called before the first frame update
    void Start()
    {
       
        float startPosX = ConvertPixelToRelativePosition(screenWidthUnits / 2, Screen.width);
        transform.position = GetUpdatedPaddlePosition(startPosX);
       
    } 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)&&transform.position.x> minRelativePosX)
        {
            transform.position -= new Vector3(paddleSpeed * Time.deltaTime, 0,0);
        }
        if (Input.GetKey(KeyCode.D)&&transform.position.x< maxRelativePosX)
        {
            transform.position += new Vector3(paddleSpeed * Time.deltaTime, 0, 0);
        }
        if(transform.localScale.x > 2)
        {
            minRelativePosX =2.8f;
            maxRelativePosX = 13.2f; 
        }
        else
        {
            minRelativePosX = 1f;
            maxRelativePosX = 15f;
        }
         // var relativePosX = ConvertPixelToRelativePosition(Input.mousePosition.x, Screen.width);

       //   transform.position = GetUpdatedPaddlePosition(relativePosX);
        UpdateAndCheckGearPotion();
    }
    private void ResetStatusOfPaddle()
    {
        RemoveGearPotionEffect();
        RemoveBlueBottleEffec();
    }
    private void RemoveGearPotionEffect()
    {
        isInGearPotion = false;
        transform.localScale = new Vector3(1.45f, 1, 1);
        timeGear = 10;
    }
    private void UpdateAndCheckGearPotion()
    {
        if (isInGearPotion == true)
        {
            timeGear -= Time.deltaTime;
            
        }
        if (timeGear <= 0)
        {
            RemoveGearPotionEffect();


        }
    }
    private void RemoveBlueBottleEffec()
    {
        paddleSpeed = 7f;
        CancelInvoke();
        listOfChangeSpeed = new List<float>();
    }
    public Vector2 GetUpdatedPaddlePosition(float relativePosX)
    {
        // clamps the X position
        float clampedRelativePosX = Mathf.Clamp(relativePosX, minRelativePosX, maxRelativePosX);
        
        Vector2 newPaddlePosition = new Vector2(clampedRelativePosX, fixedRelativePosY);
        return newPaddlePosition;
    }
    
    public float ConvertPixelToRelativePosition(float pixelPosition, int screenWidth)
    { 
        var relativePosition = pixelPosition/screenWidth * screenWidthUnits;
        return relativePosition;
    }
  
   private void MinusSpeed()
    {
        // float round = Mathf.Round(listOfChangeSpeed[0]);
        paddleSpeed -= listOfChangeSpeed[0];
        listOfChangeSpeed.RemoveAt(0);
        foreach( float number in listOfChangeSpeed)
        {
            Debug.Log(number);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var gameSession = GameSession.Instance;
        if (collision.gameObject.CompareTag("Potions"))
        {

            if (collision.gameObject.name.StartsWith("GearPotion"))
            {
                Debug.Log("GEAR");
                if (isInGearPotion == false)
                {
                    isInGearPotion = true;
                    transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    timeGear = 10;
                }


            }

            else if (collision.gameObject.name.StartsWith("HeartPotion"))
            {
                Debug.Log("Heart");
                if (gameSession.PlayerLives <5)
                {
                    gameSession.PlayerLives++;

                }


            }
            else if (collision.gameObject.name.StartsWith("BlueBottlePotion"))
            {
                Debug.Log("Blue");
                float speedChange = paddleSpeed * 30 / 100;
                speedChange = Mathf.Round(speedChange);
                paddleSpeed += speedChange;
                listOfChangeSpeed.Add(speedChange);
                Invoke("MinusSpeed", 10f);
                
            }
            else if(collision.gameObject.name.StartsWith("EmptyBottlePotion"))
            {
                Debug.Log("Empty");
                ResetStatusOfPaddle();

            }
          
            Destroy(collision.gameObject);
        }

    }
   
}