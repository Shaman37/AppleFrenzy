using UnityEngine;

namespace AppleFrenzy.Core
{
  /// <summary>
  ///     Provides a group of configurable settings for an Apple Tree.
  /// </summary>
  [CreateAssetMenu(fileName = "AppleTreeSettings", menuName = "Scriptable Objects/Apple Tree Settings")]
  public class AppleTreeSettings : ScriptableObject{
  
      [Header("Tree Movement")]
      public float treeVelocity;
      public float chanceToChangeDirections;
  
  
      [Header("General Settings")]
      public Apple        prefabApple;
      public eAppleType[] appleFrequency = new eAppleType[] { eAppleType.GreenApple, eAppleType.GreenApple, eAppleType.GreenApple,
                                                              eAppleType.RedApple, eAppleType.RedApple,
                                                              eAppleType.GoldenApple
                                                            };
  }
}
