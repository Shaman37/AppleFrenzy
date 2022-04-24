using UnityEngine;

/// <summary>
///    Class which handles 'Game View' events and displayed information.
/// </summary>
public class GameView : BaseView
{   
    // UI Components
    [SerializeField] private Lives lives;
    [SerializeField] public ComboBar comboBar;

    private void OnEnable() {
        ScoreManager.ScoreUpdate += comboBar.UpdateScore;
        ScoreManager.ComboUpdate += comboBar.UpdateCombo;
        ScoreManager.LifeBonus += lives.UpdateLives;
    }

    private void OnDisable() {
        ScoreManager.ScoreUpdate -= comboBar.UpdateScore;
        ScoreManager.ComboUpdate -= comboBar.UpdateCombo;
        ScoreManager.LifeBonus -= lives.UpdateLives;
    }
}
