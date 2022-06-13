using UnityEngine;
using AppleFrenzy.Core;
using AppleFrenzy.UI.Components.Combo;
using AppleFrenzy.UI.Components.LivesIndicator;

namespace AppleFrenzy.UI
{
    /// <summary>
    ///     Represents the 'Game View', activated when the game is running.
    /// </summary>
    public class GameView : View
    {   
        #region [0] - Fields
        
        // UI Components
        [SerializeField] private Lives lives;
        [SerializeField] private ComboBar comboBar;
        
        #endregion

        #region [1] - Unity Event Methods

        protected override void Awake()
        {
            base.Awake();
            gameObject.GetComponent<Canvas>().worldCamera = CameraManager.Instance.gameCamera;
        }

        private void OnEnable() {
            Messenger<int>.AddListener(GameEvents.SCORE_UPDATE, comboBar.UpdateScoreText);
            Messenger<int,int,bool>.AddListener(GameEvents.COMBO_UPDATE, comboBar.UpdateComboText);

            Messenger<int>.AddListener(GameEvents.LIFE_ONE_UP, lives.AddLife);
            Messenger<int>.AddListener(GameEvents.LIFE_ONE_DOWN, lives.RemoveLife);
        }
    
        private void OnDisable() {
            Messenger<int>.RemoveListener(GameEvents.SCORE_UPDATE, comboBar.UpdateScoreText);
            Messenger<int,int,bool>.RemoveListener(GameEvents.COMBO_UPDATE, comboBar.UpdateComboText);
            
            Messenger<int>.RemoveListener(GameEvents.LIFE_ONE_UP, lives.AddLife);
            Messenger<int>.RemoveListener(GameEvents.LIFE_ONE_DOWN, lives.RemoveLife);
        }
        
        #endregion
    }
}
