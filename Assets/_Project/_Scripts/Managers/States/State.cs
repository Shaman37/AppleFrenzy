using AppleFrenzy.Core;

namespace AppleFrenzy
{
    /// <summary>
    ///    Abstract class which specifies the fields and methods of a 'State' derived Class.
    /// </summary>
    public abstract class State
    {   
        public eStateType type;
        
        public virtual void Start() {}
        public virtual void Update() {}
        public virtual void Destroy() {}

        /// <summary>
        ///     Responsible for changing the current game state.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="newState">
        ///         The new game state.
        ///     </param>
        /// </parameters>
        public void ChangeState(State newState)
        {
            GameManager.Instance.ChangeState(newState);
        }
    }
}
