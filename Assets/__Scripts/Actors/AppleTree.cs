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
    
    [Header("Settings")]
    [SerializeField] private AppleTreeSettings _settings;
    [SerializeField] private AppleSettings[]   _appleSettings;

    private Dictionary<eAppleType, AppleSettings> APPLE_DICT;
    
    private Vector3   pos;
    private float     velocity;
    private float     velocityIncrease;
    private float     waitingTimeIncrease;


    #endregion


    #region Unity Event Methods

    private void Awake() 
    {
        velocity = _settings.treeVelocity;
        pos = transform.position;

        // Create a Dictionary with the different Apple Types and corresponding Settings
        APPLE_DICT = new Dictionary<eAppleType, AppleSettings>();
        foreach (AppleSettings settings in _appleSettings)
        {
            APPLE_DICT[settings.type] = settings; 
        }

        velocityIncrease = 0;
        waitingTimeIncrease = 0;
    }

    private void OnEnable() {
        ScoreManager.NextLevel += IncreaseDifficulty;
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
            velocity *= -1;
        }
    }
    
    private void OnDisable() {
        ScoreManager.NextLevel -= IncreaseDifficulty;

    }
    #endregion


    #region Tree Actions
         
    /// <summary>
    ///     Randomly move the Apple Tree in the X axis, within the imposed screen limit.
    /// </summary>
    private void MoveRandomly()
    {
        pos += Vector3.right * velocity * Time.deltaTime;

        float edge = Camera.main.ViewportToWorldPoint(Vector3.right).x - 6f;

        // Check if the Apple Tree is within the set screen limit, else change it's velocity.
        if(pos.x < -edge)
        {
            velocity = Mathf.Abs(velocity);
        }

        else if(pos.x > edge)
        {
            velocity = -Mathf.Abs(velocity);
        }

        transform.position = pos;
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

            AppleSettings appleSettings = APPLE_DICT[type];

            // Instantiate a new Apple
            GameObject go = Instantiate(_settings.prefabApple) as GameObject;
            Apple ap = go.GetComponent<Apple>();

            // Set Apple Settings and initial position
            ap.transform.position = pos;
            ap.SetAppleSettings(appleSettings, velocityIncrease);

            // Start the Coroutine
            float waitingTime = ap.settings.secondsBetweenAppleDrops;
            yield return new WaitForSeconds(waitingTime - waitingTimeIncrease);
        }
    }

    #endregion

    #region Event Functions

    private void IncreaseDifficulty(){
        velocityIncrease += 1.5f;
        waitingTimeIncrease += 0.25f;
    }

    #endregion
}