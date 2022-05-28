using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/BasketSettings")]
public class BasketSettings : ScriptableObject
{
    [Header("Basket Animation Settings")]
    public float swapDuration = 0.5f;
    public float shakeDuration = 0.2f;
}