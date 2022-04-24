using UnityEngine;

/// <summary>
///     UI Root class, used for storing references to the various UI views.
/// </summary>
public class UIRoot : MonoBehaviour
{
    [SerializeField] private PauseView pauseView;
    public PauseView PauseView => pauseView;

    [SerializeField] private GameView gameView;
    public GameView GameView => gameView;

    [SerializeField] private GameOverView gameOverView;
    public GameOverView GameOverView => gameOverView;
}
