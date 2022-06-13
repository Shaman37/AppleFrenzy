using UnityEngine;
using System.Collections.Generic;

namespace AppleFrenzy.UI.Components.LivesIndicator
{   
    /// <summary>
    ///     Represents the UI Component which is responsible for showing the lives.
    /// </summary>
    public class Lives : MonoBehaviour
    {
        #region [0] - Fields
        
        private List<GameObject> _hearts;
        
        #endregion
     
        #region [1] - Unity Event Methods

        private void Start() 
        {
            _hearts = new List<GameObject>();
    
            foreach (Transform child in transform)
            {
                _hearts.Add(child.gameObject);
            }
        }
    
        #endregion
        
        #region [2] - Methods

        /// <summary>
        ///     Responsible for updating the lives component, adding a life icon (heart).
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="index">
        ///         The index of the element in the '_hearts' List.
        ///     </param>
        /// </parameters>
        public void AddLife(int index)
        { 
            _hearts[index].SetActive(true);
        }

        /// <summary>
        ///     Responsible for updating the lives component, removing a life icon (heart).
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="index">
        ///         The index of the element in the '_hearts' List.
        ///     </param>
        /// </parameters>
        public void RemoveLife(int index)
        {   
            _hearts[index].SetActive(false);
        }
        
        #endregion
        
    }
}
