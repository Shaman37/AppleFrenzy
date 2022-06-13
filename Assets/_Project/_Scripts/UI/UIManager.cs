using System.Collections.Generic;

namespace AppleFrenzy.UI
{    
    /// <summary>
    ///     Represents the UI Manager which holds a reference to every UI view in game and is
    ///     responsible for showing/hiding a view.
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        public Dictionary<eUIViewTypes, View> viewsDict;

        protected override void Awake()
        {
            base.Awake();
            
            viewsDict = new Dictionary<eUIViewTypes, View>();
        }

        /// <summary>
        ///     Responsible for enabling a view.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="type">
        ///         The UI scene type to be enabled.
        ///     </param>
        /// </parameters>
        public void EnableView(eUIViewTypes type)
        {
            viewsDict[type].Show();
        }

        /// <summary>
        ///     Responsible for disabling a view.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="type">
        ///         The UI scene type to be disabled.
        ///     </param>
        /// </parameters>
        public void DisableView(eUIViewTypes type)
        {
            viewsDict[type].Hide();
        }
    }
}
