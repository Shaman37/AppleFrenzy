using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Class which handles Basket behaviour (mouse movement/button input and collisions).
/// </summary>
public class Baskets : MonoBehaviour
{
    #region Variables
        
    [Header("Settings")]
    [SerializeField] private BasketSettings   settings;

    private List<GameObject> baskets = new List<GameObject>();
    private Vector3[]        fixedPositions = new Vector3[3];
    private Vector3[]        fixedScales = new Vector3[3];

    private float            swapStart;
    private bool             isSwapping = false;
    private bool             isRotationClockwise = false;
    private float            screenLimit;

    #endregion


    #region Unity Event Methods
        
    private void Awake()
    {

        int ndx = 0;
        foreach (Transform child in transform)
        {
            fixedPositions[ndx] = child.transform.localPosition;
            fixedScales[ndx] = child.transform.localScale;
            baskets.Add(child.gameObject);
            ndx++;
        }    
    }

    private void Update()
    {
        if(!GameStateManager.IS_GAME_PAUSED)
        {
            screenLimit = Camera.main.ViewportToWorldPoint(Vector3.right).x - 6f;
            HandleMouseMovement();
            HandleMouseInput();
        }
    }

    #endregion


    #region Collision Handling
    
    /// <summary>
    ///     Handles basket collision.
    /// </summary>
    /// <param name="coll">The Object the basket collided with.</param>
    private void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;
        
        if(collidedWith.tag == "Apple") 
        {
            Apple ap = collidedWith.GetComponent<Apple>();
            ScoreManager.OnAppleCaught(ap.settings.score);

            Destroy(collidedWith);
        } 
    }

    #endregion


    #region Input Controllers
        
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
        pos.x = mousePos3D.x;

        if (pos.x < -screenLimit)
        {
            pos.x = -screenLimit;
        }
        else if (pos.x > screenLimit)
        {
            pos.x = screenLimit;
        } 
        
        transform.position = pos;
    }

    /// <summary>
    ///     Handles mouse button input.
    ///     Left Mouse Button rotates baskets clockwise.
    ///     Right Mouse Button rotates baskets counter-clockwise.
    /// </summary>
    private void HandleMouseInput()
    {
        if (!isSwapping)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSwapping = true;
                swapStart = Time.time;
                isRotationClockwise = true;
            }

            if (Input.GetMouseButtonDown(1))
            {
                isSwapping = true;
                swapStart = Time.time;
                isRotationClockwise = false;
            }
        }

        else
        {
            SwapBaskets();
        }
    }

    /// <summary>
    ///     Handles basket swapping.
    /// </summary>
    public void SwapBaskets()
    {
        GameObject front = baskets[0];
        GameObject left = baskets[1];
        GameObject right = baskets[2];

        int idxFront = isRotationClockwise ? 1 : 2;
        int idxLeft = isRotationClockwise ? 2 : 0;
        int idxRight = isRotationClockwise ? 0 : 1;

        float u = (Time.time - swapStart) / settings.swapDuration;
        if(u >= 1)
        {
            u = 1;

            if(isRotationClockwise)
            {
                baskets[0] = right;
                baskets[1] = front;
                baskets[2] = left;
            }

            else
            {
                baskets[0] = left;
                baskets[1] = right;
                baskets[2] = front;
            }

            isSwapping = false;
        }

        u = Utils.Ease(u, Utils.EasingType.easeInOut);

        SwapToPosition(front, idxFront, u);
        SwapToPosition(left, idxLeft, u);
        SwapToPosition(right, idxRight, u);
    } 

    /// <summary>
    ///     Auxiliary function for basket swapping.
    ///     Uses a Time-based Linear Interpolation to swap between baskets's positions and scales.
    /// </summary>
    /// <param name="basket">The main basket</param>
    /// <param name="idx">Index to retrieve position to where the main basket will move</param>
    /// <param name="u">Interpolation ratio, based on the set swap duration</param>
    private void SwapToPosition(GameObject basket, int idx, float u)
    {
        basket.transform.localPosition = Vector3.Lerp(basket.transform.localPosition, fixedPositions[idx], u);
        basket.transform.localScale = Vector3.Lerp(basket.transform.localScale, fixedScales[idx], u);
    }

    #endregion
}
