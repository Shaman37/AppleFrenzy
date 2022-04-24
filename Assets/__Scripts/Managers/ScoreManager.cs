using System;
using UnityEngine;

public class ScoreManager: MonoBehaviour
{
    private int            level;
    private int            maxLevel = 5;
    private static int     lives;
    private static int     highScore;
    private static int     currentScore;
    private static int     nApplesCaught;
    private static int     comboProgress;
    private static int     comboMultiplier;

    // Events 
    public static event Action<int>             ScoreUpdate;
    public static event Action<int, int, bool>  ComboUpdate;
    public static event Action                  NextLevel;
    public static event Action<int, bool>       LifeBonus;
    public static event Action                  GameOver;
    public static event Action<int[]>           DisplayGameInfo;


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

        PlayerPrefs.SetInt("HighScore", highScore);
    }

    private void FixedUpdate() 
    {   
        // Every 25 000 points, increase game difficulty
        if(Mathf.Floor(currentScore / 25000) > level){
            NextLevel?.Invoke();

            level++;
        }
    }

    public static void OnAppleCaught(int appleScore)
    {   
        // Handle combo progress/multiplier
        handleComboCounter(appleScore);

        // Calculate score based on combo multiplier and have 0 be the minimum score possible
        currentScore += appleScore * comboMultiplier;
        currentScore = Mathf.Max(0, currentScore);

        if(currentScore > highScore)
        {
            highScore = currentScore;
        }

        ScoreUpdate?.Invoke(currentScore);

        // Check if we should add a Life or Remove one, based on the caught apple score
        handleLives(appleScore);
    }

    public static void OnAppleDropped(int penalty)
    {
        currentScore -= penalty;
        currentScore = Mathf.Max(0, currentScore);
        ScoreUpdate?.Invoke(currentScore);

        comboProgress = 0;
        comboMultiplier = 1;
        ComboUpdate?.Invoke(comboProgress, comboMultiplier, true);
    }

    private static void OnGameOver()
    {
        GameOver?.Invoke();
        
        int[] info = new int[] { currentScore, highScore, nApplesCaught };
        DisplayGameInfo?.Invoke(info);
    }

    private static void handleComboCounter(int appleScore)
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
            
        ComboUpdate?.Invoke(comboProgress, comboMultiplier, updateMultiplier);
    }

    private static void handleLives(int appleScore){
        // If caught Apple is a good apple (positive score)
        if(appleScore > 0) 
        {
            nApplesCaught++;

            // If player has less than 3 lives, add a life every 100 good appples caught 
            if((nApplesCaught % 100) == 0 && lives < 3)
            {
                LifeBonus?.Invoke(lives, true);
                lives++;
            }
        }
        // If caught Apple is a bad apple (negative score)
        else
        {   
            lives--;

            // If no more lives are left, end game. Else, update game UI. 
            if(lives == 0)
            {
                PlayerPrefs.SetInt("HighScore", highScore);

                OnGameOver();
            }
            else
            {
                LifeBonus?.Invoke(lives, false);
            }
        }
    }
}
