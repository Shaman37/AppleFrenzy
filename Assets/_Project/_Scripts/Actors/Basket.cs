using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using DG.Tweening;
using AppleFrenzy.Core;

namespace AppleFrenzy
{
    /// <summary>
    ///     Represents a Basket and handles it's behaviour in game.
    ///     Mainly resposible for handling basket animation, holding crucial
    ///     information (placement, targetPosition and targetScale) for it.
    /// </summary>
    public class Basket : MonoBehaviour {
    
        #region [0] - Fields
        
        public eBasketPlacement placement;
        public Vector3          position;
        public Vector3          scale;
    
        private Vector3 _targetPosition;
        private Vector3 _targetScale;
    
        #endregion
    
        #region [1] - Unity Event Methods
    
        private void Awake()
        {
            position = transform.localPosition;
            scale = transform.localScale;
        }
    
        #endregion
    
        #region [2] - Methods
    
        /// <summary>
        ///     Responsible for handling the basket swap animation.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="targetPosition">
        ///         The basket's target position.
        ///     </param>
        ///     <param name="targetScale">
        ///         The basket's target scale.
        ///     </param>
        ///     <param name="duration">
        ///         The animation's duration.
        ///     </param>
        ///     <param name="isClockwise">
        ///         If the rotation of the basket swap animation is clockwise or not.
        ///     </param>
        /// </paramenters>
        public async void PlaySwapAnimation(Vector3 targetPosition, Vector3 targetScale, float duration, bool isClockwise)
        {
            _targetPosition = targetPosition;
            _targetScale = targetScale;
    
            var tasks = new List<Task>();
    
            tasks.Add(transform.DOLocalMove(_targetPosition, duration)
                               .SetEase(Ease.InOutSine)
                               .AsyncWaitForCompletion()
                     );
    
            tasks.Add(transform.DOScale(_targetScale, duration)
                               .SetEase(Ease.InOutSine)
                               .AsyncWaitForCompletion()
                     );
        
            await Task.WhenAll(tasks);
            
            UpdateBasket(isClockwise);
        }
    
        /// <summary>
        ///     Responsible for playing the animation when an Apple is caught.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="shakeDuration">
        ///         The duration of the catch animation.
        ///     </param>
        /// </parameters>
        public void PlayCatchAnimation(float shakeDuration)
        {
            transform.DOShakePosition(shakeDuration, 0.5f, 2, 0).SetEase(Ease.InOutSine);
        }
    
        /// <summary>
        ///     Responsible for updating the basket information at the end of the 
        ///     basket swap animation.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="isClockwise">
        ///         If the rotation of the basket swap animation is clockwise or not.
        ///     </param>
        /// </parameters>
        private void UpdateBasket(bool isClockwise)
        {
            int newPlacement = (int)placement;
    
            if (isClockwise)
            {
                if (newPlacement < 2)
                {
                    newPlacement++;
                }
                else
                {
                    newPlacement = 0;
                }
            }
            else
            {
                if (newPlacement > 0)
                {
                    newPlacement--;
                }
                else
                {
                    newPlacement = 2;
                }
            }
    
            position = transform.localPosition;
            scale = transform.localScale;
            placement = (eBasketPlacement)newPlacement;
            
            BasketsManager.BASKETS[placement] = this;
        }
    
        #endregion
    }
}
