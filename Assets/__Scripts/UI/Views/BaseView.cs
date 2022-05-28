using UnityEngine;

/// <summary>
///     Class used to provide simple methods for showing/hiding the different UI Views.
/// </summary>
public abstract class BaseView : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}