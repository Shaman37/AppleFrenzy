using UnityEngine;

namespace AppleFrenzy.Core
{
    /// <summary>
    ///     Provides a group of configurable settings for an Apple.
    /// </summary>
    [CreateAssetMenu(fileName = "AppleSettings", menuName = "Scriptable Objects/Apple Settings")]
    public class AppleSettings : ScriptableObject {
        
        [Header("General Apple Settings")]
        public eAppleType type;
        public Sprite     sprite;
        public int        score;
        public int        dropPenalty;        
        public float      velocity;
        public float      secondsBetweenAppleDrops;
    }
}