using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ScoreManager : MonoBehaviour
{
    #region [0] - Fields
        
    private int _level;
    private int _maxLevel;
    private int _lives;
    private int _highScore;
    private int _currentScore;
    private int _nApplesCaught;
    private int _comboProgress;
    private int _comboMultiplier;
    private int _nGoodApplesDropped;

    // Properties

    private int nGoodApplesDropped {
        get { return _nGoodApplesDropped; }
        set {
            _nGoodApplesDropped = value;

            if (_nGoodApplesDropped == 3)
            {
                _nGoodApplesDropped = 0;

                HandleLives(true);
            }
        }
    }

    #endregion
    
    #region [1] - Unity Event Methods
    private void Awake()
    {
        _level = 1;
        _lives = 3;
        _nApplesCaught = 0;
        _comboProgress = 0;
        _comboMultiplier = 1;
        _maxLevel = 5;

        if (PlayerPrefs.HasKey("HighScore"))
        {
            _highScore = PlayerPrefs.GetInt("HighScore");
        }
    }

    private void Update()
    {
        CheckForDifficultyIncrease();
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
        Messenger<int>.AddListener(GameEvents.APPLE_CAUGHT, OnAppleCaught);
        Messenger<int>.AddListener(GameEvents.APPLE_DROPPED, OnAppleDropped);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDisable()
    {
        Messenger<int>.RemoveListener(GameEvents.APPLE_CAUGHT, OnAppleCaught);
        Messenger<int>.RemoveListener(GameEvents.APPLE_DROPPED, OnAppleDropped);
    }

    #endregion

    #region [2] - Game Events

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appleScore"></param>
    public void OnAppleCaught(int appleScore)
    {
        bool takeLife = false;
        if (appleScore > 0)
        {
            _nApplesCaught++;
            nGoodApplesDropped = 0;
        }
        else
        {
            takeLife = true;
        }

        HandleComboCounter(appleScore);
        CalculateCurrentScore(appleScore);

        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
        }

        Messenger<int>.Broadcast(GameEvents.SCORE_UPDATE, _currentScore);

        HandleLives(takeLife);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="penalty"></param>
    public void OnAppleDropped(int penalty)
    {
        _currentScore -= penalty;
        _currentScore = Mathf.Max(0, _currentScore);

        _comboProgress = 0;
        _comboMultiplier = 1;

        Messenger<int>.Broadcast(GameEvents.SCORE_UPDATE, _currentScore);
        Messenger<int, int, bool>.Broadcast(GameEvents.COMBO_UPDATE, _comboProgress, _comboMultiplier, true);

        nGoodApplesDropped++;
    }

    #endregion

    #region [3] - Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="takeLife"></param>
    private void HandleLives(bool takeLife = false)
    {

        // If player has less than 3 lives, add a life every 100 good appples caught 
        if ((_nApplesCaught % 100) == 0 && _lives < 3)
        {
            Messenger<int, bool>.Broadcast(GameEvents.LIFE_ONE_UP, _lives, true);
            _lives++;
        }


        if (takeLife)
        {
            _lives--;
            // If no more lives are left, end game. Else, update game UI.
            if (_lives == 0)
            {
                Messenger.Broadcast(GameEvents.GAME_OVER);
                PlayerPrefs.SetInt("HighScore", _highScore);
                int[] info = new int[] { _currentScore, _highScore, _nApplesCaught };
                Messenger<int[]>.Broadcast(GameEvents.GAME_STATS_DISPLAY, info);
            }
            else
            {
                Messenger<int, bool>.Broadcast(GameEvents.LIFE_ONE_UP, _lives, false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appleScore"></param>
    private void HandleComboCounter(int appleScore)
    {
        int comboLimit = _comboMultiplier * 10;
        bool updateMultiplier = false;

        if (appleScore > 0 && _comboMultiplier < 4)
        {
            _comboProgress++;

            if (_comboProgress == comboLimit)
            {
                _comboMultiplier++;
                _comboProgress = 0;
                updateMultiplier = true;
            }
        }

        if (appleScore < 0)
        {
            _comboProgress = 0;
            _comboMultiplier = 1;
            updateMultiplier = true;
        }

        // Signal combo information update with a COMBO_UPDATE Event
        Messenger<int, int, bool>.Broadcast(GameEvents.COMBO_UPDATE, _comboProgress, _comboMultiplier, updateMultiplier);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appleScore"></param>
    private void CalculateCurrentScore(int appleScore)
    {
        _currentScore += appleScore * _comboMultiplier;
        _currentScore = Mathf.Max(0, _currentScore);
    }

    private void CheckForDifficultyIncrease()
    {
        // Every 25 000 points, increase game difficulty
        if (Mathf.Floor(_currentScore / (7500 * _level)) >= _level && _level < _maxLevel)
        {
            _level++;

            Messenger<int>.Broadcast(GameEvents.INC_DIFFICULTY, _level);
        }
    }

    #endregion
}
