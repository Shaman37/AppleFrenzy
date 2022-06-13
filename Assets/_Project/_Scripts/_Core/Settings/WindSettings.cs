using UnityEngine;

namespace AppleFrenzy.Core
{
    /// <summary>
    ///     Provides a group of configurable settings for the Wind.
    /// </summary>
    [CreateAssetMenu(fileName = "WindSettings", menuName = "Scriptable Objects/Wind Settings")]
    public class WindSettings : ScriptableObject {
    
        [Header("Wind Settings")]
        public float windCooldownTime = 30f;
        public float windDurationMin = 5f;
        public float windDurationMax = 8f;
        public float windChance = 0.005f;
    
        [Header("Wind Particle System Settings")]
        public float normalVelocity = 10f;
        public float windVelocityMin = 30f;
        public float windVelocityMax = 50f;
        public float normalLeafParticles = 10f;
        public float windyLeafParticles = 100f;
    }
    
}