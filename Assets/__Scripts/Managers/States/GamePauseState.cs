using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePauseState : BaseState
{
    public override void Start()
    {        
        Time.timeScale = 0;
        Cursor.visible = true;
        GameManager.IS_GAME_PAUSED = true;

        stateMachine.UI.PauseView.OnResumeClicked += ResumeClicked;
        stateMachine.UI.PauseView.OnQuitClicked += QuitClicked;

        stateMachine.UI.PauseView.Show();
    }

    public override void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            stateMachine.ChangeState(new GameRunningState());
        }
    }

    public override void Destroy()
    {
        stateMachine.UI.PauseView.OnResumeClicked -= ResumeClicked;
        stateMachine.UI.PauseView.OnQuitClicked -= QuitClicked;

        stateMachine.UI.PauseView.Hide();
    }

    private void ResumeClicked()
    {
        stateMachine.ChangeState(new GameRunningState());
    }

    private void QuitClicked()
    {
        SceneManager.LoadScene(0);
    }
}