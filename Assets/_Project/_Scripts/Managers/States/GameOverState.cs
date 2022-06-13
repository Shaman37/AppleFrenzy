using UnityEngine;
using AppleFrenzy.UI;
using AppleFrenzy.Core;

namespace AppleFrenzy
{
    /// <summary>
    ///     Responsible for handling what happens when a 'GameOverState' is reached in game:
    ///         -> Which UI to Display;
    ///         -> Is the cursor visible?;
    ///         -> The state's Time Scale;
    ///         -> Is the game Paused
    /// </summary>
    public class GameOverState : State
    {
        public override void Start()
        {
            type = eStateType.GameOver;
            
            UIManager.Instance.EnableView(eUIViewTypes.GameOverView);
            
            Cursor.visible = true;
            Time.timeScale = 0;
            GameManager.Instance.isGamePaused = true;
        }

        public override void Destroy()
        {
            UIManager.Instance.DisableView(eUIViewTypes.GameOverView);
        }
    }
}