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
        Messenger<int>.AddListener(GameEvents.SCORE_UPDATE, comboBar.UpdateScoreText);
        Messenger<int,int,bool>.AddListener(GameEvents.COMBO_UPDATE, comboBar.UpdateComboText);
        Messenger<int,bool>.AddListener(GameEvents.LIFE_ONE_UP, lives.UpdateLives);
    }

    private void OnDisable() {
        Messenger<int>.RemoveListener(GameEvents.SCORE_UPDATE, comboBar.UpdateScoreText);
        Messenger<int,int,bool>.RemoveListener(GameEvents.COMBO_UPDATE, comboBar.UpdateComboText);
        Messenger<int,bool>.RemoveListener(GameEvents.LIFE_ONE_UP, lives.UpdateLives);
    }
}
