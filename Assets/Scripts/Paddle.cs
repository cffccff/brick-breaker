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
    private void ResetSpeedBluePotionEffect()
    {
        float x, y;
        x = _ball._rigidBody2D.velocity.x;
        y = _ball._rigidBody2D.velocity.y;
        Debug.Log("=====================Begin Reset Speed Blue Potion ========================");
        Debug.Log("Value velocity before calculate:" + _ball._rigidBody2D.velocity);

        Vector2 vector = AbsVector(_ball._rigidBody2D.velocity);
        vector = vector * 10 / 100;
        if (x >= 0.00001f)
        {
            x += vector.x;
        }
        else
        {
            x -= vector.x;
        }
        if (y >= 0.00001f)
        {
            y += vector.y;
        }
        else
        {
            y -= vector.y;
        }
        _ball._rigidBody2D.velocity = new Vector2(x, y);
        Debug.Log("Value velocity after calculated:" + _ball._rigidBody2D.velocity);
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
                    timeGear = 10;
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
                if (listOfChangeSpeed.Count <= 5)
                {
                    Debug.Log("Velocity before take Blue potion: "+ _ball._rigidBody2D.velocity);
                    Vector2 vector = _ball._rigidBody2D.velocity * 10 / 100;
                    _ball._rigidBody2D.velocity -= vector;
                    listOfChangeSpeed.Add(vector);
                    Invoke(nameof(ResetSpeedBluePotionEffect), 10f);
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