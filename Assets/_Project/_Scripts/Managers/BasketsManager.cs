using UnityEngine;
using System.Collections.Generic;
using AppleFrenzy.Core;

namespace AppleFrenzy
{
    /// <summary>
    ///     Responsible for handling the baskets' collision and play
    ///     each basket's animation.
    /// </summary>
    public class BasketsManager : MonoBehaviour
    {
        #region [0] - Fields

        static public Dictionary<eBasketPlacement, Basket> BASKETS;
            
        [Header("Settings")]
        [SerializeField] private BasketsSettings _settings;
    
        // Animation fields
        public bool isInSwapAnimation;
        public bool isInCatchAnimation;
        
        private float _swapAnimationEnd;
        private float _catchAnimationEnd;
        
        #endregion

        #region [1] - Unity Event Methods
    
        private void Awake()
        {
            BASKETS = new Dictionary<eBasketPlacement, Basket>();
    
            int index = 0;
            foreach (Transform child in transform)
            {
                Basket basket = child.gameObject.GetComponent<Basket>();
                basket.placement = (eBasketPlacement)index;
    
                BASKETS.Add((eBasketPlacement)index, basket);
    
                index++;
            }
        }
    
        private void Update()
        {
            if(!GameManager.Instance.isGamePaused)
            {
                if (Time.time >= _swapAnimationEnd) {
                    isInSwapAnimation = false;
                }
                if (Time.time >= _catchAnimationEnd) {
                    isInCatchAnimation = false;
                }
            }
        }
    
        #endregion
    
        #region [2] - Collision Handling
        
        private void OnCollisionEnter(Collision coll)
        {
            GameObject collidedWith = coll.gameObject;
            Apple apple = collidedWith.GetComponent<Apple>();            
            
            if(apple != null) 
            {
                Messenger<int>.Broadcast(GameEvents.APPLE_CAUGHT, apple.settings.score);
    
                Destroy(collidedWith);
    
                if (!isInSwapAnimation)
                {
                    BASKETS[eBasketPlacement.front].PlayCatchAnimation(_settings.shakeDuration);
                    isInCatchAnimation = true;
                    _catchAnimationEnd = Time.time + _settings.shakeDuration;
                }
            } 
        }
    
        #endregion
    
        #region [3] - Methods

            /// <summary>
            ///     Responsible for playing the baskets' swap animation.
            /// </summary>
            /// 
            /// <parameters>
            ///     <param name="isClockwise">
            ///         If the rotation is clockwise or not.    
            ///     </param>
            /// </parameters>
            public void SwapBaskets(bool isClockwise)
            {
                isInSwapAnimation = true;
                float duration = _settings.swapDuration;
        
                Basket front = isClockwise ? BASKETS[eBasketPlacement.left]  : BASKETS[eBasketPlacement.right];
                Basket left  = isClockwise ? BASKETS[eBasketPlacement.right] : BASKETS[eBasketPlacement.front];
                Basket right = isClockwise ? BASKETS[eBasketPlacement.front] : BASKETS[eBasketPlacement.left];
        
                foreach (Basket basket in BASKETS.Values)
                {
                    switch (basket.placement)
                    {
                        case eBasketPlacement.front:
                            basket.PlaySwapAnimation(front.position, front.scale, duration, isClockwise);
                            break;
        
                        case eBasketPlacement.left:
                            basket.PlaySwapAnimation(left.position, left.scale, duration, isClockwise);
                            break;
        
                        case eBasketPlacement.right:
                            basket.PlaySwapAnimation(right.position, right.scale, duration, isClockwise);
                            break;
                    }
                }
        
                _swapAnimationEnd = Time.time + duration;
            }  
            
        #endregion
    }
}
