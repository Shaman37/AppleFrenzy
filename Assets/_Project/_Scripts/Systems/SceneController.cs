using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppleFrenzy
{
    /// <summary>
    ///     Represents a custom scene manager, responsible for loading and unloading scenes at different points in the game.
    ///         -> Scene (0) = Main Menu Scene;
    ///         -> Scene (1) = Game Scene;
    ///         -> uiScenes  = UI Scenes containing the different UI views;
    /// </summary>
    public class SceneController : PersistentSingleton<SceneController>
    {
        
        #region [0] - Fields
        
        public List<string> uiScenes;
        
        #endregion

        #region [1] - Unity Event Methods
        
        protected override void Awake()
        {
            base.Awake();
        }
            
        #endregion    

        #region [2] - Methods

        /// <summary>
        ///     Responsible for initially loading the game.
        /// </summary>
        public void LoadGame()
        {
            SceneManager.LoadScene(1);

            foreach (string scene in uiScenes)
            {
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            }
        }

        /// <summary>
        ///     Responsible for restarting the game.
        /// </summary>
        public void RestartGame()
        {
            SceneManager.LoadScene(1);

            foreach (string scene in uiScenes)
            {
                ReloadScene(scene);
            }
        }

        /// <summary>
        ///     Responsible for reloading an additive scene (unload + load).
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="scene">
        ///         The scene to be reloaded.
        ///     </param>
        /// </parameters>
        private void ReloadScene(string scene)
        {
            SceneManager.UnloadSceneAsync(scene);

            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }

        /// <summary>
        ///     Responsible for returning to the Main Menu, unloading all additive scenes and
        ///     loading 'Scene (0)'.
        /// </summary>
        public void ReturnToMainMenu()
        {
            foreach (string scene in uiScenes)
            {
                SceneManager.UnloadSceneAsync(scene);
            }

            SceneManager.LoadSceneAsync(0);
        }

        #endregion
    }
}