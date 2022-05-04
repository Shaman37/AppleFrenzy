using UnityEngine;

[CreateAssetMenu(fileName = "AppleTreeSettings", menuName = "Scriptable Objects/Apple Tree Settings")]
public class AppleTreeSettings : ScriptableObject{

    [Header("Tree Movement")]
    public float        treeVelocity;
    public float        chanceToChangeDirections;


    [Header("General Settings")]
    public int          lives = 3;
    public GameObject   prefabApple;

    public eAppleType[] appleFrequency = new eAppleType[] { eAppleType.GreenApple, eAppleType.GreenApple, eAppleType.GreenApple,
                                                            eAppleType.RedApple, eAppleType.RedApple,
                                                            eAppleType.GoldenApple
                                                          };
}
