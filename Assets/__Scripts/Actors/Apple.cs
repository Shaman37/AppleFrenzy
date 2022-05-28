using UnityEngine;

/// <summary>
///     Enum with the different types of apples.
/// </summary>
public enum eAppleType
{
    GreenApple, //worth 100 score points
    RedApple, //worth 250 score points
    GoldenApple, //worth 500 score points   
    SpoiledApple, //-500 score points
    Stick, //-1000 score points
}

/// <summary>
///     Class which handles Apple behaviour.
/// </summary>
public class Apple : MonoBehaviour
{
    #region [0] - Fields

    public AppleSettings  settings;
    private Rigidbody      _rigid;
    private SpriteRenderer _spriteRenderer;
    private float          _bottomY;
    private float          _maxAppleX;
    private float          _windSpeedModifier = 2f;

    #endregion

    #region [1] - Unity Event Methods

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        _rigid = gameObject.GetComponent<Rigidbody>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _bottomY = -Camera.main.orthographicSize;
        _maxAppleX = Camera.main.aspect * Camera.main.orthographicSize;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {   
        if(_rigid == null) Debug.LogError("Rigid body for apple not found");
        if(_spriteRenderer == null) Debug.LogError("Sprite REnderer for apple not found");

        SetAppleSettings(settings);
    }

    private void Update()
    {
        if(transform.position.y < _bottomY)
        {
            bool isWithinBounds = Mathf.Abs(transform.position.x) < _maxAppleX ;
            bool isGoodApple = settings.score > 0;

            if (isGoodApple && isWithinBounds)
            {
                Messenger<int>.Broadcast(GameEvents.APPLE_DROPPED, settings.dropPenalty);
            }

            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        _rigid.AddForce(Vector3.down * settings.velocity);

        if(Wind.IS_WINDY){
            _rigid.AddForce(Vector3.right * _windSpeedModifier);
        }
    }

    #endregion
    
    #region [2] - Methods

    /// <summary>
    ///     Responsible for setting up an Apple after it is Instantiated.
    ///     Increase Apple's velocity depending on the current level and 
    ///     adds a bit of horizontal velocity at the beggining of the drop
    /// </summary>
    /// <param name="appleSettings">The Settings scriptable object associated to the instantiated Apple Type.</param>
    /// <param name="difficultyVelocityIncrease">Represents the velocity increase on the apple, according to the current dificulty level</param>
    public void SetAppleSettings(AppleSettings appleSettings, float difficultyVelocityIncrease = 0)
    {
        settings = appleSettings;
        _spriteRenderer.sprite = settings.sprite;
        gameObject.layer = LayerMask.NameToLayer(settings.type.ToString());
        
        Vector3 horizontalVel = Vector3.right * Random.Range(-2f, 2f);
        Vector3 dropVel = Vector3.down * difficultyVelocityIncrease;
        _rigid.velocity = horizontalVel + dropVel;
    }

    #endregion
}   
