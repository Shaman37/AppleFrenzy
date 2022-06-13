using UnityEngine;
using AppleFrenzy.Core;

namespace AppleFrenzy
{
    /// <summary>
    ///     Handles mouse movement and input from the player.
    /// </summary>
    public class BasketsInputController : MonoBehaviour
    {
        #region [0] - Fields

        [SerializeField] private BasketsManager  _baskets;
        [SerializeField] private BasketsSettings _settings;

        private float _screenLimit;

        #endregion

        #region [1] - Unity Event Methods
        private void Awake()
        {
            _screenLimit = CameraManager.Instance.gameCamera.ViewportToWorldPoint(Vector3.right).x - 4f;  
        }

        private void Update()
        {
            if(!GameManager.Instance.isGamePaused)
            {
                HandleMouseMovement();
                HandleMouseButtons();
            }
        }

        #endregion

        #region [2] - Methods

        /// <summary>
        ///     Responsible for capturing mouse movement and update the position of the attatched
        ///     gameObject, to the cursor's position.
        /// </summary>
        private void HandleMouseMovement()
        {
            
            Vector3 mousePos2D = Input.mousePosition;
            mousePos2D.z = -CameraManager.Instance.gameCamera.transform.position.z;

            Vector3 mousePos3D = CameraManager.Instance.gameCamera.ScreenToWorldPoint(mousePos2D);

            Vector3 pos = transform.position;
            float x = Mathf.Lerp(pos.x, mousePos3D.x, _settings.moveSpeed);

            pos.x = x;
            if (pos.x < -_screenLimit)
            {
                pos.x = -_screenLimit;
            }
            else if (pos.x > _screenLimit)
            {
                pos.x = _screenLimit;
            } 

            transform.position = pos; 
        }

        /// <summary>
        ///     Responsible for handling mouse button input from the player.
        ///     Left Mouse Button rotates baskets clockwise.
        ///     Right Mouse Button rotates baskets counter-clockwise.
        /// </summary>
        private void HandleMouseButtons()
        {
            if (!_baskets.isInSwapAnimation)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _baskets.SwapBaskets(true);
                }
    
                if (Input.GetMouseButtonDown(1))
                {
                    _baskets.SwapBaskets(false);
                }
            }
        }

        #endregion
    }
}
