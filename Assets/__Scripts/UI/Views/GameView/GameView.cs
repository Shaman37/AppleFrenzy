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
        Messenger<int>.AddListener(GameEvent.SCORE_UPDATE, comboBar.UpdateScoreText);
        Messenger<int,int,bool>.AddListener(GameEvent.COMBO_UPDATE, comboBar.UpdateComboText);
        Messenger<int,bool>.AddListener(GameEvent.LIFE_ONE_UP, lives.UpdateLives);
    }

    private void OnDisable() {
        Messenger<int>.RemoveListener(GameEvent.SCORE_UPDATE, comboBar.UpdateScoreText);
        Messenger<int,int,bool>.RemoveListener(GameEvent.COMBO_UPDATE, comboBar.UpdateComboText);
        Messenger<int,bool>.RemoveListener(GameEvent.LIFE_ONE_UP, lives.UpdateLives);
    }
}
