using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Apple Settings")]
public class AppleSettings : ScriptableObject {
    public eAppleType type;
    public Sprite sprite;
    public int score;
    public int dropPenalty;

    public float velocity;
    public float secondsBetweenAppleDrops;
}