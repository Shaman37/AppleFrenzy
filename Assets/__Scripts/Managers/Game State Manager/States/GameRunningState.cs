using UnityEngine;

public class GameRunningState : BaseState
{
    public override void Start()
    {
        base.Start();

        Time.timeScale = 1;
        Cursor.visible = false;
        GameStateManager.isGamePaused = false;

        stateMachine.UI.GameView.Show();
    }

    public override void Update() {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            stateMachine.SetState(new GamePauseState());
        }
    }

    public override void Destroy()
    {
        base.Destroy();

        stateMachine.UI.GameView.Hide();
    }
}