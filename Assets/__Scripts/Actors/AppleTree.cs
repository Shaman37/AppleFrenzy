using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
///     Class which handles the Apple Tree behaviour.
///     It can move randomly along the X axis and randomly drop Apples.
/// </summary>
public class AppleTree : MonoBehaviour
{
    #region [0] - Fields

    [HideInInspector] public float velocity;
    [HideInInspector] public float waitingTimeModifier;

    [SerializeField] private AppleTreeSettings    _settings;
    [SerializeField] private AppleSettings[]      _appleSettings;
    private Dictionary<eAppleType, AppleSettings> _appleDict;
    private float                                 _velocityIncrease;
    private float                                 _waitingTime;

    #endregion

    #region [1] - Unity Event Methods

    private void Awake()
    {
        _velocityIncrease = 0;
        waitingTimeModifier = 0;
        velocity = _settings.treeVelocity;

        // Create a Dictionary with the different Apple Types and corresponding Settings
        _appleDict = new Dictionary<eAppleType, AppleSettings>();
        foreach (AppleSettings settings in _appleSettings)
        {
            _appleDict[settings.type] = settings;
        }
    }

    private void Update()
    {
        MoveRandomly();

        if (_waitingTime <= 0)
        {
            DropApple();
        }
        else
        {
            _waitingTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (Random.value < _settings.chanceToChangeDirections)
        {
            velocity *= -1;
        }
    }

    #endregion

    #region [2] - Tree Actions

    /// <summary>
    ///     Randomly move the Apple Tree in the X axis, within the imposed screen limit.
    /// </summary>
    private void MoveRandomly()
    {
        float edge = Camera.main.ViewportToWorldPoint(Vector3.right).x - 6f;

        // Check if the Apple Tree is within the set screen limit, else change it's velocity.
        if (transform.position.x < -edge)
        {
            velocity = Mathf.Abs(velocity);
        }

        else if (transform.position.x > edge)
        {
            velocity = -Mathf.Abs(velocity);
        }

        transform.position += Vector3.right * velocity * Time.deltaTime;
    }

    /// <summary>
    ///     Responsible for dropping random apples from the Apple Tree, setting the "waitingTime" variable to it'«s correct value.
    /// </summary>
    private void DropApple()
    {
        // Get random apple type from frequency list and corresponding settings
        int index;
        eAppleType type;

        if (Wind.IS_WINDY)
        {
            // If it is Windy, sticks may fall
            index = Random.Range(0, _settings.appleFrequency.Length);
            type = _settings.appleFrequency[index];
        }
        else
        {
            //If it is not Windy, sticks won't fall
            eAppleType[] noSticks = _settings.appleFrequency.Where(apT => apT != eAppleType.Stick).ToArray();
            index = Random.Range(0, noSticks.Length);
            type = noSticks[index];
        }

        Apple apple = CreateApple(type);

        _waitingTime = apple.settings.secondsBetweenAppleDrops + waitingTimeModifier;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Apple CreateApple(eAppleType type)
    {
        Apple apple = Instantiate(_settings.prefabApple);

        apple.transform.position = transform.position;
        AppleSettings appleSettings = _appleDict[type];
        apple.SetAppleSettings(appleSettings, _velocityIncrease);

        return apple;
    }

    #endregion
}