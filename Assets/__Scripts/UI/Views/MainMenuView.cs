using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Class which handles the 'Main Menu' events and displayed information.
/// </summary>
public class MainMenuView : BaseView
{
    public void PlayClick()
    {
        SceneManager.LoadScene(1);
    }

    public void OptionsClick()
    {
        Debug.Log("Options Clicked");
    }
}
