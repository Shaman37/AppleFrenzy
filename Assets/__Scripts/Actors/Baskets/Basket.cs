using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public enum eBasketPlacement
{
    front,
    left,
    right
}

/// <summary>
/// 
/// </summary>
public class Basket : MonoBehaviour {

    #region [0] - Fields

    public eBasketPlacement placement;
    public Vector3          position;
    public Vector3          scale;

    private Vector3 _targetPosition;
    private Vector3 _targetScale;

    // Catch Apple Animation
    private float _expandStart;
    private float _expandDuration;
    private float _retractStart;
    private float _retractDuration;

    // Animation States
    private bool _isSwapping;
    private bool _isExpanding;
    private bool _isRetracting;

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
    /// 
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="targetScale"></param>
    /// <param name="duration"></param>
    /// <param name="isClockwise"></param>
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
    /// 
    /// </summary>
    /// <param name="shakeDuration"></param>
    public void PlayCatchAnimation(float shakeDuration)
    {
        this.transform.DOShakePosition(shakeDuration, 0.5f, 2, 0).SetEase(Ease.InOutSine);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isClockwise"></param>
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
