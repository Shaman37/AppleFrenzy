using UnityEngine;

namespace AppleFrenzy.Core
{
    /// <summary>
    ///     Provides a group of configurable settings for the Baskets.
    /// </summary> 
    [CreateAssetMenu(fileName = "BasketsSettings", menuName = "Scriptable Objects/BasketaSettings")]
    public class BasketsSettings : ScriptableObject
    {
        [Header("Basket Movement Settings")]
        public float moveSpeed = 0.5f;

        [Header("Basket Animation Settings")]
        public float swapDuration = 0.5f;
        public float shakeDuration = 0.2f;
    }
}
