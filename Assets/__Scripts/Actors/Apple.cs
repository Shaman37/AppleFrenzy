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
    #region Variables
        
    // Variables
    private AppleSettings  _settings;
    private Rigidbody      _rigid;
    private SpriteRenderer _spriteRenderer;
    private float          _bottomY;
    private float          _maxAppleX;

    
    #endregion


    #region Unity Event Methods
    
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
        SetAppleSettings(settings);
    }

    private void Update()
    {
        if(transform.position.y < _bottomY)
        {
            float appleX = Mathf.Abs(transform.position.x);
            // Checks if dropped apple is not a Spoiled Apple or a Stick
            if (settings.score > 0 && appleX < _maxAppleX)
            {
                ScoreManager.OnAppleDropped(settings.dropPenalty);
            }

            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        // If windy, add a horizontal force to the apple
        if(Wind.isWindy){
            _rigid.AddForce(Vector3.right * 7.5f * _rigid.mass);
        }
    }

    #endregion
    

    #region Methods

    /// <summary>
    ///     Responsible for setting up an Apple after it is Instantiated.
    /// </summary>
    /// <param name="appleSettings">The Settings scriptable object associated to the instantiated Apple Type.</param>
    public void SetAppleSettings(AppleSettings appleSettings, float difficultyVelocityIncrease = 0)
    {
        settings = appleSettings;
        _spriteRenderer.sprite = settings.sprite;
        gameObject.layer = LayerMask.NameToLayer(settings.type.ToString());

        // Adds a bit of horizontal velocity at the beggining of the drop
        _rigid.velocity += Vector3.right * Random.Range(-2f, 2f);

        // Increase Apple's velocity depending on the current level
        _rigid.velocity += Vector3.down * difficultyVelocityIncrease;
    }

    #endregion


    #region Properties
        
    public AppleSettings settings
    {
        get 
        { 
            return _settings; 
        }
        set 
        {
            _settings = value; 
        }
    }
    
    #endregion
}   
