using UnityEngine;

namespace AppleFrenzy.UI
{
   /// <summary>
    ///    Abstract class which specifies the fields and methods of a 'View' derived class.
    /// </summary>
    public abstract class View : MonoBehaviour
    {
        public eUIViewTypes type;

        protected virtual void Awake()
        {
            UIManager.Instance.viewsDict.Add(type, this);
        }

        /// <summary>
        ///     Responsible for activating the view.
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        ///     Responsible for disabing the view.
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
