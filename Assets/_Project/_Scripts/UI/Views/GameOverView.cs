using UnityEngine;
using TMPro;
using AppleFrenzy.Core;

namespace AppleFrenzy.UI
{
    /// <summary>
    ///     Represents the 'Game Over View', activated when the player loses all lives.
    /// </summary>
    public class GameOverView : View
    {
        #region [0] - Fields

        // UI Variables
        [SerializeField] private TextMeshProUGUI textApplesCaught;
        [SerializeField] private TextMeshProUGUI textScore;
        [SerializeField] private TextMeshProUGUI textHighScore;

        #endregion

        #region [1] - Unity Event Methods

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            Hide();
        }

        private void OnEnable() {
            Messenger<int[]>.AddListener(GameEvents.GAME_OVER, DisplayInfo);
        }
    
        private void OnDisable() {
            Messenger<int[]>.RemoveListener(GameEvents.GAME_OVER, DisplayInfo);
        }

        #endregion
    
        #region [2] - Methods
        
        /// <summary>
        ///     Responsible for displaying the game statistics in the current 'View'.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="info">
        ///         [0] -> Current Score;
        ///         [1] -> High Score;
        ///         [2] -> Total number of apples caught;
        ///     </param>
        /// </parameters>
        private void DisplayInfo(int[] info) 
        {   
            textApplesCaught.text += info[2];
            textScore.text += info[0];
            textHighScore.text += info[1]; 
        }
        
        #endregion
    
        #region [3] - Button Events

        /// <summary>
        ///     Responsible for restarting the game when 'Play Again' is clicked.
        /// </summary>
        public void PlayAgainClick()
        {
            SceneController.Instance.RestartGame();
        }

        /// <summary>
        ///     Responsible for quiting the game to the 'Main Menu' when 'Quit' is clicked.
        /// </summary>
        public void QuitClick()
        {
            SceneController.Instance.ReturnToMainMenu();
        }

        #endregion
    }
    
}