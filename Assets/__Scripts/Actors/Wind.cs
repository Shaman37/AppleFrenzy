using UnityEngine;

/// <summary>
///     Class responsible for Wind Behaviour.
/// </summary>
public class Wind : MonoBehaviour
{
    #region Variables
        
    [Header("Settings")]
    [SerializeField] WindSettings settings;

    private ParticleSystem.EmissionModule             pEmission;
    private ParticleSystem.VelocityOverLifetimeModule pVelocity;
    private float                                     readyTime;
    private float                                     stopTime;
    public static bool                                IS_WINDY = false;


    #endregion


    #region Unity Event Methods
        
    private void Awake()
    {
        readyTime = settings.windCooldownTime;
        ParticleSystem pSystem = gameObject.GetComponent<ParticleSystem>();
        pEmission = pSystem.emission;
        pVelocity = pSystem.velocityOverLifetime;
    }

    private void FixedUpdate()
    {
        if(readyTime < Time.time)
        {
            WindChance();
        }
        else
        {
            if(Time.time > stopTime)
            {
                IS_WINDY = false;

                // Return the number of leaf particles and their velocity to normal
                pEmission.rateOverTime = settings.normalLeafParticles;
                pVelocity.xMultiplier = settings.normalVelocity;
            }
        }
    }

    #endregion


    #region Methods
        
    /// <summary>
    ///     Responsible for spawning a gust of wind.
    ///     Handles the wind duration and velocity and increases the number of particles if the wind is active.
    /// </summary>
    private void WindChance()
    {
        
        float chance = Random.value;

        if (chance < settings.windChance)
        {
            IS_WINDY = true;

            // Wind StopTime and Cooldown calculations
            float timeStart = Time.time;
            float windDuration = Random.Range(settings.windDurationMin, settings.windDurationMax);

            stopTime = timeStart + windDuration;
            readyTime = timeStart + settings.windCooldownTime;

            // Increase leaf particles and velocity across the X axis
            pEmission.rateOverTime = settings.windyLeafParticles;
            
            float windVelocity = Random.Range(settings.windVelocityMin, settings.windVelocityMax);
            pVelocity.xMultiplier = settings.windVelocityMax;
        }
        
    } 

    #endregion
}
