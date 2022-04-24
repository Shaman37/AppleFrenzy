using UnityEngine;

/// <summary>
///     This class is a State Machine which keeps track of our Game State.
///     Handles 
/// </summary>
public class GameStateManager : MonoBehaviour
{   
    // State related variables
    private BaseState currentState;
    public static bool isGamePaused;

    // Root object for the different UI views
    [SerializeField] private UIRoot ui;
    public  UIRoot UI => ui;


    #region UNITY EVENT FUNCTIONS
    
    /// <summary>
    ///     Initializes the Game State Manager with a GameRunning State.
    /// </summary>
    private void Start() 
    {
        SetState(new GameRunningState());
    }

    private void OnEnable() 
    {
        ScoreManager.GameOver += EndGame;
    }

    /// <summary>
    ///     Casts the Update function inside the current game state every frame to listen for state changes.
    /// </summary>
    private void Update() 
    {
        currentState.Update();
    }
        
    private void OnDisable() 
    {
        ScoreManager.GameOver -= EndGame;
    }
    #endregion

    /// <summary>
    ///     Responsible for changing the current game state to a new one.
    /// </summary>
    /// <param name="newState">The new game state</param>
    public void SetState(BaseState newState)
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
    public void EndGame()
    {
        SetState(new GameOverState());
    }
}
