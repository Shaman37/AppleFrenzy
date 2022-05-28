using UnityEngine;

/// <summary>
///     Class responsible for creating gusts of wind throughout the game and controlling
///     the associated Particle System.
/// </summary>
public class Wind : MonoBehaviour
{
    #region [0] - Fields

    static public bool IS_WINDY = false;
        
    [Header("Settings")]
    [SerializeField] private WindSettings _settings;
    [SerializeField] private ParticleSystem _particleSystem;

    private ParticleSystem.EmissionModule             _particleEmission;
    private ParticleSystem.VelocityOverLifetimeModule _particleVelocity;
    private float                                     _readyTime;
    private float                                     _stopTime;

    #endregion

    #region [1] - Unity Event Methods
        
    private void Awake()
    {
        _readyTime = Time.time + _settings.windCooldownTime;

        if (_particleSystem == null)
        {
            Debug.LogError("Particle System not assigned in 'Wind.cs' !");
        }
        else
        {
            _particleEmission = _particleSystem.emission;
            _particleVelocity = _particleSystem.velocityOverLifetime; 
        }
    }

    private void FixedUpdate()
    {
        if(_readyTime < Time.time)
        {
            WindChance();
        }
        else
        {
            if(Time.time > _stopTime)
            {
                IS_WINDY = false;

                // Return the number of leaf particles and their velocity to normal
                _particleEmission.rateOverTime = _settings.normalLeafParticles;
                _particleVelocity.xMultiplier = _settings.normalVelocity;
            }
        }
    }

    #endregion
        
    /// <summary>
    ///     Responsible for spawning a gust of wind.
    ///     Handles the wind duration and velocity and increases the number of particles if the wind is active.
    /// </summary>
    private void WindChance()
    {
        
        float chance = Random.value;

        if (chance < _settings.windChance)
        {
            IS_WINDY = true;

            // Wind StopTime and Cooldown calculations
            float timeStart = Time.time;
            float windDuration = Random.Range(_settings.windDurationMin, _settings.windDurationMax);

            _stopTime = timeStart + windDuration;
            _readyTime = timeStart + _settings.windCooldownTime;

            // Increase leaf particles and velocity across the X axis
            _particleEmission.rateOverTime = _settings.windyLeafParticles;
            
            float windVelocity = Random.Range(_settings.windVelocityMin, _settings.windVelocityMax);
            _particleVelocity.xMultiplier = windVelocity;
        }  
    }
}
