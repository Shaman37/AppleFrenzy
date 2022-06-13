using UnityEngine;
using AppleFrenzy.Core;

namespace AppleFrenzy
{
    /// <summary>
    ///     Responsible for handling game state changes and general game information.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {   
        #region [0] - Fields

        // State related fields
        public bool   isGamePaused;
        private State _currentState;

        // General Game fields
        private int _level;
        private int _maxLevel;
        private int _lives;

        // Manager fields
        [SerializeField] private ScoreManager _scoreManager;

        #endregion
    
        #region [1] - Unity Event Methods
        
        protected override void Awake()
        {
            base.Awake();

            _level = 1;
            _lives = 3;
            _maxLevel = 5;
        }

        private void Start() 
        {
            ChangeState(new GameRunningState());
        }
    
        private void Update() 
        {
            _currentState.Update();

            if (_currentState.type == eStateType.GameRunning)
            {
                CheckForDifficultyIncrease();
            }
        }
    
        #endregion
    
        /// <summary>
        ///     Responsible for changing the current game state to a new one.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="newState">
        ///         The new game state.
        ///     </param>
        /// </parameters>
        public void ChangeState(State newState)
        {   
            if(_currentState != null)
            {
                _currentState.Destroy();
            }
    
            _currentState = newState;
    
            _currentState.Start();
        }
    
        /// <summary>
        ///     Responsible for handling what happens when a Game Over state is reached.
        /// </summary>
        public void GameOver()
        {
            Save();
            ChangeState(new GameOverState());
            
            NotifyGameStatsDisplay();
        }

        /// <summary>
        ///     Responsible for signaling UI to display the final game statistics.
        /// </summary>
        private void NotifyGameStatsDisplay()
        {
            int score = _scoreManager.currentScore;
            int highScore = _scoreManager.highScore;
            int applesCaught = _scoreManager.nApplesCaught;

            int[] info = new int[] { score, highScore, applesCaught };

            Messenger<int[]>.Broadcast(GameEvents.GAME_OVER, info);
        }

        /// <summary>
        ///     Responsible for saving relevant information.
        /// </summary>
        private void Save()
        {
            PlayerPrefs.SetInt("HighScore", _scoreManager.highScore);
        }

        /// <summary>
        ///     Responsible for adding or removing lives, notifying UI when a change is made.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="takeLife">
        ///         If a life should be taken or not.
        ///     </param>
        /// </parameters>
        public void HandleLives(bool takeLife = false)
        {
            int applesCaught = _scoreManager.nApplesCaught;

            if (applesCaught != 0 && (applesCaught % 100) == 0 && _lives < 3)
            {
                Messenger<int>.Broadcast(GameEvents.LIFE_ONE_UP, _lives);
                _lives++;
            }

            if (takeLife)
            {
                _lives--;
                
                if (_lives == 0)
                {
                    GameOver();  
                }
                else
                {
                    Messenger<int>.Broadcast(GameEvents.LIFE_ONE_DOWN, _lives);
                }
            }
        }

        /// <summary>
        ///     Responsible for checking if the game's difficulty needs to be increased.
        /// </summary>
        private void CheckForDifficultyIncrease()
        {
            int score = _scoreManager.currentScore;

            // Every 7 500 points, increase game difficulty
            if (Mathf.Floor(score / (7500 * _level)) >= _level && _level < _maxLevel)
            {
                _level++;
    
                Messenger<int>.Broadcast(GameEvents.INC_DIFFICULTY, _level);
            }
        }
    }
}
