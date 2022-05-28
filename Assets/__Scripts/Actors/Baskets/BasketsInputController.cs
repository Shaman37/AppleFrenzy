using UnityEngine;

namespace shaman37
{
    public class BasketsInputController : MonoBehaviour
    {
        [SerializeField] private BasketsManager baskets;
        [SerializeField] private float          moveSpeed = 0.5f;
        
        private float _screenLimit;

        private void Awake()
        {
            _screenLimit = Camera.main.ViewportToWorldPoint(Vector3.right).x - 6f;
            
        }
        private void Update()
        {
            HandleMouseMovement();
            HandleMouseButtons();
        }
        /// <summary>
        ///     Handles Mouse Input.
        ///     Moves baskets to mouse position along the X axis, respecting the imposed screen limit.
        /// </summary>
        private void HandleMouseMovement()
        {
            Vector3 mousePos2D = Input.mousePosition;
            mousePos2D.z = -Camera.main.transform.position.z;

            Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
            Vector3 pos = transform.position;
            float x = Mathf.Lerp(pos.x, mousePos3D.x, moveSpeed);
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
        ///     Handles mouse button input.
        ///     Left Mouse Button rotates baskets clockwise.
        ///     Right Mouse Button rotates baskets counter-clockwise.
        /// </summary>
        private void HandleMouseButtons()
        {
            if (!baskets.isInSwapAnimation)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    baskets.SwapBaskets(true);
                }
    
                if (Input.GetMouseButtonDown(1))
                {
                    baskets.SwapBaskets(false);
                }
            }
        }
    }
}
