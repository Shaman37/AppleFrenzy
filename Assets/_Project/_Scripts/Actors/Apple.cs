using UnityEngine;
using AppleFrenzy.Core;

namespace AppleFrenzy
{
    /// <summary>
    ///     Represents an Apple and handles it's behaviour throughout the game.
    /// </summary>
    public class Apple : MonoBehaviour
    {
        #region [0] - Fields
    
        public AppleSettings settings;

        [SerializeField] private Rigidbody      _rigid;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private float _bottomY;
        private float _maxAppleX;
        private float _windSpeedModifier = 2f;
    
        #endregion
    
        #region [1] - Unity Event Methods
    
        private void Awake()
        {
            if(_rigid == null) Debug.LogError("Rigid body for Apple not found");
            if(_spriteRenderer == null) Debug.LogError("Sprite REnderer for Apple not found");

            var orthographicSize = CameraManager.Instance.gameCamera.orthographicSize;
            _bottomY = -orthographicSize;
            _maxAppleX = CameraManager.Instance.gameCamera.aspect * orthographicSize;
        }
    
        private void Start()
        {       
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
        ///     Increases Apple's velocity depending on the current level adding
        ///     a bit of horizontal velocity at the beggining of the drop
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="appleSettings">
        ///         The Settings scriptable object associated to the instantiated Apple Type.
        ///     </param>
        ///     <param name="difficultyVelocityIncrease">
        ///         Represents the velocity increase on the apple, according to the current dificulty level.
        ///     </param>
        /// </parameters> 
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
}   
