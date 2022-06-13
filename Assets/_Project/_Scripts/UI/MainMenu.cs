using UnityEngine;

namespace AppleFrenzy.UI
{
    /// <summary>
    ///     Represents the Main Menu of the game.
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        ///     Responsible for loading the game, when 'Play' is clicked.
        /// </summary>
        public void PlayClick()
        {
            SceneController.Instance.LoadGame();
        }
    }  
}