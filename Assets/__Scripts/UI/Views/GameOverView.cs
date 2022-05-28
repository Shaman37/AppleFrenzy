using System;
using UnityEngine;
using TMPro;

/// <summary>
///     Class which handles 'Game Over View' button events and displayed information.
/// </summary>
public class GameOverView : BaseView
{
    // UI Variables
    [SerializeField] private TextMeshProUGUI textApplesCaught;
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textHighScore;

    // Button Event variables
    public event Action OnPlayAgainClicked;
    public event Action OnQuitClicked;


    private void OnEnable() {
        Messenger<int[]>.AddListener(GameEvents.GAME_STATS_DISPLAY, DisplayInfo);
    }

    private void OnDisable() {
        Messenger<int[]>.RemoveListener(GameEvents.GAME_STATS_DISPLAY, DisplayInfo);
    }

    private void DisplayInfo(int[] info) 
    {   
        textApplesCaught.text += info[2];
        textScore.text += info[0];
        textHighScore.text += info[1]; 
    }

    // Button events
    public void PlayAgainClick()
    {
        OnPlayAgainClicked?.Invoke();
    }

    public void QuitClick()
    {
        OnQuitClicked?.Invoke();
    }
}
