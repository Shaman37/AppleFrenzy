public abstract class BaseState
{
    protected GameStateManager stateMachine;

    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void Destroy() {}

    public void SetStateMachine(GameStateManager sm)
    {
        stateMachine = sm;
    }
}
