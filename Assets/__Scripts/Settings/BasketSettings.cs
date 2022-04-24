using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/BasketSettings")]
public class BasketSettings : ScriptableObject
{
    [Header("Basket Settings")]
    public float leftAndRightEdge = 30f;
    public float swapDuration = 0.5f;

}