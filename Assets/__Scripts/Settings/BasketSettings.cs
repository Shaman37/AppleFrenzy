using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/BasketSettings")]
public class BasketSettings : ScriptableObject
{
    [Header("Basket Settings")]
    public float swapDuration = 0.5f;
    public float moveSpeed = 0.5f;

}