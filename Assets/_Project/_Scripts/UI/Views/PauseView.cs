namespace AppleFrenzy.UI
{
    /// <summary>
    ///     Represents the 'Pause View', activated when the game is paused.
    /// </summary>
    public class PauseView : View
    {   
        #region [0] - Unity Event Methods
        
        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            Hide();
        }

        #endregion

        #region [1] - Button Events

        /// <summary>
        ///     Responsible for resuming the game, changing the current state of the
        ///     game:
        ///         
        ///         - " GamePausedState -> GameRunningState "
        /// </summary>
        public void ResumeClick()
        {
            GameManager.Instance.ChangeState(new GameRunningState());
        }

        /// <summary>
        ///     Responsible for quiting the game to the 'Main Menu'.
        /// </summary>
        public void QuitClick()
        {
            SceneController.Instance.ReturnToMainMenu();
        }

        #endregion
    }
}
