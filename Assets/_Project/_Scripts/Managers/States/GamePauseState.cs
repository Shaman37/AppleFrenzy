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
    public class GamePauseState : State
    {
        public override void Start()
        {
            type = eStateType.GamePaused;

            UIManager.Instance.EnableView(eUIViewTypes.PauseView);

            Time.timeScale = 0;
            Cursor.visible = true;
            GameManager.Instance.isGamePaused = true;
        }
    
        public override void Update() 
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                ChangeState(new GameRunningState());
            }
        }
    
        public override void Destroy()
        {
            UIManager.Instance.DisableView(eUIViewTypes.PauseView);
        }
    }
}