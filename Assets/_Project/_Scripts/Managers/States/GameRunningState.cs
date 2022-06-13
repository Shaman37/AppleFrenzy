using UnityEngine;
using AppleFrenzy.UI;
using AppleFrenzy.Core;

namespace AppleFrenzy
{
    /// <summary>
    ///     Responsible for handling what happens when a 'GamePauseState' is reached in game:
    ///         -> Which UI to Display;
    ///         -> Is the cursor visible?;
    ///         -> The state's Time Scale;
    ///         -> Is the game Paused;
    ///         -> What happens when 'Escape' is pressed;
    /// </summary>
    public class GameRunningState : State
    {
        public override void Start()
        {
            type = eStateType.GameRunning;

            UIManager.Instance.EnableView(eUIViewTypes.GameView);

            Time.timeScale = 1;
            Cursor.visible = false;
            GameManager.Instance.isGamePaused = false;    
        }
    
        public override void Update() 
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                ChangeState(new GamePauseState());
            }
        }
    
        public override void Destroy()
        {
            UIManager.Instance.DisableView(eUIViewTypes.GameView);
        }
    }
}
