using UnityEngine;

public class GameRunningState : BaseState
{
    public override void Start()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        GameManager.IS_GAME_PAUSED = false;

        stateMachine.UI.GameView.Show();
    }

    public override void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            stateMachine.ChangeState(new GamePauseState());
        }
    }

    public override void Destroy()
    {
        stateMachine.UI.GameView.Hide();
    }
}