using UnityEngine;
using AppleFrenzy.Core;

namespace AppleFrenzy
{
    /// <summary>
    ///     Represents the game's Score Manager, holding relevant information 
    ///     about the current score, highscore, combo and number of apples caught/dropped.
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        #region [0] - Fields
        
        public int nApplesCaught;
        public int highScore;
        public int currentScore;

        private int _comboProgress;
        private int _comboMultiplier;
        private int _nGoodApplesDropped;
    
        // Properties
        private int nGoodApplesDropped 
        {
            get { return _nGoodApplesDropped; }
            set {
                _nGoodApplesDropped = value;
    
                if (_nGoodApplesDropped == 3)
                {
                    _nGoodApplesDropped = 0;
    
                    GameManager.Instance.HandleLives(true);
                }
            }
        }
    
        #endregion
        
        #region [1] - Unity Event Methods
        private void Awake()
        {
            nApplesCaught = 0;
            _comboProgress = 0;
            _comboMultiplier = 1;
    
            if (PlayerPrefs.HasKey("HighScore"))
            {
                highScore = PlayerPrefs.GetInt("HighScore");
            }
        }

        private void OnEnable()
        {
            Messenger<int>.AddListener(GameEvents.APPLE_CAUGHT, OnAppleCaught);
            Messenger<int>.AddListener(GameEvents.APPLE_DROPPED, OnAppleDropped);
        }

        private void OnDisable()
        {
            Messenger<int>.RemoveListener(GameEvents.APPLE_CAUGHT, OnAppleCaught);
            Messenger<int>.RemoveListener(GameEvents.APPLE_DROPPED, OnAppleDropped);
        }
    
        #endregion
    
        #region [2] - Game Events
    
        /// <summary>
        ///     Responsible for handling what happens when an Apple is caught.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="appleScore">
        ///         The score of the apple caught.
        ///     </param>
        /// </parameters>
        public void OnAppleCaught(int appleScore)
        {
            bool takeLife = false;
            bool isAGoodApple = false;

            if (appleScore > 0)
            {
                isAGoodApple = true;
                nApplesCaught++;
                nGoodApplesDropped = 0;
            }
            else
            {
                isAGoodApple = false;
                takeLife = true;
            }
    
            HandleComboCounter(isAGoodApple);
            CalculateCurrentScore(appleScore);
        
            GameManager.Instance.HandleLives(takeLife);
        }
    
        /// <summary>
        ///    Responsible for updating the score and resetting the combo counter when a 
        ///    good Apple (positive score) is dropped on the ground, notifying the UI about the
        ///    changes made.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="penalty">
        ///         The score penalty when an apple is dropped on the ground.
        ///     </param>
        /// </parameters> 
        public void OnAppleDropped(int penalty)
        {
            currentScore -= penalty;
            currentScore = Mathf.Max(0, currentScore);

            _comboProgress = 0;
            _comboMultiplier = 1;
    
            Messenger<int>.Broadcast(GameEvents.SCORE_UPDATE, currentScore);
            Messenger<int, int, bool>.Broadcast(GameEvents.COMBO_UPDATE, _comboProgress, _comboMultiplier, true);
    
            nGoodApplesDropped++;
        }
    
        #endregion
    
        #region [3] - Methods
    
        /// <summary>
        ///     Responsible for updating the combo counter and notifies the UI about the changes
        ///     made.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="isAGoodApple">
        ///         Is it a good or bad apple.
        ///     </param>
        /// </parameters>
        private void HandleComboCounter(bool isAGoodApple)
        {
            int comboLimit = _comboMultiplier * 10;
            bool shouldUpdateMultiplier = false;
    
            if (isAGoodApple && _comboMultiplier < 4)
            {
                _comboProgress++;
    
                if (_comboProgress == comboLimit)
                {
                    _comboMultiplier++;
                    _comboProgress = 0;
                    shouldUpdateMultiplier = true;
                }
            }
    
            if (!isAGoodApple)
            {
                _comboProgress = 0;
                _comboMultiplier = 1;
                shouldUpdateMultiplier = true;
            }

            // Signal combo information update with a COMBO_UPDATE Event
            Messenger<int, int, bool>.Broadcast(GameEvents.COMBO_UPDATE, _comboProgress, _comboMultiplier, shouldUpdateMultiplier);
        }

        /// <summary>
        ///     Responsible for calculating the new current score and notifying the UI about the changes made.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="appleScore">
        ///         The score of the apple caught.
        ///     </param>
        /// </parameters>
        private void CalculateCurrentScore(int appleScore)
        {
            currentScore += appleScore * _comboMultiplier;
            currentScore = Mathf.Max(0, currentScore);

            Messenger<int>.Broadcast(GameEvents.SCORE_UPDATE, currentScore);

            if (currentScore > highScore)
            {
                highScore = currentScore;
            }
        }  
    
        #endregion
    }
}