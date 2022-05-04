using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePauseState : BaseState
{
    public override void Start()
    {
        base.Start();
        
        Time.timeScale = 0;
        Cursor.visible = true;
        GameStateManager.IS_GAME_PAUSED = true;

        stateMachine.UI.PauseView.OnResumeClicked += ResumeClicked;
        stateMachine.UI.PauseView.OnOptionsClicked += OptionsClicked;
        stateMachine.UI.PauseView.OnQuitClicked += QuitClicked;

        stateMachine.UI.PauseView.Show();
    }

    public override void Update() {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            stateMachine.SetState(new GameRunningState());
        }
    }

    public override void Destroy()
    {
        base.Destroy();

        stateMachine.UI.PauseView.OnResumeClicked -= ResumeClicked;
        stateMachine.UI.PauseView.OnOptionsClicked -= OptionsClicked;
        stateMachine.UI.PauseView.OnQuitClicked -= QuitClicked;

        stateMachine.UI.PauseView.Hide();
    }

    private void ResumeClicked()
    {
        stateMachine.SetState(new GameRunningState());
    }

    private void OptionsClicked()
    {
        Debug.Log("Options Clicked");
    }

    private void QuitClicked()
    {
        SceneManager.LoadScene(0);
    }
}