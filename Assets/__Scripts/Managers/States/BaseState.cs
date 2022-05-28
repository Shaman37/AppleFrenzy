public abstract class BaseState
{
    protected GameManager stateMachine;

    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void Destroy() {}

    public void SetStateMachine(GameManager sm)
    {
        stateMachine = sm;
    }
}
