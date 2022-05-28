using UnityEngine;

/// <summary>
///     This class is a State Machine which keeps track of our Game State.
///     Handles 
/// </summary>
public class GameManager : MonoBehaviour
{   
    // State related variables
    private BaseState currentState;
    public static bool IS_GAME_PAUSED;

    // Root object for the different UI views
    [SerializeField] private UIRoot ui;
    public  UIRoot UI => ui;


    #region [1] - Unity Event Methods
    
    /// <summary>
    ///     Initializes the Game State Manager with a GameRunning State.
    /// </summary>
    private void Start() 
    {
        ChangeState(new GameRunningState());
    }

    /// <summary>
    ///     Casts the Update function inside the current game state every frame to listen for state changes.
    /// </summary>
    private void Update() 
    {
        currentState.Update();
    }
    
    private void OnEnable() 
    {
        Messenger.AddListener(GameEvents.GAME_OVER, OnGameOver);
    }

    private void OnDisable() 
    {
        Messenger.RemoveListener(GameEvents.GAME_OVER, OnGameOver);
    }

    #endregion

    /// <summary>
    ///     Responsible for changing the current game state to a new one.
    /// </summary>
    /// <param name="newState">The new game state</param>
    public void ChangeState(BaseState newState)
    {   
        if(currentState != null)
        {
            currentState.Destroy();
        }

        currentState = newState;
        currentState.SetStateMachine(this);

        currentState.Start();
    }

    /// <summary>
    ///     Responsible for changing the current game state to a new one.
    /// </summary>
    public void OnGameOver()
    {
        ChangeState(new GameOverState());
    }
}
