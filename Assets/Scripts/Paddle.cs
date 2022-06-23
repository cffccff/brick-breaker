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
    private float timeGearPotion = 10;
    private bool isInGearPotion = false;
    //to handle time and function of BluePotion
    private float timeBluePotion = 10f;

    private Ball _ball;

    List<Vector2> listOfChangeSpeed;
    private void Awake()
    {
        listOfChangeSpeed = new List<Vector2>();
        _ball = FindObjectOfType<Ball>();
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
        var relativePosX = ConvertPixelToRelativePosition(Input.mousePosition.x, Screen.width);
        transform.position = GetUpdatedPaddlePosition(relativePosX);
        UpdateAndCheckGearPotion();
    }
    public void ResetAllPotionEffect()
    {
        RemoveGearPotionEffect();
        RemoveBlueBottleEffect();
    }
    private void RemoveGearPotionEffect()
    {
        isInGearPotion = false;
        transform.localScale = new Vector3(1f, 1, 1);
        timeGearPotion = 10;
    }
    private void UpdateAndCheckGearPotion()
    {
        if (isInGearPotion == true)
        {
            timeGearPotion -= Time.deltaTime;

        }
        if (timeGearPotion <= 0)
        {
            RemoveGearPotionEffect();


        }
    }
    private void RemoveBlueBottleEffect()
    {
        CancelInvoke();
        ResetSpeedEmptyPotionEffect();
        listOfChangeSpeed = new List<Vector2>();
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
        var relativePosition = pixelPosition / screenWidth * screenWidthUnits;
        return relativePosition;
    }
    private void ResetSpeedEmptyPotionEffect()
    {
       for(int i=0;i <= listOfChangeSpeed.Count-1; i++)
        {
            _ball._rigidBody2D.velocity -= _ball._rigidBody2D.velocity * 10 / 100;
        }
    }
    // this function will increase velocity of ball 
    private void ResetSpeedBluePotionEffect()
    {
        float x, y;
        x = _ball._rigidBody2D.velocity.x;
        y = _ball._rigidBody2D.velocity.y;
        Debug.Log("=====================Begin Reset Speed Blue Potion ========================");
        Debug.Log("Value velocity before calculate:" + _ball._rigidBody2D.velocity);

        // because this author use Velocity to move the ball, so the ball's velocity will change everytime the ball collides with something
        // so i have to take the ball's velocity and increase it 10% because the Blue potion is out of effect

        //take the value of ball's velocity
        Vector2 vector = AbsVector(_ball._rigidBody2D.velocity);
        //calculated the the value of 10% of ball's velocity 
        vector = vector * 10 / 100;
        //Because the value of ball's velocity have x and y, and it is negative or positive. So if i want to increase the ball's velocity, i have to know whenever to minus or plus the 10% value of ball's velocity
        // Example if x =10, to increase it, just plus 10% value
        if (x >= 0.00001f)
        {
            x += vector.x;
        }
        //but if x=-10, to increase it, just minus 10% value
        else
        {
            x -= vector.x;
        }
        //same thing go for y
        if (y >= 0.00001f)
        {
            y += vector.y;
        }
        else
        {
            y -= vector.y;
        }
        //after calculated set the ball's velocity = new Velocity have more velocity 10%
        _ball._rigidBody2D.velocity = new Vector2(x, y);
        Debug.Log("Value velocity after calculated:" + _ball._rigidBody2D.velocity);
        //remove index for check condition, ball's velocity can be effected by 5 times
        listOfChangeSpeed.RemoveAt(0);
        Debug.Log("=====================End========================");
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
                    timeGearPotion = 10;
                }
            }
            else if (collision.gameObject.name.StartsWith("HeartPotion"))
            {
                Debug.Log("Heart");
                if (gameSession.PlayerLives < 5)
                {
                    gameSession.PlayerLives++;
                }
            }
            else if (collision.gameObject.name.StartsWith("BlueBottlePotion"))
            {
                Debug.Log("Blue");
                //if ball had taken the effect of Blue potion smaller 5 times
                if (listOfChangeSpeed.Count <= 5)
                {
                    Debug.Log("Velocity before take Blue potion: "+ _ball._rigidBody2D.velocity);
                    //calculated the value of 10% of ball's velocity
                    Vector2 vector = _ball._rigidBody2D.velocity * 10 / 100;
                    //decrease ball's velocity
                    _ball._rigidBody2D.velocity -= vector;
                    // add new value to condition check effect blue potion taken 
                    listOfChangeSpeed.Add(vector);
                    //after 10s run this function ResetSpeedBluePotionEffect
                    Invoke(nameof(ResetSpeedBluePotionEffect), timeBluePotion);
                    Debug.Log("Velocity after took Blue potion: " + _ball._rigidBody2D.velocity);
                }
            }
            else if (collision.gameObject.name.StartsWith("EmptyBottlePotion"))
            {
                Debug.Log("Empty");
                ResetAllPotionEffect();
            }
            Destroy(collision.gameObject);
        }
    }
    public Vector2 AbsVector(Vector2 vector)
    {
        return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    }
}