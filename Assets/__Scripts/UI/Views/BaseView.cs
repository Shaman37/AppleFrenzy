using UnityEngine;

/// <summary>
///     Class used to provide simple methods for showing/hiding the different UI Views.
/// </summary>
public class BaseView : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}