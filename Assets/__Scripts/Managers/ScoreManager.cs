using System;
using UnityEngine;

public class ScoreManager: MonoBehaviour
{
    private int level;
    private int maxLevel = 5;
    private int lives;
    private int highScore;
    private int currentScore;
    private int nApplesCaught;
    private int nGoodApplesDropped;
    private int comboProgress;
    private int comboMultiplier;


    private void Awake() 
    {
        level = 1;
        lives = 3;
        nApplesCaught = 0;
        comboProgress = 0;
        comboMultiplier = 1;

        if(PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }
    }

    private void Update() 
    {   
        // Every 25 000 points, increase game difficulty
        if(Mathf.Floor(currentScore / (7500 * level)) >= level && level < maxLevel){
            level++;

            Messenger<int>.Broadcast(GameEvent.INC_DIFFICULTY, level);
        }
    }

    private void OnEnable()
    {
        Messenger<int>.AddListener(GameEvent.APPLE_CAUGHT, OnAppleCaught);
        Messenger<int>.AddListener(GameEvent.APPLE_DROPPED, OnAppleDropped);
    }

    private void OnDisable()
    {
        Messenger<int>.RemoveListener(GameEvent.APPLE_CAUGHT, OnAppleCaught);
        Messenger<int>.RemoveListener(GameEvent.APPLE_DROPPED, OnAppleDropped);
    }

    private void CalculateCurrentScore(int appleScore)
    {
        currentScore += appleScore * comboMultiplier;
        currentScore = Mathf.Max(0, currentScore);
    }

    public void OnAppleCaught(int appleScore)
    {
        bool takeLife = false;
        if (appleScore > 0)
        {
            nApplesCaught++;
            nGoodApplesDropped = 0;
        }
        else
        {
            takeLife = true;
        }

        // Handle combo progress/multiplier
        HandleComboCounter(appleScore);

        // Calculate score based on combo multiplier
        CalculateCurrentScore(appleScore);

        if(currentScore > highScore)
        {
            highScore = currentScore;
        }

        // Signal score update with a SCORE_UPDATE Event
        Messenger<int>.Broadcast(GameEvent.SCORE_UPDATE, currentScore);

        // Check if we should add a Life or Remove one
        HandleLives(takeLife);
    }

    public void OnAppleDropped(int penalty)
    {
        currentScore -= penalty;
        currentScore = Mathf.Max(0, currentScore);

        comboProgress = 0;
        comboMultiplier = 1;

        // Signal score information update with a SCORE_UPDATE Event
        Messenger<int>.Broadcast(GameEvent.SCORE_UPDATE, currentScore);
        // Signal combo information update with a COMBO_UPDATE Event
        Messenger<int,int,bool>.Broadcast(GameEvent.COMBO_UPDATE, comboProgress, comboMultiplier, true);

        HandleApplesDropped();
    }

    private void HandleApplesDropped()
    {
        nGoodApplesDropped++;

        if (nGoodApplesDropped == 3)
        {
            nGoodApplesDropped = 0;

            bool takeLife = true;
            HandleLives(takeLife);
        }
    }   

    private void HandleComboCounter(int appleScore)
    {
        int comboLimit = comboMultiplier * 10;
        bool updateMultiplier = false;

        if(appleScore > 0 && comboMultiplier <= 4)
        {
            comboProgress++;
            
            if(comboProgress == comboLimit)
            {
                comboMultiplier++;
                comboProgress = 0;
                updateMultiplier = true;
            }
        }

        if(appleScore < 0)
        {
            comboProgress = 0;
            comboMultiplier = 1;
            updateMultiplier = true;
        }
        
        // Signal combo information update with a COMBO_UPDATE Event
        Messenger<int,int,bool>.Broadcast(GameEvent.COMBO_UPDATE, comboProgress, comboMultiplier, updateMultiplier);
    }

    private void HandleLives(bool takeLife = false){
        
        // If player has less than 3 lives, add a life every 100 good appples caught 
        if((nApplesCaught % 100) == 0 && lives < 3)
        {
            Messenger<int, bool>.Broadcast(GameEvent.LIFE_ONE_UP, lives, true);
            lives++;
        }
        
        
        if(takeLife)
        {
            lives--;
            // If no more lives are left, end game. Else, update game UI.
            if(lives == 0)
            {   
                Messenger.Broadcast(GameEvent.GAME_OVER);
                PlayerPrefs.SetInt("HighScore", highScore);
                int[] info = new int[] { currentScore, highScore, nApplesCaught };
                Messenger<int[]>.Broadcast(GameEvent.GAME_STATS_DISPLAY, info);
            }
            else
            {
                Messenger<int, bool>.Broadcast(GameEvent.LIFE_ONE_UP, lives, false);
            }
        }
    }
}
