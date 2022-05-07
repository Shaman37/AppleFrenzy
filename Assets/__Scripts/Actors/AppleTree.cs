 using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///     Class which handles the Apple Tree behaviour.
///     It can move randomly along the X axis and randomly drop Apples.
/// </summary>
public class AppleTree : MonoBehaviour
{   
    #region Variables
    
    [SerializeField] private AppleTreeSettings _settings;
    [SerializeField] private AppleSettings[]   _appleSettings;
    [SerializeField] private GameObject _prefabApple;

    private Dictionary<eAppleType, AppleSettings> _appleDict;
    
    private float _velocity;
    private float _velocityIncrease;
    private float _waitingTimeModifier;

    #endregion

    #region Unity Event Methods

    private void Awake() 
    {
        _velocityIncrease = 0;
        _waitingTimeModifier = 0;
        _velocity = _settings.treeVelocity;
        
        // Create a Dictionary with the different Apple Types and corresponding Settings
        _appleDict = new Dictionary<eAppleType, AppleSettings>();
        foreach (AppleSettings settings in _appleSettings)
        {
            _appleDict[settings.type] = settings; 
        }
    }

    private void Start() 
    {
        StartCoroutine(DropAppleCoroutine());
    }

    private void Update()
    {
        MoveRandomly();
    }

    private void FixedUpdate()
    {   
        if(Random.value < _settings.chanceToChangeDirections)
        {
            _velocity *= -1;
        }
    }

    #endregion


    #region Tree Actions
         
    /// <summary>
    ///     Randomly move the Apple Tree in the X axis, within the imposed screen limit.
    /// </summary>
    private void MoveRandomly()
    {
        float edge = Camera.main.ViewportToWorldPoint(Vector3.right).x - 6f;

        // Check if the Apple Tree is within the set screen limit, else change it's velocity.
        if(transform.position.x < -edge)
        {
            _velocity = Mathf.Abs(_velocity);
        }

        else if(transform.position.x > edge)
        {
            _velocity = -Mathf.Abs(_velocity);
        }

        transform.position += Vector3.right * _velocity * Time.deltaTime;
    }

    /// <summary>
    ///     A Coroutine of an Apple drop. 
    ///     Wait the respective amount of time before dropping another Apple.
    /// </summary>
    private IEnumerator DropAppleCoroutine()
    {
        while(true)
        {
            // Get random apple type from frequency list and corresponding settings
            int ndx;
            eAppleType type;       

            if(Wind.IS_WINDY)
            {   
                // If it is Windy, sticks may fall
                ndx = Random.Range(0, _settings.appleFrequency.Length);
                type = _settings.appleFrequency[ndx];
            }
            else
            {
                //If it is not Windy, sticks won't fall
                eAppleType[] noSticks = _settings.appleFrequency.Where(apT => apT != eAppleType.Stick).ToArray();
                ndx = Random.Range(0, noSticks.Length);
                type = noSticks[ndx];
            }

            AppleSettings appleSettings = _appleDict[type];

            // Instantiate a new Apple
            GameObject go = Instantiate(_settings.prefabApple) as GameObject;
            Apple ap = go.GetComponent<Apple>();

            // Set Apple Settings and initial position
            ap.transform.position = transform.position;
            ap.SetAppleSettings(appleSettings, _velocityIncrease);

            // Start the Coroutine
            float waitingTime = ap.settings.secondsBetweenAppleDrops;
            yield return new WaitForSeconds(waitingTime + _waitingTimeModifier);
        }
    }

    #endregion

    #region Event Functions

    #endregion

    public float velocity
    {
        get
        {
            return _velocity;
        }
        set
        {
            _velocity = value;
        }
    }

    public float waitingTimeModifier
    {
        get
        {
            return _waitingTimeModifier;
        }
        set
        {
            _waitingTimeModifier = value;
        }
    }
    
    
}