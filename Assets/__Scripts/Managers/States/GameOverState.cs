using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverState : BaseState
{
    public override void Start()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        GameManager.IS_GAME_PAUSED = true;

        stateMachine.UI.GameOverView.OnPlayAgainClicked += PlayAgainClicked;
        stateMachine.UI.GameOverView.OnQuitClicked += QuitClicked;

        stateMachine.UI.GameOverView.Show();
    }

    public override void Destroy()
    {
        stateMachine.UI.GameOverView.OnPlayAgainClicked -= PlayAgainClicked;
        stateMachine.UI.GameOverView.OnQuitClicked -= QuitClicked;

        stateMachine.UI.GameOverView.Hide();
    }

    private void PlayAgainClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void QuitClicked()
    {
        SceneManager.LoadScene(0);
    }
}